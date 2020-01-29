using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitEvent : MonoBehaviour
{
    public static SwordHitEvent instance;

    [SerializeField]
    CriAtomSource hit;

    private void Awake()
    {
        instance = this;
    }

    public void HitEV()
    {
        hit.Play();
    }
}
