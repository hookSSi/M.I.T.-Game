using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< .merge_file_a01268
public enum PlayerState { Idle, Walk, Jump, Attack, Attacked }

public class PlayerController : MonoBehaviour
{
    public int _speed = 10;
    public float _jumpPower = 1.0f;
    public float _attackCollTime = 0.1f;
    private float _immuneCollTime = 1;
    public Transform _meleeTransform;
    public Vector2 _attackSize;
    public GameObject _playerHp;
    public float _playerForce = 5;

    [SerializeField]
    private PlayerState _state = PlayerState.Idle;
    private int _playerHpCount = 5;
    private Animator _playerAni;
    private Rigidbody2D _rigid;
    private bool _isJumping = false;
    private float _attackCurrentTime;
    private float _immuneCurrentTime;
    

    // Start is called before the first frame update
    private void Start()
    {
        _playerAni = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        InitState();
        Attack();
        if(_attackCurrentTime <= 0)
        {
            Move();
        }
        CountingCollTime();
    }

    private void InitState()
    {
        _state = PlayerState.Idle;
        _playerAni.SetBool("idle_to_walk", false);
        _playerAni.SetBool("idle_to_bow", false);
    }

    private void Attack()
    {
        if(_attackCurrentTime <= 0)
        {
            if (Input.GetKey(KeyCode.Z))     // 추후 콘솔의 공격로 변경
            {
                // atk
                _state = PlayerState.Attack;
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(_meleeTransform.position, _attackSize, 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    //Debug.Log(collider.tag);
                    if(collider.tag == "Enemy")
                    {
                        collider.GetComponent<Enemy>().TakeDamage(transform, _playerForce);
                        //Debug.Log("EEEEEEE");
                    }
                }
                _playerAni.SetTrigger("idle_to_attack");
                _attackCurrentTime = _attackCollTime;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_meleeTransform.position, _attackSize);
    }

   private void Move()
    {
        
        if (Input.GetKey(KeyCode.LeftArrow))     // 추후 콘솔의 이동키로 변경
        {
            _state = PlayerState.Walk;
            _playerAni.SetBool("idle_to_walk", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))     // 추후 콘솔의 이동키로 변경
        {
            _state = PlayerState.Walk;
            _playerAni.SetBool("idle_to_walk", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))     // 추후 콘솔의 이동키 또는 점프키로 변경
        {
            if (_isJumping == false)
            {
                Jump();
            }
        }
        //if (Input.GetKey(KeyCode.DownArrow))     // 추후 콘솔의 앉기키로 변경
        //{
        //    playerAni.SetBool("idle_to_bow", true);
        //}
    }

    private void Jump()
    {
        _state = PlayerState.Jump;
        _isJumping = true;
        _playerAni.SetBool("idle_to_jump", true);
        _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
    }

    public void Attacked()
    {
        if(_immuneCurrentTime <= 0)
        {
            _playerHpCount--;
            // attacked

            _immuneCurrentTime = _immuneCollTime;
            //Debug.Log("Attacked!!");
        }
    }

    private void CountingCollTime()
    {
        _attackCurrentTime -= Time.deltaTime;
        _immuneCurrentTime -= Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            _playerAni.SetBool("idle_to_jump", false);
            _isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            _playerAni.SetBool("idle_to_jump", true);
            _isJumping = true;
=======
namespace MIT.SamtleGame.Stage1
{
    public enum PlayerState { Idle = 0, Walk = 1, Jump = 2, Crouch = 3, Dead, Hitted }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Player))]
    public class PlayerController : Controller
    {
        [SerializeField]
        private int _playerHpCount = 10;
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
                attackRange = _crouchKickRange;
            if(!_isGround)
                attackRange = _jumpKickRange;

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
>>>>>>> .merge_file_a10772
        }
    }
}
