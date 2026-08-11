using System.Collections;
using UnityEngine;

public class MenuMusicScript : MonoBehaviour
{
    void Start()
    {
        this.audioDevice.Play();
        StartCoroutine(this.MusicReplacement());
    }

    IEnumerator MusicReplacement()
    {
        while (this.audioDevice.isPlaying)
            yield return null;

        this.audioDevice.clip = menuLoop;
        this.audioDevice.loop = true;
        this.audioDevice.Play();
    }

    public void PlayMenuSelectSound()
    {
        this.audioDevice.PlayOneShot(this.selectSound);
    }
    public void PlayMenuRejectSound()
    {
        this.audioDevice.PlayOneShot(this.rejectSound);
    }

    public void PlayMenuPlaySound()
    {
        this.audioDevice.PlayOneShot(this.playSound);
    }

    [SerializeField] AudioSource audioDevice;
    [SerializeField] AudioClip menuLoop;
    [SerializeField] AudioClip selectSound;
    [SerializeField] AudioClip rejectSound;
    [SerializeField] AudioClip playSound;
}
