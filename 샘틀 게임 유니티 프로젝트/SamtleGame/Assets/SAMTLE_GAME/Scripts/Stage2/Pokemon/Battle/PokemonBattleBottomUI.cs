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

        public void UpdateDialog()
        {
            _dialogUI.SetActive(true);
            _skillUI.SetActive(false);
            _actionUI.SetActive(false);
        }

        public void UpdateInformation() => UpdateNone();

        public void UpdateNone()
        {
            _dialogUI.SetActive(false);
            _skillUI.SetActive(false);
            _actionUI.SetActive(false);
        }
    }
}