using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MIT.SamtleGame.Stage2.NPC
{
    /*
    *   @desc 할배 NPC 클래스
    */
    [RequireComponent(typeof(Animator))]
    public class GrandFather : EventNpc
    {
        protected override void Update() 
        {
            base.Update();

            _animator.SetFloat("Horizontal", _currentDir.x);
            _animator.SetFloat("Vertical", _currentDir.y);
        }

        private void StartMove()
        {
            StartCoroutine("ActionRoutine");
        }
    }
}
