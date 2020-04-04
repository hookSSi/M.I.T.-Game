using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;
using MIT.SamtleGame.Stage2.NPC;
using MIT.SamtleGame.DesignPattern;
using MIT.SamtleGame.Attributes;

namespace MIT.SamtleGame.Stage2
{
    public class GameManager : Singleton<GameManager>
    {
        public Dictionary<int, Npc> _npcs = new Dictionary<int, Npc>();
        [Header("스테이지 2 BGM")]
        [GameBgm] public string _stage2Bgm;

        private void Start()
        {
            BgmManager.Instance.Play(_stage2Bgm, true);
        }

        public void AddNpc(Npc npc)
        {
            if(!_npcs.ContainsKey(npc._id))
            {
                _npcs.Add(npc._id, npc);
            }
        }
    }
}
