using System;

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
}

[Serializable]
public class SaveData_Challenge
{
    public ushort fileVersion;
    public bool[] challengeUnlocks;
    public int[] itemsUsed_NullStyle;
    public float[] bestTime;
    public int[] totalDetentions;
}