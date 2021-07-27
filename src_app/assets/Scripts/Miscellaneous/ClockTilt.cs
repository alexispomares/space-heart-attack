using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClockTilt : MonoBehaviour {

    public int tiltTimeLimit;

    Image img;
    Animator anim;
    RectTransform rectTransform;
    Vector2 initSize;
    Color green = new Color(83 / 255f, 1, 0, 1);
    Color grey = new Color(150 / 255f, 180 / 255f, 130 / 255f, 150 / 255f);

    void Awake ()
    {
        img = GetComponent<Image>();
        anim = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();

        initSize = rectTransform.sizeDelta;
	}
	
	void Update ()
    {
        if (GameManager.timeCountdown > tiltTimeLimit || GameManager.timeCountdown <= 0)
        {
            if (anim.enabled)
            {
                rectTransform.sizeDelta = initSize;
                rectTransform.rotation = Quaternion.identity;
            }
            anim.enabled = false;
        }
        else
            anim.enabled = true;

        if (!GameManager.isTurbo)
            img.color = Color.Lerp(img.color, green, 0.8f * Time.deltaTime);
        else
            img.color = Color.Lerp(img.color, grey, 0.8f * Time.deltaTime);
    }
}
