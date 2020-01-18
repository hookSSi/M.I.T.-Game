using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class PokemonBattleSkill : MonoBehaviour
    {
        [SerializeField] private Pokemon _playerPokemon;
        [SerializeField] private GameObject[] _skills;
        private Text[] _skillTexts;

        private void Awake()
        {

        }

        // 미완성된 함수 : PokemonInfo에 접근할 수 없음
        public void SetPokemon(Pokemon playerPokemon)
        {
            _playerPokemon = playerPokemon;

            if (_skillTexts.Length == 0)
            {
                _skillTexts = new Text[4];
            }

            for (int i = 0; i < 4; i++)
            {
                if (_skills[i])
                {
                    if (_skillTexts[i] == null)
                    {
                        _skillTexts[i] = _skills[i].GetComponentInChildren<Text>();
                    }

                    // info를 통해 텍스트를 변경해야 하는데 private라 접근할 수 없다...
                    // 성후 선배가 포켓몬의 PokemonInfo의 접근할 수 있는 프로퍼티를 추가해주셔야 할듯.
                    // _skillTexts[i].text = playerPokemon._info;
                }
            }
        }
    }
}