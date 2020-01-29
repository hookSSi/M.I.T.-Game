using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    [RequireComponent(typeof(Animator))]
    public class Civil : Enemy
    {
        private Player _player;
        private Animator _animator;
        public float _detectionRange = 4f;

        protected override void Initialization()
        {
            base.Initialization();
            _player = FindObjectOfType<Player>();
            _animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if( Mathf.Abs(_player.Pos.x - this.transform.position.x) < _detectionRange )
            {
                _animator.SetBool("AttackReady", true);
            }
            if( Mathf.Abs(_player.Pos.x - this.transform.position.x) < _attackSize.x )
            {
                _animator.SetTrigger("Attack");
            }
            else
            {
                Move();
            }
        }

        protected override void Attack()
        {
            base.Attack();
        }

        protected override IEnumerator DestoyEnemy()
        {
            _isAlive = false;
            _boxColider.isTrigger = true;
            _animator.SetTrigger("Death");

            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
            GameManager._totalEnemyCount -= 1;
        }
    }
}

