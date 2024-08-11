using UnityEngine;

public class NullKill : MonoBehaviour
{
    private void Start()
    {
        this.gameOver = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !this.nullAudioScript.audioDevice.isPlaying && !this.gameOver)
		{
            this.gameOver = true;
			Debug.Log("Collision with Kill trigger");
            player.gameOver = true;
            nullAudioScript.audioDevice.Stop();
            nullAudioScript.nullGlitchLoop.Stop();
            killAudio.Play();
            RenderSettings.skybox = this.blackSky;
		}
    }

    [SerializeField] private PlayerScript player;
    [SerializeField] private Material blackSky;
    [SerializeField] private AudioSource killAudio;
    private bool gameOver;
    [SerializeField] NullAudio nullAudioScript;
}
