using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterMusicController : MonoBehaviour
{
    private void Start()
    {
        this.audioDevice = this.GetComponent<AudioSource>();
        this.audioDevice.loop = true;
        this.audioDevice.clip = null;
    }

    public void Play(string type)
    {
        this.audioDevice.loop = true;
        switch (type)
        {
            case "math":
                this.curMathProblem++;
                if (this.curMathProblem == 1 && !this.gc.spoopMode)
                {
                    this.audioDevice.clip = this.learnMusic[0];
                    this.audioDevice.loop = true;
                    this.audioDevice.Play();
                }
                else if (this.curMathProblem > 1 && !this.gc.spoopMode)
                    this.StartCoroutine(this.WaitForMathMusic());
                break;
        }
    }

    public void Stop()
    {
        this.audioDevice.loop = false;
        this.audioDevice.Stop();
        this.audioDevice.clip = null;
        this.audioDevice.outputAudioMixerGroup = this.mainMixer;
    }

    public void InitializeMathMusic()
    {
        this.audioDevice.outputAudioMixerGroup = this.learnMixer;
        this.mathScript = FindObjectOfType<MathGameScript>();
        this.curMathProblem = 0;
    }

    private IEnumerator WaitForMathMusic()
    {
        this.audioDevice.loop = false;

        while (this.audioDevice.isPlaying)
            yield return null;
        
        if (this.curMathProblem == 2 && !this.gc.spoopMode)
        {
            this.audioDevice.loop = true;
            this.audioDevice.clip = this.learnMusic[1];
            this.audioDevice.Play();
        }
        else if (this.curMathProblem >= 3 && !this.gc.spoopMode)
        {
            this.audioDevice.loop = true;
            this.audioDevice.clip = this.learnMusic[2];
            this.audioDevice.Play();
        }
    }

    [SerializeField] private GameControllerScript gc;
    private AudioSource audioDevice;
    [SerializeField] private AudioMixerGroup mainMixer;
    [SerializeField] private AudioMixerGroup learnMixer;

    [Header("Math Game")]
    [HideInInspector] public MathGameScript mathScript;
    [SerializeField] private AudioClip[] learnMusic;
    [SerializeField] private int curMathProblem;
}
