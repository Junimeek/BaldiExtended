using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NullBoss : MonoBehaviour
{
    void Start()
    {
        this.goCrazy = false;
        this.audioDevice = base.GetComponent<AudioSource>();
        this.audioDevice.clip = this.introClip_start;
        this.audioDevice.Play();
        this.playerScript.IncreaseFightSpeed(0);
        this.playerScript.isInvincible = true;
        this.sprite.sprite = this.normalSprite;
        FindObjectOfType<CameraScript>().baldi = base.transform;
    }

    public void WarpToExit(Vector3 position)
    {
        this.agent.Warp(position);

        this.metronome.StartMetronome();
        this.musicController.QueueClips(this.musicController.playlist[0]);
    }

    void Update()
    {
        if (this.allowMovement)
        {
            this.agent.speed = this.NullSpeed();
            this.agent.SetDestination(this.player.position);
        }
        else
            this.agent.speed = 0f;
    }

    float NullSpeed()
    {
        if (this.speedOverride != 0f)
            return this.speedOverride;
        else
            return this.hits * 5.5f + 13f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Projectile"))
        {
            Destroy(other.gameObject);

            if (!this.isFightStarted)
            {
                this.musicController.EndInitialLoop(this.metronome.curMeasure + 1);

                this.audioDevice.loop = false;
                this.audioDevice.Stop();
                this.audioDevice.clip = this.pain;
                this.audioDevice.Play();
                this.StartCoroutine(this.BeginFight());
            }
            else if (this.hits < 10)
                this.StartCoroutine(this.GetHit());
        }
    }

    IEnumerator BeginFight()
    {  
        this.hits++;
        this.playerScript.IncreaseFightSpeed(1);
        this.challengeController.DeleteProjectiles();
        this.sprite.sprite = this.grayscaleSprite;

        while (this.audioDevice.isPlaying)
        {
            this.sprite.color = this.RandomColor();
            yield return null;
        }
        
        this.isFightStarted = true;
        this.sprite.sprite = this.normalSprite;
        this.sprite.color = UnityEngine.Color.white;
        this.audioDevice.clip = this.bossClip_start;
        this.audioDevice.Play();

        while (this.audioDevice.isPlaying)
            yield return null;
        
        this.audioDevice.clip = this.bossClip_startYell;
        this.audioDevice.Play();
        this.healthBar.InitializeHealthBar();

        float remTime = 2.6f;
        while (remTime > 0f)
        {
            remTime -= Time.deltaTime;
            yield return null;
        }

        this.challengeController.InitializeProjectileArrays();
        this.challengeController.CreateProjectile(4);
        this.allowMovement = true;
        this.playerScript.isInvincible = false;
        
        while (this.audioDevice.isPlaying)
            yield return null;
        
        this.audioDevice.Stop();
        this.audioDevice.clip = this.pain;
    }

    IEnumerator GetHit()
    {
        this.playerScript.isInvincible = true;
        this.hits++;
        this.QueueNextClip();

        if (this.hits == 10)
        {
            this.StartCoroutine(this.End());
            this.challengeController.DeleteProjectiles();
            yield break;
        }

        this.playerScript.IncreaseFightSpeed(this.hits);
        this.audioDevice.clip = this.pain;
        this.audioDevice.Play();
        this.allowMovement = false;
        this.sprite.sprite = this.grayscaleSprite;
        this.healthBar.DecreaseHealthBar();

        while (this.audioDevice.isPlaying)
        {
            this.sprite.color = this.RandomColor();
            yield return null;
        }
        
        this.sprite.sprite = this.normalSprite;
        this.sprite.color = UnityEngine.Color.white;
        this.allowMovement = true;
        this.playerScript.isInvincible = false;
    }

    void QueueNextClip()
    {
        switch(this.hits)
        {
            case 2:
                this.musicController.QueueClips(this.musicController.playlist[3]);
                break;
            case 3:
                this.musicController.QueueClips(this.musicController.playlist[4]);
                break;
            case 4:
                this.musicController.QueueClips(this.musicController.playlist[5]);
                break;
            case 5:
                this.musicController.QueueClips(this.musicController.playlist[6]);
                this.musicController.QueueClips(this.musicController.playlist[7]);
                break;
            case 6:
                this.musicController.QueueClips(this.musicController.playlist[8]);
                this.musicController.QueueClips(this.musicController.playlist[9]);
                break;
            case 7:
                this.musicController.QueueClips(this.musicController.playlist[10]);
                this.musicController.QueueClips(this.musicController.playlist[11]);
                break;
            case 8:
                this.musicController.QueueClips(this.musicController.playlist[12]);
                this.musicController.QueueClips(this.musicController.playlist[13]);
                break;
            case 9:
                this.musicController.QueueClips(this.musicController.playlist[14]);
                break;
        }
    }

    Color RandomColor()
    {
        float[] colors = {
            Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)
        };

        return new Color(colors[0], colors[1], colors[2]);
    }

    IEnumerator End()
    {
        this.musicController.ClearQueue();
        this.audioDevice.clip = this.endClip_null;
        this.audioDevice.Play();
        this.playerScript.IncreaseFightSpeed(0);
        this.allowMovement = false;
        this.healthBar.HideHealthBar();

        ProgressionController progressionController = FindObjectOfType<ProgressionController>();
        progressionController.mapUnlocks[0] = true;
        progressionController.SaveProgressionData();

        float remTime = 6.206f;

        while (remTime > 0f)
        {
            remTime -= Time.deltaTime;
            yield return null;
        }

        this.audioDevice.PlayOneShot(this.endClip_bg);
        this.remainingCrazyTime = 14.47f;
        this.goCrazy = true;
        this.sprite.sprite = this.grayscaleSprite;

        while (this.remainingCrazyTime > 0f)
        {
            this.remainingCrazyTime -= Time.deltaTime;
            this.sprite.color = this.RandomColor();
            yield return null;
        }
        
        this.challengeController.DeactivateBossFight();
        Destroy(base.gameObject);
    }

    void FixedUpdate()
    {
        if (this.goCrazy)
            this.sprite.transform.localPosition = this.RandomPosition(this.remainingCrazyTime);
    }

    Vector3 RandomPosition(float intensity)
    {
        float newIntensity = 14.5f - intensity;

        float posX = Random.Range(-1f * newIntensity, newIntensity);
        float posY = Random.Range(-1f * newIntensity, newIntensity);
        float posZ = Random.Range(-1f * newIntensity, newIntensity);

        return new Vector3(posX, posY + 1.5f, posZ);
    }

    [SerializeField] float speedOverride;
    [SerializeField] Transform player;
    [SerializeField] PlayerScript playerScript;
    [SerializeField] GameControllerScript gc;
    [SerializeField] ChallengeController challengeController;
    [SerializeField] BossHealthBar healthBar;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] AudioSource audioDevice;
    [SerializeField] BossMusicController musicController;
    [SerializeField] MetronomeScript metronome;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite grayscaleSprite;

    [Header("Null State")]
    public bool isFightStarted;
    [SerializeField] bool allowMovement;
    public byte hits;
    [SerializeField] bool goCrazy;
    [SerializeField] float remainingCrazyTime;

    [Header("Audio")]
    [SerializeField] AudioClip introClip_start;
    [SerializeField] AudioClip introClip_loop;
    [SerializeField] AudioClip bossClip_start;
    [SerializeField] AudioClip bossClip_startYell;
    [SerializeField] AudioClip pain;
    public AudioClip mockingClip;
    [SerializeField] AudioClip endClip_null;
    [SerializeField] AudioClip endClip_bg;
}
