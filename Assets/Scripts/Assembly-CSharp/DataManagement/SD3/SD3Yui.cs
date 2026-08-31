using System;

[Serializable]
public class Yui
{
    public SD3PlayerData playerData;
    public SD3StoryData[] storyDatas;
    public SD3EndlessData[] endlessDatas;
    public SD3ChallengeData[] challengeDatas;
    public SD3ProgressionData progressionData;

    public Yui (
        SD3PlayerData inputPlayerData,
        SD3StoryData[] inputStoryData,
        SD3EndlessData[] inputEndlessData,
        SD3ChallengeData[] inputChallengeData,
        SD3ProgressionData inputProgressionData
    ) {
        this.playerData = inputPlayerData;
        this.storyDatas = inputStoryData;
        this.endlessDatas = inputEndlessData;
        this.challengeDatas = inputChallengeData;
        this.progressionData = inputProgressionData;
    }

    public Yui(SD3PlayerData inputPlayerData)
    {
        this.playerData = inputPlayerData;
        this.storyDatas = new SD3StoryData[0];
        this.endlessDatas = new SD3EndlessData[0];
        this.challengeDatas = new SD3ChallengeData[0];
        this.progressionData = new SD3ProgressionData();
    }
}