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

    [SerializeField] AudioSource audioDevice;
    [SerializeField] AudioClip menuLoop;
}
