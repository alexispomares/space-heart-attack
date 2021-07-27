using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Highscores : MonoBehaviour {

    [HideInInspector]
    public string privateCode, publicCode;
    public const string privateCode_noob = "kz_9xou7M0yW1lUk-UlIowDk34lXypgEaTxsDDgvHzLA";
    public const string publicCode_noob = "5825ac548af603130c10f8e2";
    public const string privateCode_beast = "oLqokQqshUqmBPlElYdGDALSVyffTi9Emsdq3KnC72hA";
    public const string publicCode_beast = "58255e3a8af603130c10a529";
    public const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;
    public bool inputDatabaseHighscores = false;
    public int maxHighscores;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (inputDatabaseHighscores)
        {
            AddNewHighscore("example", 12345);
        }

        if (PlayerPrefs.GetInt("BeastMode", 0) == 0) {
            privateCode = privateCode_noob;
            publicCode = publicCode_noob;
        }
        else {
            privateCode = privateCode_beast;
            publicCode = publicCode_beast;
        }
    }

    public void AddNewHighscore(string username, float score)
    {
        StartCoroutine(UploadNewHighscore(username, score));
    }

    IEnumerator UploadNewHighscore(string username, float score)
    {
        username = Regex.Replace(username, "[^\\w\\ _-]", "");
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            print("Upload succesful");
        else
            print("Error uploading: " + www.error);
    }

    public void DownloadHighscores()
    {
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/" + maxHighscores);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            FormatHighscores(www.text);
        else
            print("Error downloading -> " + www.error);
    }

    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0].Replace("+", " ");
            float score = float.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username, score);
            //print(highscoresList[i].username + ": " + highscoresList[i].score);
        }
    }

}

public struct Highscore
{
    public string username;
    public float score;

    public Highscore(string _username, float _score)
    {
        username = _username;
        score = _score;
    }
}