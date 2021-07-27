using UnityEngine;
using System.Collections;

public class Rotate3D : MonoBehaviour {

    public float rotationSpeed;
    public bool localRotation;

    Vector3 axes;

    void Start()
    {
        if (localRotation)
            axes = transform.up;
        else
            axes = Vector3.up;
    }

	void Update ()
    {
        transform.RotateAround(transform.position, axes, rotationSpeed * Time.deltaTime);
	}
}
