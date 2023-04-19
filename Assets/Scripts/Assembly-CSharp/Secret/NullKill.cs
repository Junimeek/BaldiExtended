using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !nullAudioScript.audioDevice.isPlaying)
		{
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
    [SerializeField] NullAudio nullAudioScript;
}
