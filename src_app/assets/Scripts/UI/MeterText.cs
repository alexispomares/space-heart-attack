using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeterText : MonoBehaviour {

    Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        float d = Mathf.Round(RoadManager.distance);

        if (d < 1000)
            text.text = d + " m";
        else if (d < 100000)
            text.text = Mathf.Round(d/100) / 10f + " Km";
        else if (d < LevelManager.maxCountableDistance)
            text.text = Mathf.Round(d/1000) + " Km";
        else
            text.text = "?&+$%!(_`^*-";
    }
}
