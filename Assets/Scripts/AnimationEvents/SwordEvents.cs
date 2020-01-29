using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEvents : MonoBehaviour
{
    [SerializeField]
    GameObject swTrail, swCollider;
    [SerializeField]
    CriAtomSource sw1, sw2;

    void SwordSwingStart()
    {
        swCollider.SetActive(true);
        swTrail.SetActive(true);
    }

    void SwordSwingEnd()
    {
        swCollider.SetActive(false);
        swTrail.SetActive(false);
    }

    void Swing1()
    {
        sw1.Play();
    }

    void Swing2()
    {
        sw2.Play();
    }
}
