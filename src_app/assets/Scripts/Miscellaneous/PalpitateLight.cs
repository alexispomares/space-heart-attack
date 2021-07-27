using UnityEngine;
using System.Collections;

public class PalpitateLight : MonoBehaviour {

    public float speed;
    public float range;

    Light lightObj;
    float initValue;

    void Start ()
    {
        lightObj = GetComponent<Light>();
        initValue = lightObj.intensity;
    }
	
	void Update ()
    {
        lightObj.intensity = initValue + Mathf.Sin(Time.time * speed) * range;
	}
}
