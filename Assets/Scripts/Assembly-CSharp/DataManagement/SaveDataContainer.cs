using UnityEngine;

public class SaveDataContainer : MonoBehaviour
{
    [Header("Shared Data")]
    public int currentLoadedFileID;
    public int saveFileVersion;
    public int[] gameVersion;
    public int[] bestTimes;
    
    [Header("Story Mode")]
    public int[] sItems_classic;
    public int[] sItems_classicExtended;
    public int[] sItems_juniperHills;
    public int[] sItems_mitakihara;
    public int[] sDetentions;

    [Header("Endless Mode")]
    public int[] eItems_classic;
    public int[] eItems_classicExtended;
    public int[] eItems_juniperHills;
    public int[] eItems_mitakihara;
    public int[] eDetentions;
    public int[] eNotebooks;

    [Header("Challenge Mode")]
    public bool[] challengeUnlocks;
    public int[] cItemsClassicDark;

    public void SetData(string jsonData)
    {
        JsonUtility.FromJsonOverwrite(jsonData, this);
    }
}
