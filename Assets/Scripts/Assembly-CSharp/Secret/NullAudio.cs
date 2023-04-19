using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullAudio : MonoBehaviour
{
    private void Start()
    {
        audioCollider = GetComponent<BoxCollider>();
        isPlaying = false;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !isPlaying)
		{
            isPlaying = true;
            audioDevice.PlayOneShot(this.nullMonologue);
			Debug.Log("Collision with Audio trigger");
            player.isSecret = true;
            gc.gameOverDelay = this.gameOverDelay;
            StartCoroutine(WaitForGlitch());
		}
    }

    private IEnumerator WaitForGlitch()
    {
        while (this.audioDevice.isPlaying)
        {
            nullAgent.allowMovement = false;
            yield return null;
        }

        audioCollider.enabled = !audioCollider.enabled;

        nullAgent.allowMovement = true;
        nullGlitchLoop.Play();
    }

    public PlayerScript player;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private float gameOverDelay;
    [SerializeField] private NullAgent nullAgent;
    [SerializeField] private Collider audioCollider;
    [SerializeField] private bool isPlaying;
    public AudioSource audioDevice;
    public AudioClip nullMonologue;
    public AudioSource nullGlitchLoop;
}
