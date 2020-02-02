using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class PokemonBattleBottomUI : MonoBehaviour
    {
        [SerializeField] private GameObject _dialog;
        [SerializeField] private GameObject _actionUI;
        [SerializeField] private GameObject _skillUI;

        public PokemonBattleAction ActionUI
        {
            get { return _actionUI.GetComponent<PokemonBattleAction>(); }
        }
        
        private void OnEnable()
        {
            _dialog.SetActive(true);
            _actionUI.SetActive(false);
            _skillUI.SetActive(false);
        }

        public void UpdateActionUI()
        {
            _dialog.SetActive(true);
            _skillUI.SetActive(false);
            _actionUI.SetActive(true);
        }
        
        public void UpdateSkillUI()
        {
            _dialog.SetActive(true);
            _skillUI.SetActive(true);
            _actionUI.SetActive(false);
        }

        // 여기서 대화 업데이트할 수 있게 만들면 좋겠음.
        public void UpdateDialog()
        {
            _dialog.SetActive(true);
            _skillUI.SetActive(false);
            _actionUI.SetActive(false);
        }
    }
}