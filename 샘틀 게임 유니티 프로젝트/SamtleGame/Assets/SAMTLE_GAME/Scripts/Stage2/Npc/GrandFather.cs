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
            /// 플레이어에게 이동
            yield return StartCoroutine(MoveToPlayerRoutine());

            /// 대화
            Talk(false);
            yield return WaitUntillTalkEnd();

            /// 웨이포인트 이동
            yield return StartCoroutine(WayPointsMoveRoutine());

            PlayerControllerEvent.Trigger(true);
            Debug.Log("이벤트 끝");
            yield break;
        }

        protected override IEnumerator WayPointsMoveRoutine()
        {
            /// 플레이어에게 웨이 포인트 전달
            _detectedPlayer.AddWayPoint(_wayPoints);
            _detectedPlayer.WayPointMove();

            var prevWalkSize = _walkSize;
            var prevWalkCount = _walkCount;
            var prevWalkTime = _walkTime;

            _walkTime = 0f;

            foreach(var dest in _wayPoints)
            {
                Direction dir = Maths.Vector2ToDirection(dest.position - transform.position);
                _currentDir = Maths.DirectionToVector2(dir);

                /// 웨이포인트로 이동
                float walkAmount = _walkSize / 2;
                while( Vector2.Distance(transform.position, dest.position) > walkAmount )
                {
                    yield return StartCoroutine(MoveRoutine(_currentDir));
                }
            }

            _walkSize = prevWalkSize;
            _walkCount = prevWalkCount;
            _walkTime = prevWalkTime;

            yield break;
        }
    }
}
