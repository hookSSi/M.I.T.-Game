using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MIT.SamtleGame.Tools;

using MIT.SamtleGame.Stage2.NPC;
using MIT.SamtleGame.Stage2.Tools;

namespace MIT.SamtleGame.Stage2
{
    public struct PlayerControllerEvent
    {
        public bool _isControllable;
        public Direction _dir;

        public PlayerControllerEvent(bool isControllable, Direction dir = Direction.NONE)
        {
            _isControllable = isControllable;
            _dir = dir;
        }

        static PlayerControllerEvent _event;

        public static void Trigger(bool isControllable, Direction dir = Direction.NONE)
        {
            _event._dir = dir;
            _event._isControllable = isControllable;
            EventManager.TriggerEvent(_event);
        }
    }

    public class PlayerController : MonoBehaviour, EventListener<PlayerControllerEvent>
    {
        [SerializeField]
        protected Vector2 _currentDir = Vector2.down;
        protected int  _currentWalkCount = 0;
        [SerializeField]
        protected bool _isControllable = true;

        [Header("플레이어 정보")]
        public float _walkSize = 1;
        public int _walkCount = 10;
        public float _walkTime = 0.1f;
        public float _jumpPower = 30f;
        public bool _isMoving = false;

        [Header("웨이포인트")]
        public Queue<WayPoint> _wayPoints = new Queue<WayPoint>();

        [Header("충돌되는 태그")]
        public Tag[] _obstacles;
        
        [Header("사운드 설정")]
        public string _walkSound;

        [Header("애니메이션")]
        public Animator _animator;

        [Header("점프 관련")]
        public Transform _test;
        public Transform _test2;

        void Update()
        {
            HandleInput();
        }

        #region  입력처리
        protected virtual void HandleInput()
        {
            if(_isControllable && !_isMoving && Pokemon.PokemonBattleManager.Instance.IsEnd())
            {
                #region 이동 입력 처리
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    _currentDir = Vector2.up;
                    Move();
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    _currentDir = Vector2.left;
                    Move();
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _currentDir = Vector2.down;
                    Move();
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    _currentDir = Vector2.right;
                    Move();
                }

                if(Input.GetKeyDown(KeyCode.E))
                {
                    Interact();
                }

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    //Jump();
                }
                #endregion
            }

            _animator.SetFloat("Horizontal", _currentDir.x);
            _animator.SetFloat("Vertical", _currentDir.y);
        }
        #endregion

        #region  상호작용
        void Interact()
        {
            Vector2 origin = ((Vector2)transform.position);
            Vector2 dest = ((Vector2)transform.position + _currentDir * _walkSize);
            
            RaycastHit2D[] hitted = Physics2D.LinecastAll(origin, dest);

            foreach(var obj in hitted)
            {
                if(obj.collider.tag == "Npc")
                {
                    /// if readyToTalk
                    Npc npc = obj.collider.GetComponent<Npc>();
                    Debug.LogFormat("Npc({0})와 대화 시작", npc.transform.name);
                    npc.SetDirection(_currentDir * -1);
                    npc.Talk();
                }
            }
        }
        #endregion

        #region 이동
        void Move()
        {
            StartCoroutine(MoveRoutine(true));
        }

        IEnumerator MoveRoutine(bool isBlock = false)
        {
            if(isBlock)
            {
                Vector2 origin = ((Vector2)transform.position);
                Vector2 dest = ((Vector2)transform.position + _currentDir * _walkSize);
        
                if(ColliderChecker.CheckColliders(origin, dest, _obstacles))
                {
                    yield break;
                }
            }

            _isMoving = true;
            Vector2 walkAmount = _currentDir * ( _walkSize / _walkCount );

            while(true)
            {
                this.transform.Translate(walkAmount);
                _currentWalkCount++;

                if(_currentWalkCount == _walkCount)
                {
                    _currentWalkCount = 0;
                    _isMoving = false;
                    yield break;
                }

                yield return new WaitForSeconds(_walkTime);
            }
        }

        public void SetDirection(Direction dir)
        {
            _currentDir = Maths.DirectionToVector2(dir);
        }
        #endregion

        #region 웨이포인트 이동
        public void AddWayPoint(List<WayPoint> wayPoints)
        {
            foreach(var wayPoint in wayPoints)
            {
                _wayPoints.Enqueue(wayPoint);
            }
        }

        public void WayPointMove()
        {
            StartCoroutine(WayPointsMoveRoutine());
        }

        public virtual IEnumerator WayPointsMoveRoutine()
        {
            var prevWalkSize = _walkSize;
            var prevWalkCount = _walkCount;
            var prevWalkTime = _walkTime;

            _walkTime = 0f;

            /// 웨이포인트로 이동
            double walkAmount = _walkSize / 2;
            while(_wayPoints.Count > 0 && !_isControllable)
            {
                WayPoint wayPoint = _wayPoints.Peek();
                Transform wayPointTransform = wayPoint.transform;

                Direction dir = Maths.Vector2ToDirection(wayPointTransform.position - transform.position);
                _currentDir = Maths.DirectionToVector2(dir);
                
                while( Vector2.Distance(transform.position, wayPointTransform.position) > walkAmount && !_isControllable)
                {
                    yield return StartCoroutine(MoveRoutine(false));
                }
                
                _currentDir = Maths.DirectionToVector2(wayPoint._dir);
                yield return wayPoint.Trigger(this);
                _wayPoints.Dequeue();
            }

            _walkSize = prevWalkSize;
            _walkCount = prevWalkCount;
            _walkTime = prevWalkTime;

            _wayPoints.Clear();
            yield break;
        }
        #endregion


        #region 점프
        void Jump()
        {
            StartCoroutine(JumpRoutine());
        }

        IEnumerator JumpRoutine()
        {
            yield return Tweens.MoveTransform(this, this.transform, this.transform, _test, new WaitForSeconds(0.1f), 0.1f, 1f, Tweens.TweenCurve.LinearTween);
            yield return Tweens.MoveTransform(this, this.transform, this.transform, _test2, new WaitForSeconds(0.1f), 0.1f, 1f, Tweens.TweenCurve.LinearTween);
        }
        #endregion
        
        #region 이벤트 처리
        public virtual void OnEvent(PlayerControllerEvent playerControllerEvent)
        {
            Debug.LogFormat("플레이어 컨트롤 여부: {0}", playerControllerEvent._isControllable);
            _isControllable = playerControllerEvent._isControllable;

            switch(playerControllerEvent._dir)
            {
                case Direction.UP:
                    _currentDir = Vector2.up;
                    break;
                case Direction.DOWN:
                    _currentDir = Vector2.down;
                    break;
                case Direction.RIGHT:
                    _currentDir = Vector2.right;
                    break;
                case Direction.LEFT:
                    _currentDir = Vector2.left;
                    break;
                case Direction.NONE:
                    break;
            }
        }

        private void OnEnable() 
        {
            this.EventStartListening<PlayerControllerEvent>();
        }

        private void OnDisable() 
        {
            this.EventStopListening<PlayerControllerEvent>();
        }
        #endregion
        private void OnDrawGizmosSelected() 
        {
            Vector2 point = ((Vector2)transform.position + _currentDir * _walkSize);
            Gizmos.DrawCube(point, Vector2.one);
        }
    }
}