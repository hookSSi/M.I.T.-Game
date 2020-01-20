using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage1;

public enum EnemyState { Walk, Attack }

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private EnemyState _state = EnemyState.Walk;
    private BoxCollider2D _boxColider;
    private bool _isAlive = true;

    [Header("적 정보")]
    [Tooltip("적의 데미지")]
    public float _damage = 0.1f;
    [Tooltip("잡으면 주는 스코어")]
    public int _score =  100;
    public float _enemySpeed = 1.5f;
    public GameObject _hittedEffect;

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
            collision.gameObject.GetComponent<PlayerController>().Hitted(_damage);
            _state = EnemyState.Attack;
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

    public virtual void Hitted(Transform collisionObjectTransform)
    {
        if(_isAlive)
        {
            ScoreUpEvent.Trigger(_score);
            Instantiate(_hittedEffect, collisionObjectTransform);
            StartCoroutine(DestoyEnemy());
        }
    }

    protected virtual IEnumerator DestoyEnemy()
    {
        _isAlive = false;
        _boxColider.isTrigger = true;
        this.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
