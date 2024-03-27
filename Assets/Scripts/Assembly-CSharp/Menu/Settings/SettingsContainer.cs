using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
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
    }

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

    private static SettingsContainer instance;

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
