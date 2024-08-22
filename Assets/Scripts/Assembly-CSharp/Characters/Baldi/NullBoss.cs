using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NullBoss : MonoBehaviour
{
    private void Start()
    {
        this.audioDevice = base.GetComponent<AudioSource>();
        this.audioDevice.clip = this.introClip_start;
        this.audioDevice.Play();
        this.playerScript.IncreaseFightSpeed(0);
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

            if (!this.allowMovement && !this.isHit)
            {
                this.isHit = true;
                this.audioDevice.loop = false;
                this.audioDevice.Stop();
                this.audioDevice.clip = this.pain;
                this.audioDevice.Play();
                this.StartCoroutine(this.BeginFight());
            }
            else if (!this.isHit)
            {
                this.isHit = true;
                this.StartCoroutine(this.GetHit());
            }
        }
    }

    private IEnumerator BeginFight()
    {  
        this.hits++;
        this.playerScript.IncreaseFightSpeed(1);

        float color1;
        float color2;
        float color3;

        while (this.audioDevice.isPlaying)
        {
            color1 = Random.Range(0f, 1f);
            color2 = Random.Range(0f, 1f);
            color3 = Random.Range(0f, 1f);

            this.sprite.color = new Color(color1, color2, color3);

            yield return null;
        }
        
        this.sprite.color = Color.white;
        this.audioDevice.clip = this.bossClip_start;
        this.audioDevice.Play();

        float remTime = 10.5f;
        while (remTime > 0f)
        {
            remTime -= Time.deltaTime;
            yield return null;
        }

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

        if (this.hits == 10)
            Destroy(base.gameObject);

        this.playerScript.IncreaseFightSpeed(this.hits);
        this.audioDevice.clip = this.pain;
        this.audioDevice.Play();
        this.allowMovement = false;

        float color1;
        float color2;
        float color3;

        while (this.audioDevice.isPlaying)
        {
            color1 = Random.Range(0f, 1f);
            color2 = Random.Range(0f, 1f);
            color3 = Random.Range(0f, 1f);

            this.sprite.color = new Color(color1, color2, color3);

            yield return null;
        }
        
        this.sprite.color = Color.white;
        this.allowMovement = true;
        this.isHit = false;
    }

    [SerializeField] private Transform player;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private SpriteRenderer sprite;

    [Header("Null State")]
    [SerializeField] private bool allowMovement;
    [SerializeField] private bool isHit;
    [SerializeField] private int hits;

    [Header("Audio")]
    [SerializeField] private AudioClip introClip_start;
    [SerializeField] private AudioClip introClip_loop;
    [SerializeField] private AudioClip bossClip_start;
    [SerializeField] private AudioClip pain;
}
