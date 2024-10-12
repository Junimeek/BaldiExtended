using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NullAudio : MonoBehaviour
{
    private void Start()
    {
        this.audioCollider = GetComponent<BoxCollider>();
        this.isPlaying = false;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !this.isPlaying)
		{
            this.isPlaying = true;
            Debug.Log("Collision with Audio trigger");
            
            if (PlayerPrefs.GetInt("gps_safemode") == 1)
                StartCoroutine(this.WaitForDab());
            else
            {
                this.audioDevice.PlayOneShot(this.nullMonologue);
                this.player.isSecret = true;
                this.gc.gameOverDelay = this.gameOverDelay;
                StartCoroutine(this.WaitForGlitch());

                this.progressionController.mapUnlocks[0] = true;
                this.progressionController.SaveProgressionData();
            }
		}
    }

    private IEnumerator WaitForGlitch()
    {
        this.nullAgent.allowMovement = false;

        while (this.audioDevice.isPlaying)
            yield return null;

        this.audioCollider.enabled = !audioCollider.enabled;

        this.nullAgent.allowMovement = true;
        this.nullGlitchLoop.Play();
    }

    private IEnumerator WaitForDab()
    {
        this.baldiLecture.Play();

        while (this.baldiLecture.isPlaying)
            yield return null;

        this.audioCollider.enabled = !audioCollider.enabled;

        this.balDab.SetActive(true);
        this.balIdle.SetActive(false);
        this.baldiLmfao.Play();

        while (this.baldiLmfao.isPlaying)
            yield return null;

        this.baldiLmfao.Play();
        this.baldiLmfao.loop = true;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public PlayerScript player;
    [SerializeField] private ProgressionController progressionController;
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
