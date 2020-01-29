using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName ="EnemyData")]
public class EnemyDataList : ScriptableObject
{
    public List<EnemyData> EnemyList = new List<EnemyData>();

    private static EnemyDataList instance;
    public static EnemyDataList Instance
    {
        get
        {
            if(instance == null)
            {
                instance = Resources.Load<EnemyDataList>("EnemyList");
            }
            return instance;
        }
    }
}

[System.Serializable]
public class EnemyData
{
    public string Name;
    public int HP;
    public float Spd;
    public int Atk;
    public GameObject prefab;

    public EnemyData() { }
    public EnemyData(EnemyData data)
    {
        Name = data.Name;
        HP = data.HP;
        Spd = data.Spd;
        Atk = data.Atk;
        prefab = data.prefab;
    }
}