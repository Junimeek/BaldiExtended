using UnityEngine;
using Tsuruno;
using TMPro;

public class HighScoresMenu : MonoBehaviour
{
    void OnEnable()
    {
        if (this.scoresFetched)
            return;
        
        Yui yui = DataLoader.LoadAllV3Data(1);
        this.storyDatas = yui.storyDatas;
        this.endlessDatas = yui.endlessDatas;
        this.challengeDatas = yui.challengeDatas;

        this.itemTexts = this.itemListParent.GetComponentsInChildren<TMP_Text>();
        this.ClearItemScores();

        this.scoresFetched = true;
    }

    public void SetItemScores(int[] itemList)
    {
        for (int i = 0; i < this.itemTexts.Length; i++)
            this.itemTexts[i].text = itemList[i].ToString();
    }

    public void ClearItemScores()
    {
        for (int i = 0; i < this.itemTexts.Length; i++)
            this.itemTexts[i].text = "-";
    }

    public SD3StoryData GetStoryData(int mapID)
    {
        if (this.storyDatas.Length == 0)
            return new SD3StoryData();
        
        for (int i = 0; i < this.storyDatas.Length; i++)
        {
            if (this.storyDatas[i].mapID == mapID)
                return storyDatas[i];
        }

        return new SD3StoryData();
    }

    public SD3EndlessData GetEndlessData(int mapID)
    {
        if (this.endlessDatas.Length == 0)
            return new SD3EndlessData();
        
        for (int i = 0; i < this.endlessDatas.Length; i++)
        {
            if (this.endlessDatas[i].mapID == mapID)
                return endlessDatas[i];
        }

        return new SD3EndlessData();
    }

    public SD3ChallengeData GetChallengeData(int mapID)
    {
        if (this.challengeDatas.Length == 0)
            return new SD3ChallengeData();
        
        for (int i = 0; i < this.challengeDatas.Length; i++)
        {
            if (this.challengeDatas[i].mapID == mapID)
                return challengeDatas[i];
        }

        return new SD3ChallengeData();
    }

    [SerializeField] GameObject itemListParent;
    public TMP_Text[] itemTexts;
    bool scoresFetched;
    SD3StoryData[] storyDatas;
    SD3EndlessData[] endlessDatas;
    SD3ChallengeData[] challengeDatas;
}
