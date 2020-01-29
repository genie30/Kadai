using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        PlayerStatus.instance.Heal = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        PlayerStatus.instance.Heal = false;
    }
}
