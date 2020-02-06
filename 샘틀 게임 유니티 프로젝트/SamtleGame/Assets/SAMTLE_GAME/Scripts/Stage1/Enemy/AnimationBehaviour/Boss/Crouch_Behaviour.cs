using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public class Crouch_Behaviour : StateMachineBehaviour
    {
        /// AI를 위한 변수들
        private int _attackCount = 0;
        private int _maxAttackCount = 3;
        private float _pastTime = 0.5f;

        private Boss _boss;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<Boss>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            int rndNum = Random.Range(0, 100);
            int attackProbability = 1 / (_attackCount + 1) * 100;

            if(_boss.IsPlayerClose())
            {
                if( _boss._delay <= _pastTime && rndNum <= attackProbability ) 
                {
                    Attack(animator);
                }
                else if( _boss._delay > _pastTime )
                {
                    if( rndNum <= 70 )
                        Stand(animator);
                    if( 70 < rndNum )
                        Defense(animator);
                }
            }
            else
            {
                Stand(animator);
            }
        
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

        void Stand(Animator animator)
        {
            animator.SetBool("IsCrouch", false);
            Debug.Log("일어나기");
        }
    }
}
