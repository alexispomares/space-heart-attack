using UnityEngine;
using System.Collections;

public class RunBackwards : MonoBehaviour {

    public static float eSlope;
    public static bool stop = false;

    public bool useExponential = true;

    PlayerController playerController;
    float vSpeed, v;
    Rigidbody rb;

    void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        rb = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        if (useExponential)
            v = -eSlope * Mathf.Exp(0.06f * playerController.heartRate); //if targetHR=88: v = -eSlope * Mathf.Exp(0.07f * playerController.heartRate);
        else
            v = -playerController.vSpeed * Mathf.Sqrt(1.5f * playerController.heartRate); //if targetHR=88: v = -playerController.vSpeed * Mathf.Sqrt(1.7f * playerController.heartRate); 

        v = Mathf.Lerp(rb.velocity.z, v, playerController.vSlope);

        if (rb.velocity.z == 0)
            stop = false;
        else if (stop)
            v = 0;

        rb.velocity = new Vector3(0f, 0f, v);
    }
    
}
