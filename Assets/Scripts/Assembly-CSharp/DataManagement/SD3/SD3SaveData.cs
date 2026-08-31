using System;

public class SD3SummaryData
{
    public int fileVersion;
    public int[] gameVersion;
    public string playerName;
    public int playerIcon;
    public int totalPlayTimeInSeconds;
    public int[] lastPlayDate;
}

[Serializable]
public class SD3PlayerData
{
    public int fileVersion;
    public string playerName;
    public int playerIcon;
    public int totalPlaySeconds;
    public int[] gameVersion;
}

[Serializable]
public class SD3StoryData
{
    public int mapID;
    public string mapName;
    public int[] lifetimeItemsUsed;
    public int bestTime;
    public int lifetimeDetentions;

    public SD3StoryData()
    {
        this.mapID = 0;
        this.mapName = string.Empty;
        this.lifetimeItemsUsed = new int[18];
        this.bestTime = 0;
        this.lifetimeDetentions = 0;
    }
}

[Serializable]
public class SD3EndlessData
{
    public int mapID;
    public string mapName;
    public int[] lifetimeItemsUsed;
    public int bestNotebooks;
    public int lifetimeDetentions;

    public SD3EndlessData()
    {
        this.mapID = 0;
        this.mapName = string.Empty;
        this.lifetimeItemsUsed = new int[18];
        this.bestNotebooks = 0;
        this.lifetimeDetentions = 0;
    }
}

[Serializable]
public class SD3ChallengeData
{
    public int mapID;
    public string mapName;
    public int[] lifetimeItemsUsed;
    public int bestTime;
    public int lifetimeDetentions;

    public SD3ChallengeData()
    {
        this.mapID = 0;
        this.mapName = string.Empty;
        this.lifetimeItemsUsed = new int[18];
        this.bestTime = 0;
        this.lifetimeDetentions = 0;
    }
}

[Serializable]
public class SD3ProgressionData
{
    public bool[] challengeUnlocks;

    public SD3ProgressionData()
    {
        this.challengeUnlocks = new bool[1];
    }
}