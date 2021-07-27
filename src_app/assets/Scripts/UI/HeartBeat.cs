using UnityEngine;
using System.Collections;

public class HeartBeat : MonoBehaviour {

    public bool realTimeBeat = false;

    LevelManager levelManager;
    float initScale;

	void Start ()
    {
        levelManager = FindObjectOfType<LevelManager>();

        initScale = transform.localScale.x;
    }

    void Update ()
    {
        float value = 1;

        if (realTimeBeat)
            value = initScale * (1f + 0.1f * Mathf.Sin(Time.time * 2 * Mathf.PI * levelManager.heartRate / 60));
        else if (levelManager)
            value = 1f + 0.1f * Mathf.Sin(Time.time * Mathf.Pow(Mathf.Round(levelManager.heartRate / 10) * 10f, 2f) / 800);

        transform.localScale = new Vector3 (value, value, 1);
    }
}
