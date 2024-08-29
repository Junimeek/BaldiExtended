using System;
using UnityEngine;

public class EndlessScoresMenu : MonoBehaviour
{
    private void OnEnable()
    {
        this.endlessData = SaveDataController.LoadEndlessData();

        for (int i = 0; i < this.informationScripts.Length; i++)
        {
            this.informationScripts[i].endlessScript = this;
            this.informationScripts[i].informationType = 1;
        }

        this.SetEndlessScores();
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

    public void GetEndlessScore(int mapSelected)
    {
        this.endlessText = "ENDLESS: " + this.scores[mapSelected] + " Notebooks";
        this.informationScripts[mapSelected].HighlightItem();
    }

    [SerializeField] private ItemInformationScript[] informationScripts;
    private SaveData_Endless endlessData;
    [SerializeField] private string[] maps;
    [SerializeField] private int[] scores;
    public string endlessText;
}
