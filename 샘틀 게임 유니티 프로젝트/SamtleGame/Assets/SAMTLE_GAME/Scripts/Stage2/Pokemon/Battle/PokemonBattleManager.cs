using MIT.SamtleGame.DesignPattern;
using System.Collections;
using System.Reflection; // 테스트용
using UnityEngine;
using UnityEngine.Events;
using MIT.SamtleGame.Attributes;


namespace MIT.SamtleGame.Stage2.Pokemon
{
    public enum BattleState
    {
        None, Start, Introduction,
        SelectAction, SelectSkill, SelectItem, Information,
        Act, End
    }

    public class PokemonBattleManager : Singleton<PokemonBattleManager>
    {
        public PokemonBattleUIManager _uiManager;
        public PokemonBattleItemManager _itemManager;
        public BattleDialogueController _dialogueController;
        public Pokemon _myPokemon;
        public Pokemon _enemyPokemon;
        [GameAudio] public string _hitSound = "Hit";
        [GameAudio] public string _commitSound = "BattleCommit";
        [GameAudio] public string _appearSound = "RetroVideoGameFx";
        [GameBgm] public string _battleTrack;
        [GameBgm] public string _victoryTrack;

        private Tool.PokemonBattleEventSystem _eventSystem;
        private WaitWhile _waitDialogueUpdating;
        private WaitWhile _waitInputing;
        private WaitWhile _waitUIUpdating;
        private string _prevTrack;

        [Header("배틀 테스트용 인자")]
        [SerializeField] private bool _isTest = false;
        [SerializeField] private string _testMyPokemon;
        [SerializeField] private string _testEnemyPokemon;

        public BattleState _state { get; private set; }

        public bool IsEnd() { return _state == BattleState.None; }
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
        }

        private void Update()
        {
            if (IsEnd() == false && (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")))
            {
                SoundEvent.Trigger(_commitSound, SoundStatus.Stop);
                SoundEvent.Trigger(_commitSound);
            }

#if UNITY_EDITOR
            if (_isTest)
            {
                _isTest = false;
                StartBattle(_testMyPokemon, _testEnemyPokemon);
            }
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

            _uiManager.SetActive(false);

            _uiManager._mainUI.Init();
            _uiManager._mainUI.UpdatePlayerImage(false);
            _uiManager._mainUI.UpdateEnemyImage(false);
            _uiManager._mainUI.UpdateValue(_myPokemon, _enemyPokemon, 10, 10, 0f, 100f);
            _uiManager._bottomUI._skill.Init();
            _uiManager._bottomUI._skill.SetPokemon(_myPokemon);
            _uiManager._bottomUI._skill.UpdateText();

            _uiManager._skillClass.Init();

            _prevTrack = BgmManager.Instance.CurrentTrack;

            BgmManager.Instance.Pause();
            BgmManager.Instance.Play(_battleTrack, true);

            StartCoroutine("StartBattleCoroutine");
        }

        private IEnumerator StartBattleCoroutine()
        {
            // 대사
            _dialogueController.ClearPages();
            _dialogueController.AddNextPage("야생의 " + _enemyPokemon.Info._name + "이(가) 나타났다!");
            if (_myPokemon.Info._name != "신입생")
                _dialogueController.AddNextPage("가랏! " + _myPokemon.Info._name + "!", true);
            else
            {
                _dialogueController.AddNextPage("가랏! 신입생!");
                _dialogueController.AddNextPage("신입! 신입!", true);
            }

            StartCoroutine(_uiManager._mainUI.FadeInBattle(1.5f));
            yield return new WaitForSeconds(1.5f);
            _uiManager.SetActive(true);

            _uiManager._mainUI.UpdateEnemyImage(true);
            _dialogueController.NextDialogue();
            yield return _waitDialogueUpdating;

            // 새내기 이동
            _uiManager._mainUI.MoveIpsangSide();
            yield return new WaitWhile(() => _uiManager._mainUI._isIpsangMoving);

            // 소환 이펙트
            _uiManager._mainUI.UpdatePlayerImage(true, true);
            SoundEvent.Trigger(_hitSound, SoundStatus.Stop);
            SoundEvent.Trigger(_appearSound);
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

            StartCoroutine(ActPhase(playerSkill._event, NextEnemySkill()._event));
        }

        public void UseItem(BattleEvent battleEvent)
        {
            _state = BattleState.Act;

            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Battle);
            _uiManager._bottomUI.UpdateDialog();

            StartCoroutine(ActPhase(battleEvent, NextEnemySkill()._event));
        }

        IEnumerator ActPhase(BattleEvent playerEvent, BattleEvent enemyEvent)
        {
            for (int i = 0; i < 2; i++)
            {
                bool isFriendlyTurn = i == 0;
                float previousPlayerHealth = _myPokemon.Health;
                float previousEnemyHealth = _enemyPokemon.Health;

                _dialogueController.ClearPages();

                if (isFriendlyTurn) // 아군의 턴
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
                
                _uiManager._effect.StartAnim(i);
                yield return new WaitUntil(() => _uiManager._effect._isAnimating == false);

                _uiManager._effect.ResetAnim();

                if (_myPokemon.Health != previousPlayerHealth)
                {
                    if (_myPokemon.Health < previousPlayerHealth)
                    {
                        _uiManager._mainUI.HitPlayer();
                        StartCoroutine("PlayHitSound");
                    }
                    _uiManager._mainUI.UpdatePlayerHpUI(_myPokemon.Health, _myPokemon.MaxHealth, true);
                }

                if (_enemyPokemon.Health != previousEnemyHealth)
                {
                    if (_enemyPokemon.Health < previousEnemyHealth)
                    {
                        _uiManager._mainUI.HitEnemy();
                        StartCoroutine("PlayHitSound");
                    }
                    _uiManager._mainUI.UpdateEnemyHpUI(_enemyPokemon.Health, _enemyPokemon.MaxHealth, true);
                }

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

            BgmManager.Instance.Pause();
            BgmManager.Instance.Play(_victoryTrack);

            if (_myPokemon.Health <= 0f) _uiManager._mainUI.UpdatePlayerImage(false, true);
            if (_enemyPokemon.Health <= 0f) _uiManager._mainUI.UpdateEnemyImage(false, true);

            if (_myPokemon.Health > 0f)
            {
                if (_enemyPokemon.Info._name != "민지의 고양이")
                    _dialogueController.AddNextPage("신난다! " + _enemyPokemon.Info._name + "과의 과제에서 이겼다!", true);
                else
                    _dialogueController.AddNextPage("신난다! " + _enemyPokemon.Info._name + "에게서 승리했다!", true);
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

            StartCoroutine(_uiManager._mainUI.FadeInImage(_uiManager._mainUI._blackBox, 0.5f));

            yield return new WaitForSeconds(0.5f);

            PlayerControllerEvent.Trigger(true);
            _uiManager.SetActive(false);

            StartCoroutine(_uiManager._mainUI.FadeOutImage(_uiManager._mainUI._blackBox, 0.5f));
            yield return new WaitForSeconds(0.5f);

            BgmManager.Instance.Pause();
            if (_prevTrack != "(None)")
                BgmManager.Instance.Play(_prevTrack, false);

            _state = BattleState.None;
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
        public void Escape() => StartCoroutine("EscapeCoroutine");

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
            int enemySkillLength = _enemyPokemon.Info._skills.Count;

            Skill enemySkill = _enemyPokemon.UseSkill(Random.Range(0, enemySkillLength));

            if (enemySkill._currentCount <= 0)
                enemySkill = PokemonManager.DefaultSkill();

            enemySkill._currentCount--;

            return enemySkill;
        }

        private IEnumerator PlayHitSound()
        {
            SoundEvent.Trigger(_commitSound, SoundStatus.Stop);
            SoundEvent.Trigger(_hitSound, SoundStatus.Play, false, 0.7f);
            yield return new WaitForSeconds(0.3f);

            SoundEvent.Trigger(_hitSound, SoundStatus.Stop);
            SoundEvent.Trigger(_hitSound, SoundStatus.Play, false, 0.7f);
        }
    }
}