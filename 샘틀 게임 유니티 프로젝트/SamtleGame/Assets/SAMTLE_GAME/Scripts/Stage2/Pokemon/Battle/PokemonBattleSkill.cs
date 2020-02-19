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

        private Text[] _skillTexts;
        private Selectable[] _selectable;
        private Pokemon _playerPokemon;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _skillTexts = new Text[4];
            _selectable = new Selectable[4];

            for (int i = 0; i < _skillTexts.Length; i++)
            {
                if (_skills[i])
                {
                    if (!_skillTexts[i])
                        _skillTexts[i] = _skills[i].GetComponentInChildren<Text>();

                    _selectable[i] = _skills[i].GetComponent<Selectable>();
                }
                
            }
        }

        public void SetPokemon(Pokemon playerPokemon)
        {
            if (playerPokemon == null)
                return;

            _playerPokemon = playerPokemon;
        }

        public void UpdateText()
        {
            for (int i = 0; i < _skillTexts.Length; i++)
            {
                if (_skills[i])
                {
                    // 해당하는 스킬이 존재할 때
                    if (i < _playerPokemon.Info._skills.Length)
                    {
                        _selectable[i].interactable = true;
                        _skillTexts[i].text = _playerPokemon.Info._skills[i]._name;
                    }
                    else
                    {
                        _selectable[i].interactable = false;
                        _skillTexts[i].text = "";
                    }
                }
            }
        }

        public void UpdateSkill(int indexOfSkill)
        {
            if (_playerPokemon == null)
                return;

            if (indexOfSkill >= _playerPokemon.Info._skills.Length)
                return;

            if (_skillPpText != null)
                _skillPpText.text = _playerPokemon.Info._skills[indexOfSkill]._currentCount +
                    "/   " + _playerPokemon.Info._skills[indexOfSkill]._count;
        }

    }
}