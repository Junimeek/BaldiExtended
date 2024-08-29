using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NullBoss : MonoBehaviour
{
    private void Start()
    {
        this.goCrazy = false;
        this.audioDevice = base.GetComponent<AudioSource>();
        this.audioDevice.clip = this.introClip_start;
        this.audioDevice.Play();
        this.playerScript.IncreaseFightSpeed(0);
        this.sprite.sprite = this.normalSprite;
    }

    public void WarpToExit(Vector3 position)
    {
        this.agent.Warp(position);

        this.musicController.QueueClips(this.musicController.playlist[0]);
    }

    private void Update()
    {
        if (this.allowMovement)
        {
            this.agent.speed = this.NullSpeed();
            this.agent.SetDestination(this.player.position);
        }
        else
            this.agent.speed = 0f;
    }

    private float NullSpeed()
    {
        return this.hits * 6f + 8f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Projectile"))
        {
            Destroy(other.gameObject);

            if (!this.isFightStarted)
            {
                this.musicController.QueueClips(this.musicController.playlist[1]);
                this.musicController.QueueClips(this.musicController.playlist[2]);
                this.musicController.ForceQueue();

                this.isFightStarted = true;
                this.isHit = true;
                this.audioDevice.loop = false;
                this.audioDevice.Stop();
                this.audioDevice.clip = this.pain;
                this.audioDevice.Play();
                this.StartCoroutine(this.BeginFight());
            }
            else if (this.hits < 10)
            {
                this.isHit = true;
                this.StartCoroutine(this.GetHit());
            }
        }
    }

    private IEnumerator BeginFight()
    {  
        this.hits++;
        this.gc.allowedProjectiles--;
        this.playerScript.IncreaseFightSpeed(1);
        this.sprite.sprite = this.grayscaleSprite;

        while (this.audioDevice.isPlaying)
        {
            this.sprite.color = this.RandomColor();
            yield return null;
        }
        
        this.sprite.sprite = this.normalSprite;
        this.sprite.color = UnityEngine.Color.white;
        this.audioDevice.clip = this.bossClip_start;
        this.audioDevice.Play();

        float remTime = 10.5f;
        while (remTime > 0f)
        {
            remTime -= Time.deltaTime;
            yield return null;
        }

        this.gc.CreateProjectile(3);
        this.allowMovement = true;
        this.isHit = false;
        
        while (this.audioDevice.isPlaying)
            yield return null;
        
        this.audioDevice.Stop();
        this.audioDevice.clip = this.pain;
    }

    private IEnumerator GetHit()
    {
        this.hits++;
        this.gc.allowedProjectiles--;

        if (this.hits == 10)
        {
            this.StartCoroutine(this.End());
            this.gc.DeleteProjectiles();
            yield break;
        }

        this.playerScript.IncreaseFightSpeed(this.hits);
        this.audioDevice.clip = this.pain;
        this.audioDevice.Play();
        this.allowMovement = false;
        this.sprite.sprite = this.grayscaleSprite;

        while (this.audioDevice.isPlaying)
        {
            this.sprite.color = this.RandomColor();
            yield return null;
        }
        
        this.sprite.sprite = this.normalSprite;
        this.sprite.color = UnityEngine.Color.white;
        this.allowMovement = true;
        this.isHit = false;
    }

    private UnityEngine.Color RandomColor()
    {
        float color1;
        float color2;
        float color3;

        color1 = Random.Range(0f, 1f);
        color2 = Random.Range(0f, 1f);
        color3 = Random.Range(0f, 1f);

        return new UnityEngine.Color(color1, color2, color3);
    }

    private IEnumerator End()
    {
        this.musicController.ClearQueue();
        this.audioDevice.clip = this.endClip_null;
        this.audioDevice.Play();
        this.playerScript.IncreaseFightSpeed(0);
        this.allowMovement = false;

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
        
        this.gc.DeactivateBossFight();
        Destroy(base.gameObject);
    }

    private void FixedUpdate()
    {
        if (this.goCrazy)
            this.sprite.transform.localPosition = this.RandomPosition(this.remainingCrazyTime);
    }

    private Vector3 RandomPosition(float intensity)
    {
        float newIntensity = 14.5f - intensity;

        float posX = Random.Range(-1f * newIntensity, newIntensity);
        float posY = Random.Range(-1f * newIntensity, newIntensity);
        float posZ = Random.Range(-1f * newIntensity, newIntensity);

        return new Vector3(posX, posY + 1.5f, posZ);
    }

    [SerializeField] private Transform player;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private BossMusicController musicController;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite grayscaleSprite;

    [Header("Null State")]
    [SerializeField] private bool isFightStarted;
    [SerializeField] private bool allowMovement;
    [SerializeField] private bool isHit;
    public int hits;
    [SerializeField] private bool goCrazy;
    [SerializeField] private float remainingCrazyTime;

    [Header("Audio")]
    [SerializeField] private AudioClip introClip_start;
    [SerializeField] private AudioClip introClip_loop;
    [SerializeField] private AudioClip bossClip_start;
    [SerializeField] private AudioClip pain;
    [SerializeField] private AudioClip endClip_null;
    [SerializeField] private AudioClip endClip_bg;
}
