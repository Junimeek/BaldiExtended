using UnityEngine;
using OldSaveData;

public class ProgressionController : MonoBehaviour
{
    private void Start()
    {
        this.LoadProgressionData();
    }

    private void LoadProgressionData()
    {
        ProgressionData data = OldSaveDataLoader.LoadOldProgressionData();

        this.fileVersion = data.fileVersion;
        this.mapUnlocks = data.mapUnlocks;
    }

    public void SaveProgressionData()
    {
        OldSaveDataLoader.SaveOldProgressionData(this);
    }

    public ushort fileVersion;
    public bool[] mapUnlocks;
}
