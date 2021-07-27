using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Menu currentMenu;

    public void Start()
    {
        ShowMenu(currentMenu);
    }

    public void ShowMenu(Menu menu)
    {
        if (currentMenu != null)
            currentMenu.IsOpen = false;

        currentMenu = menu;
        currentMenu.IsOpen = true;
    }

    public void LoadScene(string s)
    {
        StartCoroutine(LoadSceneC(s));
    }

    IEnumerator LoadSceneC(string s)
    {
        var fading = GameObject.Find("LevelManager").GetComponent<Fading>();
        float fadeTime = fading.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
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

    public void Exit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
            Application.Quit();
    }
}
