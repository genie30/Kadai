using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float maxz = -0.5f;
    private const float minz = -15f;
    private float distance;

    [SerializeField]
    private Transform lookpoint;
    [SerializeField]
    private float scrollSensitivity = 0.001f;
    private float bdis;

    private void Start()
    {
        distance = transform.localPosition.z;
    }

    private void Update()
    {
        transform.LookAt(lookpoint);
        if (Input.touchCount >= 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);
            if (t2.phase == TouchPhase.Began)
            {
                bdis = Vector2.Distance(t1.position, t2.position);
            }
            if (t1.phase == TouchPhase.Moved && t2.phase == TouchPhase.Moved)
            {
                var ndis = Vector2.Distance(t1.position, t2.position);
                updateDistance((bdis - ndis) * -5);
                bdis = ndis;
            }
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel") * 1000;
        updateDistance(scroll);
    }

    void updateDistance(float scroll)
    {
        scroll = distance - scroll * scrollSensitivity;
        distance = Mathf.Clamp(scroll, minz, maxz);
        transform.localPosition = new Vector3(0f, 3.5f, distance);
    }
}
