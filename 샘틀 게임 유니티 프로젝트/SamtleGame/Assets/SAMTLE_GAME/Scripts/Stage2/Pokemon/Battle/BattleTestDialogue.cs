using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTestDialogue : MIT.SamtleGame.Stage2.NPC.Npc
{
    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Debug.Log("대화 시작");
            Interact();
        }
    }
}
