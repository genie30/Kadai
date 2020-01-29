using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    [SerializeField]
    EnemyStatus state;
    [SerializeField]
    ParticleSystem hit;

    private void OnTriggerEnter(Collider other)
    {
        if (state.Data.HP <= 0) return; 
        if (other.tag != "PlayerAttack") return;
        SwordHitEvent.instance.HitEV();
        hit.Play();
        state.HPDown();
    }
}
