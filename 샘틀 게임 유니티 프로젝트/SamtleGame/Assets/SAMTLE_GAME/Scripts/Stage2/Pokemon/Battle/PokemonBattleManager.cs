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
        public Pokemon _myPokemon;
        public Pokemon _enemyPokemon;
        public string _textSound = "";

        private Tool.PokemonBattleEventSystem _eventSystem;

        [Header("배틀 테스트용 인자")]
        [SerializeField] private string _testMyPokemon;
        [SerializeField] private string _testEnemyPokemon;
        [SerializeField] private bool _isTest = false;

        public BattleState _state { get; private set; }

        [HideInInspector] public List<string> _textList = new List<string>();

        public bool _isGameOver { get { return _myPokemon.Health <= 0 || _enemyPokemon.Health <= 0; }}

        public static void AddNextText(string nextText)
        {
            // Instance._uiManager._bottomUI._dialogBox.AddNextPage(nextText);
        }

        public static void ClearText()
        {
            // Instance._uiManager._bottomUI._dialogBox.Clear();
        }

        private void Start()
        {
            _state = BattleState.None;

            if (_uiManager == null)
            {
                _uiManager = FindObjectOfType<PokemonBattleUIManager>();
            }

            if (_itemManager == null)
            {
                _itemManager = FindObjectOfType<PokemonBattleItemManager>();
            }

            _eventSystem = FindObjectOfType<Tool.PokemonBattleEventSystem>();

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

            _textList = new List<string>();

            _uiManager._bottomUI._skill.Init();
            _uiManager._bottomUI._skill.SetPokemon(_myPokemon);
            _uiManager._bottomUI._skill.UpdateText();

            // 대사 나올 곳

            _uiManager._mainUI.UpdatePlayerImage(true);
            _uiManager._mainUI.UpdateEnemyImage(true);

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
            // var waitNextInput = new WaitUntil
                // (() => { return _uiManager._bottomUI._dialogBox._isPageEnded && Input.GetButtonDown("Interact"); });
            var waitUIUpdating = new WaitWhile
                (() => { return _uiManager._mainUI._isPlayerHpAnimating || _uiManager._mainUI._isEnemyHpAnimating; });

            for (int i = 0; i < 2; i++)
            {
                float previousPlayerHealth = _myPokemon.Health;
                float previousEnemyHealth = _enemyPokemon.Health;

                ClearText();

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

                // 대사 1(예정)(일단 전체 출력)
                // _uiManager._bottomUI._dialogBox.Talk("", DialogueStatus.Start);


#if UNITY_EDITOR
                Debug.Log((i == 0 ? "플레이어" : "적") + " 턴 : ");

                for (int j = 0; j < _textList.Count; j++)
                {
                    Debug.Log(_textList[j]);
                }
#endif

                if (_myPokemon.Health != previousPlayerHealth)
                    _uiManager._mainUI.UpdatePlayerHpUI(_myPokemon.Health, _myPokemon.MaxHealth, true);

                if (_enemyPokemon.Health != previousEnemyHealth)
                    _uiManager._mainUI.UpdateEnemyHpUI(_enemyPokemon.Health, _enemyPokemon.MaxHealth, true);

                yield return null;

                yield return waitUIUpdating;

                // 대사 출력 2(예정)

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
            _textList.Clear();

            if (_myPokemon.Health <= 0f) _uiManager._mainUI.UpdatePlayerImage(false);
            if (_enemyPokemon.Health <= 0f) _uiManager._mainUI.UpdateEnemyImage(false);

            if (_myPokemon.Health > 0f)
            {
                AddNextText("신난다! " + _enemyPokemon.Info._name + "과의 과제에서 이겼다!");
                Debug.Log("승리!");
            }
            else
            {
                AddNextText(_myPokemon.Info._name + "가 최후의 오류를 내뿜었다...");
                AddNextText("새내기는 눈앞이 깜깜해졌다!");
                Debug.Log("패배...");
            }

            yield return null;

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
            _textList.Clear();
            AddNextText("안돼! 이번 학기 학점을 이렇게 버릴 수 없어!");

            _uiManager._bottomUI.UpdateDialog();

            // 대화 부분

            // 대화 끝나고...
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