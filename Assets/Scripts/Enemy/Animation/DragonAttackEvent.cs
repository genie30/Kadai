using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject attackcollider,breatheffect,breathcollider;
    [SerializeField]
    Animator anim;

    CriAtomSource playing;

    void ColliderOn()
    {
        attackcollider.SetActive(true);
    }

    void ColliderOff()
    {
        attackcollider.SetActive(false);
    }

    void BreathStart()
    {
        SEPlay(SEManager.instance.dragonbreath);
        breatheffect.SetActive(true);
        breathcollider.SetActive(true);
    }
    void BreathEnd()
    {
        playing.Stop();
        breatheffect.SetActive(false);
        breathcollider.SetActive(false);
    }
    
    void Hound()
    {
        if(anim.GetBool("Battle"))
        SEPlay(SEManager.instance.dragonhound);
    }

    void Flying()
    {
        SEPlay(SEManager.instance.dragonflying);
    }

    void FlyStay()
    {
        SEPlay(SEManager.instance.dragonwing);
    }

    void SEPlay(CriAtomSource se)
    {
        playing = se;
        playing.Play();
    }
}
