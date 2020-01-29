using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    private Image title, subtitle, tapbutton;

    private void Start()
    {
        var seq = DOTween.Sequence();

        seq.AppendInterval(0.5f);

        seq.Append(DOTween.To(
            () => Camera.main.backgroundColor,
            color => Camera.main.backgroundColor = color,
            Color.black,
            0.5f
        ));

        seq.Append(DOTween.ToAlpha(
            () => title.color,
            color => title.color = color,
            1f,
            0.1f
        ));

        seq.Append(DOTween.To(
            () => Camera.main.backgroundColor,
            color => Camera.main.backgroundColor = color,
            new Color32(200,200,200,0),
            3f
        ));

        seq.Append(title.DOColor(new Color32(100, 255, 240, 255), 1f));
        seq.Append(subtitle.DOColor(new Color(subtitle.color.r, subtitle.color.g, subtitle.color.b, 1), 2f));
        seq.AppendInterval(2f).OnComplete(() => tapbutton.gameObject.SetActive(true));
    }
}
