using UnityEngine;
using System.Collections;

public class RoadManager : MonoBehaviour
{
    public static float distance;

    public float tileLength;
    public Material roadMaterial;

    float initialZ;
    Rigidbody rb;

    void Start()
    {
        distance = 0;

        rb = GetComponent<Rigidbody>();

        initialZ = transform.localScale.y / 2.1f;
        transform.position = new Vector3(0f, -1.5f, initialZ);

        roadMaterial.SetTextureScale("_MainTex", new Vector2(1, transform.localScale.y / tileLength * 0.5f));
    }

    void Update()
    {
        distance += -rb.velocity.z * Time.deltaTime;

        if (transform.position.z < initialZ - tileLength * 100)
            transform.position += new Vector3(0f, 0f, tileLength * 100);

        if (transform.position.z < -2000)
            transform.position = new Vector3(0f, -1.5f, 2380.941f);
    }

}
