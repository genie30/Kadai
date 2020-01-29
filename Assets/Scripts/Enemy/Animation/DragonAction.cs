using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class DragonAction : MonoBehaviour
{
    [SerializeField]
    CharacterController cc;
    [SerializeField]
    GameObject player = null;

    EnemyStatus ins;
    float distance = 999;
    [SerializeField]
    float stopDistance = 4f;
    bool rndmove = false;

    float speed;
    int atk;
    public bool movestop;
    bool firstaction;

    private void Start()
    {
        DataLoad();

        var upStream = this.UpdateAsObservable().TakeUntilDestroy(this).Share();

        upStream.TakeUntilDestroy(this)
            .Where(x => player != null)
            .Where(x => !ins.anim.GetBool("Fly"))
            .Subscribe(_ => TargetMove());

        upStream.TakeUntilDestroy(this)
            .Where(x => ins.anim.GetBool("Fly"))
            .Where(x => !movestop)
            .Subscribe(_ => FlyMove());

        upStream.TakeUntilDestroy(this)
            .Where(x => !rndmove)
            .Where(x => player == null)
            .Where(x => !movestop)
            .Where(x => !ins.anim.GetBool("Fly"))
            .Subscribe(_ => RandomMove());

        upStream.TakeUntilDestroy(this)
            .Where(x => ins.anim.GetBool("Destroy"))
            .Subscribe(_ => {
                GameManager.instance.MonsterDieName("Dragon");
                Destroy(gameObject); 
            });
    }

    private void RandomMove()
    {
        rndmove = true;
        var x = Random.Range(-10, 10);
        var z = Random.Range(-10, 10);
        StartCoroutine(SlowMove(x, z));
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
        if (player == null)
        {
            AnimReset();
            return;
        }
        var playerpos = player.transform.position;
        var enemypos = transform.position;
        var movepos = playerpos - enemypos;
        movepos = new Vector3(movepos.x, 0, movepos.z);

        distance = (playerpos - enemypos).magnitude;
        if (distance < stopDistance)
        {
            if (ins.anim.GetBool("Move")) ins.anim.SetBool("Move", false);
            if (ins.anim.GetBool("Sense")) ins.anim.SetBool("Sense", false);
            if (!ins.anim.GetBool("Action") && !ins.anim.GetBool("KnockBack"))
            {
                float sel = Random.Range(0f, 2.8f);
                if(sel > 2.6f)
                {
                    ins.anim.SetBool("Fly", true);
                    return;
                }
                var atkselect = (int)sel;

                ins.Damage = atk;
                if (atkselect == 1) ins.Damage = (int)(atk * 1.2f);
                if (atkselect == 2) ins.Damage = (int)(atk * 1.7f);

                ins.anim.SetInteger("Attack", atkselect);
                ins.anim.SetBool("Action", true);
                if (atkselect == 2) ins.anim.SetTrigger("Breath");
            }
            return;
        }

        if (!ins.anim.GetBool("Move")) ins.anim.SetBool("Move", true);
        transform.LookAt(player.transform.position);
        cc.SimpleMove(movepos * speed * Time.deltaTime);
    }

    private void FlyMove()
    {
        ins.Damage = atk;
        if(player == null)
        {
            AnimReset();
            firstaction = false;
            return;
        }
        var playerpos = player.transform.position;
        var enemypos = transform.position;
        var movepos = playerpos - enemypos;
        movepos = new Vector3(movepos.x, 0, movepos.z);

        distance = (playerpos - enemypos).magnitude;
        if (distance < stopDistance)
        {
            if (ins.anim.GetBool("Move")) ins.anim.SetBool("Move", false);
            if (ins.anim.GetBool("Sense")) ins.anim.SetBool("Sense", false);
            if (!ins.anim.GetBool("Action") && !ins.anim.GetBool("KnockBack"))
            {
                float sel = Random.Range(0f, 2.4f);
                var atkselect = (int)sel; // 0:降りる 1:継続 2:ブレス
                if (!firstaction)
                {
                    atkselect = 2;
                    firstaction = true;
                }

                switch (atkselect)
                {
                    case 0:
                        ins.anim.SetBool("Fly", false);
                        firstaction = false;
                        break;

                    case 1:
                        StartCoroutine(FlyWait());
                        break;

                    case 2:
                        StartCoroutine(FlyingBreath());
                        ins.Damage = (int)(atk * 1.7f);
                        break;
                }
            }
            transform.DOLookAt(player.transform.position, 0.5f);
            return;
        }

        if (!ins.anim.GetBool("Move")) ins.anim.SetBool("Move", true);
        transform.LookAt(player.transform.position);
        cc.SimpleMove(movepos * speed * 2 * Time.deltaTime);
    }

    IEnumerator FlyWait()
    {
        movestop = true;
        yield return new WaitForSeconds(2);
        movestop = false;
    }

    IEnumerator FlyingBreath()
    {
        movestop = true;
        yield return new WaitForSeconds(1f);
        var targetpos = player.transform.position;
        yield return new WaitForSeconds(1f);
        distance = (targetpos - transform.position).magnitude;
        ins.anim.SetBool("Action", true);
        while (distance > stopDistance)
        {
            cc.SimpleMove((targetpos - transform.position) * speed * Time.deltaTime);
            distance = (targetpos - transform.position).magnitude;
            yield return null;
        }
        ins.anim.SetTrigger("Breath");

        while (ins.anim.GetBool("Action"))
        {
            yield return null;
        }
        movestop = false;
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
        if (other.tag == "Player")  
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

    private void AnimReset()
    {
        ins.anim.SetBool("Fly", false);
        ins.anim.SetBool("Action", false);
        ins.anim.ResetTrigger("Breath");
    }
}
