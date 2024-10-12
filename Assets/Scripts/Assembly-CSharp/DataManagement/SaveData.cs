using System;
using UnityEngine;

[Serializable]
public class AchievementData
{
    public int ach_MapCount;
    public int[] ach_Maps;
    public bool[] completedMaps;
}

[Serializable]
public class SettingsData
{
    public bool isSettingsSaved;
}

[Serializable]
public class ProgressionData
{
    public ushort fileVersion;
    public bool[] mapUnlocks;
    public ProgressionData(ProgressionController controller)
    {
        ushort totalMaps = 1;

        if (controller == null)
        {
            this.fileVersion = 1;
            this.mapUnlocks = new bool[totalMaps];
        }
        else
        {
            this.fileVersion = controller.fileVersion;
            this.mapUnlocks = controller.mapUnlocks;
        }
    }
}

[Serializable]
public class SaveData_Story
{
    public ushort fileVersion;
    public int[] itemsUsed_Classic;
    public int[] itemsUsed_ClassicExtended;
    public int[] itemsUsed_JuniperHills;
    public float[] bestTime;
    public int[] totalDetentions;
    public SaveData_Story(StatisticsController stats)
    {
        ushort totalMaps = 3;
        ushort totalItems = 18;

        if (stats == null)
        {
            this.fileVersion = 1;
            this.totalDetentions = new int[totalMaps];
            this.itemsUsed_Classic = new int[totalItems];
            this.itemsUsed_ClassicExtended = new int[totalItems];
            this.itemsUsed_JuniperHills = new int[totalItems];

            this.bestTime = new float[totalMaps];
            for (int i = 0; i < this.bestTime.Length; i++)
                this.bestTime[i] = 9999f;
        }
        else
        {
            this.fileVersion = stats.data_fileVersion;
            this.totalDetentions = stats.data_totalDetentions;
            this.itemsUsed_Classic = stats.data_ClassicLifetimeItems;
            this.itemsUsed_ClassicExtended = stats.data_ClassicExtendedLifetimeItems;
            this.itemsUsed_JuniperHills = stats.data_JuniperHillsLifetimeItems;
            this.bestTime = stats.data_bestTime;
        }
    }
}

[Serializable]
public class SaveData_Endless
{
    public ushort fileVersion;
    public int[] itemsUsed_Classic;
    public int[] itemsUsed_ClassicExtended;
    public int[] itemsUsed_JuniperHills;
    public int[] notebooks;
    public int[] totalDetentions;
    public SaveData_Endless(StatisticsController stats)
    {
        ushort totalMaps = 3;
        ushort totalItems = 18;

        if (stats == null)
        {
            this.fileVersion = 1;
            this.notebooks = new int[totalMaps];
            this.totalDetentions = new int[totalMaps];
            this.itemsUsed_Classic = new int[totalItems];
            this.itemsUsed_ClassicExtended = new int[totalItems];
            this.itemsUsed_JuniperHills = new int[totalItems];

            this.notebooks[0] = GetPrefNotebookScore("Classic");
            this.notebooks[1] = GetPrefNotebookScore("ClassicExtended");
            this.notebooks[2] = GetPrefNotebookScore("JuniperHills");
            PlayerPrefs.DeleteKey("highbooks_Classic");
            PlayerPrefs.DeleteKey("highbooks_ClassicExtended");
            PlayerPrefs.DeleteKey("highbooks_JuniperHills");
        }
        else
        {
            this.fileVersion = stats.data_fileVersion;
            this.notebooks = stats.data_notebooks;
            this.totalDetentions = stats.data_totalDetentions;
            this.itemsUsed_Classic = stats.data_ClassicLifetimeItems;
            this.itemsUsed_ClassicExtended = stats.data_ClassicExtendedLifetimeItems;
            this.itemsUsed_JuniperHills = stats.data_JuniperHillsLifetimeItems;
        }

        int GetPrefNotebookScore(string map)
        {
            int score;

            switch(map)
            {
                case "Classic":
                    score = PlayerPrefs.GetInt("highbooks_Classic", 0);
                    break;
                case "ClassicExtended":
                    score = PlayerPrefs.GetInt("highbooks_ClassicExtended", 0);
                    break;
                case "JuniperHills":
                    score = PlayerPrefs.GetInt("highbooks_JuniperHills", 0);
                    break;
                default:
                    score = 0;
                    break;
            }

            return score;
        }
    }
}

[Serializable]
public class SaveData_Challenge
{
    public ushort fileVersion;
    public bool[] challengeUnlocks;
    public int[] itemsUsed_NullStyle;
    public float[] bestTime;
    public int[] totalDetentions;
    public SaveData_Challenge(StatisticsController stats)
    {
        ushort totalUnlocks = 1;
        ushort totalMaps = 1;
        ushort totalItems = 18;

        if (stats == null)
        {
            this.fileVersion = 1;
            this.challengeUnlocks = new bool[totalUnlocks];
            this.itemsUsed_NullStyle = new int[totalItems];
            this.totalDetentions = new int[totalMaps];
            this.bestTime = new float[totalMaps];
        }
        else
        {
            this.fileVersion = stats.data_fileVersion;
            this.challengeUnlocks = stats.data_challengeUnlocks;
            this.itemsUsed_NullStyle = stats.data_NullStyleLifetimeItems;
            this.bestTime = stats.data_bestTime;
            this.totalDetentions = stats.data_totalDetentions;
        }
    }
}