using UnityEngine;

public class MetronomeScript : MonoBehaviour
{
    private void Start()
    {
        if (this.startTimeAtScriptStart)
            this.StartMetronome();
    }

    public void Update()
    {
        if (!this.metronomeStarted)
            return;

        this.metronomeTime += Time.unscaledDeltaTime;

        if(this.metronomeTime > this.beatLength)
            this.BeatHit();
    }

    private void BeatHit()
    {
        this.curBeat++;
        this.metronomeTime -= this.beatLength;

        if (this.curBeat % 4 == 0)
            this.curMeasure++;
    }

    public void StartMetronome()
    {
        if (this.metronomeStarted)
        {
            Debug.Log("Metronome already started! Ignoring.");
            return;
        }

        this.beatLength = 60f / bpm;
        this.metronomeStarted = true;
    }

    public void StopMetronome()
    {
        if (!this.metronomeStarted) {
            Debug.Log("Metronome isn't started! Ignoring.");
            return;
        }

        this.metronomeStarted = false;
    }

    private bool metronomeStarted = false;
    [SerializeField] private double metronomeTime, beatLength = 0f;
    [SerializeField] private int curBeat = 0;
    public int curMeasure;

    [Header("Metronome Settings")]
    public float bpm = 100;
    [SerializeField] private bool startTimeAtScriptStart = false;
}