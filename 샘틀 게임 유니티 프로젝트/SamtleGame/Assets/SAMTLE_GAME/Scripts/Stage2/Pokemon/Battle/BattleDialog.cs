using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDialog : MIT.SamtleGame.NPC.Npc
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("토크쇼 시작!");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("묻고 담 페이지로 가!");
        }
    }
}
