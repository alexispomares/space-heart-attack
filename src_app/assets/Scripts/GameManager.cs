using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static int timeCountdown;
    public static bool isTurbo = false;

    public float difficulty = 0, timeToMaxDiff, maxEnemyProb, minTimeBoosts, beastFactor;
    public bool spawnPowerUps;
    public float initialTime, timeBetweenBoosts, restartWait;
    public Rigidbody road;
    public GameObject losingText;
    public GameObject[] spawnables;
    public float[] probabilities;
    public Text inputField;
    public Canvas canvas;
    [HideInInspector]
    public bool ok = false;

    float d, dDummy = 1000, difficDivide, timer;
    PlayerController playerController;
    EllipsoidParticleEmitter ep;
    LevelManager levelManager;

    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        levelManager = FindObjectOfType<LevelManager>();

        if (levelManager && levelManager.beastMode)
        {
            timeToMaxDiff /= (1.7f * beastFactor);
            maxEnemyProb *= beastFactor;
            minTimeBoosts /= beastFactor;
            dDummy *= 4f * beastFactor;
        }

        ep = playerController.GetComponentInChildren<EllipsoidParticleEmitter>();
        
        difficDivide = CalculateDifficulty(timeToMaxDiff);

        timer = 0f;
    }

    void Start ()
    {
        StartCoroutine(GameLoop());
    }

	public IEnumerator GameLoop ()
    {
        ResetGame();

        yield return StartCoroutine(GamePlaying(true));

        while (timeCountdown > 0)
        {
            yield return new WaitForSeconds(1);

            if (!isTurbo)
                timeCountdown -= 1;
            if (timeCountdown <= 0)
                yield return StartCoroutine(GamePlaying(false));
        }

        Instantiate(losingText);

        yield return new WaitForSeconds(restartWait / 2);

        bool isHighscored = levelManager.CheckHighscore(RoadManager.distance);

        if (isHighscored)
            yield return StartCoroutine(GetPlayerName());

        levelManager.playIntro = false;

        StartCoroutine(LoadSceneC("Game"));
    }

    IEnumerator GetPlayerName()
    {
        var anim = inputField.GetComponentInParent<Animator>();
        anim.SetBool("showHighscoreMenu", true);

        //while (!ok && !Input.GetKeyDown("enter"))
        while (!ok)
            yield return null;

        levelManager.StoreNamePlayer(inputField.text, RoadManager.distance);

        anim.SetBool("showHighscoreMenu", false);

        yield return new WaitForSeconds(restartWait / 2);
    }
    public void SetOk() {ok = true;}


    void ResetGame()
    {
        StartCoroutine(SpawnBoosts());

        timeCountdown = (int)initialTime;

        d = dDummy;

        InitialObjects();
    }
    
    void InitialObjects()
    {
        Instantiate(spawnables[0], new Vector3(0f, 0f, d), Quaternion.Euler(-90f, 0f, 0f));

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Initials"))
            g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    IEnumerator LoadSceneC(string s)
    {
        if (s == "Menu")
            levelManager.playIntro = true;

        var fading = GameObject.Find("LevelManager").GetComponent<Fading>();
        float fadeTime = fading.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);

        timeCountdown = (int)initialTime;
        d = 1000;

        var asyncOperation = SceneManager.LoadSceneAsync(s);
        while (!asyncOperation.isDone)
        {
            float p = Mathf.Round(asyncOperation.progress * 100);
            if (p < 89f)
                fading.percentage = p;
            else
                fading.percentage = 100;
            yield return null;
        }
    }

    IEnumerator GamePlaying(bool state)
    {
        spawnPowerUps = state;

        if (state)
        {
            //playerController.vSlope = 0.02f;
            playerController.eSlope = playerController.eSlopeDummy;
            playerController.vSpeed = playerController.vSpeedDummy;
            ep.maxEmission = 90f;
        }
        else
        {
            //playerController.vSlope /= 2f; //Make a bit longer the "salvation" time
            playerController.eSlope = 0f; //Stop exponential movement
            playerController.vSpeed = 0f; //Stop quadratic movement
            ep.maxEmission = 0f; // Turn ship engine off
        }

        while (!state && -road.velocity.z > 1 && !playerController.playerFallen)
        {
            if (timeCountdown > 0)
            {
                StartCoroutine(GamePlaying(true));
                yield break;
            }

            yield return null;
        }

        playerController.userInput = state;
        canvas.enabled = state;
    }

    void Update()
    {
        if (spawnPowerUps & RoadManager.distance > d)
        {
            Instantiate(spawnables[0], new Vector3(0f, 0f, 1000), Quaternion.Euler(-90f, 0f, 0f));
            d *= 4;
        }

        if (Input.GetKeyDown("escape"))
            StartCoroutine(ReturnToMenuC());
    }

    IEnumerator ReturnToMenuC()
    {
        bool isHighscored = levelManager.CheckHighscore(RoadManager.distance);

        if (isHighscored)
            yield return StartCoroutine(GetPlayerName());

        StartCoroutine(LoadSceneC("Menu"));
    }

    void LateUpdate()
    {
        timer += Time.deltaTime;
        difficulty = CalculateDifficulty(timer) / difficDivide;

        if (timeCountdown < 0)
            timeCountdown = 0;
    }

    IEnumerator SpawnBoosts()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenBoosts);

            if (spawnPowerUps)
            {
                int i = ChooseRandomWithProbabilities() + 1;

                Vector3 pos;
                Quaternion rot;

                if (i < probabilities.Length) //if (spawnables[i].CompareTag("PowerUp"))
                {
                    pos = new Vector3(Random.Range(playerController.xMin + 0.2f, playerController.xMax - 0.2f), 0f, 500);
                    rot = Quaternion.identity;
                }
                else
                {
                    i += Random.Range(0, 2 + 1);
                    pos = new Vector3(Random.Range(playerController.xMin + 0.5f, playerController.xMax - 0.5f), 0f, 500);
                    rot = Quaternion.Euler(20f, 180f, 0f);
                }

                Instantiate(spawnables[i], pos, rot);

                probabilities[probabilities.Length - 1] = Mathf.Lerp(probabilities[probabilities.Length - 1], maxEnemyProb, difficulty);
                timeBetweenBoosts = Mathf.Lerp(timeBetweenBoosts, minTimeBoosts, difficulty);

                //if (probabilities[probabilities.Length - 1] < 0.8f)
                //    probabilities[probabilities.Length - 1] += 0.02f;

                //if (timeBetweenBoosts > 0.6f)
                //    timeBetweenBoosts -= 0.15f;
            }
        }
    }

    int ChooseRandomWithProbabilities()
    {
        float total = 0;

        foreach (float i in probabilities)
            total += i;

        float r = Random.value * total;

        for (int i = 0; i < probabilities.Length; i++)
        {
            if (r < probabilities[i])
                return i;
            else
                r -= probabilities[i];
        }
        return probabilities.Length - 1;
    }

    float CalculateDifficulty(float x)
    {
        return Mathf.Pow(x, 2.5f);
    }
}
