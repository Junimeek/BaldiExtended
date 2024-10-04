using System;
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

        switch(loadType)
        {
            case "menuGP":
                this.canvasGP.SetActive(true);
                this.canvasPatches.SetActive(false);
                this.canvasAudio.SetActive(false);
                this.canvasData.SetActive(false);
                this.sliderSensitivity.value = container.turnSensitivity;
                this.toggleInstantReset.isOn = container.instantReset;
                this.toggleNotifBoard.isOn = container.notifBoard;
                this.toggleFamilyFriendly.isOn = container.familyFriendly;
                this.sliderScript.UpdateSensitivityText();
                break;
            case "menuPatches":
                this.canvasGP.SetActive(false);
                this.canvasPatches.SetActive(true);
                this.canvasAudio.SetActive(false);
                this.canvasData.SetActive(false);
                this.toggleDoorFix.isOn = container.doorFix;
                this.GetFramerateSettings();
                break;
            case "menuAudio":
                this.canvasGP.SetActive(false);
                this.canvasPatches.SetActive(false);
                this.canvasAudio.SetActive(true);
                this.canvasData.SetActive(false);
                this.sliderVoice.value = container.volumeVoice;
                this.sliderBGM.value = container.volumeBGM;
                this.sliderSFX.value = container.volumeSFX;
                this.toggleAdditionalMusic.isOn = container.additionalMusic;
                this.sliderScript.UpdateVolumeText();
                break;
            case "menuData":
                this.canvasGP.SetActive(false);
                this.canvasPatches.SetActive(false);
                this.canvasAudio.SetActive(false);
                this.canvasData.SetActive(true);
                break;
        }
    }

    private void GetFramerateSettings()
    {
        this.framerate = container.framerate;

        if (container.framerate == -1)
        {
            this.toggleVSync.isOn = true;
            this.toggleUnlimitedFramerate.isOn = false;
            this.framerateInput.text = Mathf.RoundToInt(Screen.currentResolution.refreshRate).ToString();
        }
        else if (container.framerate == 0)
        {
            this.toggleVSync.isOn = false;
            this.toggleUnlimitedFramerate.isOn = true;
            this.framerateInput.text = Mathf.RoundToInt(Screen.currentResolution.refreshRate).ToString();
        }
        else
        {
            this.toggleVSync.isOn = false;
            this.toggleUnlimitedFramerate.isOn = false;
            this.framerateInput.text = container.framerate.ToString();
        }

        this.framerateInput.ActivateInputField();
    }

    private void SetFramerateSettings()
    {
        if (!this.canvasPatches.activeInHierarchy)
            return;
        
        Debug.Log("Framerate saved");

        this.framerateInput.DeactivateInputField();
        if (this.toggleVSync.isOn)
        {
            this.framerate = -1;
        }
        else if (this.toggleUnlimitedFramerate.isOn)
        {
            this.framerate = 0;
        }
        else
        {
            try
            {
                this.framerate = Mathf.RoundToInt(Int32.Parse(this.framerateInput.text));
                Debug.Log(this.framerate);
            }
            catch
            {
                this.framerate = Mathf.RoundToInt(Screen.currentResolution.refreshRate);
                this.toggleVSync.isOn = false;
                this.toggleUnlimitedFramerate.isOn = false;
            }
        }

        container.framerate = this.framerate;
    }

    public void ToggleFramerateButtons(int type)
    {
        switch(type)
        {
            case 1:
                if (this.toggleVSync.isOn)
                {
                    this.toggleUnlimitedFramerate.interactable = false;
                    this.toggleUnlimitedFramerate.isOn = false;

                    this.toggleUnlimitedFramerate.GetComponentInChildren<TMP_Text>().color = new Color(1f, 1f, 1f, 0.25f);
                    foreach (Image image in this.toggleUnlimitedFramerate.GetComponentsInChildren<Image>())
                        image.color = new Color(1f, 1f, 1f, 0.25f);

                    this.framerateInput.DeactivateInputField();
                    this.framerateInput.text = Mathf.RoundToInt(Screen.currentResolution.refreshRate).ToString();
                }
                else
                {
                    this.toggleUnlimitedFramerate.interactable = true;
                    this.toggleUnlimitedFramerate.isOn = true;

                    this.toggleUnlimitedFramerate.GetComponentInChildren<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);
                    foreach (Image image in this.toggleUnlimitedFramerate.GetComponentsInChildren<Image>())
                        image.color = new Color(1f, 1f, 1f, 1f);
                    
                    this.framerateInput.ActivateInputField();
                    this.framerateInput.text = Mathf.RoundToInt(Screen.currentResolution.refreshRate).ToString();
                }
                break;
            case 2:
                if (this.toggleUnlimitedFramerate.isOn)
                {
                    this.framerateInput.DeactivateInputField();
                    this.framerateInput.text = Mathf.RoundToInt(Screen.currentResolution.refreshRate).ToString();
                }
                else
                {
                    this.framerateInput.ActivateInputField();
                    this.framerateInput.text = Mathf.RoundToInt(Screen.currentResolution.refreshRate).ToString();
                }
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
                break;
            case "menuAudio":
                container.volumeVoice = sliderVoice.value;
                container.volumeBGM = sliderBGM.value;
                container.volumeSFX = sliderSFX.value;
                container.additionalMusic = toggleAdditionalMusic.isOn;
                break;
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

    [SerializeField] private SettingsContainer container;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SettingsSliders sliderScript;
    [SerializeField] private string sceneName;
    public string curSetting;
    //private IDataService DataService = new JsonDataService();
    //private SettingsContainer settingsContainer;

    [Header("Menus")]
    [SerializeField] private GameObject canvasGP;
    [SerializeField] private GameObject canvasPatches;
    [SerializeField] private GameObject canvasAudio;
    [SerializeField] private GameObject canvasData;

    [Header("Toggles")]
    [SerializeField] private Slider sliderSensitivity;
    [SerializeField] private Slider sliderVoice;
    [SerializeField] private Slider sliderBGM;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Toggle toggleAdditionalMusic;
    [SerializeField] private Toggle toggleInstantReset;
    [SerializeField] private Toggle toggleNotifBoard;
    [SerializeField] private Toggle toggleFamilyFriendly;
    [SerializeField] private Toggle toggleDoorFix;
    [SerializeField] private Toggle toggleVSync;
    [SerializeField] private int framerate;
    [SerializeField] private TMP_InputField framerateInput;
    [SerializeField] private Toggle toggleUnlimitedFramerate;

}
