using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    private void Start()
    {
        this.LoadProgressionData();
    }

    private void LoadProgressionData()
    {
        ProgressionData data = SaveDataController.LoadProgressionData();

        this.fileVersion = data.fileVersion;
        this.mapUnlocks = data.mapUnlocks;
    }

    public void SaveProgressionData()
    {
        SaveDataController.SaveProgressionData(this);
    }

    public ushort fileVersion;
    public bool[] mapUnlocks;
}
