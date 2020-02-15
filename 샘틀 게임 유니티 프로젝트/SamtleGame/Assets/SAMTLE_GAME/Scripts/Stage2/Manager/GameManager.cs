using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using MIT.SamtleGame.Stage2.NPC;
using MIT.SamtleGame.DesignPattern;

namespace MIT.SamtleGame.Stage2
{
    public class GameManager : Singleton<GameManager>
    {
        public Dictionary<int, Npc> _npcs = new Dictionary<int, Npc>();

        public void AddNpc(Npc npc)
        {
            if(!_npcs.ContainsKey(npc._id))
            {
                _npcs.Add(npc._id, npc);
            }
        }
    }
}
