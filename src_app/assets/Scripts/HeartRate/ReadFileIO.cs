using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadFileIO : MonoBehaviour
{
    public bool randomHR = false;
    public float updateInterval = 1f;
    public Text pathText;
    public InputField inputField;
    
    string filePath;
    LevelManager levelManager;
    bool readHR = true;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

        filePath = PlayerPrefs.GetString("path", "");
        inputField.text = filePath;

        StartCoroutine(MainLoop());
    }


    public void SearchPath ()
    {
        #if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select the LabView .txt file, please :)", "", "txt");
        
        if (path.Length != 0)
            filePath = pathText.text = path;
        #endif
    }


    public void WritePath ()
    {
        filePath = inputField.text;

        PlayerPrefs.SetString("path", filePath);
    }


    IEnumerator MainLoop()
    {
        while (readHR)
        {
            if (!randomHR)
            {
                Load(filePath);

                yield return new WaitForSeconds(updateInterval);
            }
            else
            {
                float d = UnityEngine.Random.Range(-2, 3);

                levelManager.heartRate += d;

                yield return new WaitForSeconds(updateInterval * 1f);
            }

        }
    }


    private bool Load(string fileName)
    {
        try
        {
            string line;

            StreamReader theReader = new StreamReader(fileName, Encoding.Default);

            using (theReader)
            {
                line = theReader.ReadLine();

                if (line != null)
                    levelManager.heartRate = float.Parse(line);

                theReader.Close();
                return true;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("{0}\n", e.Message);
            return false;
        }
    }

}
