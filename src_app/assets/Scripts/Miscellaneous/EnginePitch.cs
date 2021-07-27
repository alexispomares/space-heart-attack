using UnityEngine;
using System.Collections;

public class EnginePitch : MonoBehaviour
{
    public float sensitivity;

    Rigidbody rb;
    AudioSource audioSource;

	void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
    void Update()
    { 
	    audioSource.pitch = 1 + rb.velocity.x * sensitivity / 100;
    }
}
