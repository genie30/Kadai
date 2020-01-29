using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject swordcollider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) AttackTap();
    }

    public void AttackTap() 
    {
        if (anim.GetBool("KnockBack")) return;
        if (anim.GetBool("Guard")) anim.SetBool("Guard", false);
        anim.SetBool("Action", true);
        swordcollider.SetActive(false);
        anim.SetTrigger("Attack");
    }
}
