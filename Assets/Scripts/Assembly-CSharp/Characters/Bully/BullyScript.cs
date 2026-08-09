using UnityEngine;

public class BullyScript : MonoBehaviour
{
	void Start()
	{
		this.audioDevice = base.GetComponent<AudioSource>(); //Get the Audio Source
		this.waitTime = Random.Range(60f, 120f); //Set the amount of time before the bully appears again
	}

	void Update()
	{
		if (this.waitTime > 0f)
			this.waitTime -= Time.deltaTime;
		else if (!this.active)
			this.Activate();

		if (this.active)
		{
			this.activeTime += Time.deltaTime;
			if (this.activeTime >= 180f && (base.transform.position - this.player.position).magnitude >= 120f) //If the bully has been in the map for a long time and the player is far away
			{
				this.audioDevice.PlayOneShot(this.aud_Bored);
				this.Reset();
			}
		}
		if (this.guilt > 0f)
			this.guilt -= Time.deltaTime;
	}

	void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 4f, 0f), direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & raycastHit.transform.CompareTag("Player") & (base.transform.position - this.player.position).magnitude <= 30f & this.active)
		{
			if (!this.spoken)
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 1f));
				this.audioDevice.PlayOneShot(this.aud_Taunts[num]);
				this.spoken = true;
			}
			this.guilt = 10f;
		}
	}

	void Activate()
	{
		this.isDetention = false;
		base.transform.position = this.wanderer.GetNewNPCTarget(AILocationSelectorScript.NPCTargetType.Bully) + new Vector3(0f, 5f, 0f);
		while ((base.transform.position - this.player.position).magnitude < 20f)
		{
			base.transform.position = this.wanderer.GetNewNPCTarget(AILocationSelectorScript.NPCTargetType.Bully) + new Vector3(0f, 5f, 0f);
        }
		this.active = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			if (this.isDetention)
				this.audioDevice.PlayOneShot(this.aud_Bored);
			else
			{
				if (this.gc.IsNoItems())
					this.audioDevice.PlayOneShot(this.aud_Denied);
				else
					this.TakeItem();
			}
		}
	}

	void TakeItem()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, this.gc.totalSlotCount - 1));
		while (this.gc.item[num] == 0)
		{
			num = Mathf.RoundToInt(Random.Range(0f, this.gc.totalSlotCount - 1));
		}
		this.gc.LoseItem(num);
		int num2 = Mathf.RoundToInt(Random.Range(0f, 1f));
		this.longAudioDevice.PlayOneShot(this.aud_Thanks[num2]);
		this.Reset();
	}

	void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "Principal of the Thing" && this.guilt > 0f && !this.isDetention)
		{
			this.isDetention = true;
			this.Reset();
		}
	}

	void Reset()
	{
		if (this.isDetention)
			base.transform.position = this.detentionPos;
		else
			base.transform.position = base.transform.position + new Vector3(0f, 150f, 0f);

		this.waitTime = Random.Range(60f, 120f);
		this.active = false;
		this.activeTime = 0f;
		this.spoken = false;
	}

	public Transform player;
	public GameControllerScript gc;
	public Renderer bullyRenderer;
	public AILocationSelectorScript wanderer;
	public float waitTime;
	public float activeTime;
	public float guilt;
	public bool active;
	public bool spoken;
	private AudioSource audioDevice;
	[SerializeField] private AudioSource longAudioDevice;
	[SerializeField] private bool isDetention;
	[SerializeField] private Vector3 detentionPos;
	public AudioClip[] aud_Taunts = new AudioClip[2];
	public AudioClip[] aud_Thanks = new AudioClip[2];
	public AudioClip aud_Denied;
	public AudioClip aud_Bored;
}
