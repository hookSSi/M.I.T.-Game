using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage1
{
    public class Boss : Enemy
    {
        private Animator _animator;
        private bool _isDefending =  false;

        [Header("보스 상태 정보")]
        public int _health = 100;
        public DialogueBox _dialogue;

        [Header("보스 공격 정보")]
        public float _delay = 0.5f;
        public float _defenseTime = 0.33f;

        protected override void Initialization()
        {
            base.Initialization();
            _animator = GetComponent<Animator>();
        }

        protected override void Update() 
        {
            /// Update를 안돌리기 위해 비워둠
        }

        public bool IsPlayerClose()
        {
            if( Mathf.Abs( transform.position.x - Player._pos.x ) < _attackSize.x )
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
            StartCoroutine(DefenseRoutine());
        }

        protected IEnumerator DefenseRoutine()
        {
            _isDefending = true;
            yield return new WaitForSeconds(_defenseTime);
            _isDefending = false;
            yield break;
        }

        public override void Hitted(Transform collisionObjectTransform)
        {
            if(_isAlive && !_isDefending)
            {
                _health -= 10;
                BossHittedEvent.Trigger(_health);
                
                if(_health == 0)
                {
                    ScoreUpEvent.Trigger(_score);
                    Instantiate(_hittedEffect, collisionObjectTransform);
                    StartCoroutine(DestoySelf(true));
                }
            }
        }

        protected override IEnumerator DestoySelf(bool isHitted = false, float duration = 1f)
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
			ClearGameEvent.Trigger();
			Debug.Log("Call Game clear");
            //LoadingSceneManager.LoadScene("공대관");
        }
    }

}
