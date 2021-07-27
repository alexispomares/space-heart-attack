using UnityEngine;
using System.Collections;

public class ScreenSideTouch : MonoBehaviour {

    public bool useAccelerometer;

    PlayerController pc;

    void Awake ()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        var lm = GameObject.Find("LevelManager");
        if (lm)
            useAccelerometer = lm.GetComponent<LevelManager>().useAccelerometer;
    }
	
	void Update ()
    {
        if (useAccelerometer)
            pc.h = Mathf.Clamp(Input.acceleration.x * 2.5f, -1f, 1f);
        else if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.position.x < Screen.width / 2)
                pc.h = -1;
            else
                pc.h = 1;
        }
        else
            pc.h = Input.GetAxis("Horizontal");
    }
}
