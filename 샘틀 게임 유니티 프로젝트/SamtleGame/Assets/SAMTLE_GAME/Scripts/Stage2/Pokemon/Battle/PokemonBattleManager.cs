using UnityEngine;
using UnityEngine.UI;


namespace Pokemon
{
    public enum BattleState
    {
        Start,
        Introduction,
        SelectAction, SelectSkill, SelectItem,
        Act,
        AfterAct,
        End
    }

    public class PokemonBattleManager : MonoBehaviour
    {

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

        public void Start()
        {
            if (_uiManager == null)
            {
                _uiManager = FindObjectOfType<PokemonBattleUIManager>();
            }
        }

        public void StartBattle(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            _myPokemon = myPokemon;
            _enemyPokemon = enemyPokemon;

            _state = BattleState.Start;

            Debug.Log("배틀 시작!");
            SelectAction();
        }

        private void SelectAction()
        {
            _state = BattleState.SelectAction;
        }

        public void SelectSkill()
        {
            _state = BattleState.SelectSkill;

            // UI Update
            Debug.Log("스킬 선택하기...");
        }

        public void SelectItem()
        {
            _state = BattleState.SelectItem;

            // UI Update
            Debug.Log("아이템 선택하기...");
        }

        public void UseSkill(int indexOfSkill)
        {
            _state = BattleState.Act;
            _myPokemon.UseSkill(indexOfSkill);
        }

        private void UseItem()
        {
            _state = BattleState.Act;
            Debug.Log("아이템 사용!");
        }

        // 포켓몬 고르기(정보)
        public void SelectPokemonInformation()
        {
            Debug.Log("포켓몬 정보 보기...");
        }

        // 도주(아무 기능 없음)
        public void Escape()
        {
            Debug.Log("[도망치기]안돼! 이번 학기 학점을 이렇게 버릴 수 없어!");
        }
    }
}