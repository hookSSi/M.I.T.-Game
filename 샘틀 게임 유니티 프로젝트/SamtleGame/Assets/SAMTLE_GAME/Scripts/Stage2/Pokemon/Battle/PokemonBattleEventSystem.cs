using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using MIT.SamtleGame.Stage2.Pokemon;

namespace MIT.SamtleGame.Stage2.Tool
{
    public class PokemonBattleEventSystem : MonoBehaviour
    {
        private PokemonBattleManager _battleManager;

        private EventSystem _eventSystem;
        
        private GameObject _selected;

        [SerializeField] private GameObject _firstActionSelection;
        [SerializeField] private GameObject _firstItemSelection;
        [SerializeField] private GameObject _firstSkillSelection;

        private void Start()
        {
            _battleManager = PokemonBattleManager.Instance;
            _eventSystem = EventSystem.current;
        }

        private void Update()
        {
            // 선택지 중 하나가 반드시 선택되도록 고정한다
            if (_battleManager != null && _battleManager._state != BattleState.None)
            {
                if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject != _selected)
                    _selected = _eventSystem.currentSelectedGameObject;
                else if (_selected != null && _eventSystem.currentSelectedGameObject == null)
                    _eventSystem.SetSelectedGameObject(_selected);
            }
        }

        // 현재 상태에 따라 UI를 자동으로 탐색할 수 있게 First Select를 설정한다.
        // (전투 메뉴, 기술 메뉴, 가방 메뉴 등에 들어갈 때마다 실행, 처음 선택될 오브젝트를 설정함)
        public void InitializeUINavigation(BattleState currentState)
        {
            switch (currentState)
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

        // Event System의 First Select가 될 오브젝트를 정한다.
        public void SetIndexObject(GameObject firstActionObject = null, GameObject firstItemObject = null, GameObject firstSkillObject = null)
        {
            if (firstActionObject != null)
                _firstActionSelection = firstActionObject;

            if (firstItemObject != null)
                _firstItemSelection = firstItemObject;

            if (firstSkillObject != null)
                _firstSkillSelection = firstSkillObject;
        }
    }
}