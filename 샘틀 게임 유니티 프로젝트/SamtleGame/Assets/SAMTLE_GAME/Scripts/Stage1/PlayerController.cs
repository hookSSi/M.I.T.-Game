using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }
}
