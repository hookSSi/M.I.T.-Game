using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public enum PlayerState { Idle = 0, Walk = 1, Jump = 2, Crouch = 3, Dead, Hitted }

    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerState _state = PlayerState.Idle;
        private int _playerHpCount = 10;
        private Animator _playerAnimator;
        private Rigidbody2D _rigid;
        private float _attackCurrentTime;
        private float _immuneCurrentTime;
        private bool _isGround = true;
        private bool _isAlive = true;
        public int _speed = 10;
        public float _jumpPower = 1.0f;
        public float _attackCollTime = 0.1f;
        private float _immuneCollTime = 1;
        public bool _isControllable = true;

        [Header("콜라이더")]
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

        // Start is called before the first frame update
        protected virtual void Initialization()
        {
            _playerAnimator = GetComponent<Animator>();
            _rigid = GetComponent<Rigidbody2D>();
            _crouchCol.enabled = false;
        }

        private void Start()
        {
            Initialization();
        }

        // Update is called once per frame
        private void Update()
        {   
            if(_isAlive && _isControllable)
            {
                /// 깔끔함이 절실한 하드 코딩
                if( _state == PlayerState.Crouch )
                {
                    _crouchCol.enabled = true;
                    _standCol.enabled = false;
                }
                else
                {
                    _crouchCol.enabled = false;
                    _standCol.enabled = true;
                }

                InputHandle();
                
                _attackCurrentTime -= Time.deltaTime;
                _immuneCurrentTime -= Time.deltaTime;
                
                _playerAnimator.SetInteger("PlayerState", (int)_state);
                
                if(_isGround)
                    _state = PlayerState.Idle;
                else
                    _state = PlayerState.Jump;
            }
            
            if(!_isAlive)
            {
                Dead();
            }
        }

        private void Attack()
        {
            /// 공격 범위 정하기
            Transform attackRange = _idlePunchRange;
            switch(_state)
            {
                case PlayerState.Idle:
                    attackRange = _idlePunchRange;
                    break;
                case PlayerState.Crouch:
                    attackRange = _crouchKickRange;
                    break;
                case PlayerState.Jump:
                    attackRange = _jumpKickRange;
                    break;
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
            SoundEvent.Trigger("Typing");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(_idlePunchRange.position, "Punch.png", true);
            Gizmos.DrawIcon(_crouchKickRange.position, "Kick.png", true);
            Gizmos.DrawIcon(_jumpKickRange.position, "JumpKick.png", true);
        }

        private void InputHandle()
        {
            if(_attackCurrentTime <= 0)
            {
                if (Input.GetKey(KeyCode.DownArrow) && _isGround)     // 추후 콘솔의 앉기키로 변경
                {
                    _state = PlayerState.Crouch;
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

        private void Move()
        {
            if (Input.GetKey(KeyCode.LeftArrow))     // 추후 콘솔의 이동키로 변경
            {
                if(_isGround && _state != PlayerState.Crouch)
                    _state = PlayerState.Walk;

                this.gameObject.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 180, 0);

                if(_state != PlayerState.Crouch)
                    transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))     // 추후 콘솔의 이동키로 변경
            {
                if(_isGround && _state != PlayerState.Crouch)
                    _state = PlayerState.Walk;

                this.gameObject.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, 0);

                if(_camera.position.x + _width < this.transform.position.x)
                    return;

                if(_state != PlayerState.Crouch)
                    transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }
        }

        private void Jump()
        {
            _state = PlayerState.Jump;
            _rigid.velocity = Vector2.up * _jumpPower;
        }

        public void Hitted(float damage)
        {
            if(_immuneCurrentTime <= 0)
            {
                _playerHpCount--;
                PlayerHittedEvent.Trigger(damage);
                _immuneCurrentTime = _immuneCollTime;

                if(_playerHpCount == 0)
                {
                    _isAlive = false;
                }
            }
        }

        private void Dead()
        {
            _playerAnimator.SetBool("Dead", true);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.transform.tag == "Ground")
            {
                _isGround = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.tag == "Ground")
            {
                _isGround = false;
            }
        }
    }
}
