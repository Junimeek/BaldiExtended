using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathMusicScript : MonoBehaviour
{
    public void TheAwakening()
    {
        if (this.mathScript == null)
        {
            this.mathScript = FindObjectOfType<MathGameScript>();
            this.gc = FindObjectOfType<GameControllerScript>();
        }

        this.curProblem = 0;
    }

   public void PlaySong()
    {
        this.curProblem++;

        if (this.curProblem == 1 && !this.gc.spoopMode)
        {
            this.question1Device.Play();
            this.question1Device.loop = true;
        }
        else if (this.curProblem > 1 && !this.gc.spoopMode)
        {
            StartCoroutine(WaitForMusic(this.curProblem));
        }
        else if (this.gc.spoopMode) StopSong();
    }

    private IEnumerator WaitForMusic(int problem)
    {
        this.question1Device.loop = false;
        this.question2Device.loop = false;

        while (this.question1Device.isPlaying || this.question2Device.isPlaying)
        {
            yield return null;
        }

        if (problem == 2)
        {
            this.question2Device.loop = true;
            this.question2Device.Play();
        }
        else if (problem == 3)
        {
            this.question3Device.loop = true;
            this.question3Device.Play();
        }
        StopCoroutine(WaitForMusic(0));
    }

    public void StopSong()
    {
        StopAllCoroutines();
        question1Device.Stop();
        question2Device.Stop();
        question3Device.Stop();
        question1Device.loop = false;
        question2Device.loop = false;
        question3Device.loop = false;
    }

    public AudioSource question1Device;
    public AudioSource question2Device;
    public AudioSource question3Device;
    public MathGameScript mathScript;
    public GameControllerScript gc;
    [SerializeField] private int curProblem;
}
