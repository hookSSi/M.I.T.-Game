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
        [Header("플레이어를 위한 웨이포인트"), Space(20)]
        public List<WayPoint> _wayPointForPlayer;
        public GameObject _wayPointForPlayerPrefab;
        public Transform _wayPointForPlayerStorage;

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
            _detectedPlayer.AddWayPoint(_wayPointForPlayer);
            _detectedPlayer.WayPointMove();

            var prevWalkSize = _walkSize;
            var prevWalkCount = _walkCount;
            var prevWalkTime = _walkTime;

            _walkTime = 0f;

            foreach(var wayPoint in _wayPoints)
            {

                Direction dir = Maths.Vector2ToDirection(wayPoint.transform.position - transform.position);
                _currentDir = Maths.DirectionToVector2(dir);

                /// 웨이포인트로 이동
                float walkAmount = _walkSize / 2;
                while( Vector2.Distance(transform.position, wayPoint.transform.position) > walkAmount )
                {
                    yield return StartCoroutine(MoveRoutine(_currentDir));
                }
                
                _currentDir = Maths.DirectionToVector2(wayPoint._dir);
                yield return wayPoint.Trigger(this);
            }

            _walkSize = prevWalkSize;
            _walkCount = prevWalkCount;
            _walkTime = prevWalkTime;

            yield break;
        }

        #region  스크립트 사용 편의성
        public void GenerateWayPointForPlayer()
        {
            for(var i = _wayPointForPlayer.Count - 1; i > -1; i--)
            {
                if (_wayPointForPlayer[i] == null)
                    _wayPointForPlayer.RemoveAt(i);
                else
                    _wayPointForPlayer[i].name = string.Format("WayPoint - {0}", i + 1);
            }

            var newWayPoint = Instantiate(_wayPointForPlayerPrefab);
            newWayPoint.transform.SetParent(_wayPointForPlayerStorage);
            newWayPoint.transform.localPosition = _wayPointForPlayer[_wayPointForPlayer.Count - 1].transform.localPosition;
            newWayPoint.transform.localRotation = Quaternion.identity;
            _wayPointForPlayer.Add(newWayPoint.GetComponent<WayPoint>());

            newWayPoint.name = string.Format("WayPoint - {0}", _wayPointForPlayer.Count);
        }
        #endregion
    }
}
