using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalculateRangeHR : MonoBehaviour {

    public InputField inputField;

    Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

	public void CalculateRange ()
    {
        if (inputField.text == "")
            text.text = "<i>Waiting to compute normal HR range...</i>";
        else
        {
            float min = 110 - Mathf.Round(0.5f * float.Parse(inputField.text));
            float max = 187 - Mathf.Round(0.85f * float.Parse(inputField.text));

            text.text = "<i>Normal HR range: </i><b><size=40>" + min + " - " + max + "</size></b>  <i>bpm.</i>";
        }
    }
}
