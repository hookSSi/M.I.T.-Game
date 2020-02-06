using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage1
{    
    public class KeepWalk_Behabiour : StateMachineBehaviour
    { 
        Vector3 _bossPos;
        bool _isActive = true;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Boss>()._enemySpeed = 20f;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_isActive)
            {
                _bossPos = animator.GetComponent<Transform>().position;

                if(Mathf.Abs(Player._pos.x - _bossPos.x) < 7)
                {
                    animator.SetTrigger("IsPlayerVisible");
                    _isActive = false;
                }
            }
        }
    }
}
