using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MIT.SamtleGame.DesignPattern;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class PokemonBattleUIManager : MonoBehaviour
    {
        public PokemonBattleMainUI _mainUI;
        public PokemonBattleBottomUI _bottomUI;
        public PokemonBattleEffect _effect;
        public SkillClass _skillClass;

        private void Awake()
        {
            if (!_mainUI)
            {
                _mainUI = GetComponent<PokemonBattleMainUI>();
            }

            if (!_bottomUI)
            {
                _bottomUI = GetComponent<PokemonBattleBottomUI>();
            }
        }

        public void SetActive(bool boolean)
        {
            if (boolean)
            {
                _mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.Battle);
                _bottomUI.UpdateDialog();
            }
            else
            {
                _mainUI.UpdateMainUI(PokemonBattleMainUI.UIState.None);
                _bottomUI.UpdateNone();
            }
        }

        private void OnEnable()
        {
            _mainUI?.gameObject.SetActive(true);
            _bottomUI?.gameObject.SetActive(true);
        }
    }
}