using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollision : MonoBehaviour
{
    [SerializeField]
    EnemyStatus state;

    [SerializeField]
    string type;

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag != "Player") return;
        if (type == "Dragon")
        {
            SEManager.instance.dragonclaw.Play();
        }
        else if(type == "Boar")
        {
            SEManager.instance.boaratk.Play();
        }
        else if(type == "Golem")
        {
            SEManager.instance.hit4.Play();
        }
        else if(type == "Turtle")
        {
            SEManager.instance.hit2.Play();
        }
        else
        {
            SEManager.instance.hit1.Play();
        }

        PlayerStatus.instance.PlayerDamage(state.Damage);
    }
}
