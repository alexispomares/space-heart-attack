using UnityEngine;
using System.Collections;

public class BackgroundFollow : MonoBehaviour
{
    Transform cam;
    Vector3 offset;

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        offset = transform.position - cam.position;
    }

    void LateUpdate()
    {
        transform.position = cam.position + offset;
    }
}
