using System.IO;
using System.Collections;
using UnityEngine;
using UpgradeSystem;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using System.Diagnostics;
using System;

public class Launcher : MonoBehaviour
{
    private void Start()
    {
        this.stopCanvas.SetActive(false);
        this.logoCanvas.SetActive(false);
        this.launcherCanvas.SetActive(false);
        this.fileCanvas.SetActive(false);
        this.upgradeCanvas_Start.SetActive(false);
        this.upgradeCanvas_Process.SetActive(false);
        this.upgradeCanvas_End.SetActive(false);
        this.errorCanvas.SetActive(false);
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
            this.versionText.text += "Beta V" + this.updateScript.stableBuild;

        this.savePath = Application.persistentDataPath + "/BaldiData/";
        this.FileChecks();
    }

    private void FileChecks()
    {
        this.upgradeFlag = "none";

        if (!Directory.Exists(this.savePath))
        {
            Directory.CreateDirectory(this.savePath);
            this.upgradeFlag = "factory";
        }
        else if (!File.Exists(savePath + "story.sav") || !File.Exists(savePath + "endless.sav") || !File.Exists(savePath + "challenge.sav"))
            this.upgradeFlag = "fileMissing";
        
        if (!Directory.Exists(Application.persistentDataPath + "/BaldiData_Backup"))
            Directory.CreateDirectory(Application.persistentDataPath + "/BaldiData_Backup");
    }

    private void Update()
    {
        // This is only here because OBS doesn't pick up the game right away when launching
        if (this.allowInterruption && Input.GetKeyDown(KeyCode.Alpha7))
        {
            this.isInterrupted = true;
            this.stopCanvas.SetActive(true);
        }
    }

    public void RestartBootup()
    {
        this.stopCanvas.SetActive(false);
        this.StartCoroutine(this.WaitForStart());
    }

    private IEnumerator WaitForStart()
    {
        this.isInterrupted = false;
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
            this.audioDevice.PlayOneShot(music);
            
            if (PlayerPrefs.HasKey("highbooks_Classic") || PlayerPrefs.HasKey("highbooks_ClassicExtended") || PlayerPrefs.HasKey("highbooks_JuniperHills"))
                UnityEngine.Debug.Log("playerprefs load");
                //SaveDataController.UpgradeSaves("endless", this.saveFileVersion);
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
            case 81: // Opens the location of the save file data
                this.audioDevice.PlayOneShot(this.acceptSound);
                this.OpenFileLocation("mainSave");
                break;
            case 82: // Opens the location of the backup save data
                this.audioDevice.PlayOneShot(this.acceptSound);
                this.OpenFileLocation("backupSave");
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

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    private void OpenFileMenu()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
            this.helpfulLinks.SetActive(false);
        
        this.fileCanvas.SetActive(true);
    }

    public void OpenFileLocation(string location)
    {
        try {
            switch(location)
            {
                case "mainSave":
                    if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        string systemUsername = Environment.UserName;
                        string folderPath = "C:\\Users\\" + systemUsername + "\\AppData\\LocalLow\\JuniDev\\BaldiExtended\\BaldiData";
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "explorer.exe", Arguments = folderPath
                        };
                        Process.Start(startInfo);
                    }
                    break;
                case "backupSave":
                    if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        string systemUsername = Environment.UserName;
                        string folderPath = "C:\\Users\\" + systemUsername + "\\AppData\\LocalLow\\JuniDev\\BaldiExtended\\BaldiData_Backup";
                        ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                            FileName = "explorer.exe", Arguments = folderPath
                        };
                        Process.Start(startInfo);
                    }
                    break;
            }
        }
        catch(System.Exception e) {
            this.launcherCanvas.SetActive(false);
            this.ThrowError(e.ToString());
        }
    }

    private IEnumerator WaitForLaunch()
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

        //if (!SaveDataController.CheckFileExist("endless"))

        if (this.upgradeFlag != "none")
        {
            switch(this.upgradeFlag)
            {
                case "factory":
                    this.StartCoroutine(this.WaitForFactoryFileCreation());
                    break;
                case "fileMissing":
                    this.SaveMenuPrompt();
                    break;
            }
        }
        else
            this.StartCoroutine(this.WaitForLogo());
    }

    private IEnumerator WaitForQuit(float delay)
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

    private IEnumerator WaitForLogo()
    {
        this.loadingManager = FindObjectOfType<LoadingManager>();
        this.logoCanvas.SetActive(true);
        this.basicallyLogo.SetActive(true);
        this.juniLogo.SetActive(false);
        
        SaveDataController.LoadEndlessData();

        float remTime = 2.5f;

        while (remTime > 0f)
        {
            remTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.basicallyLogo.SetActive(false);
        this.juniLogo.SetActive(true);

        remTime = 1f;

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

    private void AdvanceUpgradePrompt()
    {
        this.upgradeState++;
    }

    private IEnumerator WaitForFactoryFileCreation()
    {
        this.upgradeState = 0;
        this.logoCanvas.SetActive(true);
        this.upgradeCanvas_Start.SetActive(true);
        this.upgradeText_start.text = "Welcome to Baldi's Extended Schoolhouse!\n\nNew save files will now be created.";

        while (this.upgradeState == 0)
            yield return null;

        this.upgradeCanvas_Process.SetActive(true);
        this.upgradeCanvas_Start.SetActive(false);
        this.upgradeText_process.text = "Creating story data...";
        try {
            SaveDataController.SaveStoryData(null);
        }
        catch (System.Exception e) {
            this.ThrowError(e.ToString());
            UnityEngine.Debug.LogError(e);
            yield break;
        }

        float remTime = 0.5f;
        while (remTime > 0f)
        {
            remTime -= Time.unscaledDeltaTime;
            yield return null;
        }
        
        this.upgradeText_process.text = "Creating endless data...";
        try {
            SaveDataController.SaveEndlessData(null);
        }
        catch (System.Exception e) {
            this.ThrowError(e.ToString());
            UnityEngine.Debug.LogError(e);
            yield break;
        }
        
        remTime = 0.5f;
        while (remTime > 0f)
        {
            remTime -= Time.deltaTime;
            yield return null;
        }

        this.upgradeText_process.text = "Creating challenge data...";
        try {
            SaveDataController.SaveChallengeData(null);
        }
        catch (System.Exception e) {
            this.ThrowError(e.ToString());
            UnityEngine.Debug.LogError(e);
            yield break;
        }
        
        remTime = 0.5f;
        while (remTime > 0f)
        {
            remTime -= Time.deltaTime;
            yield return null;
        }

        this.AdvanceUpgradePrompt();
        this.upgradeCanvas_End.SetActive(true);
        this.upgradeCanvas_Process.SetActive(false);
        this.upgradeText_End.text = "Save file creation complete.\n\n\nNew save files have been created at the following location:";
        this.upgradeText_EndLocation.text = this.savePath;

        while (this.upgradeState == 2)
            yield return null;
        
        this.upgradeCanvas_End.SetActive(false);
        this.StartCoroutine(this.WaitForLogo());
    }

    private void ThrowError(string error)
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

    private IEnumerator WaitForFileChecks()
    {
        this.upgradeState = 0;
        this.logoCanvas.SetActive(true);
        this.upgradeCanvas_Start.SetActive(true);
        this.upgradeText_start.text = "One or more save files were not found.\n\nNew save files will now be created.";

        while (this.upgradeState == 0)
            yield return null;

        this.upgradeCanvas_Process.SetActive(true);
        this.upgradeCanvas_Start.SetActive(false);
        this.upgradeText_process.text = "Creating data...";

        if (!File.Exists(this.savePath + "story.sav"))
        {
            try {
                SaveDataController.SaveStoryData(null);
            }
            catch (System.Exception e) {
                this.ThrowError(e.ToString());
                UnityEngine.Debug.LogError(e);
                yield break;
            }
        }
        if (!File.Exists(this.savePath + "endless.sav"))
        {
            try {
                SaveDataController.SaveEndlessData(null);
            }
            catch (System.Exception e) {
                this.ThrowError(e.ToString());
                UnityEngine.Debug.LogError(e);
                yield break;
            }
        }
        if (!File.Exists(this.savePath + "challenge.sav"))
        {
            try {
                SaveDataController.SaveChallengeData(null);
            }
            catch (System.Exception e) {
                this.ThrowError(e.ToString());
                UnityEngine.Debug.LogError(e);
                yield break;
            }
        }

        float remTime = 1.5f;

        while (remTime > 0f)
        {
            remTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.AdvanceUpgradePrompt();
        this.upgradeCanvas_End.SetActive(true);
        this.upgradeCanvas_Process.SetActive(false);
        this.upgradeText_End.text = "Save file creation complete.\n\n\nNew save files have been created at the following location:";
        this.upgradeText_EndLocation.text = this.savePath;

        while (this.upgradeState == 2)
            yield return null;
        
        this.upgradeCanvas_End.SetActive(false);
        this.StartCoroutine(this.WaitForLogo());
    }

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
    private readonly int saveFileVersion = 1;
    [SerializeField] private string savePath;

    [Header("UI")]
    [SerializeField] private GameObject launcherCanvas;
    [SerializeField] private GameObject logoCanvas;
    [SerializeField] private GameObject stopCanvas;
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
    [SerializeField] private TMP_Text upgradeText_EndLocation;
    [SerializeField] private GameObject errorCanvas;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private TMP_Text versionText;
}
