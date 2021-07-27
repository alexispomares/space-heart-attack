using UnityEngine;
using System.Collections;

public class SunMovement : MonoBehaviour
{
    public bool dayNightCycle;
    public float dayDuration;
    public SpinFree speedReference;
    public float earthDumping;
    public Transform playerTransform;
    public float stopSunDistance;

    float xRot = 50;
    float yRot = -30;
    Vector3 point;

    void Start()
    {
        point = new Vector3(0f, 2.5f, -4.5f);
        //transform.RotateAround(Vector3.zero, new Vector3(yRot, 0, 0), 135);
    }
    
    void Update ()
    {
        if (dayNightCycle && playerTransform.position.y > stopSunDistance)
        {
            float deltaX = - (360 / dayDuration + Mathf.Pow(speedReference.speed, 2f) / earthDumping) * Time.deltaTime;
            
            if (transform.position.y < -3)
                deltaX *= 5;

            xRot = Mathf.Repeat(xRot + deltaX, 360);

            transform.RotateAround(point, new Vector3(yRot, 0, 0), deltaX);
            transform.LookAt(point);
        }
    }
}

