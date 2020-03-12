using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehavior : StateMachineBehaviour
{
    int _prevSortingOrder = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _prevSortingOrder = animator.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        animator.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _prevSortingOrder;
    }
}
