using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class PokemonBattleUIManager : MonoBehaviour
    {
        public PokemonBattleMainUIManager _mainUI;
        public PokemonBattleBottomUIManager _bottomUI;

        private void Awake()
        {
            if (!_mainUI)
            {
                _mainUI = GetComponent<PokemonBattleMainUIManager>();
            }

            if (!_bottomUI)
            {
                _bottomUI = GetComponent<PokemonBattleBottomUIManager>();
            }
        }
    }
}