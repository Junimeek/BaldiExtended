using UnityEngine;

public class MenuTestEnvMusic : MonoBehaviour
{
    [SerializeField] AudioSource audioDevice;
    [SerializeField] AudioClip loopClip;
    [SerializeField] AudioClip endClip;

    void Start()
    {
        StartLoop();
    }

    public void StartLoop()
    {
        this.audioDevice.Stop();
        this.audioDevice.clip = this.loopClip;
        this.audioDevice.loop = true;
        this.audioDevice.Play();
    }

    public void EndLoop()
    {
        this.audioDevice.Stop();
        this.audioDevice.loop = false;
        this.audioDevice.clip = this.endClip;
        this.audioDevice.Play();
    }
}
