using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject attackcollider;

    private void StartAttack()
    {
        attackcollider.SetActive(true);
    }

    private void EndAttack()
    {
        attackcollider.SetActive(false);
    }
}
