using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    [SerializeField]
    int hp = 50;
    [SerializeField]
    Animator anim;
    [SerializeField]
    Slider bar;

    public bool Heal;
    public static ReactiveProperty<int> HP = new ReactiveProperty<int>();

    private void Awake()
    {
        instance = this;
        HP.Value = hp;
    }

    private void Start()
    {
        bar.maxValue = hp;

        Observable.Interval(TimeSpan.FromSeconds(0.3))
            .Select(x => Heal).Where(x => x).Where(x => HP.Value < hp)
            .Subscribe(_ => PlayerHeal()).AddTo(this);
        HP.TakeUntilDestroy(this).DistinctUntilChanged().Subscribe(_ => bar.value = HP.Value);
    }

    public void PlayerDamage(int damage)
    {
        if (anim.GetBool("Die")) return;
        HP.Value -= damage;
        if(HP.Value < 0)
        {
            Heal = false;
            PlayerDie();
            return;
        }
    }

    public void PlayerHeal()
    {
        if (HP.Value < hp) HP.Value++;
    }

    private void PlayerDie()
    {
        anim.SetBool("Die", true);
        Scene1.instance.ShowResult(false);
    }
}
