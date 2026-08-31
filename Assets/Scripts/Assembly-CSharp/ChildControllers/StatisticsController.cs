using UnityEngine;

public class StatisticsController : MonoBehaviour
{
    void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        this.mapID = this.GetMapID();

        this.finalMS = 0;
        this.itemsUsed = new int[18];
        this.detentions = 0;

        this.InitializeSaveController();
        this.itemUsageOrder = new int[20];
        this.itemUsageOrderIndex = 0;
    }

    void Update()
    {
        this.currentSeconds += Time.unscaledDeltaTime;
        if (this.currentSeconds >= 60f)
        {
            this.currentMinutes++;
            this.currentSeconds -= 60f;
        }
    }

    public void UpdateItemCount(int itemID)
    {
        this.itemsUsed[itemID]++;
        this.UpdateItemOrder(itemID);
    }

    void UpdateItemOrder(int itemID)
    {
        this.itemUsageOrderIndex++;
        if (this.itemUsageOrderIndex >= this.itemUsageOrder.Length)
            System.Array.Resize(ref this.itemUsageOrder, this.itemUsageOrderIndex + 10);

        this.itemUsageOrder[itemUsageOrderIndex - 1] = itemID;
    }

    void InitializeSaveController()
    {
        saveController.SetCurrentGamemode(gc.gameMode);
        saveController.currentMapName = this.GetMapName();
    }

    public void BeginSaveProcess(bool calculateBestTime)
    {
        if (this.saveProcessStarted)
            return;
        
        this.saveProcessStarted = true;
        
        if (calculateBestTime)
        {
            Debug.Log("Beginning save process and checking best time.");
            this.finalMS = this.GetTimeInMilliseconds();
        }
        else
        {
            Debug.Log("Beginning save process and ignoring best time.");
            this.finalMS = 0;
        }
        
        this.SendRunData();
        saveController.OverwriteFullSaveFile(this);
    }

    int GetTimeInMilliseconds()
    {
        float truncatedSeconds = System.MathF.Round(this.currentSeconds, 3);
        int secondsInMilliseconds = Mathf.RoundToInt(truncatedSeconds * 1000f);
        int minutesInMilliseconds = this.currentMinutes * 60000;
        return secondsInMilliseconds + minutesInMilliseconds;
    }

    void SendRunData()
    {
        saveController.currentMapID = this.mapID;
        saveController.currentMapName = this.GetMapName();
        saveController.currentItemsUsed = this.itemsUsed;
        saveController.currentTime = this.finalMS;
        saveController.currentDetentions = this.detentions;
        saveController.currentNotebookScore = this.notebooks;

        SaveHead saveHead = FindObjectOfType<SaveHead>();
        if (saveHead != null)
            saveHead.ActivateSaveHead(1.4f);
    }

    string GetMapName()
    {
        GameControllerScript.GameMode curMode = gc.gameMode;
        if (curMode == GameControllerScript.GameMode.Challenge)
        {
            switch(this.mapID)
            {
                case 1:
                    return "ClassicDark";
                case 2:
                    return "Vertigo";
                default:
                    return string.Empty;
            }
        }
        else
        {
            switch(this.mapID)
            {
                case 1:
                    return "Classic";
                case 2:
                    return "ClassicExtended";
                case 3:
                    return "JuniperHills";
                case 4:
                    return "Mitakihara";
                default:
                    return string.Empty;
            }
        }
    }

    int GetMapID()
    {
        if (this.gc.gameMode != GameControllerScript.GameMode.Challenge)
        {
            return this.gc.curMap switch
            {
                "Classic" => 1,
                "ClassicExtended" => 2,
                "JuniperHills" => 3,
                "Mitakihara" => 4,
                _ => 1,
            };
        }
        else
        {
            return this.gc.curMap switch
            {
                "ClassicDark" => 1,
                "Vertigo" => 2,
                _ => 1,
            };
        }
    }

    [SerializeField] GameControllerScript gc;
    [SerializeField] SD3Controller saveController;
    [SerializeField] static StatisticsController instance;

    [Header("Game State")]
    public bool disableSaving;
    [SerializeField] bool saveProcessStarted;
    [SerializeField] int[] itemsUsed;
    [SerializeField] int itemUsageOrderIndex;
    public int[] itemUsageOrder;
    public int mapID;
    public float currentSeconds;
    public int currentMinutes;
    public int finalMS;
    public int prevBestTime;
    public int notebooks;
    public int detentions;
}
