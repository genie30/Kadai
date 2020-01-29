using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Scene1 : MonoBehaviour
{
    public static Scene1 instance;

    public int MissionCount { get; set; }
    int ClearCount;
    int stage;

    [SerializeField]
    Transform mobs,porter;
    [SerializeField]
    GameObject maincamera, controller, result, clear, failure;
    [SerializeField]
    TMP_Text text;

    [SerializeField]
    CriAtomSource comp, fail;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        stage = GameManager.instance.StageNum;
        switch (stage)
        {
            case 1:
                ClearCount = 10;
                for (var i = 0; i < 100; i++)
                {
                    var rnd = Random.Range(0, 2);
                    MobSpawn(rnd);
                }
                break;
            case 2:
                ClearCount = 3;
                for (var i = 0; i < 30; i++)
                {
                    MobSpawn(2);
                }
                for (var i = 0; i < 70; i++)
                {
                    var rnd = Random.Range(0, 2);
                    MobSpawn(rnd);
                }
                break;
            case 3:
                ClearCount = 2;
                for (var i = 0; i < 20; i++)
                {
                    MobSpawn(3);
                }
                for (var i = 0; i < 80; i++)
                {
                    var rnd = Random.Range(0, 3);
                    MobSpawn(rnd);
                }
                break;
            case 4:
                ClearCount = 1;
                for (var i = 0; i < 2; i++)
                {
                    DragonSpawn(4);
                }
                for (var i = 0; i < 20; i++)
                {
                    MobSpawn(3);
                }
                for (var i = 0; i < 75; i++)
                {
                    var rnd = Random.Range(0, 3);
                    MobSpawn(rnd);
                }
                break;
        }
        TextRenew();
    }

    private void TextRenew()
    {
        switch (stage)
        {
            case 1:
                text.text = "クリア条件： モンスター討伐　" + MissionCount + " / 10匹";
                break;
            case 2:
                text.text = "クリア条件： ゴーレム討伐　" + MissionCount + " / 3匹";
                break;
            case 3:
                text.text = "クリア条件： レッドボア討伐　" + MissionCount + " / 2匹";
                break;
            case 4:
                text.text = "クリア条件： レッドドラゴン討伐　" + MissionCount + " / 1匹";
                break;
        }
    }

    public void Mission(string name)
    {
        switch (stage)
        {
            case 1:
                if (!name.Contains("Slime") && !name.Contains("Turtle")) return;
                MissionCount++;
                break;
            case 2:
                if (!name.Contains("Golem")) return;
                MissionCount++;
                break;
            case 3:
                if (!name.Contains("Boar")) return;
                MissionCount++;
                break;
            case 4:
                if (!name.Contains("Dragon")) return;
                MissionCount++;
                break;
        }
        TextRenew();

        if (MissionCount == ClearCount)
        {
            Destroy(mobs.gameObject);
            controller.SetActive(false);

            var initpos = maincamera.transform.position;
            var targetpos = porter.position + (Vector3.up * 30);
            var camcon = maincamera.gameObject.GetComponent<CameraController>();
            camcon.enabled = false;

            var seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.Append(maincamera.transform.DOLookAt(porter.position, 1f))
            .Append(maincamera.transform.DOMove(targetpos, 3f))
            .Append(maincamera.transform.DOLookAt(porter.position,1f));
            seq.AppendCallback(() => PorterController.instance.PorterSet());
            seq.AppendInterval(3f);
            seq.Append(maincamera.transform.DOMove(initpos, 0f));
            seq.AppendCallback(() => { camcon.enabled = true; controller.SetActive(true); });
            seq.Play();
        }
    }

    private void MobSpawn(int mobnum)
    {
        var pref = EnemyDataList.Instance.EnemyList[mobnum].prefab;
        var pos = Spawner.instance.NormalSpawn();

        Instantiate(pref, pos, Quaternion.identity, mobs);
    }

    private void DragonSpawn(int mobnum)
    {
        var pref = EnemyDataList.Instance.EnemyList[mobnum].prefab;
        var pos = Spawner.instance.BigSpawn();
        Instantiate(pref, pos, Quaternion.identity, mobs);
    }

    public void ShowResult(bool b)
    {
        controller.SetActive(false);

        var seq = DOTween.Sequence();
        if (b)
        {
            seq.AppendCallback(() => {
                result.SetActive(true);
                comp.Play();
                clear.SetActive(true);
            });
            seq.AppendInterval(0.5f);
            seq.Append(clear.transform.DOScale(new Vector3(3f,3f,1f), 1f));
            seq.Append(clear.transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutBounce));
            seq.AppendInterval(4f);
            seq.AppendCallback(() => GameManager.instance.SceneLoad("StageSelect"));
        }
        else
        {
            seq.AppendCallback(() => {
                result.SetActive(true);
                fail.Play();
                failure.SetActive(true);
            });
            seq.Append(failure.transform.DOLocalMove(Vector3.zero, 3f).SetEase(Ease.OutBounce));
            seq.AppendInterval(1f);
            seq.Append(failure.transform.DOLocalRotate(new Vector3(0f,0f,-20f) ,1f).SetEase(Ease.InExpo));
            seq.AppendInterval(2f);
            seq.AppendCallback(() => GameManager.instance.SceneLoad("StageSelect"));
        }
        seq.Play();
    }
}
