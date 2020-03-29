using System;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    [System.Serializable]
    [RequireComponent(typeof(SkillClass))]
    public class PokemonData
    {
        public PokemonInfo _info;
        [HideInInspector]
        public bool _isFoldOut = true;
    }

    [RequireComponent(typeof(SkillClass))]
    public class PokemonManager : DesignPattern.Singleton<PokemonManager>
    {
        [HideInInspector]
        public List<PokemonData> _pokemonList;

        public static bool GetPokemonInfo(string pokemonName, out PokemonInfo _outInfo)
        {
            try
            {
                int index = Instance._pokemonList.FindIndex((dict) => { return dict._info._name == pokemonName; });
                _outInfo = Instance._pokemonList[index]._info;
                return true;
            }
            catch (ArgumentException e)
            {
                Debug.LogError("오류 : " + e.Message + "(포켓몬을 찾을 수 없습니다 : " + pokemonName + ")");
                _outInfo = null;
                return false;
            }
        }

        public static Skill DefaultSkill()
        {
            Skill defaultSkill = new Skill
            {
                _name = "뇌정지",
                _count = 999,
                _event = Instance.GetComponent<SkillClass>().StopThinking
        };

            return defaultSkill;
        }
    }
}