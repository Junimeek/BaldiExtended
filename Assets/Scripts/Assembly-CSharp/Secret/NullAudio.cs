using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullAudio : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
		{
			Debug.Log("Collision with Audio trigger");
            player.isSecret = true;
            gc.gameOverDelay = this.gameOverDelay;
            audioDevice.PlayOneShot(this.nullMonologue);
		}
    }

    public PlayerScript player;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private float gameOverDelay;
    public AudioSource audioDevice;
    public AudioClip nullMonologue;
    public AudioClip nullGlitchLoop;
}
