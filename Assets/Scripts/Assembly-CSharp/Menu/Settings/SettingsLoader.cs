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
    }

    [SerializeField] private SettingsContainer container;
    [SerializeField] private string sceneName;
    [SerializeField] private string curSetting;

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
