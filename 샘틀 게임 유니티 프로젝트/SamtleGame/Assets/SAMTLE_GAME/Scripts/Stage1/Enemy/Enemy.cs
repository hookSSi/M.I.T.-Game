using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage1;

public enum EnemyState { Walk, Attack }

public class Enemy : MonoBehaviour
{
    public float _enemySpeed = 1.5f;

    [SerializeField]
    private EnemyState _state = EnemyState.Walk;
    private BoxCollider2D _boxColider;

    protected virtual void Initialization()
    {
        _boxColider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        Initialization();
    }

    protected virtual void Move()
    {
        if(_state == EnemyState.Walk)
        {
            transform.Translate(Vector2.right * _enemySpeed * Time.deltaTime);
            _state = EnemyState.Walk;
        }
    }

    private void Update()
    {
        Move();
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Attacked();
            _state = EnemyState.Attack;
            
            StartCoroutine(DestoyEnemy());
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log(collision);
        if (collision.transform.tag == "Player")
        {
            _state = EnemyState.Walk;
        }
    }

    public virtual void TakeDamage(Transform collisionObjectTransform, float playerForce)
    {
        GameController.RisingScore(100);

        StartCoroutine(DestoyEnemy());
    }

    protected virtual IEnumerator DestoyEnemy()
    {
        _boxColider.isTrigger = true;

        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
