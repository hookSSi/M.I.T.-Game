using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public delegate void BattleDelegate(Pokemon myPokemon, Pokemon enemyPokemon, out string[] dialog);

    public struct BattleEvent
    {
        public int _priority;
        public BattleDelegate _event;

        public BattleEvent(int priority, BattleDelegate newEvent)
        {
            _priority = priority;
            _event = newEvent;
        }
    }
}
