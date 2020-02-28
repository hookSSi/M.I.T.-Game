using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public class Run_Behaviour : StateMachineBehaviour
    {
        /// AI를 위한 변수들
        private int _attackCount = 0;
        private int _maxAttackCount = 3;
        private float _pastTime = 0.5f;

        private Boss _boss;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsCrouch", false);
            _boss = animator.GetComponent<Boss>();
            _boss._enemySpeed = 3f;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            int rndNum = Random.Range(0, 100);
            int attackProbability = 1 / (_attackCount + 1) * 100;

            if( _boss.IsPlayerClose() && !_boss._isDefending && !_boss._isBackstemping)
            {
                if( _boss._delay <= _pastTime && rndNum <= attackProbability + 30 ) 
                {
                    Attack(animator);
                }
                else if( _boss._delay > _pastTime )
                {
                    if( rndNum <= 20 )
                        Crouch(animator);
                    if( 20 < rndNum )
                        Defense(animator);
                }
            }
            else
            {
                if( rndNum <= 20 )
                    Crouch(animator);
                if( 20 < rndNum )
                    _boss.Move();
            }

            _pastTime += Time.deltaTime;
        }
        void Attack(Animator animator)
        {
            Debug.Log("공격");
            animator.SetTrigger("Attack");
            _attackCount = (_attackCount + 1) % _maxAttackCount;
            _pastTime = 0;
        }

        void Defense(Animator animator)
        {
            animator.SetTrigger("Defense");
            Debug.Log("방어");
        }
        
        void Crouch(Animator animator)
        {
            animator.SetBool("IsCrouch", true);
            Debug.Log("앉기");
        }

        void Backstep(Animator animator)
        {
            animator.SetTrigger("Backstep");
            Debug.Log("백스텝");
        }
    }
}
