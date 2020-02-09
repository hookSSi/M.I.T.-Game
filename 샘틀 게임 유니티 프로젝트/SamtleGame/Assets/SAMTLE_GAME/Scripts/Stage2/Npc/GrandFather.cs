using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2.Tools;
using MIT.SamtleGame.Tools;

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

        protected override IEnumerator EventRoutine()
        {
            PlayerControllerEvent.Trigger(false, Action.VectorToDir(-_currentDir));
            
            /// 느낌표!
            yield return Tweens.MoveTransform(this, _reactMark.transform, _reactMark.transform, _reactMarkDest, new WaitForSeconds(0.1f), 0.1f, 1f, Tweens.TweenCurve.EaseInOutBounce);
            _reactMark.SetActive(false);

            yield return StartCoroutine(MoveToPlayerRoutine());
            yield return StartCoroutine(WayPointsRoutine());

            PlayerControllerEvent.Trigger(true);
            Debug.Log("이벤트 끝");
            yield break;
        }

        private void StartMove()
        {
            StartCoroutine("ActionRoutine");
        }
    }
}
