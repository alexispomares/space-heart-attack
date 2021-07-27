using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public bool followX;
    public bool followY;
    public bool followZ;

    Transform player;
    Vector3 offset;
    Vector3 axes;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        axes = new Vector3(followX ? 1 : 0, followY ? 1 : 0, followZ ? 1 : 0);

        transform.position = Vector3.Scale(player.position, axes) + offset;
    }
}
