using MIT.SamtleGame.DesignPattern;
using System.Collections;
using UnityEngine;


namespace MIT.SamtleGame.Stage2.Pokemon
{
    public enum BattleState
    {
        None, Start, Introduction,
        SelectAction, SelectSkill, SelectItem,
        Act, End
    }
    public class PokemonBattleManager : Singleton<PokemonBattleManager>
    {
        private Tool.PokemonBattleEventSystem _eventSystem;

        public PokemonBattleUIManager _uiManager;
        public PokemonBattleItemManager _itemManager;

        public Pokemon _myPokemon { get; private set; }
        public Pokemon _enemyPokemon { get; private set; }

        public BattleState _state { get; private set; }

        public bool _isGameOver { get { return _myPokemon.Info._health <= 0 || _enemyPokemon.Info._health <= 0; }}

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

            // 전투 시작(테스트용)
            StartBattle(null, null);
        }

        public void StartBattle(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            _myPokemon = myPokemon;
            _enemyPokemon = enemyPokemon;

            _state = BattleState.Start;

            Debug.Log("배틀 시작!");

            _uiManager.gameObject.SetActive(true);
            _uiManager._bottomUI._skill.SetPokemon(myPokemon);

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
            Debug.Log("스킬 선택하기...");

            _uiManager._bottomUI.UpdateSkillUI();

            _eventSystem.InitializeUINavigation(BattleState.SelectSkill);
        }

        public void SelectItem()
        {
            _state = BattleState.SelectItem;

            // UI Update
            Debug.Log("아이템 선택하기...");

            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Bag);
            _uiManager._bottomUI.UpdateDialog();
            
            // 처음 탐색할 아이템을 할당하는 PokemonBattleItemManager.Initialize가 필요함
            _eventSystem.InitializeUINavigation(BattleState.SelectItem);
        }

        public void UseSkill(int indexOfSkill)
        {
            _state = BattleState.Act;

            Skill playerSkill = _myPokemon?.UseSkill(indexOfSkill);

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
            // 우선순위(priority)에 따른 선공 결정
            BattleDelegate[] Actions = new BattleDelegate[2];

            if (playerEvent._priority <= enemyEvent._priority)
            {
                Actions[0] = playerEvent._event;
                Actions[1] = enemyEvent._event;
            }
            else
            {
                Actions[0] = enemyEvent._event;
                Actions[1] = playerEvent._event;
            }

            string[] nextScript;

            foreach (var Action in Actions)
            {
                float previousPlayerHealth = _myPokemon.Info._health;
                float previousEnemyHealth = _enemyPokemon.Info._health;

                Action(_myPokemon, _enemyPokemon, out nextScript);

                // nextScript를 이용한 대사 출력 1(예정)

                if (_myPokemon.Info._health != previousPlayerHealth)
                    _uiManager._mainUI.UpdatePlayerHpUI(_myPokemon.Info._health, 100f, true);

                if (_enemyPokemon.Info._health != previousEnemyHealth)
                    _uiManager._mainUI.UpdateEnemyHpUI(_enemyPokemon.Info._health, 100f, true);

                yield return new WaitForSeconds(0.1f);

                System.Func<bool> predicate =
                    () => { return _uiManager._mainUI._isPlayerHpAnimating || _uiManager._mainUI._isEnemyHpAnimating; };

                yield return new WaitWhile(predicate);

                // nextScript를 이용한 대사 출력 2(예정)

                if (_isGameOver)
                    break;
            }

            // 게임 종료 체크
            if (_isGameOver)
            {
                if (_myPokemon.Info._health <= 0f)
                {
                    Debug.Log("게임오버... 패배...");
                }
                else
                {
                    Debug.Log("전투 승리!");
                }
            }
            else
            {
                SelectAction();
            }
        }


        // 포켓몬 고르기(정보)
        public void SelectPokemonInformation()
        {
            Debug.Log("포켓몬 정보 보기...");

            _uiManager._mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Information);
            _uiManager._bottomUI.UpdateDialog();
        }

        // 도주(아무 기능 없음)
        public void Escape()
        {
            Debug.Log("[도망치기]안돼! 이번 학기 학점을 이렇게 버릴 수 없어!");

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
            return _enemyPokemon.UseSkill(Random.Range(0, enemySkillLength));
        }
    }
}