using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle = 0, Walk = 1, Jump = 2, Crouch = 3 }

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerState _state = PlayerState.Idle;
    private int _playerHpCount = 5;
    private Animator _playerAnimator;
    private Rigidbody2D _rigid;
    private float _attackCurrentTime;
    private float _immuneCurrentTime;
    private bool _isGround = true;
    public int _speed = 10;
    public float _jumpPower = 1.0f;
    public float _attackCollTime = 0.1f;
    private float _immuneCollTime = 1;
    public Transform _meleeTransform;
    public Vector2 _attackSize;
    public GameObject _playerHp;
    public float _playerForce = 5;
    

    // Start is called before the first frame update
    protected virtual void Initialization()
    {
        _playerAnimator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Initialization();
    }

    // Update is called once per frame
    private void Update()
    {
        Attack();
        if(_attackCurrentTime <= 0)
        {
            InputHandle();
        }
        
        _attackCurrentTime -= Time.deltaTime;
        _immuneCurrentTime -= Time.deltaTime;

        _playerAnimator.SetInteger("PlayerState", (int)_state);
    }

    private void Attack()
    {
        if(_attackCurrentTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))     // 추후 콘솔의 공격로 변경
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(_meleeTransform.position, _attackSize, 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    if(collider.tag == "Enemy")
                    {
                        collider.GetComponent<Enemy>().TakeDamage(transform, _playerForce);
                    }
                }
                _playerAnimator.SetTrigger("Attack");
                _attackCurrentTime = _attackCollTime;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(_meleeTransform.position, "Punch.png", true);
    }

    private void InputHandle()
    {
        if(_isGround)
            _state = PlayerState.Idle;

        if (Input.GetKey(KeyCode.DownArrow) && _isGround)     // 추후 콘솔의 앉기키로 변경
        {
            _state = PlayerState.Crouch;
        }

        Move();

        if (Input.GetKeyDown(KeyCode.UpArrow))     // 추후 콘솔의 이동키 또는 점프키로 변경
        {
            Jump();
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))     // 추후 콘솔의 이동키로 변경
        {
            if(_isGround && _state != PlayerState.Crouch)
                _state = PlayerState.Walk;

            transform.rotation = Quaternion.Euler(0, 180, 0);

            if(_state != PlayerState.Crouch)
                transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))     // 추후 콘솔의 이동키로 변경
        {
            if(_isGround && _state != PlayerState.Crouch)
                _state = PlayerState.Walk;

            transform.rotation = Quaternion.Euler(0, 0, 0);

            if(_state != PlayerState.Crouch)
                transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if(_state != PlayerState.Jump)
        {
            _state = PlayerState.Jump;
            _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    public void Attacked()
    {
        if(_immuneCurrentTime <= 0)
        {
            _playerHpCount--;
            _immuneCurrentTime = _immuneCollTime;
        }
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
            _state = PlayerState.Jump;
            _isGround = false;
        }
    }
}
