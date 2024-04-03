using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsLoader : MonoBehaviour
{
    private void OnEnable()
    {
        curSetting = null;
        this.container = FindObjectOfType<SettingsContainer>();
        this.audioManager = FindObjectOfType<AudioManager>();
        Scene curScene = SceneManager.GetActiveScene();
        sceneName = curScene.name;
        
        if (sceneName == "MainMenu") LoadSettings("menuGP");
    }

    public void LoadSettings(string loadType)
    {
        if (curSetting != null) this.StoreSettings();
        curSetting = loadType;

        if (loadType == "menuGP")
        {
            canvasGP.SetActive(true);
            canvasAudio.SetActive(false);
            canvasData.SetActive(false);
            sliderSensitivity.value = container.turnSensitivity;
            toggleInstantReset.isOn = container.instantReset;
            toggleNotifBoard.isOn = container.notifBoard;

            //ReadJson("settings_gp");
        }

        if (loadType == "menuAudio")
        {
            canvasGP.SetActive(false);
            canvasAudio.SetActive(true);
            canvasData.SetActive(false);
            sliderVoice.value = container.volumeVoice;
            sliderBGM.value = container.volumeBGM;
            sliderSFX.value = container.volumeSFX;
            toggleAdditionalMusic.isOn = container.additionalMusic;
        }

        if (loadType == "menuData")
        {
            canvasGP.SetActive(false);
            canvasAudio.SetActive(false);
            canvasData.SetActive(true);
        }
    }

    public void StoreSettings()
    {
        if (curSetting == "menuGP")
        {
            container.turnSensitivity = this.sliderSensitivity.value;
            container.instantReset = this.toggleInstantReset.isOn;
            container.notifBoard = this.toggleNotifBoard.isOn;
            this.SaveSettings();
        }
        else if (curSetting == "menuAudio")
        {
            container.volumeVoice = sliderVoice.value;
            container.volumeBGM = sliderBGM.value;
            container.volumeSFX = sliderSFX.value;
            container.additionalMusic = toggleAdditionalMusic.isOn;
            this.SaveSettings();
        }
    }

    public void SaveSettings()
    {
        container.RegistrySave();
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

    [SerializeField] private SettingsContainer container;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private string sceneName;
    [SerializeField] private string curSetting;
    //private IDataService DataService = new JsonDataService();
    //private SettingsContainer settingsContainer;

    [Header("Menus")]
    [SerializeField] private GameObject canvasGP;
    [SerializeField] private GameObject canvasAudio;
    [SerializeField] private GameObject canvasData;
    [SerializeField] private GameObject canvasPause;

    [Header("Toggles")]
    [SerializeField] private Slider sliderSensitivity;
    [SerializeField] private Slider sliderVoice;
    [SerializeField] private Slider sliderBGM;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Toggle toggleAdditionalMusic;
    [SerializeField] private Toggle toggleInstantReset;
    [SerializeField] private Toggle toggleNotifBoard;

}
