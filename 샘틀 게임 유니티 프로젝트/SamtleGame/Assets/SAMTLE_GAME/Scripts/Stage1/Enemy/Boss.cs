using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public class Boss : Enemy
    {
        private Animator _animator;

        [Header("보스 상태 정보")]
        [SerializeField]
        private EnemyState _state = EnemyState.Stand;
        public int _health = 100;
        public DialogueBox _dialogue;

        [Header("보스 공격 정보")]
        public float _delay = 0.5f;

        protected override void Initialization()
        {
            base.Initialization();
            _animator = GetComponent<Animator>();
        }

        public bool IsPlayerClose()
        {
            if( Mathf.Abs( _attackRange.position.x - Player._pos.x ) < _attackSize.x / 2 )
            {
                return true;
            }
            else
                return false;
        }


        public override void Move()
        {
            // 서있는 상태라면
            transform.Translate(Vector2.right * _enemySpeed * Time.deltaTime);
            Debug.Log("앞으로 이동");
        }

        public void NextPage()
        {
            _dialogue.NextPage();
        }

        public void StartCombat()
        {
            _enemySpeed = 3f;
            _animator.SetTrigger("StartCombat");
            PlayGameEvent.Trigger();
        }

        public void Backstep()
        {
            transform.Translate(Vector2.left * _enemySpeed * Time.deltaTime);
            Debug.Log("뒤로 이동");
        }

        public override void Attack()
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
        public virtual void Defense()
        {
            Debug.Log("방어!");
        }
    }

}
