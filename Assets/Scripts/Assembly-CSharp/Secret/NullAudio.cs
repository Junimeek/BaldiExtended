using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NullAudio : MonoBehaviour
{
    void Start()
    {
        this.audioCollider = GetComponent<BoxCollider>();
        this.isPlaying = false;
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !this.isPlaying)
		{
            this.isPlaying = true;
            
            if (PlayerPrefs.GetInt("gps_safemode") == 1)
                StartCoroutine(this.WaitForDab());
            else
            {
                this.audioDevice.PlayOneShot(this.nullMonologue);
                this.player.isSecret = true;
                this.gc.gameOverDelay = this.gameOverDelay;
                StartCoroutine(this.WaitForGlitch());

                this.SavePlayerProgressionData();
            }
		}
    }

    void SavePlayerProgressionData()
    {
        Yui yui = Tsuruno.DataLoader.LoadAllV3Data(1);
        SD3ProgressionData newProgressionData = yui.progressionData;
        newProgressionData.challengeUnlocks[0] = true;

        Yui newYui = new Yui(yui.playerData, yui.storyDatas, yui.endlessDatas, yui.challengeDatas, newProgressionData);
        Tsuruno.DataSaver.SaveAllV3Data(newYui);

        SaveHead saveHead = FindObjectOfType<SaveHead>();
        if (saveHead != null)
            saveHead.ActivateSaveHead(1.4f);
    }

    IEnumerator WaitForGlitch()
    {
        this.nullAgent.allowMovement = false;

        while (this.audioDevice.isPlaying)
            yield return null;

        this.audioCollider.enabled = !audioCollider.enabled;

        this.nullAgent.allowMovement = true;
        this.nullGlitchLoop.Play();
    }

    IEnumerator WaitForDab()
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
    [SerializeField] GameControllerScript gc;
    [SerializeField] float gameOverDelay;
    [SerializeField] NullAgent nullAgent;
    [SerializeField] Collider audioCollider;
    [SerializeField] bool isPlaying;
    [SerializeField] GameObject balIdle;
    [SerializeField] GameObject balDab;
    public AudioSource audioDevice;
    [SerializeField] AudioSource baldiLecture;
    [SerializeField] AudioSource baldiLmfao;
    public AudioClip nullMonologue;
    public AudioSource nullGlitchLoop;
}
