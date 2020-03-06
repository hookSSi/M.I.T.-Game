using MIT.SamtleGame.DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MIT.SamtleGame.Stage2.Pokemon
{
    public enum BattleState
    {
        None, Start, Introduction,
        SelectAction, SelectSkill, SelectItem, Information,
        Act, End
    }

    [System.Serializable]
    public class BattleEvent : UnityEvent<Pokemon, Pokemon> { }

    public class PokemonBattleManager : Singleton<PokemonBattleManager>
    {
        public PokemonBattleUIManager _uiManager;
        public PokemonBattleItemManager _itemManager;
        public BattleDialogueController _dialogueController;
        public Pokemon _myPokemon;
        public Pokemon _enemyPokemon;

        private Tool.PokemonBattleEventSystem _eventSystem;
        private WaitWhile _waitDialogueUpdating;
        private WaitWhile _waitInputing;
        private WaitWhile _waitUIUpdating;

        [Header("배틀 테스트용 인자")]
        [SerializeField] private string _testMyPokemon;
        [SerializeField] private string _testEnemyPokemon;
        [SerializeField] private bool _isTest = false;

        public BattleState _state { get; private set; }

        public bool _isGameOver { get { return _myPokemon.Health <= 0 || _enemyPokemon.Health <= 0; }}

        private void Start()
        {
            _state = BattleState.None;

            _eventSystem = FindObjectOfType<Tool.PokemonBattleEventSystem>();

            _uiManager.gameObject.SetActive(false);

            _waitDialogueUpdating = new WaitWhile
                (() => { return _dialogueController._isEnd == false; });
            _waitInputing = new WaitWhile
                (() => { return !Input.GetButtonDown("Interact") && !Input.GetButtonDown("Submit"); });
            _waitUIUpdating = new WaitWhile
                (() => { return _uiManager._mainUI._isPlayerHpAnimating || _uiManager._mainUI._isEnemyHpAnimating; });

#if UNITY_EDITOR
            if (_isTest)
                StartBattle(_testMyPokemon, _testEnemyPokemon);
#endif
        }

        public void StartBattle(string myPokemonName, string enemyPokemonName)
        {
            PokemonInfo myInfo, enemyInfo;

            PlayerControllerEvent.Trigger(false);

            _uiManager.gameObject.SetActive(true);

            if (!PokemonManager.GetPokemonInfo(myPokemonName, out myInfo))
                return;

            if (!PokemonManager.GetPokemonInfo(enemyPokemonName, out enemyInfo))
                return;

            _myPokemon.SetInfo(myInfo);
            _enemyPokemon.SetInfo(enemyInfo);

            _state = BattleState.Start;

            _uiManager._mainUI.UpdatePlayerImage(false);
            _uiManager._mainUI.UpdateEnemyImage(false);
            _uiManager._mainUI.UpdateValue(_myPokemon, _enemyPokemon, 10, 10, 0f, 100f);
            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Battle);
            _uiManager._bottomUI.UpdateDialog();
            _uiManager._bottomUI._skill.Init();
            _uiManager._bottomUI._skill.SetPokemon(_myPokemon);
            _uiManager._bottomUI._skill.UpdateText();

            StartCoroutine("StartBattleCoroutine");
        }

        private IEnumerator StartBattleCoroutine()
        {
            // 대사 나올 곳
            _dialogueController.ClearPages();
            _dialogueController.AddNextPage("야생의 " + _enemyPokemon.Info._name + "이(가) 나타났다!");
            _dialogueController.AddNextPage("가랏! " + _myPokemon.Info._name + "!", true);

            _uiManager._mainUI.UpdateEnemyImage(true);
            _dialogueController.NextDialogue();
            yield return _waitDialogueUpdating;
            // 새내기 이동
            _uiManager._mainUI.MoveIpsangSide();
            yield return new WaitWhile(() => _uiManager._mainUI._isIpsangMoving);
            // 소환 이펙트
            _uiManager._mainUI.UpdatePlayerImage(true);
            yield return _waitInputing;
            _dialogueController.EndDialogue();

            SelectAction();
        }

        public void SelectAction()
        {
            _state = BattleState.SelectAction;

            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Battle);
            _uiManager._bottomUI.UpdateActionUI();

            _eventSystem.InitializeUINavigation(BattleState.SelectAction);
        }

        public void SelectSkill()
        {
            _state = BattleState.SelectSkill;

            // UI Update
            _uiManager._bottomUI.UpdateSkillUI();

            _eventSystem.InitializeUINavigation(BattleState.SelectSkill);
        }

        public void SelectItem()
        {
            _state = BattleState.SelectItem;

            // UI Update
            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Bag);
            _uiManager._bottomUI.UpdateDialog();
            
            _eventSystem.InitializeUINavigation(BattleState.SelectItem);
        }

        public void UseSkill(int indexOfSkill)
        {
            _state = BattleState.Act;

            Skill playerSkill = _myPokemon?.UseSkill(indexOfSkill);
            if (playerSkill._currentCount <= 0)
                playerSkill = PokemonManager.DefaultSkill();

            playerSkill._currentCount--;
            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Battle);
            _uiManager._bottomUI.UpdateDialog();

            StartCoroutine(ActPhase(playerSkill._battleEvent, NextEnemySkill()._battleEvent));
        }

        public void UseItem(BattleEvent battleEvent)
        {
            _state = BattleState.Act;

            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Battle);
            _uiManager._bottomUI.UpdateDialog();

            StartCoroutine(ActPhase(battleEvent, NextEnemySkill()._battleEvent));
        }

        IEnumerator ActPhase(BattleEvent playerEvent, BattleEvent enemyEvent)
        {
            for (int i = 0; i < 2; i++)
            {
                float previousPlayerHealth = _myPokemon.Health;
                float previousEnemyHealth = _enemyPokemon.Health;

                _dialogueController.ClearPages();

                if (i == 0) // 아군의 턴
                {
                    playerEvent.Invoke(_myPokemon, _enemyPokemon);
                    // 상태 이상
                    if (_myPokemon._effectCount > 0) _myPokemon._effectCount--;
                    else _myPokemon._status = Pokemon.StatusEffect.None;
                }
                else // 적의 턴
                {
                    enemyEvent.Invoke(_enemyPokemon, _myPokemon);
                    // 상태 이상
                    if (_enemyPokemon._effectCount > 0) _enemyPokemon._effectCount--;
                    else _enemyPokemon._status = Pokemon.StatusEffect.None;
                }

                // 대사 1(데미지 판정 이전)
                _dialogueController.NextDialogue();
                yield return _waitDialogueUpdating;
                /* 이곳에서 이펙트 재생 및 기다리기 */

                if (_myPokemon.Health != previousPlayerHealth)
                    _uiManager._mainUI.UpdatePlayerHpUI(_myPokemon.Health, _myPokemon.MaxHealth, true);

                if (_enemyPokemon.Health != previousEnemyHealth)
                    _uiManager._mainUI.UpdateEnemyHpUI(_enemyPokemon.Health, _enemyPokemon.MaxHealth, true);

                yield return 0.1f;
                yield return _waitUIUpdating;
                // 대사 2(데미지 판정 이후)
                _dialogueController.NextDialogue();
                yield return _waitDialogueUpdating;
                yield return _waitInputing;
                _dialogueController.EndDialogue();

                if (_isGameOver)
                    break;
            }

            // 게임 종료 체크
            if (_isGameOver)
            {
                StartCoroutine("EndBattle");
            }
            else
            {
                SelectAction();
            }
        }

        private IEnumerator EndBattle()
        {
            _state = BattleState.End;
            _dialogueController.ClearPages();

            if (_myPokemon.Health <= 0f) _uiManager._mainUI.UpdatePlayerImage(false);
            if (_enemyPokemon.Health <= 0f) _uiManager._mainUI.UpdateEnemyImage(false);

            if (_myPokemon.Health > 0f)
            {
                _dialogueController.AddNextPage("신난다! " + _enemyPokemon.Info._name + "과의 과제에서 이겼다!", true);
            }
            else
            {
                _dialogueController.AddNextPage(_myPokemon.Info._name + "가 최후의 오류를 내뿜었다...");
                _dialogueController.AddNextPage("새내기는 눈앞이 깜깜해졌다!", true);
            }

            _dialogueController.NextDialogue();

            yield return _waitDialogueUpdating;
            yield return _waitInputing;

            _dialogueController.EndDialogue();

            PlayerControllerEvent.Trigger(true);
            _uiManager.gameObject.SetActive(false);
        }


        // 포켓몬 고르기(정보)
        public void SelectPokemonInformation()
        {
            Debug.Log("포켓몬 정보 보기...");

            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Information);
            _uiManager._bottomUI.UpdateInformation();
            _eventSystem.InitializeUINavigation(BattleState.Information);
        }

        // 도주(아무 기능 없음)
        public void Escape()
        {
            StartCoroutine("EscapeCoroutine");
        }

        private IEnumerator EscapeCoroutine()
        {
            _uiManager._bottomUI.UpdateDialog();

            _dialogueController.EndDialogue();
            _dialogueController.ClearPages();
            _dialogueController.AddNextPage("안돼! 이번 학기 학점을 이렇게 버릴 수 없어!", true);
            _dialogueController.NextDialogue();

            yield return _waitDialogueUpdating;
            yield return _waitInputing;

            _dialogueController.EndDialogue();

            _uiManager._bottomUI.UpdateActionUI();
            _eventSystem.InitializeUINavigation(BattleState.SelectAction);
        }

        // 다음에 적이 사용할 스킬
        private Skill NextEnemySkill()
        {
            int enemySkillLength = _enemyPokemon.Info._skills.Length;

            Skill enemySkill = _enemyPokemon.UseSkill(Random.Range(0, enemySkillLength));

            if (enemySkill._currentCount <= 0)
                enemySkill = PokemonManager.DefaultSkill();

            // PokemonManager.Instance._previousSkillName = enemySkill._name;
            enemySkill._currentCount--;

            return enemySkill;
        }
    }
}