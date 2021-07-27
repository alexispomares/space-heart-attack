using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroAnimation : MonoBehaviour {

    public bool playIntro;
    public float initialWait;
    public GameObject[] gameObjects;
    public MonoBehaviour[] scripts;
    public Text readyGoText;
    [HideInInspector]
    public float alphaBackground = 1;

    LevelManager levelManager;
    Animator animator;

    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();

        if (levelManager)
            playIntro = levelManager.playIntro;

        for (int i = 0; i < gameObjects.Length; i++)
            gameObjects[i].SetActive(false);

        for (int i = 0; i < scripts.Length; i++)
            scripts[i].enabled = false;

        animator = GetComponent<Animator>();
    }

    void Start ()
    {
        if (playIntro)
            animator.Play("CameraZoomBeginning", 0, 0);
        else
            animator.Play("CameraZoomBeginning", 0, 1);
    }

    IEnumerator GameStart()
    {
        EnableRoad();

        StartCoroutine(ReadyGo(0.2f));

        yield return new WaitForSeconds(initialWait);

        for (int i = 1; i < gameObjects.Length; i++)
            gameObjects[i].SetActive(true);

        for (int i = 0; i < scripts.Length; i++)
            scripts[i].enabled = true;
    }

    IEnumerator ReadyGo(float t)
    {
        yield return new WaitForSeconds(initialWait * 1f / 4f);

        readyGoText.text = "READY ?";
        readyGoText.CrossFadeAlpha(0.01f, 0f, false);
        readyGoText.enabled = true;
        readyGoText.CrossFadeAlpha(1f, t, false);

        yield return new WaitForSeconds(initialWait *  3f / 4f);

        readyGoText.text = "GO !";

        yield return new WaitForSeconds(initialWait * 1.5f / 4f);

        readyGoText.CrossFadeAlpha(0.01f, t, false);
        yield return new WaitForSeconds(t);
        readyGoText.enabled = false;
    }

    void EnableRoad()
    {
        gameObjects[0].SetActive(true);
    }

}
