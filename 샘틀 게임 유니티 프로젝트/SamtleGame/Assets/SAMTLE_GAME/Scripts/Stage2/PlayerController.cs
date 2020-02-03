using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MIT.SamtleGame.Stage2.NPC;

namespace MIT.SamtleGame.Stage2
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _currentDir = Vector2.down;
        private int  _currentWalkCount = 0;
        private bool _isControllable = true;

        [Header("플레이어 이동 정보")]
        public float _walkSize = 1;
        public int _walkCount = 10;
        public float _walkTime = 0.1f;
        public float _speed = 1;
        public Vector2 _colSize;
        
        [Header("사운드 설정")]
        public string _walkSound;

        [Header("애니메이션")]
        public Animator _animator;

        void Update()
        {
            HandleInput();
        }

        /// 입력처리
        void HandleInput()
        {
            if(_isControllable)
            {
                #region 이동 입력 처리
                if (Input.GetKey(KeyCode.W))
                {
                    _currentDir = Vector2.up;
                    Move(_currentDir);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    _currentDir = Vector2.left;
                    Move(_currentDir);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    _currentDir = Vector2.down;
                    Move(_currentDir);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    _currentDir = Vector2.right;
                    Move(_currentDir);
                }

                if(Input.GetKeyDown(KeyCode.E))
                {
                    Interact();
                }
                #endregion
            }

            _animator.SetFloat("Horizontal", _currentDir.x);
            _animator.SetFloat("Vertical", _currentDir.y);
        }

        /// 상호작용
        void Interact()
        {
            Vector2 point = ((Vector2)transform.position + _currentDir * _walkSize);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(point, _colSize, 0);

            foreach(var col in colliders)
            {
                if(col.tag == "Npc")
                {
                    col.GetComponent<Npc>().Talk();
                }
            }
        }

        /// 충돌처리
        bool CheckCollider(Vector2 dir, string tag)
        {
            Vector2 point = ((Vector2)transform.position + dir * _walkSize);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(point, _colSize, 0);
            
            foreach(var col in colliders)
            {
                if(col.tag == tag)
                {
                    return true;
                }
            }
            return false;
        }

        void Move(Vector2 dir)
        {
            if(!CheckCollider(dir, "Obstacle"))
                StartCoroutine(MoveRoutine(dir));
        }

        IEnumerator MoveRoutine(Vector2 dir)
        {
            _isControllable = false;
            Vector2 walkAmount = dir * ( _walkSize / _walkCount ) * _speed;

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

        private void OnDrawGizmos() 
        {
            Vector2 point = ((Vector2)transform.position + _currentDir * _walkSize);
            Gizmos.DrawCube(point, _colSize);
        }
    }
}