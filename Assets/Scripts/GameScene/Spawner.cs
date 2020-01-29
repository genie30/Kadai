using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    [SerializeField]
    Transform PlayerSpawner;
    [SerializeField]
    List<Transform> NormalSpawner, BigSpawner;

    public static Vector3 PlayerSpawn { get; private set; }

    private void Awake()
    {
        instance = this;
        PlayerSpawn = PlayerSpawner.position;
    }

    public Vector3 NormalSpawn()
    {
        var rnd = Random.Range(0, NormalSpawner.Count);
        return NormalSpawner[rnd].position;
    }

    public Vector3 BigSpawn()
    {
        var rnd = Random.Range(0, BigSpawner.Count);
        return BigSpawner[rnd].position;
    }
}
