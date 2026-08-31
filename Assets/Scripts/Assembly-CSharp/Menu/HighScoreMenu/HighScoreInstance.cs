using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreInstance : MonoBehaviour
{
    void Start()
    {
        this.image = GetComponent<Image>();
        this.lifetimeItems = new int[18];
    }

    public void DisplayStats()
    {
        this.GetHighScoreData();
        this.image.color = Color.green;
        this.titleText.text  = this.mapName;
        this.bestTimeText.text = this.GetLowerString();
        highScoresController.SetItemScores(this.lifetimeItems);
    }

    public void HideStats()
    {
        this.image.color = Color.white;
        this.titleText.text = string.Empty;
        this.bestTimeText.text = string.Empty;
        highScoresController.ClearItemScores();
    }

    string GetLowerString()
    {
        string finalString = string.Empty;

        if (this.styleType == StyleType.StoryEndless)
        {
            finalString = "STORY: ";
            finalString += this.GetBestTime();
            finalString += "\n";
            finalString += "ENDLESS: ";
            finalString += this.endlessData.bestNotebooks;
            finalString += " Notebooks";
        }
        else
        {
            finalString = "CHALLENGE: ";
            finalString += this.GetBestTime();
        }

        return finalString;
    }

    string GetBestTime()
    {
        int timeInMS;
        if (this.styleType == StyleType.StoryEndless)
            timeInMS = this.storyData.bestTime;
        else
            timeInMS = this.challengeData.bestTime;
        
        if (timeInMS == 0)
            return "No best time";
        
        float timeInSeconds = timeInMS / 1000f;
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        timeInSeconds -= minutes * 60f;

        return minutes.ToString() + ":" + timeInSeconds.ToString("00.000");
    }

    void GetHighScoreData()
    {
        if (this.dataReceived)
            return;
        
        if (this.styleType == StyleType.StoryEndless)
        {
            this.storyData = highScoresController.GetStoryData(this.mapID);
            this.AddItemsToList(this.storyData.lifetimeItemsUsed);
            this.endlessData = highScoresController.GetEndlessData(this.mapID);
            this.AddItemsToList(this.endlessData.lifetimeItemsUsed);
        }
        else
        {
            this.challengeData = highScoresController.GetChallengeData(this.mapID);
            this.AddItemsToList(this.challengeData.lifetimeItemsUsed);
        }

        this.dataReceived = true;
    }

    void AddItemsToList(int[] itemList)
    {
        for (int i = 0; i < itemList.Length; i++)
            this.lifetimeItems[i] += itemList[i];
    }

    enum StyleType
    {
        StoryEndless, Challenge
    }
    [SerializeField] StyleType styleType;
    [SerializeField] int mapID;
    [SerializeField] string mapName;
    [SerializeField] HighScoresMenu highScoresController;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text bestTimeText;

    Image image;
    bool dataReceived;
    int[] lifetimeItems;
    [SerializeField] SD3StoryData storyData;
    [SerializeField] SD3EndlessData endlessData;
    SD3ChallengeData challengeData;
}
