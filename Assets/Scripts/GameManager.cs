using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private const string Loading = "Loading";
    private string scenename;
    private bool next = false;
    public int StageNum = 0;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void SceneLoad(string scene)
    {
        scenename = scene;
        SceneManager.LoadSceneAsync(Loading);
    }

    public void LoadWait(Slider slider)
    {
        StartCoroutine(LoadingScene(slider));
    }

    public void NextScene()
    {
        next = true;
    }

    IEnumerator LoadingScene(Slider slider)
    {
        var async = SceneManager.LoadSceneAsync(scenename);
        async.allowSceneActivation = false;

        while(!next)
        {
            slider.value = async.progress;
            yield return null;
        }
        next = false;
        async.allowSceneActivation = true;
    }

    public void MonsterDieName(string name)
    {
        if(StageNum < 5) Scene1.instance.Mission(name);
    }
}
