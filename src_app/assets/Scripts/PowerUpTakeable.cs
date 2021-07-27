using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PowerUpTakeable : MonoBehaviour {

    public enum PowerUpType { TimeGoal, Green, Yellow, Red, Enemy };
    public PowerUpType powerUpType;

    public float timeAttribute, distanceAttribute;
    public GameObject textPrefab, explosionPrefab;

    GameObject HUDCanvas;
    PlayerController playerController;
    AudioSource audioSource;
    Animator animator;
    Rigidbody rb;
    bool check = true, effectCheck = true;
    
    void Start ()
    {
        HUDCanvas = GameObject.Find("HUDCanvas");

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        audioSource = playerController.GetComponent<AudioSource>();
        animator = playerController.GetComponent<Animator>();
        
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (check && (transform.position.z + rb.velocity.z * Time.fixedDeltaTime) < 0)
        {
            check = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
    }
	
	void OnTriggerEnter (Collider other)
    {
        if (effectCheck && other.CompareTag("Player"))
        {
            effectCheck = false;

            if (powerUpType == PowerUpType.TimeGoal)
                audioSource.clip = playerController.timeGoalClip;
            else if (powerUpType == PowerUpType.Enemy)
                audioSource.clip = playerController.explosionClip[Random.Range(0, playerController.explosionClip.Length - 1)];
            else if (powerUpType == PowerUpType.Red)
                audioSource.clip = playerController.redPowerUp;
            else
                audioSource.clip = playerController.powerUpCollectedClip;

            audioSource.pitch = Random.Range(0.97f, 1.03f);
            audioSource.Play();

            DisplayEffect();
            DoEffect();
            
            Destroy(gameObject);
        }
	}

    void DoEffect()
    {
        switch (powerUpType)
        {
            case PowerUpType.TimeGoal:
                GameManager.timeCountdown += (int) timeAttribute;
                break;
            case PowerUpType.Enemy:
                GameManager.timeCountdown -= (int)timeAttribute;
                animator.SetTrigger("brakeShip");
                RunBackwards.stop = true;
                Instantiate(explosionPrefab, new Vector3 (transform.position.x, transform.position.y, 0f), Quaternion.identity);
                break;
            case PowerUpType.Green:
                GameManager.timeCountdown += (int) timeAttribute;
                break;
            case PowerUpType.Yellow:
                RoadManager.distance += timeAttribute * 1000 + RoadManager.distance * distanceAttribute;
                break;
            case PowerUpType.Red:
                if (playerController.eSlope == 0f)
                    playerController.TemporalAcc(timeAttribute);
                playerController.boostMultiplier = distanceAttribute;
                animator.SetFloat("animMultiplier", 5f / timeAttribute);
                animator.SetTrigger("accelShip");
                break;
        }
    }

    void DisplayEffect()
    {
        Vector3 textLocation = Camera.main.WorldToViewportPoint(audioSource.GetComponentInParent<Transform>().position);
        if (powerUpType != PowerUpType.TimeGoal)
            textLocation.y += 0.2f;
        textLocation = Camera.main.ViewportToScreenPoint(textLocation);

        GameObject prefab = (GameObject) Instantiate(textPrefab, Vector3.zero, Quaternion.identity);
        prefab.transform.SetParent(HUDCanvas.transform, false);
        prefab.transform.position = textLocation;

        Text text = prefab.GetComponent<Text>();

        switch (powerUpType)
        {
            case PowerUpType.TimeGoal:
                text.text = "+ " + timeAttribute + " s";
                text.color = new Color(250, 0, 216);
                break;
            case PowerUpType.Enemy:
                text.text = "- " + timeAttribute + " s";
                text.color = Color.red;
                break;
            case PowerUpType.Green:
                text.text = "+ " + timeAttribute + " s";
                text.color = Color.green;
                break;
            case PowerUpType.Yellow:
                float d = Mathf.Round(timeAttribute * 1000 + RoadManager.distance * distanceAttribute);
                
                if (d < 1000)
                    text.text = "+ " + d + " m";
                else if (d < 10000)
                    text.text = "+ " + Mathf.Round(d / 100f) / 10f + " Km";
                else if (d < LevelManager.maxCountableDistance)
                    text.text = "+ " + Mathf.Round(d / 1000f) + " Km";

                text.color = Color.yellow;
                break;
            case PowerUpType.Red:
                text.text = "x " + distanceAttribute;
                text.color = Color.red;
                break;
        }
    }

}

#if UNITY_EDITOR
[CustomEditor( typeof(PowerUpTakeable) )]
 public class MyEditor : Editor
 {
     public override void OnInspectorGUI()
     {
        PowerUpTakeable script = (PowerUpTakeable) target;

        script.powerUpType = (PowerUpTakeable.PowerUpType) EditorGUILayout.EnumPopup("Power Up Type", script.powerUpType);
        script.textPrefab = (GameObject) EditorGUILayout.ObjectField("CollectionText Prefab", script.textPrefab, (typeof(GameObject)), true);
        script.explosionPrefab = (GameObject) EditorGUILayout.ObjectField("Explosion Prefab", script.explosionPrefab, (typeof(GameObject)), true);

        switch (script.powerUpType.ToString())
        {
            case "TimeGoal":
                script.timeAttribute = EditorGUILayout.FloatField("Time boost", script.timeAttribute);
                break;
            case "Enemy":
                script.timeAttribute = EditorGUILayout.FloatField("Time lost", script.timeAttribute);
                script.distanceAttribute = EditorGUILayout.FloatField("Velocity brake", script.distanceAttribute);
                break;
            case "Green":
                script.timeAttribute = EditorGUILayout.FloatField("Extra time", script.timeAttribute);
                break;
            case "Yellow":
                script.timeAttribute = EditorGUILayout.FloatField("Km sum", script.timeAttribute);
                script.distanceAttribute = EditorGUILayout.FloatField("Distance multiplier", script.distanceAttribute);
                break;
            case "Red":
                script.distanceAttribute = EditorGUILayout.FloatField("Velocity boost", script.distanceAttribute);
                script.timeAttribute = EditorGUILayout.FloatField("Duration time", script.timeAttribute);
                break;
        }
    }
}
#endif