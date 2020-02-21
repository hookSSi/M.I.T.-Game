using System.Collections;
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
        private AudioSource _audio;

        [SerializeField] private AudioClip _submitClip;

        [SerializeField] private GameObject _firstActionObject;
        [SerializeField] private GameObject _firstItemObject;
        [SerializeField] private GameObject _firstSkillObject;

        private void Start()
        {
            _battleManager = PokemonBattleManager.Instance;
            _eventSystem = EventSystem.current;

            _audio = GetComponent<AudioSource>();
        }

        private void Update()
        {
            // 선택지 중 하나가 반드시 선택되도록 고정한다
            if (_battleManager != null && _battleManager._state != BattleState.None)
            {
                if (_eventSystem.currentSelectedGameObject != null && (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")))
                    _audio.PlayOneShot(_submitClip);

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
                    if (_firstActionObject)
                        _eventSystem.SetSelectedGameObject(_firstActionObject);
                    break;
                case BattleState.SelectItem:
                    _eventSystem.SetSelectedGameObject(_firstItemObject);
                    break;
                case BattleState.SelectSkill:
                    if (_firstSkillObject)
                        _eventSystem.SetSelectedGameObject(_firstSkillObject);
                    break;
            }
        }

        // Event System의 First Select가 될 오브젝트를 정한다.
        public void SetIndexObject(GameObject firstActionObject = null, GameObject firstItemObject = null, GameObject firstSkillObject = null)
        {
            if (firstActionObject != null)
                _firstActionObject = firstActionObject;

            if (firstItemObject != null)
                _firstItemObject = firstItemObject;

            if (firstSkillObject != null)
                _firstSkillObject = firstSkillObject;
        }
    }
}