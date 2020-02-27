using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class PokemonBattleBottomUI : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogUI;
        [SerializeField] private GameObject _actionUI;
        [SerializeField] private GameObject _skillUI;

        public PokemonBattleSkill _skill { get; private set; }

        public PokemonBattleDialogueBox _dialogBox;

        private void Awake()
        {
            _skill = _skillUI.GetComponent<PokemonBattleSkill>();
        }

        private void OnEnable()
        {
            UpdateDialog();
        }

        public void UpdateActionUI()
        {
            _dialogUI.SetActive(true);
            _skillUI.SetActive(false);
            _actionUI.SetActive(true);
        }
        
        public void UpdateSkillUI()
        {
            _dialogUI.SetActive(true);
            _skillUI.SetActive(true);
            _actionUI.SetActive(false);
        }

        // 여기서 대화 업데이트할 수 있게 만들면 좋겠음.
        public void UpdateDialog()
        {
            _dialogUI.SetActive(true);
            _skillUI.SetActive(false);
            _actionUI.SetActive(false);
        }

        public void UpdateInformation()
        {
            _dialogUI.SetActive(false);
            _skillUI.SetActive(false);
            _actionUI.SetActive(false);
        }
    }
}