using UnityEngine;
using Tsuruno;

public class SD3Controller : MonoBehaviour
{
    public void SetCurrentGamemode(GameControllerScript.GameMode gameMode)
    {
        switch(gameMode)
        {
            case GameControllerScript.GameMode.Endless:
                this.runtimeGamemode = CurrentGamemode.Endless;
                break;
            case GameControllerScript.GameMode.Challenge:
                this.runtimeGamemode = CurrentGamemode.Challenge;
                break;
            default:
                this.runtimeGamemode = CurrentGamemode.Story;
                break;
        }
    }

    public void OverwritePlayTimeOnly(StatisticsController statisticsController)
    {
        Yui yui = DataLoader.LoadAllV3Data(1);
        SD3PlayerData loadedPlayerData = yui.playerData;
        SD3StoryData[] loadedStoryDatas = yui.storyDatas;
        SD3EndlessData[] loadedEndlessDatas = yui.endlessDatas;
        SD3ChallengeData[] loadedChallengeDatas = yui.challengeDatas;
        SD3ProgressionData loadedProgressionData = yui.progressionData;

        loadedPlayerData.totalPlaySeconds += Mathf.CeilToInt(statisticsController.currentMinutes * 60f + statisticsController.currentSeconds);
        Debug.Log("Added " + Mathf.CeilToInt(statisticsController.currentMinutes * 60f + statisticsController.currentSeconds) + " seconds to total play time");
        Debug.Log("New playtime total: " + loadedPlayerData.totalPlaySeconds + " seconds");

        yui = new Yui(loadedPlayerData, loadedStoryDatas, loadedEndlessDatas, loadedChallengeDatas, loadedProgressionData);
        DataSaver.SaveAllV3Data(yui);
    }

    public void OverwriteFullSaveFile(StatisticsController statisticsController)
    {
        Yui yui = DataLoader.LoadAllV3Data(1);
        SD3PlayerData loadedPlayerData = yui.playerData;
        SD3StoryData[] loadedStoryDatas = yui.storyDatas;
        SD3EndlessData[] loadedEndlessDatas = yui.endlessDatas;
        SD3ChallengeData[] loadedChallengeDatas = yui.challengeDatas;
        SD3ProgressionData loadedProgressionData = yui.progressionData;

        loadedPlayerData.totalPlaySeconds += Mathf.CeilToInt(statisticsController.currentMinutes * 60f + statisticsController.currentSeconds);
        Debug.Log("Added " + Mathf.CeilToInt(statisticsController.currentMinutes * 60f + statisticsController.currentSeconds) + " seconds to total play time");
        Debug.Log("New playtime total: " + loadedPlayerData.totalPlaySeconds + " seconds");
        
        switch(this.runtimeGamemode)
        {
            case CurrentGamemode.Story:
                loadedStoryDatas = this.OverwriteStoryData(loadedStoryDatas);
                break;
            case CurrentGamemode.Endless:
                loadedEndlessDatas = this.OverwriteEndlessData(loadedEndlessDatas);
                break;
            case CurrentGamemode.Challenge:
                loadedChallengeDatas = this.OverwriteChallengeData(loadedChallengeDatas);
                break;
        }

        statisticsController.prevBestTime = this.prevBestTime;

        yui = new Yui(loadedPlayerData, loadedStoryDatas, loadedEndlessDatas, loadedChallengeDatas, loadedProgressionData);
        DataSaver.SaveAllV3Data(yui);
    }

    SD3StoryData[] OverwriteStoryData(SD3StoryData[] inputStoryDatas)
    {
        SD3StoryData[] storyDatas = inputStoryDatas;
        bool existingDataFound = false;
        int i = 0;
        
        for (i = 0; i < storyDatas.Length; i++)
        {
            if (storyDatas[i].mapID == this.currentMapID)
            {
                Debug.Log("Writing new Story data to map ID " + this.currentMapID + "...");
                for (int j = 0; j < 18; j++)
                    storyDatas[i].lifetimeItemsUsed[j] += this.currentItemsUsed[j];
                storyDatas[i].lifetimeDetentions += this.currentDetentions;
                this.prevBestTime = storyDatas[i].bestTime;
                existingDataFound = true;
                break;
            }
        }

        if (storyDatas.Length == 0)
            Debug.LogWarning("No existing Story data found!");

        if (!existingDataFound)
        {
            Debug.LogWarning("No existing Story data found for the current map ID! Creating new data with map ID " + this.currentMapID + "...");
            i++;
            System.Array.Resize(ref storyDatas, i);
            i--;
            storyDatas[i] = new SD3StoryData();
            storyDatas[i].mapID = this.currentMapID;
            storyDatas[i].mapName = this.currentMapName;
            storyDatas[i].lifetimeItemsUsed = this.currentItemsUsed;
            this.prevBestTime = this.currentTime + 1;
            storyDatas[i].bestTime = this.currentTime;
            storyDatas[i].lifetimeDetentions = this.currentDetentions;
        }
        
        storyDatas[i].bestTime = this.CalculateBestTime(storyDatas[i].bestTime);
        return storyDatas;
    }

    SD3EndlessData[] OverwriteEndlessData(SD3EndlessData[] inputEndlessDatas)
    {
        SD3EndlessData[] endlessDatas = inputEndlessDatas;
        bool existingDataFound = false;
        int i = 0;
        
        for (i = 0; i < endlessDatas.Length; i++)
        {
            if (endlessDatas[i].mapID == this.currentMapID)
            {
                Debug.Log("Writing new Endless data to map ID " + this.currentMapID + "...");
                for (int j = 0; j < 18; j++)
                    endlessDatas[i].lifetimeItemsUsed[j] += this.currentItemsUsed[j];
                endlessDatas[i].lifetimeDetentions += this.currentDetentions;
                existingDataFound = true;
                break;
            }
        }

        if (endlessDatas.Length == 0)
            Debug.LogWarning("No existing Endless data found!");

        if (!existingDataFound)
        {
            Debug.LogWarning("No existing Endless data found for the current map ID! Creating new data with map ID " + this.currentMapID + "...");
            i++;
            System.Array.Resize(ref endlessDatas, i);
            i--;
            endlessDatas[i] = new SD3EndlessData();
            endlessDatas[i].mapID = this.currentMapID;
            endlessDatas[i].mapName = this.currentMapName;
            endlessDatas[i].lifetimeItemsUsed = this.currentItemsUsed;
            endlessDatas[i].lifetimeDetentions = this.currentDetentions;
        }
        
        endlessDatas[i].bestNotebooks = this.CalculateBestNotebooks(endlessDatas[i].bestNotebooks);
        return endlessDatas;
    }

    SD3ChallengeData[] OverwriteChallengeData(SD3ChallengeData[] inputChallengeDatas)
    {
        SD3ChallengeData[] challengeDatas = inputChallengeDatas;
        bool existingDataFound = false;
        int i = 0;
        
        for (i = 0; i < challengeDatas.Length; i++)
        {
            if (challengeDatas[i].mapID == this.currentMapID)
            {
                Debug.Log("Writing new Challenge data to map ID " + this.currentMapID + "...");
                for (int j = 0; j < 18; j++)
                    challengeDatas[i].lifetimeItemsUsed[j] += this.currentItemsUsed[j];
                challengeDatas[i].lifetimeDetentions += this.currentDetentions;
                this.prevBestTime = challengeDatas[i].bestTime;
                existingDataFound = true;
                break;
            }
        }

        if (challengeDatas.Length == 0)
            Debug.LogWarning("No existing Challenge data found!");

        if (!existingDataFound)
        {
            Debug.LogWarning("No existing Challenge data found for the current map ID! Creating new data with map ID " + this.currentMapID + "...");
            i++;
            System.Array.Resize(ref challengeDatas, i);
            i--;
            challengeDatas[i] = new SD3ChallengeData();
            challengeDatas[i].mapID = this.currentMapID;
            challengeDatas[i].mapName = this.currentMapName;
            challengeDatas[i].lifetimeItemsUsed = this.currentItemsUsed;
            this.prevBestTime = this.currentTime + 1;
            challengeDatas[i].bestTime = this.currentTime;
            challengeDatas[i].lifetimeDetentions = this.currentDetentions;
        }
        
        challengeDatas[i].bestTime = this.CalculateBestTime(challengeDatas[i].bestTime);
        return challengeDatas;
    }

    int CalculateBestTime(int timeToCheck)
    {
        if (this.currentTime == 0)
        {
            Debug.Log("No new best recorded.");
            return timeToCheck;
        }
        else if (timeToCheck == 0)
        {
            Debug.Log("New best of " + this.currentTime + "ms recorded.");
            return this.currentTime;
        }
        else if (this.currentTime < timeToCheck)
        {
            Debug.Log("New best of " + this.currentTime + "ms recorded.");
            return this.currentTime;
        }
        else
        {
            Debug.Log("No new best recorded.");
            return timeToCheck;
        }
    }

    int CalculateBestNotebooks(int currentHighScore)
    {
        if (this.currentNotebookScore > currentHighScore)
            return this.currentNotebookScore;
        else
            return currentHighScore;
    }

    [Header("Configuration")]
    [SerializeField] GameControllerScript gc;

    [Header("Game State")]
    [SerializeField] CurrentGamemode runtimeGamemode;
    enum CurrentGamemode
    {
        Story, Endless, Challenge
    }
    public int currentMapID;
    public string currentMapName;
    public int[] currentItemsUsed;
    public int currentTime;
    [SerializeField] int prevBestTime;
    public int currentDetentions;
    public int currentNotebookScore;
}
