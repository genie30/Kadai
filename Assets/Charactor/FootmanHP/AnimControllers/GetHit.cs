using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHit : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Action", false);
        animator.SetBool("KnockBack", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("KnockBack", false);
    }
}
