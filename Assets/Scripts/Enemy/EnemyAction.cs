using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyAction : MonoBehaviour
{
    [SerializeField]
    CharacterController cc;
    [SerializeField]
    GameObject player = null;

    EnemyStatus ins;
    float distance = 999; // 1.5くらいで停止
    [SerializeField]
    float stopDistance = 3f;
    bool rndmove = false;

    float speed;
    int atk;
    public bool movestop;

    private void Start()
    {
        DataLoad();

        var upStream = this.UpdateAsObservable().TakeUntilDestroy(this).Share();

        upStream.TakeUntilDestroy(this)
            .Where(x => player != null)
            .Subscribe(_ => TargetMove());

        upStream.TakeUntilDestroy(this)
            .Where(x => !rndmove)
            .Where(x => player == null)
            .Where(x => !movestop)
            .Subscribe(_ => RandomMove());

        upStream.TakeUntilDestroy(this)
            .Where(x => ins.anim.GetBool("Destroy"))
            .Subscribe(_ => {
                GameManager.instance.MonsterDieName(gameObject.name);
                Destroy(gameObject);
                });
    }

    private void RandomMove()
    {
        rndmove = true;
        var x = Random.Range(-10, 10);
        var z = Random.Range(-10, 10);
        StartCoroutine(SlowMove(x,z));
    }

    IEnumerator SlowMove(int x, int z)
    {
        ins.anim.SetBool("Move", true);
        var count = 0f;

        while (player == null || !ins.anim.GetBool("Battle"))
        {
            var pos = new Vector3(x * (speed / 2) * Time.deltaTime, 0, z * (speed / 2) * Time.deltaTime);
            pos = transform.position + pos;
            var movepos = pos - transform.position;
            movepos = new Vector3(movepos.x, 0, movepos.z);
            transform.LookAt(pos);
            cc.SimpleMove(movepos);

            count += Time.deltaTime;
            if (count > 3f) break;
            yield return null;
        }
        ins.anim.SetBool("Move", false);

        if (!ins.anim.GetBool("Battle"))
        {
            ins.anim.SetBool("Sense", true);
            while (ins.anim.GetBool("Sense"))
            {
                yield return null;
            }
            rndmove = false;
        }
    }

    private void TargetMove()
    {
        ins.Damage = atk;
        var playerpos = player.transform.position;
        var enemypos = transform.position;
        var movepos = playerpos - enemypos;
        movepos = new Vector3(movepos.x, 0, movepos.z);

        distance = (playerpos - enemypos).magnitude;
        if (distance < stopDistance)
        {
            if(ins.anim.GetBool("Move")) ins.anim.SetBool("Move", false);
            if (ins.anim.GetBool("Sense")) ins.anim.SetBool("Sense", false);
            if (!ins.anim.GetBool("Action") && !ins.anim.GetBool("KnockBack"))
            {
                float sel = Random.Range(0f, 1.1f);
                var atkselect = sel <= 1f ? 0 : 1;

                ins.Damage = atkselect == 0 ? atk : atk * 2;

                ins.anim.SetInteger("Attack", atkselect);
                ins.anim.SetBool("Action", true);
                if(gameObject.name.Contains("Golem") && atkselect == 1)
                {
                    SEManager.instance.golemcry.Play();
                }
            }
            return;
        }

        if(!ins.anim.GetBool("Move")) ins.anim.SetBool("Move", true);
        transform.LookAt(player.transform.position);
        cc.SimpleMove(movepos * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            player = other.gameObject;
            ins.anim.SetBool("Battle", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            player = null;
            ins.anim.SetBool("Battle", false);
        }
    }

    private void DataLoad()
    {
        ins = GetComponent<EnemyStatus>();
        speed = ins.Data.Spd;
        atk = ins.Data.Atk;
    }
}
