using System;
using TMPro;
using UnityEngine;

public class HighScoresMenu : MonoBehaviour
{
    private void OnEnable()
    {
        this.storyData = SaveDataController.LoadStoryData();
        this.endlessData = SaveDataController.LoadEndlessData();
        this.challengeData = SaveDataController.LoadChallengeData();

        this.itemsUsed_Classic = this.storyData.itemsUsed_Classic;
        this.itemsUsed_ClassicExtended = this.storyData.itemsUsed_ClassicExtended;
        this.itemsUsed_JuniperHills = this.storyData.itemsUsed_JuniperHills;

        for (int i = 0; i < 16; i++)
        {
            this.itemsUsed_Classic[i] += this.endlessData.itemsUsed_Classic[i];
            this.itemsUsed_ClassicExtended[i] += this.endlessData.itemsUsed_ClassicExtended[i];
            this.itemsUsed_JuniperHills[i] += this.endlessData.itemsUsed_JuniperHills[i];
        }

        for (int i = 0; i < this.informationScripts.Length; i++)
        {
            this.informationScripts[i].highScoresScript = this;
            this.informationScripts[i].informationType = 1;
        }

        this.SetEndlessScores();
        this.SetStoryTimes();
        this.ClearItemScores();
    }

    private void SetEndlessScores()
    {
        Array.Resize(ref this.scores, this.maps.Length);
        
        for (int i = 0; i < this.maps.Length; i++)
        {
            int highScore;

            switch(this.maps[i])
            {
                case "Classic":
                    highScore = this.endlessData.notebooks[0];
                    break;
                case "ClassicExtended":
                    highScore = this.endlessData.notebooks[1];
                    break;
                case "JuniperHills":
                    highScore = this.endlessData.notebooks[2];
                    break;
                default:
                    highScore = 0;
                    break;
            }

            this.scores[i] = highScore;
        }
        Debug.Log("Loaded High Scores");
    }

    private void SetStoryTimes()
    {
        Array.Resize(ref this.bestTimes, this.maps.Length);
        string time;

        for (int i = 0; i < this.maps.Length; i++)
        {

            switch(this.maps[i])
            {
                case "Classic":
                    time = this.TimeConversion(this.storyData.bestTime[0]);
                    break;
                case "ClassicExtended":
                    time = this.TimeConversion(this.storyData.bestTime[1]);
                    break;
                case "JuniperHills":
                    time = this.TimeConversion(this.storyData.bestTime[2]);
                    break;
                default:
                    time = "0:00.000";
                    break;
            }

            this.bestTimes[i] = time;
        }
    }

    private string TimeConversion(float time)
    {
        if (time == 9999f)
            return "None";

        float seconds = time % 60f;
		int minutes =  Mathf.FloorToInt(time / 60f);
		int hours = Mathf.FloorToInt(minutes / 60f);
		minutes -= hours * 60;
		
		return hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00.000");
    }

    public void GetEndlessScore(int mapSelected)
    {
        this.endlessText = "STORY: " + this.bestTimes[mapSelected] + "\nENDLESS: " + this.scores[mapSelected] + " Notebooks";
        this.informationScripts[mapSelected].HighlightItem();

        switch (mapSelected)
        {
            case 0:
                for (int i = 0; i < this.itemTexts.Length; i++)
                    this.itemTexts[i].text = this.itemsUsed_Classic[i].ToString();
                break;
            case 1:
                for (int i = 0; i < this.itemTexts.Length; i++)
                    this.itemTexts[i].text = this.itemsUsed_ClassicExtended[i].ToString();
                break;
            case 2:
                for (int i = 0; i < this.itemTexts.Length; i++)
                    this.itemTexts[i].text = this.itemsUsed_JuniperHills[i].ToString();
                break;
        }
    }

    public void ClearItemScores()
    {
        for (int i = 0; i < this.itemTexts.Length; i++)
            this.itemTexts[i].text = "-";
    }

    [SerializeField] private ItemInformationScript[] informationScripts;
    private SaveData_Story storyData;
    private SaveData_Endless endlessData;
    private SaveData_Challenge challengeData;
    [SerializeField] private string[] maps;
    [SerializeField] private int[] scores;
    [SerializeField] private string[] bestTimes;
    [SerializeField] private int[] itemsUsed_Classic;
    [SerializeField] private int[] itemsUsed_ClassicExtended;
    [SerializeField] private int[] itemsUsed_JuniperHills;
    [SerializeField] private TMP_Text[] itemTexts;
    public string endlessText;
}
