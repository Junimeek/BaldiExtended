using System.IO;
using System.Collections;
using UnityEngine;
using UpgradeSystem;
using TMPro;

public class Launcher : MonoBehaviour
{
    private void Start()
    {
        stopCanvas.SetActive(false);
        logoCanvas.SetActive(false);
        launcherCanvas.SetActive(false);
        this.upgradeCanvas.SetActive(false);
        StartCoroutine(WaitForStart());

        if (updateScript.isNightly)
            this.versionText.text = "Build: " + updateScript.nightlyBuild;
        else
            this.versionText.text = string.Empty;
        
        this.FileChecks();
    }

    private void FileChecks()
    {
        this.upgradeFlag = "none";

        if (!Directory.Exists(Application.persistentDataPath + "/BaldiData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/BaldiData");
            this.upgradeFlag = "factory";
        }
        
        if (!Directory.Exists(Application.persistentDataPath + "/BaldiData_Backup"))
            Directory.CreateDirectory(Application.persistentDataPath + "/BaldiData_Backup");
    }

    private void Update()
    {
        // This is only here because OBS doesn't pick up the game right away when launching
        if (allowInterruption && Input.GetKeyDown(KeyCode.Alpha7))
        {
            isInterrupted = true;
            stopCanvas.SetActive(true);
        }
    }

    public void RestartBootup()
    {
        stopCanvas.SetActive(false);
        StartCoroutine(WaitForStart());
    }

    private IEnumerator WaitForStart()
    {
        isInterrupted = false;
        allowInterruption = true;
        float time = 0.3f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        allowInterruption = false;

        if (!isInterrupted)
        {
            launcherCanvas.SetActive(true);
            audioDevice.PlayOneShot(music);
            
            if (PlayerPrefs.HasKey("highbooks_Classic") || PlayerPrefs.HasKey("highbooks_ClassicExtended") || PlayerPrefs.HasKey("highbooks_JuniperHills"))
                SaveDataController.UpgradeSaves("endless", this.saveFileVersion);
        }
    }

    public void DaClick(int id)
    {
        if (id == 1 && !isLaunching) // Generic
            audioDevice.PlayOneShot(acceptSound);

        else if (id == 2) // Github
        {
            audioDevice.PlayOneShot(acceptSound);
            Application.OpenURL("https://github.com/Junimeek/BaldiExtended");
        }

        else if (id == 3 && !isLaunching) // Quit Game
        {
            audioDevice.PlayOneShot(quitSound);
            StartCoroutine(WaitForQuit());
        }

        else if (id == 99 && !isLaunching) // Launch Game
        {
            audioDevice.volume = 0.5f;
            audioDevice.PlayOneShot(splap);
            StartCoroutine(WaitForLaunch());
        }
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    private IEnumerator WaitForLaunch()
    {
        float time = 0.5f;
        isLaunching = true;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.logoCanvas.SetActive(true);
        this.launcherCanvas.SetActive(false);
        this.audioDevice.volume = 0f;

        if (this.upgradeFlag != "none")
        {
            this.basicallyLogo.SetActive(false);
            this.save_init.SetActive(false);
            this.save_upgrade.SetActive(false);

            switch(this.upgradeFlag)
            {
                case "factory":
                    this.StartCoroutine(this.WaitForFactoryFileCreation());
                    break;
            }
        }
        else
        {
            StartCoroutine(WaitForLogo());
        }
    }

    private IEnumerator WaitForQuit()
    {
        float time = 1.2f;
        isLaunching = true;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        Debug.LogWarning("Game Quit");
        Application.Quit();
    }

    private IEnumerator WaitForLogo()
    {
        loadingManager = FindObjectOfType<LoadingManager>();
        basicallyLogo.SetActive(true);
        juniLogo.SetActive(false);
        
        SaveDataController.LoadEndlessData();

        float time1 = 2.5f;

        while (time1 > 0f)
        {
            time1 -= Time.unscaledDeltaTime;
            yield return null;
        }

        basicallyLogo.SetActive(false);
        juniLogo.SetActive(true);

        float time2 = 1f;

        while (time2 > 0f)
        {
            time2 -= Time.unscaledDeltaTime;
            yield return null;
        }

        loadingManager.LoadNewScene("Warning", 1);
    }

    private IEnumerator WaitForFactoryFileCreation()
    {
        this.upgradeCanvas.SetActive(true);
        this.save_init.SetActive(true);
        float remTime = 2f;

        // Replace this with something else later
        SaveDataController.UpgradeSaves("endless", 1);

        while (remTime > 0f)
        {
            remTime -= Time.deltaTime;
            yield return null;
        }

        this.upgradeCanvas.SetActive(false);
        this.StartCoroutine(this.WaitForLogo());
    }

    [SerializeField] private readonly int saveFileVersion = 1;
    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip splap;
    [SerializeField] private AudioClip quitSound;
    [SerializeField] private AudioClip acceptSound;
    [SerializeField] private bool isLaunching;
    [SerializeField] private bool allowInterruption;
    [SerializeField] private bool isInterrupted;
    [SerializeField] private GameObject launcherCanvas;
    [SerializeField] private GameObject logoCanvas;
    [SerializeField] private GameObject stopCanvas;
    [SerializeField] private GameObject basicallyLogo;
    [SerializeField] private GameObject juniLogo;
    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private GameObject save_init;
    [SerializeField] private GameObject save_upgrade;
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private VersionCheck updateScript;
    [SerializeField] private string upgradeFlag;
}
