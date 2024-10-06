using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangelogReader : MonoBehaviour
{
    private void Start()
    {
        this.fileLocation = Application.streamingAssetsPath + "/Changelog.txt";
        this.ReadFromFile();
    }

    public void ReadFromFile()
    {
        string line;
        try
        {
            StreamReader reader = new StreamReader(this.fileLocation);
            line = reader.ReadLine();

            while (line != null)
            {
                this.text.text += line;
                line = reader.ReadLine();
            }

            reader.Close();
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    [SerializeField] private string fileLocation;
    [SerializeField] private TMP_Text text;
}
