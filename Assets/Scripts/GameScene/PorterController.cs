using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorterController : MonoBehaviour
{
    public static PorterController instance;

    [SerializeField]
    GameObject porter;
    [SerializeField]
    Collider col;
    [SerializeField]
    ParticleSystem part;
    [SerializeField]
    CriAtomSource porton;

    private void Awake()
    {
        instance = this;
    }

    public void PorterSet()
    {
        porter.SetActive(true);
        col.enabled = true;
        var main = part.main;
        main.simulationSpeed = 1f;
        part.Play();
        porton.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            other.gameObject.GetComponent<PlayerController>().enabled = false;
            Scene1.instance.ShowResult(true); 
        }
    }
}
