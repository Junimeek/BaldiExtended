using System;
using System.Collections;
using UnityEngine;

public class BossMusicController : MonoBehaviour
{
    private void Start()
    {
        this.curGameStage = 0;
        this.curAudioStage = 0;
        this.audioQueue = new AudioClip[1];
    }

    private void Update()
    {
        if (this.curAudioStage == this.curGameStage)
        {
            this.audioDevice1.loop = true;
            this.audioDevice2.loop = true;
        }
        else
        {
            this.audioDevice1.loop = false;
            this.audioDevice2.loop = false;
        }
    }

    private void LateUpdate()
    {
        if (!this.audioDevice1.isPlaying && !this.audioDevice2.isPlaying && this.audioQueue.Length > 1)
            this.AdvanceQueue();
    }

    public void QueueClips(AudioClip clip)
    {
        this.audioDevice1.loop = false;
        this.audioDevice2.loop = false;
        Array.Resize(ref this.audioQueue, this.audioQueue.Length + 1);
        this.audioQueue[this.audioQueue.Length - 1] = clip;
        this.curGameStage++;
    }

    public void ForceQueue()
    {
        this.audioDevice1.Stop();
    }

    public void EndInitialLoop(int nextMeasure)
    {
        this.StartCoroutine(this.EndLoopStart(nextMeasure));
    }

    private IEnumerator EndLoopStart(int nextMeasure)
    {
        this.QueueClips(this.playlist[1]);
        this.QueueClips(this.playlist[2]);

        while (this.metronome.curMeasure < nextMeasure)
            yield return null;
        
        this.ForceQueue();
        this.metronome.StopMetronome();
    }

    private void AdvanceQueue()
    {
        for (int i = 0; i < this.audioQueue.Length; i++)
        {
            if (i + 1 == this.audioQueue.Length)
                this.audioQueue[i] = null;
            else
                this.audioQueue[i] = this.audioQueue[i + 1];
        }
        
        Array.Resize(ref this.audioQueue, this.audioQueue.Length - 1);

        if (this.curAudioStage % 2 == 0)
        {
            this.audioDevice1.clip = this.audioQueue[0];
            this.audioDevice1.Play();
        }
        else
        {
            this.audioDevice2.clip = this.audioQueue[0];
            this.audioDevice2.Play();
        }

        this.curAudioStage++;
    }

    public void ClearQueue()
    {
        for (int i = 0; i < this.audioQueue.Length; i++)
        {
            this.audioQueue[i] = null;
            Array.Resize(ref this.audioQueue, 0);
        }
        this.audioDevice1.Stop();
        this.audioDevice2.Stop();
    }

    [SerializeField] private AudioSource audioDevice1;
    [SerializeField] private AudioSource audioDevice2;
    public AudioClip[] playlist;
    [SerializeField] private AudioClip[] audioQueue;
    [SerializeField] private int curGameStage;
    [SerializeField] private int curAudioStage;
    [SerializeField] private MetronomeScript metronome;
}
