using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField]
    private int EnemyNum;
    public Animator anim;

    public EnemyData Data;
    public int Damage { get; set; }

    [SerializeField]
    ParticleSystem die;

    private void Awake()
    {
        StatusLoad();
    }
    private void StatusLoad()
    {
        Data = new EnemyData(EnemyDataList.Instance.EnemyList[EnemyNum]);
        Damage = Data.Atk;
    }

    public void HPDown()
    {
        if (Data.Name.Contains("Dragon"))
        {
            if (anim.GetInteger("Attack") == 2 || anim.GetBool("Fly")) return;

            var miss = Random.Range(0, 5);
            if (miss == 4)
            {
                //Guard Effect
                return;
            }

            if(!anim.GetBool("Action") || anim.GetInteger("Attack") == 0)
            {
                anim.SetBool("Action", false);
                anim.SetBool("Move", false);
                anim.SetTrigger("Damage");
            }
            Data.HP--;
        }
        else if (Data.Name.Contains("Boar"))
        {
            if (anim.GetInteger("Attack") == 1) return;
            anim.SetBool("Action", false);
            anim.SetBool("Move", false);
            anim.SetTrigger("Damage");
            Data.HP--;
        }
        else
        {
            anim.SetBool("Action", false);
            anim.SetBool("Move", false);
            anim.SetTrigger("Damage");
            Data.HP--;
        }

        if (Data.HP == 0) DieMobs();
    }

    public void DieMobs()
    {
        anim.SetBool("Battle", false);
        anim.SetTrigger("Die");
        die.Play();
    }
}
