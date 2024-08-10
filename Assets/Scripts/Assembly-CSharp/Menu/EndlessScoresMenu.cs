using System;
using UnityEngine;

public class EndlessScoresMenu : MonoBehaviour
{
    private void OnEnable()
    {
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
                    highScore = PlayerPrefs.GetInt("highbooks_Classic");
                    break;
                case "ClassicExtended":
                    highScore = PlayerPrefs.GetInt("highbooks_ClassicExtended");
                    break;
                case "JuniperHills":
                    highScore = PlayerPrefs.GetInt("highbooks_JuniperHills");
                    break;
                default:
                    highScore = 0;
                    break;
            }

            this.scores[i] = highScore;
        }
        Debug.Log("Loaded High Scores from registry");
    }

    public void GetEndlessScore(int mapSelected)
    {
        this.endlessText = "ENDLESS: " + this.scores[mapSelected] + " Notebooks";
        this.informationScripts[mapSelected].HighlightItem();
    }

    [SerializeField] private ItemInformationScript[] informationScripts;
    [SerializeField] private string[] maps;
    [SerializeField] private int[] scores;
    public string endlessText;
}
