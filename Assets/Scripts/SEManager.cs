using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager instance;

    public CriAtomSource dragonclaw, dragonhound, dragonbreath, dragonflying, dragonwing;
    public CriAtomSource boarrun, boarcry, boaratk;
    public CriAtomSource golemcry, hit1, hit2, hit3, hit4;

    private void Awake()
    {
        instance = this;
    }
}
