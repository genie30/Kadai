using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoarAttackEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject attackcollider;
    [SerializeField]
    private CharacterController cc;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private EnemyAction act;

    private bool run;

    void ColliderOn()
    {
        attackcollider.SetActive(true);
    }

    void ColliderOff()
    {
        attackcollider.SetActive(false);
        run = false;
        act.movestop = false;
        transform.LookAt(target);
    }

    void HornAttack()
    { 
        ColliderOn();
        run = true;
        act.movestop = true;
        SEManager.instance.boarcry.Play();
        SEManager.instance.boarrun.Play();
        StartCoroutine(ForwardMove());
    }
    IEnumerator ForwardMove()
    {
        var targetpos = (transform.forward * 20f) + (transform.right * 0.3f);
        while (run)
        {
            var pos = (targetpos - transform.position) * Time.deltaTime;
            pos = new Vector3(pos.x, transform.position.y, pos.z);
            cc.Move(pos);
            yield return null;
        }
    }
}
