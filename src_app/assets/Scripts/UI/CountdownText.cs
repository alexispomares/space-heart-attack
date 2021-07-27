using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownText : MonoBehaviour {

    Text text;
    Color green = new Color(83/255f, 1, 0, 1);
    Color grey = new Color(150/255f, 180/255f, 130/255f, 150/255f);

    void Start ()
    {
        text = GetComponent<Text>();
    }
	
	void Update ()
    {
        text.text = GameManager.timeCountdown + " s";

        if (!GameManager.isTurbo)
            text.color = Color.Lerp(text.color, green, 0.8f * Time.deltaTime);
        else
            text.color = Color.Lerp(text.color, grey, 0.8f * Time.deltaTime);
    }
}
