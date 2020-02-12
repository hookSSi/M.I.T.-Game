using System.Collections;
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
        public List<PokemonDictionary> _pokemonList;
    }
}