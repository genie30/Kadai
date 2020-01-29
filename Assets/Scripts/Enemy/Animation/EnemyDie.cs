using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyDie : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DOTween.Sequence().AppendInterval(1).AppendCallback(() => animator.SetBool("Destroy", true));
    }
}
