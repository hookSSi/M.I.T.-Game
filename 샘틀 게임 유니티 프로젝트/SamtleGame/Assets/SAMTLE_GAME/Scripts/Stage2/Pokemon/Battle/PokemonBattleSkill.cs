using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class PokemonBattleSkill : MonoBehaviour
    {
        [SerializeField] private GameObject[] _skills = new GameObject[4];
        [SerializeField] private Text _skillPpText;

        private Pokemon _playerPokemon;
        private Text[] _skillTexts;

        public void SetPokemon(Pokemon playerPokemon)
        {
            if (playerPokemon == null)
                return;

            _playerPokemon = playerPokemon;
        }

        public void UpdateText()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_skills[i])
                {
                    if (_skillTexts[i] == null)
                    {
                        _skillTexts[i] = _skills[i].GetComponentInChildren<Text>();
                    }

                    _skillTexts[i].text = _playerPokemon.Info._skills[i]._name;
                }
            }
        }

        public void UpdateSkill(int indexOfSkill)
        {
            if (_playerPokemon == null)
                return;
            
            if (_skillPpText != null)
                _skillPpText.text = _playerPokemon.Info._skills[indexOfSkill]._count + "/   20";
        }

    }
}