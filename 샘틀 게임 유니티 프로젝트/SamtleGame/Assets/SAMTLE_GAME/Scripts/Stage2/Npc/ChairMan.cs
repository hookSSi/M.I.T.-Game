using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.NPC
{
    /*
    *   @desc 회장 클래스
    */
    public class ChairMan : EventNpc
    {
        protected override void Update()
        {
            base.Update();

            _animator.SetFloat("Horizontal", _currentDir.x);
            _animator.SetFloat("Vertical", _currentDir.y);
        }

        protected override IEnumerator EventRoutine()
        {
            /// 플레이어에게 이동
            yield return StartCoroutine(MoveToPlayerRoutine());

            Talk(false);
            yield return WaitUntillTalkEnd();

            PokemonBattle();
            yield return WaitUntillBattleEnd();

            yield break;
        }
    }
}
