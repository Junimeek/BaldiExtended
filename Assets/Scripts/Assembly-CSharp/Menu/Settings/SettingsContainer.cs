using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SettingsContainer : MonoBehaviour
{
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

        this.dataPath = Application.persistentDataPath + "/settings.sav";
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("OptionsSet"))
        {
            this.optionsSet = true;
            this.turnSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
            this.volumeVoice = PlayerPrefs.GetFloat("VolumeVoice");
            this.volumeBGM = PlayerPrefs.GetFloat("VolumeBGM");
            this.volumeSFX = PlayerPrefs.GetFloat("VolumeSFX");

            if (PlayerPrefs.GetInt("AnalogMove") == 1) this.analogMove = true;
            else this.analogMove = false;

            if (PlayerPrefs.GetInt("Rumble") == 1) this.rumble = true;
            else this.rumble = false;

            if (PlayerPrefs.GetInt("InstantReset") == 1) this.instantReset = true;
            else this.instantReset = false;

            if (PlayerPrefs.GetInt("AdditionalMusic") == 1) this.additionalMusic = true;
            else this.additionalMusic = false;

            if (PlayerPrefs.GetInt("NotifBoard") == 1) this.notifBoard = true;
            else this.notifBoard = false;
        }

        else
        {
            PlayerPrefs.SetInt("OptionsSet", 1);
            this.freeRun = false;

            this.turnSensitivity = 0.2f;
            this.volumeVoice = 0f;
            this.volumeBGM = 0f;
            this.volumeSFX = 0f;

            this.instantReset = true;
            this.additionalMusic = false;
            this.notifBoard = false;

            this.analogMove = false;
            this.rumble = false;
        }

        LoadSettingsData(1);
    }

    public void SaveSettingsData(int type)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(this.dataPath);
        SettingsData data = new SettingsData();

        switch(type)
        {
            case 1:
                data.turnSensitivity = this.turnSensitivity;
                data.isInstantReset = this.instantReset;
                data.isNotifBoard = this.notifBoard;
                data.volVoice = this.volumeVoice;
                data.volBGM = this.volumeBGM;
                data.volSFX = this.volumeSFX;
                data.isAdditionalMusic = this.additionalMusic;
            break;
            case 2:
                data.turnSensitivity = this.turnSensitivity;
                data.volVoice = this.volumeVoice;
                data.volBGM = this.volumeBGM;
                data.volSFX = this.volumeSFX;
            break;
            default:
                Debug.LogError("Failed to save settings");
                file.Close();
            return;
        }
        
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("saved settings");
    }

    public void LoadSettingsData(int type)
    {
        if (File.Exists(this.dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(this.dataPath, FileMode.Open);
            SettingsData data = (SettingsData)bf.Deserialize(file);
            file.Close();

            switch(type)
            {
                case 1:
                    this.turnSensitivity = data.turnSensitivity;
                    this.instantReset = data.isInstantReset;
                    this.notifBoard = data.isNotifBoard;
                    this.volumeVoice = data.volVoice;
                    this.volumeBGM = data.volBGM;
                    this.volumeSFX = data.volSFX;
                    this.additionalMusic = data.isAdditionalMusic;
                break;
                case 2:
                    this.turnSensitivity = data.turnSensitivity;
                    this.volumeVoice = data.volVoice;
                    this.volumeBGM = data.volBGM;
                    this.volumeSFX = data.volSFX;
                break;
                default:
                    Debug.LogError("Failed to load settings.");
                return;
            }
            Debug.Log("Loaded Settings");
        }
        else
        {
            Debug.LogError("Settings file not found, resetting to defaults...");
            this.turnSensitivity = 0.2f;
            this.instantReset = true;
            this.notifBoard = false;
            this.volumeVoice = 0f;
            this.volumeBGM = 0f;
            this.volumeSFX = 0f;
            this.additionalMusic = false;
            this.SaveSettingsData(1);
        }
    }

/*
    public void RegistrySave()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", this.turnSensitivity);
        PlayerPrefs.SetFloat("VolumeVoice", this.volumeVoice);
        PlayerPrefs.SetFloat("VolumeBGM", this.volumeBGM);
        PlayerPrefs.SetFloat("VolumeSFX", this.volumeSFX);

        if (this.analogMove == true) PlayerPrefs.SetInt("AnalogMove", 1);
        else PlayerPrefs.SetInt("AnalogMove", 0);

        if (this.rumble == true) PlayerPrefs.SetInt("Rumble", 1);
        else PlayerPrefs.SetInt("Rumble", 0);

        if (this.instantReset == true) PlayerPrefs.SetInt("InstantReset", 1);
        else PlayerPrefs.SetInt("InstantReset", 0);

        if (this.instantReset == true) PlayerPrefs.SetInt("NotifBoard", 1);
        else PlayerPrefs.SetInt("NotifBoard", 0);

        if (this.additionalMusic == true) PlayerPrefs.SetInt("AdditionalMusic", 1);
        else PlayerPrefs.SetInt("AdditionalMusic", 0);
    }
    */

    private static SettingsContainer instance;
    [SerializeField] private string dataPath;

    public bool optionsSet;
    public bool freeRun;
    
    public float turnSensitivity;
    public float volumeVoice;
    public float volumeBGM;
    public float volumeSFX;
    public bool instantReset;
    public bool additionalMusic;
    public bool notifBoard;

    // useless settings
    public bool analogMove;
    public bool rumble;
}
