using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Debug.Log("Collision with Audio trigger");
            
            if (PlayerPrefs.GetInt("gps_safemode") == 1)
            {
                StartCoroutine(WaitForDab());
            }
            else
            {
                audioDevice.PlayOneShot(this.nullMonologue);
                player.isSecret = true;
                gc.gameOverDelay = this.gameOverDelay;
                StartCoroutine(WaitForGlitch());
            }
            
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

    private IEnumerator WaitForDab()
    {
        baldiLecture.Play();

        while (this.baldiLecture.isPlaying)
        {
            yield return null;
        }

        audioCollider.enabled = !audioCollider.enabled;

        balDab.SetActive(true);
        balIdle.SetActive(false);
        baldiLmfao.Play();

        while (this.baldiLmfao.isPlaying)
        {
            yield return null;
        }

        baldiLmfao.Play();
        baldiLmfao.loop = true;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public PlayerScript player;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private float gameOverDelay;
    [SerializeField] private NullAgent nullAgent;
    [SerializeField] private Collider audioCollider;
    [SerializeField] private bool isPlaying;
    [SerializeField] private GameObject balIdle;
    [SerializeField] private GameObject balDab;
    public AudioSource audioDevice;
    [SerializeField] private AudioSource baldiLecture;
    [SerializeField] private AudioSource baldiLmfao;
    public AudioClip nullMonologue;
    public AudioSource nullGlitchLoop;
}
