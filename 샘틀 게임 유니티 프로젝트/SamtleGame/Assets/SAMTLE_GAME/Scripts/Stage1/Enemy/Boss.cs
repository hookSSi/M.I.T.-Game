using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public class Boss : Enemy
    {
        /// AI를 위한 변수들
        private Player _player;
        private int _attackCount = 0;
        private int _maxAttackCount = 3;
        private float _pastTime = 0.5f;

        [Header("보스 상태 정보")]
        [SerializeField]
        private EnemyState _state = EnemyState.Stand;
        public int _health = 100;

        [Header("보스 공격 정보")]
        public Transform _attackRange;
        public Vector2 _attackSize;
        public float _delay = 0.5f;

        protected override void Initialization()
        {
            base.Initialization();
            _player = FindObjectOfType<Player>();
        }

        protected override void Update()
        {
            int rndNum = Random.Range(0, 100);
            int attackProbability = 1 / (_attackCount + 1) * 100;

            if(IsPlayerClose())
            {
                if( _delay <= _pastTime && rndNum <= attackProbability ) 
                {
                    Attack();
                }
                else if( _delay > _pastTime )
                {
                    if( _state == EnemyState.Crouch )
                    {
                        if( rndNum <= 70 )
                            Stand();
                        if( 70 < rndNum )
                            Defense();
                    }
                    if( _state == EnemyState.Stand )
                    {
                        if( rndNum <= 20 )
                            Crouch();
                        if( rndNum < 20 && rndNum <= 40 )
                            BackStep();
                        if( 40 < rndNum )
                            Defense();
                    }
                }
            }
            else
            {
                if( _state == EnemyState.Crouch )
                {
                    Stand();
                }
                if( _state == EnemyState.Stand )
                {
                    if( rndNum <= 10 )
                        Crouch();
                    if( rndNum < 10 && rndNum <= 20 )
                        BackStep();
                    if( 20 < rndNum )
                        Move();
                }
            }

            _pastTime += Time.deltaTime;
        }

        protected bool IsPlayerClose()
        {
            if((_attackRange.position - _player.Pos).magnitude < _attackSize.magnitude)
            {
                return true;
            }
            else
                return false;
        }

        protected override void Move()
        {
            // 서있는 상태라면
            transform.Translate(Vector2.right * _enemySpeed * Time.deltaTime);
            Debug.Log("앞으로 이동");
        }

        protected virtual void BackStep()
        {
            transform.Translate(Vector2.left * _enemySpeed * Time.deltaTime);
            Debug.Log("뒤로 이동");
        }

        protected override void Attack()
        {
            Debug.Log("공격");

            _attackCount = (_attackCount + 1) % _maxAttackCount;
            _pastTime = 0;
        }

        protected virtual void Defense()
        {
            Debug.Log("방어");
        }

        protected virtual void Crouch()
        {
            _state = EnemyState.Crouch;
            Debug.Log("앉기");
        }

        protected virtual void Stand()
        {
            _state = EnemyState.Stand;
            Debug.Log("일어나기");
        }
    }
}
