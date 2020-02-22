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

        private void OnEnable()
        {
            _mainUI?.gameObject.SetActive(true);
            _bottomUI?.gameObject.SetActive(true);
        }
    }
}