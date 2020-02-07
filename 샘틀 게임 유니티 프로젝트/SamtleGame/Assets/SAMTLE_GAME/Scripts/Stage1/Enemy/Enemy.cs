using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MIT.SamtleGame.Stage1;

public enum EnemyState { Stand, Crouch }
public enum EnemyDoing { Walk, Attack, Defense }

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected EnemyDoing _doingState = EnemyDoing.Walk;
    protected BoxCollider2D _boxColider;
    protected bool _isAlive = true;
    protected Vector2 _dir = Vector2.right;

    [Header("적 정보")]
    public Transform _attackRange;
    public Vector2 _attackSize;
    [Tooltip("적의 데미지")]
    public float _damage = 0.1f;

    [Header("스코어 관련 설정")]
    [Tooltip("잡으면 주는 스코어")]
    public int _score =  100;
    public float _enemySpeed = 1.5f;
    public GameObject _hittedEffect;
    public TMP_Text _scoreText;

    protected virtual void Initialization()
    {
        _boxColider = GetComponent<BoxCollider2D>();
        _scoreText.text = _score.ToString();
    }

    public void SetDirection(Vector2 dir)
    {
        _dir = dir;

        if(_dir.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            _attackRange.localPosition = new Vector3(_attackRange.localPosition.x * -1, _attackRange.localPosition.y, _attackRange.localPosition.z);
        }
    }

    private void Start()
    {
        Initialization();
    }

    /// LookAt 플레이어를 보는 함수가 있으면 좋겠다.
    public virtual void Move()
    {
        if(_doingState == EnemyDoing.Walk)
        {   
            transform.Translate(_dir * _enemySpeed * Time.deltaTime);
            _doingState = EnemyDoing.Walk;
        }
    }

    protected virtual void Update()
    {
        Move();

        if(Mathf.Abs(Player._pos.x - this.transform.position.x) > 34 || transform.position.y < -10)
        {
            StartCoroutine(DestoySelf());
        }
    }

    public virtual void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_attackRange.position, _attackSize, 0);

        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == "Player")
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                if(player._isAlive)
                {
                    player.Hitted(_damage);
                }
            }
        }
    }

    public virtual void Hitted(Transform collisionObjectTransform)
    {
        if(_isAlive)
        {
            ScoreUpEvent.Trigger(_score);
            Instantiate(_hittedEffect, collisionObjectTransform);
            StartCoroutine(DestoySelf(true));
        }
    }

    protected virtual IEnumerator DestoySelf(bool isHitted = false, float duration = 1f)
    {
        _isAlive = false;
        _boxColider.isTrigger = true;
        this.GetComponent<SpriteRenderer>().enabled = false;

        if(isHitted)
        {
            _scoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _scoreText.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
        GameManager._totalEnemyCount -= 1;
    }
}
