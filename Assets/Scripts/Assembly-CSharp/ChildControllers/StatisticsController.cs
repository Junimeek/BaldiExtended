using System;
using UnityEngine;

public class StatisticsController : MonoBehaviour
{
    private void Start()
    {
        this.finalSeconds = 0f;
        this.itemsUsed = new int[0];
        this.detentions = 0;
        this.LoadAllData();
    }

    private void LoadAllData()
    {
        this.mapID = this.GetMapID();

        if (this.gc.mode == "story" || this.gc.mode == "endless")
        {
            SaveData_Story storyData = SaveDataController.LoadStoryData();
            SaveData_Endless endlessData = SaveDataController.LoadEndlessData();

            this.data_fileVersion = storyData.fileVersion;
            this.data_bestTime = storyData.bestTime;
            this.data_notebooks = endlessData.notebooks;

            if (storyData.bestTime[this.mapID] == 0f)
                this.data_bestTime[this.mapID] = 9999f;

            if (this.gc.mode == "story")
            {
                this.data_totalDetentions = storyData.totalDetentions;
                this.data_ClassicLifetimeItems = storyData.itemsUsed_Classic;
                this.data_ClassicExtendedLifetimeItems = storyData.itemsUsed_ClassicExtended;
                this.data_JuniperHillsLifetimeItems = storyData.itemsUsed_JuniperHills;
                
                switch(this.mapID)
                {
                    case 0:
                        this.lifetimeItems = this.data_ClassicLifetimeItems;
                        break;
                    case 1:
                        this.lifetimeItems = this.data_ClassicExtendedLifetimeItems;
                        break;
                    case 2:
                        this.lifetimeItems = this.data_JuniperHillsLifetimeItems;
                        break;
                }
            }
            else
            {
                this.data_totalDetentions = endlessData.totalDetentions;
                this.data_ClassicLifetimeItems = endlessData.itemsUsed_Classic;
                this.data_ClassicExtendedLifetimeItems = endlessData.itemsUsed_ClassicExtended;
                this.data_JuniperHillsLifetimeItems = endlessData.itemsUsed_JuniperHills;

                switch(this.mapID)
                {
                    case 0:
                        this.lifetimeItems = this.data_ClassicLifetimeItems;
                        break;
                    case 1:
                        this.lifetimeItems = this.data_ClassicExtendedLifetimeItems;
                        break;
                    case 2:
                        this.lifetimeItems = this.data_JuniperHillsLifetimeItems;
                        break;
                }
            }
        }
        else if (this.gc.mode == "challenge")
        {
            SaveData_Challenge challengeData = SaveDataController.LoadChallengeData();

            this.data_fileVersion = challengeData.fileVersion;
            this.data_totalDetentions = challengeData.totalDetentions;
            this.data_bestTime = challengeData.bestTime;

            if (challengeData.bestTime[this.mapID] == 0f)
                challengeData.bestTime[this.mapID] = 9999f;
            
            this.data_NullStyleLifetimeItems = challengeData.itemsUsed_NullStyle;

            switch(this.mapID)
            {
                case 0:
                    this.lifetimeItems = this.data_NullStyleLifetimeItems;
                    break;
            }
        }

        this.gc.InitializeScores();
    }

    public void SaveAllData(string bestType)
    {
        this.data_totalDetentions[this.mapID] += this.detentions;

        if (this.gc.mode == "story" || this.gc.mode == "endless")
        {
            switch(this.mapID)
            {
                case 0:
                    for (int i = 0; i < this.lifetimeItems.Length; i++)
                        this.data_ClassicLifetimeItems[i] = this.lifetimeItems[i];
                    break;
                case 1:
                    for (int i = 0; i < this.lifetimeItems.Length; i++)
                        this.data_ClassicExtendedLifetimeItems[i] = this.lifetimeItems[i];
                    break;
                case 2:
                    for (int i = 0; i < this.lifetimeItems.Length; i++)
                        this.data_JuniperHillsLifetimeItems[i] = this.lifetimeItems[i];
                    break;
            }

            if (bestType != null)
            {
                if (bestType == "time")
                    this.data_bestTime[this.mapID] = this.finalSeconds;
                else if (bestType == "notebooks")
                    this.data_notebooks[this.mapID] = this.notebooks;
            }
        }
        else if (this.gc.mode == "challenge")
        {
            switch(this.mapID)
            {
                case 0:
                    for (int i = 0; i < this.lifetimeItems.Length; i++)
                    {
                        try {
                            this.data_NullStyleLifetimeItems[i] = this.lifetimeItems[i];
                        }
                        catch {
                            Array.Resize(ref this.data_NullStyleLifetimeItems, this.data_NullStyleLifetimeItems.Length + 1);
                            this.data_NullStyleLifetimeItems[i] = this.lifetimeItems[i];
                        }
                    }
                    break;
            }

            if (bestType != null)
            {
                if (bestType == "time")
                    this.data_bestTime[this.mapID] = this.finalSeconds;
            }
        }

        switch (this.gc.mode)
        {
            case "story":
                SaveDataController.SaveStoryData(this);
                break;
            case "endless":
                SaveDataController.SaveEndlessData(this);
                break;
            case "challenge":
                SaveDataController.SaveChallengeData(this);
                break;
        }

        SaveHead saveHead = FindObjectOfType<SaveHead>();
        if (saveHead != null)
            saveHead.ActivateSaveHead(1.4f);
    }

    private short GetMapID()
    {
        if (this.gc.mode == "story" || this.gc.mode == "endless")
        {
            switch(this.gc.curMap)
            {
                case "Classic":
                    return 0;
                case "ClassicExtended":
                    return 1;
                case "JuniperHills":
                    return 2;
                default:
                    return 0;
            }
        }
        else if (this.gc.mode == "challenge")
        {
            switch(this.gc.curMap)
            {
                case "ClassicDark":
                    return 0;
                default:
                    return 0;
            }
        }
        else
            return 0;
    }

    [SerializeField] private GameControllerScript gc;

    [Header("Game State")]
    public bool disableSaving;
    public int[] itemsUsed;
    public short mapID;
    public float finalSeconds;
    public int notebooks;
    public int detentions;
    public int[] lifetimeItems;

    [Header("File Data")]
    public ushort data_fileVersion;
    public bool[] data_challengeUnlocks;
    public float[] data_bestTime;
    public int[] data_notebooks;
    public int[] data_totalDetentions;
    public int[] data_ClassicLifetimeItems;
    public int[] data_ClassicExtendedLifetimeItems;
    public int[] data_JuniperHillsLifetimeItems;
    public int[] data_NullStyleLifetimeItems;
}
