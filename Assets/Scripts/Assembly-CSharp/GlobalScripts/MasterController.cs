using UnityEngine;
using TMPro;

public class MasterController : MonoBehaviour
{
    void Awake()
    {
        this.modeSetting = this.GetGameMode();
        this.PushVariables();
    }

    void PushVariables()
    {
        gc.stats = this.statisticsController;
        gc.notebookCountScript = this.notebookCountScript;
        gc.daFinalBookCount = this.finalNotebookCount;
        gc.totalSlotCount = this.inventorySlotCount;
        gc.gameMode = this.modeSetting;
        gc.currentChallenge = this.GetCurrentChallenge();
        gc.notebookActivityType = this.GetActivityType();
        gc.entranceList = this.exitList;
        gc.vendingMachines = this.vendingMachines;
        gc.attendanceOffice = this.attendanceOfficeLocation;
        gc.principalScript = this.princeyScript;
        gc.crafterScript = this.crafterScript;
        gc.detentionPlayerPos = this.detentionPlayerPosition;
        gc.detentionPrincipalPos = this.detentionPrincipalPosition;
        gc.forceQuarterPickup = this.forceQuarterPickup;
        gc.player = this.playerScript;
        gc.baldiScrpt = this.baldiScript;
        gc.notebookCountScript = this.notebookCountScript;
        gc.itemSelected = 0;
        gc.notebookCountText = this.notebookCountText;
        gc.notebookObject = this.notebookObject;
        gc.staminaPercentText = this.staminaPercentText;
        gc.itemSelect = this.itemSelectBG;
        gc.handIconScript = this.handIconScript;
        gc.speedrunMinutesText = this.speedrunMinutesText;
        gc.speedrunSecondsText = this.speedrunSecondsText;
        gc.dollarTextCenter = this.dollarTextCenter;
        gc.dollarTextTop = this.dollarTextTop;
        gc.exitCountGroup = this.exitCountDisplay;
        gc.exitCountText = this.exitCountText;
        gc.fpsCounter = this.fpsCounter;
        gc.mapScript = this.mapCameraScript;
        gc.playerFlashlights = this.playerFlashlights;
        gc.craftersWarpPoints = this.craftersWarpPoints;
        notifboardScript.gc = this.gc;
        notifboardScript.ruleText = this.notifRuleText;
        notifboardScript.detentionText = this.notifDetentionText;
        notifboardScript.notebooText = this.notifNotebookText;
        notifboardScript.notebooGroup = this.notifNotebookGroup;
        notifboardScript.detentionGroup = this.notifDetentionGroup;
        notifboardScript.ruleGroup = this.notifRuleGroup;
        notifboardScript.notebooColor = this.notifNotebooColor;
        this.playerScript.sneakerIcon = this.sneakerIcon;
        
        if (sweepAttendanceDetectorScript != null)
            sweepAttendanceDetectorScript.sweepParent = this.gottaSweep;
    }

    GameControllerScript.GameMode GetGameMode()
    {
        return this.gameMode switch
        {
            ForceMode.StoryMode => GameControllerScript.GameMode.Story,
            ForceMode.EndlessMode => GameControllerScript.GameMode.Endless,
            ForceMode.ChallengeMode => GameControllerScript.GameMode.Challenge,
            _ => this.GetGameModeFromRegistry(),
        };
    }

    GameControllerScript.CurrentChallenge GetCurrentChallenge()
    {
        switch(this.challengeType)
        {
            case ChallengeType.DarkMode:
                return GameControllerScript.CurrentChallenge.DarkMode;
            case ChallengeType.Vertigo:
                return GameControllerScript.CurrentChallenge.Vertigo;
            default:
                return GameControllerScript.CurrentChallenge.None;
        }
    }

    GameControllerScript.NotebookActivityType GetActivityType()
    {
        switch(this.activityType)
        {
            case ActivityType.Math:
                return GameControllerScript.NotebookActivityType.Math;
            default:
                return GameControllerScript.NotebookActivityType.None;
        }
    }

    GameControllerScript.GameMode GetGameModeFromRegistry()
    {
        return PlayerPrefs.GetString("CurrentMode") switch
        {
            "story" => GameControllerScript.GameMode.Story,
            "endless" => GameControllerScript.GameMode.Endless,
            "challenge" => GameControllerScript.GameMode.Challenge,
            _ => GameControllerScript.GameMode.Story,
        };
    }

    [Header("Game Configuration")]
    public int finalNotebookCount;
    public int inventorySlotCount;
    public EntranceScript[] exitList;
    [SerializeField] VendingMachineScript[] vendingMachines;
    public enum ForceMode
    {
        DontForce, StoryMode, EndlessMode, ChallengeMode
    }
    public ForceMode gameMode;
    enum ChallengeType
    {
        None, DarkMode, Vertigo
    }
    [SerializeField] ChallengeType challengeType;
    enum ActivityType
    {
        None, Math
    }
    [SerializeField] ActivityType activityType;
    GameControllerScript.GameMode modeSetting;
    [SerializeField] CraftersWarpPoints[] craftersWarpPoints;
    public Transform attendanceOfficeLocation;
    public Vector3 detentionPlayerPosition;
    public Vector3 detentionPrincipalPosition;
    public bool forceQuarterPickup;

    [Header("UI")]
    [SerializeField] Light[] playerFlashlights;
    [SerializeField] HandIconScript handIconScript;
    [SerializeField] NotebookCountScript notebookCountScript;
    [SerializeField] TMP_Text notebookCountText;
    [SerializeField] GameObject sneakerIcon;
    [SerializeField] GameObject notebookObject;
    [SerializeField] TMP_Text staminaPercentText;
    [SerializeField] TMP_Text speedrunMinutesText;
    [SerializeField] TMP_Text speedrunSecondsText;
    [SerializeField] TMP_Text dollarTextCenter;
    [SerializeField] TMP_Text dollarTextTop;
    [SerializeField] RectTransform itemSelectBG;
    [SerializeField] GameObject exitCountDisplay;
    [SerializeField] TMP_Text exitCountText;
    [SerializeField] TMP_Text fpsCounter;
    [SerializeField] MapCameraScript mapCameraScript;

    [Header("Notification Board")]
    [SerializeField] NotificationBoard notifboardScript;
    [SerializeField] TMP_Text notifRuleText;
    [SerializeField] TMP_Text notifDetentionText;
    [SerializeField] TMP_Text notifNotebookText;
    [SerializeField] GameObject notifNotebookGroup;
    [SerializeField] GameObject notifDetentionGroup;
    [SerializeField] GameObject notifRuleGroup;
    [SerializeField] Color notifNotebooColor;

    [Header("Game Objects")]
    [SerializeField] GameObject gottaSweep;

    [Header("Scripts")]
    [SerializeField] GameControllerScript gc;
    [SerializeField] StatisticsController statisticsController;
    [SerializeField] PlayerScript playerScript;
    [SerializeField] BaldiScript baldiScript;
    [SerializeField] PrincipalScript princeyScript;
    [SerializeField] CraftersScript crafterScript;
    [SerializeField] AttendanceDetector sweepAttendanceDetectorScript;
}
