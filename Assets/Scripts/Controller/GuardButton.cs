using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GuardButton : MonoBehaviour
{
    public static bool Guard;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2)) Guard = true;
        if (Input.GetKeyUp(KeyCode.Keypad2)) Guard = false;
    }

    public void OnDown()
    {
        Guard = true;
    }

    public void OnUp()
    {
        Guard = false;
    }
}
