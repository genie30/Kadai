using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSMobAction : MonoBehaviour
{
    private void Start()
    {
        var anim = GetComponent<Animator>();
        anim.SetBool("Move", true);
    }
}
