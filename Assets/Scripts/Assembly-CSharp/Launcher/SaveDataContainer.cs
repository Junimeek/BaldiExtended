using UnityEngine;

public class SaveDataContainer : MonoBehaviour
{
    public int currentLoadedFileID;
    public int saveFileVersion;
    public int[] gameVersion;
    public bool[] challengeUnlocks;
    public int[] items_classic;
    public int[] items_classicExtended;
    public int[] items_juniperHills;
    public int[] items_classicDark;
    public int[] items_mitakihara;
    public float[] bestTimes;
    public int[] totalDetentions;
    public int[] endlessNotebooks;

    public void SetData(string jsonData)
    {
        JsonUtility.FromJsonOverwrite(jsonData, this);
    }
}
