using System;

[Serializable]
public class AchievementData
{
    public int ach_MapCount;
    public int[] ach_Maps;
}

[Serializable]
public class SettingsData
{
    public float turnSensitivity;
    public bool isInstantReset;
    public bool isNotifBoard;
    public float volVoice;
    public float volBGM;
    public float volSFX;
    public bool isAdditionalMusic;
}
