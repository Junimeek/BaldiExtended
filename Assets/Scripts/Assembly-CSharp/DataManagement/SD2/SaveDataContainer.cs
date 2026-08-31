using UnityEngine;

public class SaveDataContainer : MonoBehaviour
{
    [Header("Shared Data")]
    public int saveFileVersion;
    public int[] gameVersion;
    public string playerName;
    
    [Header("Story Mode")]
    public int[] sItems_classic;
    public int[] sItems_classicExtended;
    public int[] sItems_juniperHills;
    public int[] sBestTimes;
    public int[] sDetentions;

    [Header("Endless Mode")]
    public int[] eItems_classic;
    public int[] eItems_classicExtended;
    public int[] eItems_juniperHills;
    public int[] eNotebooks;
    public int[] eDetentions;

    [Header("Challenge Mode")]
    public bool[] challengeUnlocks;
    public int[] cItemsDarkMode;
    public int[] cBestTimes;
    public int[] cDetentions;

    public void SetData(string jsonData)
    {
        JsonUtility.FromJsonOverwrite(jsonData, this);
    }
}
