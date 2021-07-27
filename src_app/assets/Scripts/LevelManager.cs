using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{
    static public int maxHighscores = 20, maxCountableDistance = 10000000;

    public Highscores highscoreScript;
    public float heartRate;
    [Space(10)]
    public Text[] texts;
    public bool playIntro, useAccelerometer, erasePlayerPrefs, beastMode;
    public Toggle toggle;

    Highscore[] highList;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        beastMode = PlayerPrefs.GetInt("BeastMode", 0) == 1;
        toggle.isOn = beastMode;

        string temp = "Position\n";
        for (int i = 1; i < (maxHighscores + 1f); i++)
            temp += i + ".\n";
        texts[0].text = temp.Substring(0, temp.Length - 1);

        if (erasePlayerPrefs)
            PlayerPrefs.DeleteAll();

        if (PlayerPrefs.GetInt("FirstLaunch", 0) == 0)
        {
            PlayerPrefs.SetInt("FirstLaunch", 1);
        }
    }

    void Start()
    {
        highscoreScript.maxHighscores = maxHighscores;

        StartCoroutine(RefreshHighscores(true));
    }

    IEnumerator RefreshHighscores(bool repeat)
    {
        do
        {
            highscoreScript.DownloadHighscores();

            yield return new WaitForSeconds(2f);

            highList = highscoreScript.highscoresList;

            try
            { SetHighscoreText(); }
            catch { }

            yield return new WaitForSeconds(8f);

        } while (repeat);
    }

    public bool CheckHighscore(float distance)
    {
        if (highList != null)
            for (int i = 0; i < highList.Length; i++)
            {
                if (distance > highList[i].score)
                    return true;
            }

        return false;
    }

    public void StoreNamePlayer(string namePlayer, float distance)
    {
        if (namePlayer == "Enter your name...")
            namePlayer = "???";

        highscoreScript.AddNewHighscore(namePlayer, Mathf.Round(distance / 1000f));
    }

    void SetHighscoreText()
    {
        texts[0].text = "<color=yellow>Position</color>\n";
        texts[1].text = "<color=yellow>Name</color>\n";
        texts[2].text = "<color=yellow>Score</color>\n";

        for (int i = 0; i < maxHighscores; i++)
        {
            string scoreString, nameString;

            float d = Mathf.Round(highList[i].score);
            if (d < 1000)
                scoreString = d + " m";
            else if (d < 100000)
                scoreString = Mathf.Round(d / 100) / 10f + " Km";
            else if (d < maxCountableDistance)
                scoreString = Mathf.Round(d / 1000) + " Km";
            else
                scoreString = "Uncountable";

            nameString = highList[i].username;

            texts[0].text += (i + 1).ToString() + ".\n";
            texts[1].text += nameString + "\n";
            texts[2].text += scoreString + "\n";
        }
    }

    void LateUpdate()
    {
        if (heartRate < 40)
            heartRate = 40;
        else if (heartRate > 220)
            heartRate = 220;
    }

    public void SetBeastMode(bool b)
    {
        beastMode = b;
        PlayerPrefs.SetInt("BeastMode", b ? 1 : 0);

        if (!b)
        {
            highscoreScript.privateCode = Highscores.privateCode_noob;
            highscoreScript.publicCode = Highscores.publicCode_noob;
        }
        else
        {
            highscoreScript.privateCode = Highscores.privateCode_beast;
            highscoreScript.publicCode = Highscores.publicCode_beast;
        }

        StartCoroutine(RefreshHighscores(false));
    }
}