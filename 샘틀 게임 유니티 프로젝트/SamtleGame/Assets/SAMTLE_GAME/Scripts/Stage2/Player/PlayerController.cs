using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MIT.SamtleGame.Tools;

using MIT.SamtleGame.Stage2.NPC;
using MIT.SamtleGame.Stage2.Tool;

namespace MIT.SamtleGame.Stage2
{
    public struct PlayerControllerEvent
    {
        public bool _isControllable;

        public PlayerControllerEvent(bool isControllable)
        {
            _isControllable = isControllable;
        }

        static PlayerControllerEvent _event;

        public static void Trigger(bool isControllable)
        {
            _event._isControllable = isControllable;
            EventManager.TriggerEvent(_event);
        }
    }

    public class PlayerController : MonoBehaviour, EventListener<PlayerControllerEvent>
    {
        [SerializeField]
        protected Vector2 _currentDir = Vector2.down;
        protected int  _currentWalkCount = 0;
        protected bool _isControllable = true;

        [Header("플레이어 정보")]
        public float _walkSize = 1;
        public int _walkCount = 10;
        public float _walkTime = 0.1f;
        public float _speed = 1;
        //public Vector2 _colSize;
        public float _jumpPower = 30f;

        [Header("충돌되는 태그")]
        public Tag[] _obstacles;
        
        [Header("사운드 설정")]
        public string _walkSound;

        [Header("애니메이션")]
        public Animator _animator;

        public Transform _test;
        public Transform _test2;

        void Update()
        {
            HandleInput();
        }

        /// 입력처리
        protected virtual void HandleInput()
        {
            if(_isControllable)
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

        /// 상호작용
        void Interact()
        {
            Vector2 origin = ((Vector2)transform.position);
            Vector2 dest = ((Vector2)transform.position + _currentDir * _walkSize);
            
            Collider2D[] colliders = Physics2D.OverlapBoxAll(origin, dest, 0);

            foreach(var col in colliders)
            {
                if(col.tag == "Npc")
                {
                    Npc npc = col.GetComponent<Npc>();
                    Debug.LogFormat("Npc({0})와 대화 시작", npc._id);
                    npc.SetDirection(_currentDir * -1);
                    npc.Talk();
                }
            }
        }

        #region 이동
        void Move()
        {
            Vector2 origin = ((Vector2)transform.position);
            Vector2 dest = ((Vector2)transform.position + _currentDir * _walkSize);

            if(!ColliderChecker.CheckColliders(origin, dest, _obstacles))
            {
                StartCoroutine(MoveRoutine());
            }
        }

        IEnumerator MoveRoutine()
        {
            _isControllable = false;
            Vector2 walkAmount = _currentDir * ( _walkSize / _walkCount ) * _speed;

            while(true)
            {
                this.transform.Translate(walkAmount);
                _currentWalkCount++;

                if(_currentWalkCount == _walkCount)
                {
                    _currentWalkCount = 0;
                    _isControllable = true;
                    yield break;
                }

                yield return new WaitForSeconds(_walkTime);
            }
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

        private void OnDrawGizmosSelected() 
        {
            Vector2 point = ((Vector2)transform.position + _currentDir * _walkSize);
            Gizmos.DrawCube(point, Vector2.one);
        }

        public virtual void OnEvent(PlayerControllerEvent playerControllerEvent)
        {
            Debug.LogFormat("플레이어 컨트롤 여부: {0}", playerControllerEvent._isControllable);
            _isControllable = playerControllerEvent._isControllable;
        }

        private void OnEnable() 
        {
            this.EventStartListening<PlayerControllerEvent>();
        }

        private void OnDisable() 
        {
            this.EventStopListening<PlayerControllerEvent>();
        }
    }
}