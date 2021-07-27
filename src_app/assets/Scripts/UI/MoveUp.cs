using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveUp : MonoBehaviour {

    public float upSpeed, offset, lifetime;

    Text text;
    float timer = 0f;

    void Start()
    {
        text = GetComponent<Text>();

        Destroy(gameObject, lifetime);
    }

    void Update ()
    {
        transform.position += new Vector3(0f, upSpeed * Screen.height / 280, 0f);

        timer += Time.deltaTime;
        if (timer > offset)
            text.CrossFadeAlpha(0f, lifetime - offset, false); 
	}
}
