using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    ScrollRect sc;

    [SerializeField]
    CriAtomSource selector, startb;

    int stagenum = 0; 
    int maxstage = 4;

    float rectsize;

    private void Start()
    {
        rectsize = 1f / (maxstage - 1);
    }

    public void ScrollMove(int move)
    {
        var tmp = stagenum + move;
        if (tmp < 0 || tmp > maxstage) return;

        stagenum = tmp;

        selector.Play();
        if(stagenum == 0)
        {
            sc.horizontalNormalizedPosition = 0f;
        }
        else if(stagenum == maxstage)
        {
            sc.horizontalNormalizedPosition = 1f;
        }
        else
        {
            sc.horizontalNormalizedPosition += (rectsize * move);
        }
    }

    public void StartClick(int i)
    {
        startb.Play();
        GameManager.instance.StageNum = i;
        var scene = i < 5 ? "Stage1" : "Stage2";
        GameManager.instance.SceneLoad(scene);
    }
}
