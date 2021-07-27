using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float heartRate;
    [Space(10)]
    public float tilt;
    public float xMin, xMax, vSlope, hSlope, vSpeed, hSpeed, fallingSpeed, fallingSlope;
    public AudioClip timeGoalClip, powerUpCollectedClip, redPowerUp;
    public AudioClip[] explosionClip;
    public Canvas canvas;
    public CameraFollow camFollow;
    
    [HideInInspector]
    public bool userInput;
    [HideInInspector]
    public float h;
    [HideInInspector]
    public float eSlope;
    [HideInInspector]
    public float eSlopeDummy;
    [HideInInspector]
    public bool boostSlope;
    [HideInInspector]
    public float boostMultiplier;
    [HideInInspector]
    public float vSpeedDummy;
    [HideInInspector]
    public float yCoord = 0;
    [HideInInspector]
    public float zCoord = 0;
    [HideInInspector]
    public bool playerFallen = false;

    Rigidbody rb;
    LevelManager levelManager;
    float xVelocityDummy;

	void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        levelManager = FindObjectOfType<LevelManager>();

        eSlopeDummy = 0.15f;

        if (levelManager && levelManager.beastMode)
        {
            eSlopeDummy *= 1.2f;
            vSpeed *= 1.2f;
            hSlope *= 1.1f;
            tilt *= 1.1f;
        }

        vSpeedDummy = vSpeed;
        eSlope = eSlopeDummy;
    }

    void FixedUpdate()
    {
        if (!playerFallen)
        {
            if (!userInput)
                h = 0;

            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, h * hSpeed, hSlope), 0f, 0f);
            xVelocityDummy = rb.velocity.x;
            rb.rotation = Quaternion.Euler(0f, 0f, xVelocityDummy * -tilt);
        }
        else
        { 
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, fallingSpeed, fallingSlope), 0f);
            xVelocityDummy = Mathf.Lerp(xVelocityDummy, h * hSpeed, hSlope);
            rb.rotation = Quaternion.Euler(0f, 0f, xVelocityDummy * -tilt);
        }

        if (!levelManager || !levelManager.beastMode)
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, xMin, xMax), rb.position.y, rb.position.z);
    }

    void LateUpdate()
    {
        if (levelManager)
            heartRate = levelManager.heartRate;

        if (boostSlope)
        {
            GameManager.isTurbo = true;
            RunBackwards.eSlope = eSlope * boostMultiplier;
            vSpeed = vSpeedDummy * boostMultiplier / 10;
        }
        else
        {
            GameManager.isTurbo = false;
            RunBackwards.eSlope = eSlope;
            vSpeed = vSpeedDummy;
        }

        if (levelManager && levelManager.beastMode)
        {
            if (!playerFallen && (transform.position.x < (xMin - 1) || transform.position.x > (xMax + 1)))
                PlayerFall();

            transform.position = new Vector3(transform.position.x, transform.position.y, zCoord);
        }
        else
            transform.position = new Vector3(transform.position.x, yCoord, zCoord);
    }

    void PlayerFall()
    {
        canvas.enabled = false;
        GameManager.timeCountdown = 0;
        playerFallen = true;
        camFollow.followY = true;
    }

    //Remember: These functions are only for when timeCountdown = 0
    public void TemporalAcc(float t)
    {
        StartCoroutine(TemporalAccC(t));
    }

    IEnumerator TemporalAccC(float t)
    {
        eSlope = eSlopeDummy;
        yield return new WaitForSeconds(t);
        eSlope = 0f;
    }

}
