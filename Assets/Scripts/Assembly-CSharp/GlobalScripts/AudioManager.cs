using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private BaldiScript baldi;
    [SerializeField] private float volumeBGM;
    [SerializeField] private float volumeSFX;
    [SerializeField] private float volumeVoice;

    private void Start()
    {
        volumeBGM = PlayerPrefs.GetFloat("VolumeBGM");
        volumeSFX = PlayerPrefs.GetFloat("VolumeSFX");
        volumeVoice = PlayerPrefs.GetFloat("VolumeVoice");

        if (volumeBGM <= -24f) volumeBGM = -80f;
        if (volumeSFX <= -24f) volumeSFX = -80f;
        if (volumeVoice <= -24f) volumeVoice = -80f;

        SetVolume(0);
    }

    /*
    private void Update()
    {
        volumeBGM = PlayerPrefs.GetFloat("VolumeBGM");
        volumeSFX = PlayerPrefs.GetFloat("VolumeSFX");
        volumeVoice = PlayerPrefs.GetFloat("VolumeVoice");

        SetVolume(0);
    }
    */

    public void SetVolume(int id)
    {
        if (id == 0) // normal
        {
            mixer.SetFloat("volBGM", volumeBGM);
            mixer.SetFloat("volSFX", volumeSFX);
            mixer.SetFloat("volVoice", volumeVoice);
        }

        else if (id == 1) // learn in spoop mode
        {
            mixer.SetFloat("volSFX", -80f);
            mixer.SetFloat("volVoice", -80f);
        }
    }
}
