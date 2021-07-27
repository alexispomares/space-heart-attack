using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TakeScreenshots : MonoBehaviour {

    public int superSize = 1;
    public bool resetIndex = false;
    public float waitTime;

    bool done = false;

    void Awake()
    {
        if (resetIndex)
            PlayerPrefs.SetInt("screenshot", 0);
    }

    void OnGUI()
    {
        if (!done && Input.GetMouseButtonDown(1))
        {
            done = true;
            if (Application.isPlaying)
                Invoke("SetDone", waitTime);
            else
                SetDone();

            int i = PlayerPrefs.GetInt("screenshot", 0);
            ScreenCapture.CaptureScreenshot("Screenshot_" + i + ".png", superSize);
            PlayerPrefs.SetInt("screenshot", i + 1);

            print("Done " + i);
        }
    }

    void SetDone()
    {
        done = false;
    }
}
