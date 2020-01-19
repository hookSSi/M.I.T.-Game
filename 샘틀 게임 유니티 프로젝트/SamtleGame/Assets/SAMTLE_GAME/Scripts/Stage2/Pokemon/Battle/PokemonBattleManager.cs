using UnityEngine;
using UnityEngine.UI;


namespace Pokemon
{
    public enum BattleState
    {
        None,
        Start,
        Introduction,
        SelectAction, SelectSkill, SelectItem,
        Act,
        AfterAct,
        End
    }

    public class PokemonBattleManager : MonoBehaviour
    {
        private PokemonBattleEventSystem _eventSystem;

        private Pokemon _myPokemon;
        private Pokemon _enemyPokemon;

        private PokemonBattleUIManager _uiManager;

        [SerializeField]
        private BattleState _state;

        public BattleState _currentState
        {
            get { return _state; }
        }

        /*
        public bool _isGameOver
        {
            get { return _myPokemon.hp <= 0 || _enemyPokemon.hp <= 0; }
        }
        */

        private void Start()
        {
            if (_uiManager == null)
            {
                _uiManager = FindObjectOfType<PokemonBattleUIManager>();
            }

            _eventSystem = FindObjectOfType<PokemonBattleEventSystem>();

            _state = BattleState.None;

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

            SelectAction();
        }

        public void SelectAction()
        {
            _state = BattleState.SelectAction;

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

            _uiManager._bottomUI.UpdateDialog();

            _eventSystem.InitializeUINavigation(BattleState.SelectItem);
        }

        public void UseSkill(int indexOfSkill)
        {
            _state = BattleState.Act;

            // Debug.Log(indexOfSkill);

            _myPokemon?.UseSkill(indexOfSkill);

            _uiManager._bottomUI.UpdateDialog();
        }

        private void UseItem()
        {
            _state = BattleState.Act;

            Debug.Log("아이템 사용!");
            _uiManager._bottomUI.UpdateDialog();
        }

        // 포켓몬 고르기(정보)
        public void SelectPokemonInformation()
        {
            Debug.Log("포켓몬 정보 보기...");

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
    }
}