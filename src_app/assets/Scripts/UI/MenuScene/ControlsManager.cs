using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour {

    public CanvasGroup cg;
    public Button touchButton, tiltButton;

    bool useAccelerometer = false;
    LevelManager levelManager;

    void Awake()
    {
        int boolean = PlayerPrefs.GetInt("useAccelerometer", 0);
        levelManager = FindObjectOfType<LevelManager>();

        if (boolean == 0)
            levelManager.useAccelerometer = false;
        else
            levelManager.useAccelerometer = true;
    }

    public void TouchClick(bool b)
    {
        if (b)
            useAccelerometer = false;
        else
            useAccelerometer = true;
    }

    public void SaveAndExit()
    {
        levelManager.useAccelerometer = useAccelerometer;

        if (useAccelerometer)
            PlayerPrefs.SetInt("useAccelerometer", 1);
        else
            PlayerPrefs.SetInt("useAccelerometer", 0);
    }

    void Update()
    {
        if (cg.alpha == 1)
        {
            if (useAccelerometer)
                tiltButton.Select();
            else
                touchButton.Select();
        }
    }

}
