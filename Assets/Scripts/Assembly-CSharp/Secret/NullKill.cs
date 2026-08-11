using UnityEngine;

public class NullKill : MonoBehaviour
{
    void Start()
    {
        this.gameOver = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !this.nullAudioScript.audioDevice.isPlaying && !this.gameOver)
		{
            this.gameOver = true;
            this.player.gameOver = true;
            nullAudioScript.audioDevice.Stop();
            nullAudioScript.nullGlitchLoop.Stop();
            this.killAudio.Play();
            nullAgent.DisableAgent();
		}
    }

    [SerializeField] PlayerScript player;
    [SerializeField] AudioSource killAudio;
    bool gameOver;
    [SerializeField] NullAudio nullAudioScript;
    [SerializeField] NullAgent nullAgent;
}
