using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HRText : MonoBehaviour {

    LevelManager levelManager;
    Text text;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

        text = GetComponent<Text>();
    }

    void Update()
    {
        if (levelManager)
            text.text = Mathf.Round(levelManager.heartRate) + " bpm";
    }
}
