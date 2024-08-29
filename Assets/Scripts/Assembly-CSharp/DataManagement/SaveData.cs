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
    /*
    MAPS:
    0 = Classic
    1 = Classic Extended
    2 = Juniper Hills
    */
    public float[] bestTime;
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
    /*
    CHALLENGES:
    0 = Null Style
    */
    public float[] bestTime;
}