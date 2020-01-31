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
            if(_player._controller._isAlive && _isAlive)
            {                
                if( Mathf.Abs(_player.Pos.x - this.transform.position.x) < _detectionRange )
                {
                    _animator.SetBool("AttackReady", true);
                }
                if( Mathf.Abs(_player.Pos.x - this.transform.position.x) <= _attackSize.x )
                {
                    _animator.SetTrigger("Attack");
                }
                else
                {
                    Move();
                }
            }
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

        protected override IEnumerator DestoySelf(bool isHitted = false, float duration = 1f)
        {
            _isAlive = false;
            _boxColider.isTrigger = true;
            _animator.SetTrigger("Death");

            if(isHitted)
            {
                _scoreText.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.33f);
                _scoreText.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
            GameManager._totalEnemyCount -= 1;
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.DrawCube(_attackRange.position, _attackSize);
        }
    }
}

