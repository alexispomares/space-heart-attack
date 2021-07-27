using UnityEngine;
using System.Collections;

public class ScrollingText : MonoBehaviour {

    public float speed;
    public float offset;
    public float initialPosition;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.localPosition = new Vector3(offset * initialPosition, 0, 0);
    }

	void LateUpdate ()
    {
        rectTransform.localPosition -= new Vector3(speed * Time.deltaTime, 0, 0);

        if (rectTransform.localPosition.x < -offset)
            rectTransform.localPosition = new Vector3(offset, 0, 0);
    }
}
