﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySenseEnd : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Sense", false);
    }
}
