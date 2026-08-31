using UnityEngine;

public class SD3Runtime : MonoBehaviour
{
    void Start()
    {
        this.ReadV3Data();
    }

    public void WriteV3Data()
    {
        Debug.Log("Starting data write process");
        Yui yui = new Yui(this.playerData, this.storyData, this.endlessData, this.challengeData, this.progressionData);
        Tsuruno.DataSaver.SaveAllV3Data(yui);
    }

    public void ReadV3Data()
    {
        int fileNumber = PlayerPrefs.GetInt("YuiFileNumber", 1);
        Yui yui = Tsuruno.DataLoader.LoadAllV3Data(fileNumber);
        this.playerData = yui.playerData;
        this.storyData = yui.storyDatas;
        this.endlessData = yui.endlessDatas;
        this.challengeData = yui.challengeDatas;
        this.progressionData = yui.progressionData;
    }

    [SerializeField] SD3PlayerData playerData;
    [SerializeField] SD3StoryData[] storyData;
    [SerializeField] SD3EndlessData[] endlessData;
    [SerializeField] SD3ChallengeData[] challengeData;
    [SerializeField] SD3ProgressionData progressionData;
}
