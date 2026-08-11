using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsLoader : MonoBehaviour
{
    private void OnEnable()
    {
        this.curSetting = null;
        this.container = FindObjectOfType<SettingsContainer>();
        this.audioManager = FindObjectOfType<AudioManager>();
        Scene curScene = SceneManager.GetActiveScene();
        sceneName = curScene.name;
        
        if (sceneName == "MainMenu")
            LoadSettings("menuGP");

        audioManager.GetVolume();
    }

    private void OnDisable()
    {
        this.StoreSettings();

        if (sceneName == "MainMenu")
            this.SaveSettings();
        else if (sceneName == "Classic" || sceneName == "ClassicExtended" ||
        sceneName == "JuniperHills" || sceneName == "TheLine")
            this.SaveSettings();
    }

    public void LoadSettings(string loadType)
    {
        if (curSetting != null)
            this.StoreSettings();

        curSetting = loadType;

        this.canvasGP.SetActive(false);
        this.canvasPatches.SetActive(false);
        this.canvasAccessibility.SetActive(false);
        this.canvasAudio.SetActive(false);
        this.canvasData.SetActive(false);

        switch(loadType)
        {
            case "menuGP":
                this.canvasGP.SetActive(true);
                this.sliderSensitivity.value = container.turnSensitivity;
                this.toggleInstantReset.isOn = container.instantReset;
                this.toggleNotifBoard.isOn = container.notifBoard;
                this.toggleFamilyFriendly.isOn = container.familyFriendly;
                this.sliderScript.UpdateSensitivityText();
                break;
            case "menuPatches":
                this.canvasPatches.SetActive(true);
                this.toggleDoorFix.isOn = container.doorFix;
                this.framerate = Mathf.RoundToInt(Screen.currentResolution.refreshRate);
                this.refreshText.text = this.framerate + "fps";
                this.SetFramerateToggles();
                break;
            case "menuAccessibility":
                this.canvasAccessibility.SetActive(true);
                this.toggleAutoDoors.isOn = container.automaticDoors;
                this.toggleMapToggle.isOn = container.mapToggleMode;
                break;
            case "menuAudio":
                this.canvasAudio.SetActive(true);
                this.sliderVoice.value = container.volumeVoice;
                this.sliderBGM.value = container.volumeBGM;
                this.sliderSFX.value = container.volumeSFX;
                this.toggleAdditionalMusic.isOn = container.additionalMusic;
                this.sliderScript.UpdateVolumeText();
                break;
            case "menuData":
                this.canvasData.SetActive(true);
                break;
        }
    }

    public void StoreSettings()
    {
        switch (this.curSetting)
        {
            case "menuGP":
                container.turnSensitivity = this.sliderSensitivity.value;
                container.instantReset = this.toggleInstantReset.isOn;
                container.notifBoard = this.toggleNotifBoard.isOn;
                container.familyFriendly = this.toggleFamilyFriendly.isOn;
                break;
            case "menuPatches":
                container.doorFix = this.toggleDoorFix.isOn;
                container.framerate = this.SetFramerate();
                break;
            case "menuAccessibility":
                container.automaticDoors = this.toggleAutoDoors.isOn;
                container.mapToggleMode = this.toggleMapToggle.isOn;
                break;
            case "menuAudio":
                container.volumeVoice = sliderVoice.value;
                container.volumeBGM = sliderBGM.value;
                container.volumeSFX = sliderSFX.value;
                container.additionalMusic = toggleAdditionalMusic.isOn;
                break;
        }
    }

    private void SetFramerateToggles()
    {
        if (container.framerate == 0)
            this.isUnlimited = true;
        else
            this.isUnlimited = false;
        
        this.checkmark.anchoredPosition = this.CheckmarkPosition();
    }

    public void ToggleFramerate()
    {
        if (this.isUnlimited)
            this.isUnlimited = false;
        else
            this.isUnlimited = true;
        
        this.checkmark.anchoredPosition = this.CheckmarkPosition();
    }

    private Vector3 CheckmarkPosition()
    {
        if (!this.isUnlimited)
            return new Vector3(215, 129, 0);
        else
            return new Vector3(215, 79, 0);
    }

    private int SetFramerate()
    {
        if (this.isUnlimited)
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 0;
            return 0;
        }
        else
        {
            Application.targetFrameRate = this.framerate;
            QualitySettings.vSyncCount = 1;
            return this.framerate;
        }
    }

    public void SaveSettings()
    {
        container.SaveToRegistry("settings");

        if (audioManager != null)
            audioManager.GetVolume();
    }

/*
    public void SaveJson(string file)
    {
        settingsContainer = new SettingsContainer();

        switch(file)
        {
            case "settings_gp":
                Debug.Log("Saving settings...");

                if (DataService.SaveData("/settings.json", settingsContainer, false))
                {
                    Debug.Log("Saved Settings");
                }
                else
                {
                    Debug.LogError("Failed to save.");
                }
            break;
            default:
                Debug.LogError("save input is empty you dumb bitch");
            break;
        }
    }

    public void ReadJson(string file)
    {
        switch(file)
        {
            case "settings_gp":
                try
                {
                    SettingsContainer data = DataService.LoadData<SettingsContainer>("/settings.json", false);
                    Debug.Log("Settings Loaded.");
                }
                catch
                {
                    Debug.LogError("Failed to load settings.");
                }
            break;
            default:
                Debug.LogError("rrrrrrrrr");
            break;
        }
    }
*/

    [SerializeField] SettingsContainer container;
    [SerializeField] AudioManager audioManager;
    [SerializeField] SettingsSliders sliderScript;
    [SerializeField] string sceneName;
    public string curSetting;
    //IDataService DataService = new JsonDataService();
    //SettingsContainer settingsContainer;

    [Header("Menus")]
    [SerializeField] GameObject canvasGP;
    [SerializeField] GameObject canvasPatches;
    [SerializeField] GameObject canvasAccessibility;
    [SerializeField] GameObject canvasAudio;
    [SerializeField] GameObject canvasData;

    [Header("Toggles")]
    [SerializeField] Slider sliderSensitivity;
    [SerializeField] Slider sliderVoice;
    [SerializeField] Slider sliderBGM;
    [SerializeField] Slider sliderSFX;
    [SerializeField] Toggle toggleAdditionalMusic;
    [SerializeField] Toggle toggleInstantReset;
    [SerializeField] Toggle toggleNotifBoard;
    [SerializeField] Toggle toggleFamilyFriendly;
    [SerializeField] Toggle toggleDoorFix;
    [SerializeField] int framerate;
    [SerializeField] RectTransform checkmark;
    [SerializeField] TMP_Text refreshText;
    [SerializeField] bool isUnlimited;
    [SerializeField] Toggle toggleAutoDoors;
    [SerializeField] Toggle toggleMapToggle;

}
