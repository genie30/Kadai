using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneController : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    List<Image> list;

    [SerializeField]
    CriAtomSource bgm;

    private void Start()
    {
        bgm.Play();
        GameManager.instance.LoadWait(slider);
        StartCoroutine(LoopImage());
    }

    IEnumerator LoopImage()
    {
        var leftnum = 0;

        for(var i = leftnum+3; i >= leftnum; i--)
        {
            var a = i == leftnum || i == leftnum+3 ? 0.5f : 1f;
            var item = list[i];
            item.color = new Color(item.color.r, item.color.g, item.color.b, a);
        }

        yield return new WaitForSeconds(0.2f);

        float waittime = 0f;
        while (waittime < 10f)
        {
            var tmp = leftnum;
            var item = list[tmp];
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0f);

            tmp++;
            if (tmp > 7) tmp -= 8;
            item = list[tmp];
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0.5f);

            tmp += 2;
            if (tmp > 7) tmp -= 8;
            item = list[tmp];
            item.color = new Color(item.color.r, item.color.g, item.color.b, 1f);

            tmp++;
            if (tmp > 7) tmp -= 8;
            item = list[tmp];
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0.5f);

            leftnum++;
            if (leftnum >= 8) leftnum -= 8;
            yield return new WaitForSeconds(0.2f);
            waittime += 0.2f;
        }
        GameManager.instance.NextScene();
    }
}
