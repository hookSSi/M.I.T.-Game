using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public enum PlayerState { Idle = 0, Walk = 1, Jump = 2, Crouch = 3, Dead, Hitted }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Player))]
    public class PlayerController : Controller
    {
        private Player _playerInfo;
        private float _immuneCurrentTime;
        private float _immuneCollTime = 1;

        public bool _isAlive = true;

        [Header("충돌 콜라이더")]
        public BoxCollider2D _standCol;
        public BoxCollider2D _crouchCol;

        [Header("공격 범위")]
        public Transform _idlePunchRange;
        public Transform _crouchKickRange;
        public Transform _jumpKickRange;
        public Vector2 _attackSize;
        
        [Header("이동 범위")]
        public Transform _camera;
        public float _width;

        protected override void Initialization()
        {
            _playerInfo = GetComponent<Player>();
            _playerAnimator = GetComponent<Animator>();
            _rigid = GetComponent<Rigidbody2D>();
            _crouchCol.enabled = false;
        }

        // Update is called once per frame
        protected override void Update()
        {   
            GroundCheck();
            if(_isAlive)
            {
                /// 깔끔함이 절실한 하드 코딩
                if( _isCrouch )
                {
                    _crouchCol.enabled = true;
                    _standCol.enabled = false;
                }
                else
                {
                    _crouchCol.enabled = false;
                    _standCol.enabled = true;
                }

                if(_isControllable)
                    InputHandle();
                else
                    _playerAnimator.SetFloat("horizontal", 0);

                _attackCurrentTime -= Time.deltaTime;
                _immuneCurrentTime -= Time.deltaTime;
            }
            if(!_isAlive)
            {
                Dead();
            }
        }

        protected override void InputHandle()
        {
            if(_attackCurrentTime <= 0)
            {
                if (Input.GetKey(KeyCode.DownArrow) && _isGround)     // 추후 콘솔의 앉기키로 변경
                {
                    _isCrouch = true;
                    _playerAnimator.SetBool("IsCrouch", true);
                }
                else
                {
                    _isCrouch = false;
                    _playerAnimator.SetBool("IsCrouch", false);
                }

                Move();

                if (Input.GetKey(KeyCode.UpArrow) && _isGround)     // 추후 콘솔의 이동키 또는 점프키로 변경
                {
                    Jump();
                }

                if (Input.GetKey(KeyCode.Z) && _attackCurrentTime <= 0)     // 추후 콘솔의 공격로 변경
                {
                    Attack();
                }          
            }
        }

        protected override void Attack()
        {
            /// 공격 범위 정하기
            Transform attackRange = _idlePunchRange;

            if(_isCrouch)
            {
                attackRange = _crouchKickRange;
                SoundEvent.Trigger("하단");

            }
            else if(!_isGround)
            {
                attackRange = _jumpKickRange;
                SoundEvent.Trigger("점프킥");
            }
            else
            {
                attackRange = _idlePunchRange;
                SoundEvent.Trigger("주먹");
            }

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackRange.position, _attackSize, 0);

            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Enemy")
                {
                    collider.GetComponent<Enemy>().Hitted(attackRange);
                }
            }

            _playerAnimator.SetTrigger("Attack");
            _attackCurrentTime = _attackCollTime;
        }

        protected override void Move()
        {
            _playerAnimator.SetFloat("horizontal", 0f);

            if (Input.GetKey(KeyCode.LeftArrow))     // 추후 콘솔의 이동키로 변경
            {
                this.gameObject.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 180, 0);

                if(!_isCrouch)
                {
                    transform.Translate(Vector2.right * _speed * Time.deltaTime);
                    _playerAnimator.SetFloat("horizontal", 1f);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))     // 추후 콘솔의 이동키로 변경
            {
                this.gameObject.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, 0);

                if(_camera.position.x + _width < this.transform.position.x)
                    return;

                if(!_isCrouch)
                {
                    transform.Translate(Vector2.right * _speed * Time.deltaTime);
                    _playerAnimator.SetFloat("horizontal", 1f);
                }
            }
        }

        public void Hitted(int damage)
        {
            if(_immuneCurrentTime <= 0)
            {
                _playerInfo._currentHp -= damage;
                PlayerHittedEvent.Trigger(_playerInfo._currentHp);
                _immuneCurrentTime = _immuneCollTime;

                if(_playerInfo._currentHp < 0)
                {
                    _isAlive = false;
                    Dead();
                }
            }
        }

        private void Dead()
        {
            _playerAnimator.SetBool("Dead", true);
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.red;
            if(_isCrouch)
            {
                Gizmos.DrawCube(_crouchKickRange.position, _attackSize);
            }
            else if(_isGround )
            {
                Gizmos.DrawCube(_idlePunchRange.position, _attackSize);
            }
            else if(!_isGround)
            {
                Gizmos.DrawCube(_jumpKickRange.position, _attackSize);
            }
        }
    }
}
