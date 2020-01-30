using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage1;

public enum EnemyState { Stand, Crouch }
public enum EnemyDoing { Walk, Attack, Defense }

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected EnemyDoing _doingState = EnemyDoing.Walk;
    protected BoxCollider2D _boxColider;
    protected bool _isAlive = true;

    [Header("적 정보")]
    public Transform _attackRange;
    public Vector2 _attackSize;
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

    /// LookAt 플레이어를 보는 함수가 있으면 좋겠다.
    protected virtual void Move()
    {
        if(_doingState == EnemyDoing.Walk)
        {
            transform.Translate(Vector2.right * _enemySpeed * Time.deltaTime);
            _doingState = EnemyDoing.Walk;
        }
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_attackRange.position, _attackSize, 0);

        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == "Player")
            {
                collider.GetComponent<PlayerController>().Hitted(_damage);
            }
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
        GameManager._totalEnemyCount -= 1;
    }
}
