using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class PokemonBattleAction : MonoBehaviour
    {
        private Pokemon _playerPokemon;

        [SerializeField] private GameObject[] _actions = new GameObject[4];
        [SerializeField] private string[] _actionStrings = new string[4];

        private Text[] _actionTexts = new Text[4];

        private void Update()
        {
            Initialize();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateText();
        }
#endif

        private void Initialize()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_actions[i] != null && _actionTexts[i] == null)
                {
                    _actionTexts[i] = _actions[i].GetComponent<Text>();
                }
            }
        }

        public void SetPokemon(Pokemon playerPokemon)
        {
            _playerPokemon = playerPokemon;
        }

        public void UpdateText()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_actions[i] != null && _actionTexts[i] != null)
                {
                    _actionTexts[i].text = _actionStrings[i];
                }
            }
        }
    }
}