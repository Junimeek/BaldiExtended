using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsContainer : MonoBehaviour
{
    /*
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    */

    void Start()
    {
        this.dataPath = Application.persistentDataPath + "/settings.sav";
        StartCoroutine(this.CheckForStatsObject());

        if (!File.Exists(this.dataPath))
            this.ResetSettings();
        else
        {
            this.turnSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
            this.volumeVoice = PlayerPrefs.GetFloat("VolumeVoice");
            this.volumeBGM = PlayerPrefs.GetFloat("VolumeBGM");
            this.volumeSFX = PlayerPrefs.GetFloat("VolumeSFX");
            this.framerate = PlayerPrefs.GetInt("Framerate");

            if (this.framerate == 0)
            {
                Application.targetFrameRate = -1;
                QualitySettings.vSyncCount = 0;
            }
            else
            {
                Application.targetFrameRate = this.framerate;
                QualitySettings.vSyncCount = 1;
            }

            if (PlayerPrefs.GetInt("InstantReset") == 1)
                this.instantReset = true;
            else
                this.instantReset = false;

            if (PlayerPrefs.GetInt("AdditionalMusic") == 1)
                this.additionalMusic = true;
            else
                this.additionalMusic = false;

            if (PlayerPrefs.GetInt("NotifBoard") == 1)
                this.notifBoard = true;
            else
                this.notifBoard = false;
            
            if (PlayerPrefs.GetInt("gps_familyFriendly") == 1)
                this.familyFriendly = true;
            else
                this.familyFriendly = false;
            
            if (PlayerPrefs.GetInt("pat_doorFix") == 1)
                this.doorFix = true;
            else
                this.doorFix = false;

            this.safeMode = PlayerPrefs.GetInt("gps_safemode");
            this.difficultMath = PlayerPrefs.GetInt("gps_difficultmath");

            Debug.Log("Loaded data from registry");
        }
    }

    IEnumerator CheckForStatsObject()
    {
        StatisticsController stats = FindObjectOfType<StatisticsController>();

        if (stats == null)
            yield break;
        else
        {
            float delay = 2f;

            while (delay > 0f)
            {
                delay -= Time.unscaledDeltaTime;
                yield return null;
            }

            Destroy(stats.gameObject);
        }
    }

    public void ResetSettings()
    {
        this.turnSensitivity = 2f;
        this.volumeVoice = 0f;
        this.volumeBGM = 0f;
        this.volumeSFX = 0f;
        this.framerate = Mathf.RoundToInt(Screen.currentResolution.refreshRate);
        this.instantReset = true;
        this.additionalMusic = false;
        this.notifBoard = false;
        this.familyFriendly = true;
        this.doorFix = true;
        PlayerPrefs.SetInt("AnalogMove", 0);
        PlayerPrefs.SetInt("Rumble", 0);
        PlayerPrefs.SetString("CurrentMap", "Classic");
        PlayerPrefs.SetInt("gps_safemode", 0);
        PlayerPrefs.SetInt("gps_difficultmath", 0);
        PlayerPrefs.SetInt("highbooks_Classic", 0);
        PlayerPrefs.SetInt("highbooks_ClassicExtended", 0);
        PlayerPrefs.SetInt("highbooks_JuniperHills", 0);
        PlayerPrefs.SetInt("pat_doorFix", 1);
        SaveToRegistry("settings");
        SaveToSettingsFile();
        Debug.LogWarning("Settings binary file not found. Settings reset to defaults.");
    }

    public void SaveToRegistry(string type) // lmao i give up
    {
        Debug.Log("Saved " + type + " to registry");
        
        switch(type)
        {
            case "settings":
                PlayerPrefs.SetFloat("MouseSensitivity", this.turnSensitivity);
                PlayerPrefs.SetFloat("VolumeVoice", this.volumeVoice);
                PlayerPrefs.SetFloat("VolumeBGM", this.volumeBGM);
                PlayerPrefs.SetFloat("VolumeSFX", this.volumeSFX);
                
                if (this.framerate > 0)
                {
                    PlayerPrefs.SetInt("Framerate", this.framerate);
                    QualitySettings.vSyncCount = 1;
                    Application.targetFrameRate = this.framerate;
                }
                else if (this.framerate == 0) // Unlimited FPS
                {
                    PlayerPrefs.SetInt("Framerate", 0);
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = -1;
                }

                if (this.instantReset)
                    PlayerPrefs.SetInt("InstantReset", 1);
                else
                    PlayerPrefs.SetInt("InstantReset", 0);

                if (this.additionalMusic)
                    PlayerPrefs.SetInt("AdditionalMusic", 1);
                else
                    PlayerPrefs.SetInt("AdditionalMusic", 0);

                if (this.notifBoard)
                    PlayerPrefs.SetInt("NotifBoard", 1);
                else
                    PlayerPrefs.SetInt("NotifBoard", 0);
                
                if (this.familyFriendly)
                    PlayerPrefs.SetInt("gps_familyFriendly", 1);
                else
                    PlayerPrefs.SetInt("gps_familyFriendly", 0);
                
                if (this.doorFix)
                    PlayerPrefs.SetInt("pat_doorFix", 1);
                else
                    PlayerPrefs.SetInt("pat_doorFix", 0);
                break;
            case "map":
                PlayerPrefs.SetString("CurrentMap", this.curMap);
                break;
            case "gamestyles":
                PlayerPrefs.SetInt("gps_safemode", this.safeMode);
                PlayerPrefs.SetInt("gps_difficultmath", this.difficultMath);
                break;
            case "defaults":
                PlayerPrefs.SetFloat("MouseSensitivity", 2f);
                PlayerPrefs.SetFloat("VolumeVoice", 0f);
                PlayerPrefs.SetFloat("VolumeBGM", 0f);
                PlayerPrefs.SetFloat("VolumeSFX", 0f);
                PlayerPrefs.SetInt("InstantReset", 1);
                PlayerPrefs.SetInt("AdditionalMusic", 0);
                PlayerPrefs.SetInt("NotifBoard", 0);
                break;
            default:
                Debug.LogWarning("Nothing to save");
                break;
        }
    }

    public void ToggleMenu(bool toggleSetting)
    {
        if (toggleSetting)
        {
            this.settingsCanvas.SetActive(true);
            this.menuCam.enabled = true;
            this.infoBox.SetActive(true);
        }
        else
        {
            StartCoroutine(LoadTestEnvironment());
        }
    }

    IEnumerator LoadTestEnvironment()
    {
        if (SceneManager.sceneCount == 2)
        {
            testEnvironmentController.ReEnableTestEnvironment(this.sensitivitySlider.value);
            DisableStuff(2);
            yield break;
        }
        float progressNum = 0f;
        DisableStuff(1);

        AsyncOperation loadRoutine = SceneManager.LoadSceneAsync("MenuTestEnvironment", LoadSceneMode.Additive);
        while (!loadRoutine.isDone)
        {
            progressNum = Mathf.RoundToInt(loadRoutine.progress / 0.9f * 100f);
            yield return null;
        }
        DisableStuff(2);
        this.testEnvironmentController = FindObjectOfType<TestEnvironmentController>();
    }

    void DisableStuff(int thing)
    {
        switch(thing)
        {
            case 1:
                this.eventSystem.enabled = false;
                this.baldiDevice.Stop();
                this.musicDevice.Stop();
                break;
            case 2:
                this.audioListener.enabled = false;
                this.infoBox.SetActive(false);
                this.settingsCanvas.SetActive(false);
                this.menuCam.enabled = false;
                break;
        }
    }

    void SaveToSettingsFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(this.dataPath);
        SettingsData data = new SettingsData();

        data.isSettingsSaved = true;

        try
        {
            bf.Serialize(file, data);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save settings binary file");
            Debug.LogError(e);
        }
        
        file.Close();
        Debug.Log("Settings file created");
    }

    void LoadAllUnlocks()
    {
        SaveData_Challenge challengeData = OldSaveData.OldSaveDataLoader.LoadOldChallengeData();
        this.challengeMapUnlocks = challengeData.challengeUnlocks;
    }

    [SerializeField] private string dataPath;

    [Header("Unlock Data")]
    [SerializeField] private bool[] challengeMapUnlocks;
    
    [Header("Opions")]
    public float turnSensitivity;
    public float volumeVoice;
    public float volumeBGM;
    public float volumeSFX;
    public bool instantReset;
    public bool additionalMusic;
    public bool notifBoard;
    public bool familyFriendly;

    [Header("Patches")]
    public bool doorFix;
    public int framerate;

    [Header("Gameplay Styles")]
    public string curMap;
    public int safeMode;
    public int difficultMath;

    [Header("Test Environment stuff")]
    [SerializeField] TestEnvironmentController testEnvironmentController;
    [SerializeField] Camera menuCam;
    [SerializeField] AudioListener audioListener;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject infoBox;
    [SerializeField] AudioSource baldiDevice;
    [SerializeField] AudioSource musicDevice;
    [SerializeField] Slider sensitivitySlider;
}
