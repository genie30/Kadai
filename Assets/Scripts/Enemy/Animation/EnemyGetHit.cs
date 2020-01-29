using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("KnockBack", true);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Action", false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("KnockBack", false);
    }
}
