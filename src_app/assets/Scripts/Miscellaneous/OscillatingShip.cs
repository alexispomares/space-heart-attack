using UnityEngine;
using System.Collections;

public class OscillatingShip : MonoBehaviour {

    public float speed, range;

    float initX;
    LevelManager levelManager;

    void Start ()
    {
        levelManager = FindObjectOfType<LevelManager>();
        initX = transform.position.x;
	}
	
	void LateUpdate ()
    {
        if (levelManager && levelManager.beastMode)
            transform.position = new Vector3(initX + range * Mathf.Sin(speed * Time.time), transform.position.y, transform.position.z);
	}
}
