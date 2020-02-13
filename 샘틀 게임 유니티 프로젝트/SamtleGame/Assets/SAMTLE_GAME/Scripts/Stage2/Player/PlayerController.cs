﻿using System.Collections;
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
        protected bool _isMoving = false;

        [Header("플레이어 정보")]
        public float _walkSize = 1;
        public int _walkCount = 10;
        public float _walkTime = 0.1f;
        public float _jumpPower = 30f;

        [Header("웨이포인트")]
        public Queue<Transform> _wayPoints = new Queue<Transform>();

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
            if(_isControllable && !_isMoving)
            {
                #region 이동 입력 처리
                if (Input.GetKey(KeyCode.W))
                {
                    _currentDir = Vector2.up;
                    Move();
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    _currentDir = Vector2.left;
                    Move();
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    _currentDir = Vector2.down;
                    Move();
                }
                else if (Input.GetKey(KeyCode.D))
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
                    Npc npc = obj.collider.GetComponent<Npc>();
                    Debug.LogFormat("Npc({0})와 대화 시작", npc._id);
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
        #endregion

        #region 웨이포인트 이동
        public void AddWayPoint(List<Transform> wayPoints)
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

            while(_wayPoints.Count > 0 && !_isControllable)
            {
                Transform wayPoint = _wayPoints.Peek();

                Direction dir = Maths.Vector2ToDirection(wayPoint.position - transform.position);
                _currentDir = Maths.DirectionToVector2(dir);
                
                /// 웨이포인트로 이동
                float walkAmount = _walkSize / 2;
                while( Vector2.Distance(transform.position, wayPoint.position) > walkAmount && !_isControllable)
                {
                    yield return StartCoroutine(MoveRoutine(false));
                }

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
                case Direction.DONW:
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