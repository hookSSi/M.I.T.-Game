using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
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

        private void SetPokemon(Pokemon playerPokemon, Pokemon enemyPokemon)
        {
            // _mainUI.SetPokemon(playerPokemon, enemyPokemon);
            // _bottomUI.SetPokemon(playerPokemon, enemyPokemon);
        }
    }
}