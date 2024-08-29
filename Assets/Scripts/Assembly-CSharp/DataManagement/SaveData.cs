using System;

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
    /*
    public float turnSensitivity;
    public bool isInstantReset;
    public bool isNotifBoard;
    public float volVoice;
    public float volBGM;
    public float volSFX;
    public bool isAdditionalMusic;
    */
}

[Serializable]
public class SaveData_FileVersion
{
    public int versionNumber;
}

[Serializable]
public class SaveData_Story
{
    public int fileVersion;
    public float[] bestTime = new float[3];
    public SaveData_Story(string map, int score)
    {
        switch(map)
        {
            case "defaults":
                this.bestTime[0] = 0f;
                this.bestTime[1] = 0f;
                this.bestTime[2] = 0f;
                break;
            case "Classic":
                this.bestTime[0] = score;
                break;
            case "ClassicExtended":
                this.bestTime[1] = score;
                break;
            case "JuniperHills":
                this.bestTime[2] = score;
                break;
        }
    }
}

[Serializable]
public class SaveData_Endless
{
    public int fileVersion;
    public int[] notebooks = new int[3];
    public SaveData_Endless(string map, int score)
    {
        switch(map)
        {
            case "defaults":
                this.notebooks[0] = 0;
                this.notebooks[1] = 0;
                this.notebooks[2] = 0;
                break;
            case "Classic":
                this.notebooks[0] = score;
                break;
            case "ClassicExtended":
                this.notebooks[1] = score;
                break;
            case "JuniperHills":
                this.notebooks[2] = score;
                break;
        }
    }
}

[Serializable]
public class SaveData_Challenge
{
    public int fileVersion;
    public float[] bestTime = new float[1];
    public SaveData_Challenge(string map, float score)
    {
        switch(map)
        {
            case "defaults":
                this.bestTime[0] = 0f;
                break;
            case "NullStyle":
                this.bestTime[0] = score;
                break;
        }
    }
}