using System;
using UnityEngine;
using TMPro;

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
        this.itemsUsed_NullStyle = this.challengeData.itemsUsed_NullStyle;

        for (byte i = 0; i < 18; i++) // Adds Story and Endless numbers together for now
        {
            try {
                this.itemsUsed_Classic[i] += this.endlessData.itemsUsed_Classic[i];
                this.itemsUsed_ClassicExtended[i] += this.endlessData.itemsUsed_ClassicExtended[i];
                this.itemsUsed_JuniperHills[i] += this.endlessData.itemsUsed_JuniperHills[i];
            }
            catch {
                Array.Resize(ref this.itemsUsed_Classic, this.itemsUsed_Classic.Length + 1);
                Array.Resize(ref this.itemsUsed_ClassicExtended, this.itemsUsed_ClassicExtended.Length + 1);
                Array.Resize(ref this.itemsUsed_JuniperHills, this.itemsUsed_JuniperHills.Length + 1);
                this.itemsUsed_Classic[i] = 0;
                this.itemsUsed_ClassicExtended[i] = 0;
                this.itemsUsed_JuniperHills[i] = 0;
            }
        }

        if (this.itemsUsed_NullStyle.Length < 18)
            Array.Resize(ref this.itemsUsed_NullStyle, 18);

        for (byte i = 0; i < this.informationScripts.Length; i++)
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
                case "NullStyle":
                    time = this.TimeConversion(this.challengeData.bestTime[0]);
                    break;
                default:
                    time = "None";
                    break;
            }
            this.bestTimes[i] = time;
        }
    }

    private string TimeConversion(float time)
    {
        if (time == 0f || time == 9999f)
            return "None";

        float seconds = time % 60f;
		int minutes =  Mathf.FloorToInt(time / 60f);
		int hours = Mathf.FloorToInt(minutes / 60f);
		minutes -= hours * 60;
		
		return hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00.000");
    }

    public void GetEndlessScore(int mapSelected)
    {
        if (this.IsChallengeMap(mapSelected))
            this.endlessText = "CHALLENGE: " + this.bestTimes[mapSelected];
        else
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
            case 3:
                for (int i = 0; i < this.itemTexts.Length; i++)
                    this.itemTexts[i].text = this.itemsUsed_NullStyle[i].ToString();
                break;
        }
    }

    private bool IsChallengeMap(int map)
    {
        switch(map)
        {
            case 3:
                return true;
            default:
                return false;
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
    [SerializeField] private int[] itemsUsed_NullStyle;
    [SerializeField] private TMP_Text[] itemTexts;
    public string endlessText;
}
