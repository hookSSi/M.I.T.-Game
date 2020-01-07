using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 10;
    public float jumpPower = 1.0f;
    public float attackCollTime = 0.1f;
    public Transform meleeTransform;
    public Vector2 attackSize;

    private const int idle = 0;
    private const int jump = 1;
    private const int walk = 2;
    private const int attack = 3;
    private int state = idle;
    private Animator playerAni;
    private Rigidbody2D rigid;
    private bool isJumping = false;
    private float currentTime;
    
    // Start is called before the first frame update
    private void Start()
    {
        playerAni = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        InitState();
        Attack();
        if(currentTime <= 0)
        {
            Move();
        }
    }

    private void InitState()
    {
        state = idle;
        playerAni.SetBool("idle_to_walk", false);
        playerAni.SetBool("idle_to_bow", false);
    }

    private void Attack()
    {
        if(currentTime <= 0)
        {
            if (Input.GetKey(KeyCode.Z))     // 추후 콘솔의 공격로 변경
            {
                // atk
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(meleeTransform.position, attackSize, 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    Debug.Log(collider.tag);
                    if(collider.tag == "Enemy")
                    {
                        collider.GetComponent<Enemy>().TakeDamage();
                        //Debug.Log("EEEEEEE");
                    }
                }
                state = attack;
                playerAni.SetTrigger("idle_to_attack");
                currentTime = attackCollTime;
            }
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(meleeTransform.position, attackSize);
    }

   private void Move()
    {
        
        if (Input.GetKey(KeyCode.LeftArrow))     // 추후 콘솔의 이동키로 변경
        {
            state = walk;
            playerAni.SetBool("idle_to_walk", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))     // 추후 콘솔의 이동키로 변경
        {
            state = walk;
            playerAni.SetBool("idle_to_walk", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))     // 추후 콘솔의 이동키 또는 점프키로 변경
        {
            if (isJumping == false)
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
        state = jump;
        isJumping = true;
        playerAni.SetBool("idle_to_jump", true);
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            playerAni.SetBool("idle_to_jump", false);
            isJumping = false;
        }
    }
}
