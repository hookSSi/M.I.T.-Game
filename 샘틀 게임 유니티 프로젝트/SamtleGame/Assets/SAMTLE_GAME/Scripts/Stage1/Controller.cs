using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public class Controller : MonoBehaviour
    {
        /// 로직을 위한 변수들
        [SerializeField]
        protected PlayerState _state = PlayerState.Idle;
        protected Animator _playerAnimator;
        protected Rigidbody2D _rigid;
        protected float _attackCurrentTime;

        /// 애니메이션 관련 bool
        protected bool _isCrouch = false;
        protected bool _isGround = true;

        /// 컨트롤 관련 변수들
        public bool _isControllable = true;
        public int _speed = 10;
        public float _jumpPower = 1.0f;
        public float _attackCollTime = 0.1f;

        [Header("땅체크 콜라이더")]
        public Transform _groundCheker;
        public Vector2 _groundColSize;

        protected virtual void Initialization()
        {
            _playerAnimator = GetComponent<Animator>();
            _rigid = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Initialization();
        }

        protected virtual void Update()
        {   
            GroundCheck();
            if(_isControllable)
                InputHandle();
            else
                _playerAnimator.SetFloat("horizontal", 0);

            _attackCurrentTime -= Time.deltaTime;
        }

        protected virtual void InputHandle()
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

        protected virtual void Move()
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

                // if(_camera.position.x + _width < this.transform.position.x)
                //     return;

                if(!_isCrouch)
                {
                    transform.Translate(Vector2.right * _speed * Time.deltaTime);
                    _playerAnimator.SetFloat("horizontal", 1f);
                }
            }
        }

        protected virtual void Jump()
        {
            _rigid.velocity = Vector2.up * _jumpPower;
        }

        protected virtual void Attack()
        {
            _playerAnimator.SetTrigger("Attack");
            _attackCurrentTime = _attackCollTime;
            SoundEvent.Trigger("Typing");
        }

        protected virtual void GroundCheck()
        {
            _isGround = false;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(_groundCheker.position, _groundColSize, 0);
        
            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Ground")
                {
                    _isGround = true;
                }
            }

            _playerAnimator.SetBool("IsGround", _isGround);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(_groundCheker.position, _groundColSize);
        }
    }
}

