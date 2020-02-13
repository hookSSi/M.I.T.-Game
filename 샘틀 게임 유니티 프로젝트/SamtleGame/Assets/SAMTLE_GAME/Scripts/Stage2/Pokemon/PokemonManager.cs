using System;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    [System.Serializable]
    public class PokemonDictionary
    {
        public string _key;
        public PokemonInfo _info;
    }

    [RequireComponent(typeof(SkillClass))]
    public class PokemonManager : DesignPattern.Singleton<PokemonManager>
    {
        public List<PokemonDictionary> _list;

        public static bool GetPokemonInfo(string pokemonName, out PokemonInfo _outInfo)
        {
            try
            {
                int index = Instance._list.FindIndex((dict) => { return dict._key == pokemonName; });

                _outInfo = Instance._list[index]._info;
                return true;
            }
            catch (ArgumentException e)
            {
                Debug.LogError("오류 : " + e.Message + "(포켓몬을 찾을 수 없습니다 : " + pokemonName + ")");
                _outInfo = null;
                return false;
            }
        }
    }
}