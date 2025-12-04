using System.IO;
using System.Collections;
using UnityEngine;
using UpgradeSystem;
using OldSaveData;
using TMPro;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = 1;

        this.stopCanvas.SetActive(false);
        this.logoCanvas.SetActive(false);
        this.launcherCanvas.SetActive(false);
        this.fileCanvas.SetActive(false);
        this.upgradeCanvas_Start.SetActive(false);
        this.upgradeCanvas_Process.SetActive(false);
        this.upgradeCanvas_End.SetActive(false);
        this.errorCanvas.SetActive(false);
        this.presentFiles = new bool[4];

        this.StartCoroutine(this.WaitForStart());

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor)
            this.versionText.text = "EDITOR\n";
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
            this.versionText.text = "Windows Edition - ";
        else if (Application.platform == RuntimePlatform.LinuxPlayer)
            this.versionText.text = "Linux Edition - ";

        if (this.updateScript.isNightly)
            this.versionText.text += "Development build " + this.updateScript.nightlyBuild;
        else
            this.versionText.text += "Beta v" + this.updateScript.stableBuild;

        this.savePath = Application.persistentDataPath + "/BaldiData/";
        this.FileChecks();
    }

    void FileChecks()
    {
        this.upgradeFlag = "none";

        if (!Directory.Exists(this.savePath))
        {
            Directory.CreateDirectory(this.savePath);
            if (!Directory.Exists(Application.persistentDataPath + "/BaldiData_Backup"))
                Directory.CreateDirectory(Application.persistentDataPath + "/BaldiData_Backup");
            this.upgradeFlag = "factory";
            return;
        }

        if (File.Exists(savePath + "story.sav"))
            presentFiles[0] = true;
        if (File.Exists(savePath + "endless.sav"))
            presentFiles[1] = true;
        if (File.Exists(savePath + "challenge.sav"))
            presentFiles[2] = true;
        if (File.Exists(savePath + "progression.sav"))
            presentFiles[3] = true;
        
        int checkedFiles = 0;
        
        for (int i = 0; i < presentFiles.Length; i++)
        {
            if (presentFiles[i])
                checkedFiles++;
        }

        if (checkedFiles != 4)
            this.upgradeFlag = "fileMissing";
        
        if (!Directory.Exists(Application.persistentDataPath + "/BaldiData_Backup"))
            Directory.CreateDirectory(Application.persistentDataPath + "/BaldiData_Backup");
    }

    void Update()
    {
        // This is only here because OBS doesn't pick up the game right away when launching
        if (this.allowInterruption && Input.GetKeyDown(KeyCode.Alpha7))
        {
            this.isInterrupted = true;
            this.stopCanvas.SetActive(true);
        }
    }

    public void RestartBootup(bool isSave)
    {
        if (isSave)
            PlayerPrefs.SetInt("saveNoticeSeen", 1);

        this.stopCanvas.SetActive(false);
        this.saveNoticeCanvas.SetActive(false);
        this.newSaveUpgradeCanvas.SetActive(false);
        this.StartCoroutine(this.WaitForStart());
        this.isInterrupted = false;
    }

    IEnumerator WaitForStart()
    {
        this.allowInterruption = true;

        float time = 0.3f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        this.allowInterruption = false;

        if (!isInterrupted)
        {
            this.launcherCanvas.SetActive(true);
            this.bannerCanvas.SetActive(true);
            this.audioDevice.PlayOneShot(this.music);
            
            if (PlayerPrefs.HasKey("highbooks_Classic") || PlayerPrefs.HasKey("highbooks_ClassicExtended")
            || PlayerPrefs.HasKey("highbooks_JuniperHills"))
                UnityEngine.Debug.Log("Playerprefs highscore data found. Will be deleted upon save file creation.");
        }
    }

    public void DaClick(int id)
    {
        switch(id)
        {
            case 2: // Github
                this.audioDevice.PlayOneShot(this.acceptSound);
                Application.OpenURL("https://github.com/Junimeek/BaldiExtended");
                break;
            case 4: // Upgrade prompts
                this.AdvanceUpgradePrompt();
                break;
            case 5: // Fast Quit
                this.StartCoroutine(this.WaitForQuit(0.1f));
                break;
            default:
                if (!this.isLaunching)
                {
                    switch(id)
                    {
                        case 1: // Generic
                            this.audioDevice.PlayOneShot(this.acceptSound);
                            break;
                        case 3: // Quit Game
                            this.audioDevice.PlayOneShot(this.quitSound);
                            this.StartCoroutine(this.WaitForQuit(1.2f));
                            break;
                        case 6: // Open File Management
                            this.audioDevice.PlayOneShot(this.acceptSound);
                            this.OpenFileMenu();
                            break;
                        case 99:
                            this.audioDevice.volume = 0.5f;
                            this.audioDevice.PlayOneShot(this.splap);
                            this.StartCoroutine(this.WaitForLaunch());
                            break;
                    }
                }
                break;
        }
    }

    public void OpenSaveUpgradeDialog()
    {
        this.launcherCNews.SetActive(true);
        this.launcherCMain.SetActive(false);
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    void OpenFileMenu()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
            this.helpfulLinks.SetActive(false);
        
        this.fileCanvas.SetActive(true);
    }

    IEnumerator WaitForLaunch()
    {
        float time = 0.5f;
        this.isLaunching = true;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.launcherCanvas.SetActive(false);
        this.audioDevice.volume = 0f;

        if (this.upgradeFlag != "none")
        {
            this.SaveMenuPrompt();
        }
        else
            this.StartCoroutine(this.WaitForLogo());
    }

    IEnumerator WaitForQuit(float delay)
    {
        float time = delay;
        this.isLaunching = true;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        UnityEngine.Debug.LogWarning("Game Quit");
        Application.Quit();
    }

    IEnumerator WaitForLogo()
    {
        this.loadingManager = FindObjectOfType<LoadingManager>();
        this.logoCanvas.SetActive(true);
        this.basicallyLogo.SetActive(true);
        this.juniLogo.SetActive(false);

        float remTime = 2.5f;

        while (remTime > 0f)
        {
            remTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.basicallyLogo.SetActive(false);
        this.juniLogo.SetActive(true);

        remTime = 1.5f;

        while (remTime > 0f)
        {
            remTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.loadingManager.LoadNewScene("Warning", 1);
    }

    public void SaveMenuPrompt()
    {
        switch(this.upgradeFlag)
        {
            case "factory":
                StartCoroutine(this.WaitForFactoryFileCreation());
                break;
            case "fileMissing":
                StartCoroutine(this.WaitForFileChecks());
                break;
        }
    }

    void AdvanceUpgradePrompt()
    {
        this.upgradeState++;
    }

    IEnumerator WaitForFactoryFileCreation()
    {
        this.upgradeState = 0;
        this.saveNoticeCanvas.SetActive(true);
        this.saveHead.Play("SaveHead_Nod");

        while (this.upgradeState == 0)
            yield return null;
        
        try
        {
            CreateMissingFiles();
            this.logoCanvas.SetActive(true);
            this.saveNoticeCanvas.SetActive(false);
            StartCoroutine(WaitForLogo());
        }
        catch (System.Exception e)
        {
            this.ThrowError(e.ToString());
        }
    }

    void ThrowError(string error)
    {
        this.isLaunching = false;
        this.upgradeCanvas_Start.SetActive(false);
        this.upgradeCanvas_Process.SetActive(false);
        this.upgradeCanvas_End.SetActive(false);
        this.errorCanvas.SetActive(true);
        this.upgradeState = 99;
        this.errorText.text = error;
        UnityEngine.Debug.LogError(error);
    }

    IEnumerator WaitForFileChecks()
    {
        this.upgradeState = 0;
        this.logoCanvas.SetActive(true);
        this.upgradeCanvas_Start.SetActive(true);
        this.upgradeText_start.text = "One or more save files were not found.\n\nUpon clicking the button below, new files will be created to replace any missing ones.";

        while (this.upgradeState == 0)
            yield return null;
        
        try
        {
            CreateMissingFiles();
            this.upgradeCanvas_Start.SetActive(false);
            StartCoroutine(this.WaitForLogo());
        }
        catch (System.Exception e)
        {
            this.upgradeState = 99;
            this.ThrowError(e.ToString());
        }
    }

    void CreateMissingFiles()
    {
        if (!presentFiles[0])
            OldSaveDataLoader.SaveOldStoryData(null);
        if (!presentFiles[1])
            OldSaveDataLoader.SaveOldEndlessData(null);
        if (!presentFiles[2])
            OldSaveDataLoader.SaveOldChallengeData(null);
        if (!presentFiles[3])
            OldSaveDataLoader.SaveOldProgressionData(null);
    }

    [SerializeField] bool enableJsonEncryption;
    [SerializeField] SaveDataContainer dataContainer;
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private VersionCheck updateScript;
    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip splap;
    [SerializeField] private AudioClip quitSound;
    [SerializeField] private AudioClip acceptSound;
    [SerializeField] private bool isLaunching;
    [SerializeField] private bool allowInterruption;
    [SerializeField] private bool isInterrupted;
    [SerializeField] private int upgradeState;

    [Header("Save File")]
    [SerializeField] private string upgradeFlag;
    [SerializeField] private string savePath;
    [SerializeField] bool[] presentFiles;

    [Header("UI")]
    [SerializeField] private GameObject launcherCanvas;
    [SerializeField] GameObject launcherCMain;
    [SerializeField] GameObject launcherCNews;
    [SerializeField] private GameObject logoCanvas;
    [SerializeField] private GameObject stopCanvas;
    [SerializeField] GameObject bannerCanvas;
    [SerializeField] private GameObject saveNoticeCanvas;
    [SerializeField] private Animator saveHead;
    [SerializeField] private GameObject basicallyLogo;
    [SerializeField] private GameObject juniLogo;
    [SerializeField] private GameObject fileCanvas;
    [SerializeField] private GameObject helpfulLinks;
    [SerializeField] private GameObject upgradeCanvas_Start;
    [SerializeField] private TMP_Text upgradeText_start;
    [SerializeField] private GameObject upgradeCanvas_Process;
    [SerializeField] private TMP_Text upgradeText_process;
    [SerializeField] private GameObject upgradeCanvas_End;
    [SerializeField] private TMP_Text upgradeText_End;
    [SerializeField] GameObject newSaveUpgradeCanvas;
    [SerializeField] private TMP_Text upgradeText_EndLocation;
    [SerializeField] private GameObject errorCanvas;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private TMP_Text versionText;
}
