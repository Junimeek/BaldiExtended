using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetVolume(int id)
    {
        if (id == 0)
        {
            mixer.SetFloat("volSFX", 0f);
            mixer.SetFloat("volVoice", 0f);
        }

        else if (id == 1) // learn in spoop mode
        {
            mixer.SetFloat("volSFX", -80f);
            mixer.SetFloat("volVoice", -80f);
        }
    }
}
