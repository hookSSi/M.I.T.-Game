using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public delegate void BattleDelegate(Pokemon myPokemon, Pokemon enemyPokemon, out string dialog);

    public struct BattleEvent
    {
        public int _priority;
        public BattleDelegate _event;
    }
}
