using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon
{
    public class PokemonBattleEventSystem : MonoBehaviour
    {
        private EventSystem _eventSystem;

        private PokemonBattleManager _battleManager;
        
        private GameObject _selected;

        [SerializeField] private GameObject _firstActionSelection;
        [SerializeField] private GameObject _firstItemSelection;
        [SerializeField] private GameObject _firstSkillSelection;

        private void Start()
        {
            _eventSystem = EventSystem.current;

            _battleManager = FindObjectOfType<PokemonBattleManager>();
        }

        private void Update()
        {
            // 선택지가 반드시 존재하도록 고정(전투 상태 한정)
            if (_battleManager != null && _battleManager._currentState != BattleState.None)
            {
                if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject != _selected)
                    _selected = _eventSystem.currentSelectedGameObject;
                else if (_selected != null && _eventSystem.currentSelectedGameObject == null)
                    _eventSystem.SetSelectedGameObject(_selected);
            }
        }

        public void InitializeUINavigation(BattleState battleState)
        {
            switch (battleState)
            {
                case BattleState.SelectAction:
                    if (_firstActionSelection)
                        _eventSystem.SetSelectedGameObject(_firstActionSelection);
                    break;
                case BattleState.SelectItem:
                    if (_firstItemSelection)
                        _eventSystem.SetSelectedGameObject(_firstItemSelection);
                    break;
                case BattleState.SelectSkill:
                    _eventSystem.SetSelectedGameObject(_firstSkillSelection);
                    break;
                default:
                    break;
            }
        }
    }
}