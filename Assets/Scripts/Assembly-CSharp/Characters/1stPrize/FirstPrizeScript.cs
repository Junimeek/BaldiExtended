using UnityEngine;
using UnityEngine.AI;

public class FirstPrizeScript : MonoBehaviour
{
	private void OnEnable()
	{
		this.currentSpeed = this.normSpeed;
		this.isDisabled = false;
		this.agent = base.GetComponent<NavMeshAgent>();
		this.gc = FindObjectOfType<GameControllerScript>();
		this.coolDown = 1f;
		this.Wander();
	}

	private void Update()
	{
		if (this.coolDown > 0f)
			this.coolDown -= 1f * Time.deltaTime;

		if (this.autoBrakeCool > 0f)
			this.autoBrakeCool -= 1f * Time.deltaTime;
		else
			this.agent.autoBraking = true;
		
		this.angleDiff = Mathf.DeltaAngle(base.transform.eulerAngles.y, Mathf.Atan2(this.agent.steeringTarget.x - base.transform.position.x, this.agent.steeringTarget.z - base.transform.position.z) * 57.29578f);
		
		if (this.crazyTime <= 0f)
		{
			if (Mathf.Abs(this.angleDiff) < 5f)
			{
				base.transform.LookAt(new Vector3(this.agent.steeringTarget.x, base.transform.position.y, this.agent.steeringTarget.z));
				this.agent.speed = this.currentSpeed;
			}
			else
			{
				base.transform.Rotate(new Vector3(0f, this.turnSpeed * Mathf.Sign(this.angleDiff) * Time.deltaTime, 0f));
				this.agent.speed = 0f;
			}
		}
		else
		{
			this.agent.speed = 0f;
			base.transform.Rotate(new Vector3(0f, 180f * Time.deltaTime, 0f));
			this.crazyTime -= Time.deltaTime;
		}
		this.motorAudio.pitch = (this.agent.velocity.magnitude + 1f) * Time.timeScale;

		this.actualSpeed = agent.velocity.magnitude;
		//if (this.prevSpeed - this.agent.velocity.magnitude > 15f)
		//{
		//	this.audioDevice.PlayOneShot(this.audBang);
		//}
		//this.prevSpeed = this.agent.velocity.magnitude;
	}

	private void FixedUpdate()
	{
		if (this.isDisabled || this.isParty)
			return;
		
		RaycastHit windowRayCastHit;
		bool windowRayCast = Physics.Raycast(base.transform.position, base.transform.rotation * Vector3.forward, out windowRayCastHit, float.PositiveInfinity);
		if (this.actualSpeed > 30f && windowRayCast && windowRayCastHit.transform.CompareTag("BreakableWindow"))
		{
			if ((base.transform.position - windowRayCastHit.transform.position).magnitude < 5f)
			{
				WindowScript windowScript = windowRayCastHit.transform.GetComponent<WindowScript>();
				windowScript.FirstPrizeHit();
				this.audioDevice.PlayOneShot(this.metalpipe);
			}
		}

		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player")
		{
			if (!this.playerSeen && !this.audioDevice.isPlaying)
			{
				int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				this.audioDevice.PlayOneShot(this.aud_Found[num]);
			}
			this.playerSeen = true;
			this.TargetPlayer();
			this.currentSpeed = this.runSpeed;
		}
		else
		{
			this.currentSpeed = this.normSpeed;
			if (this.playerSeen & this.coolDown <= 0f)
			{
				if (!this.audioDevice.isPlaying)
				{
					int num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
					this.audioDevice.PlayOneShot(this.aud_Lost[num2]);
				}
				this.playerSeen = false;
				this.Wander();
			}
			else if (this.actualSpeed <= 1f & this.coolDown <= 0f & (base.transform.position - this.agent.destination).magnitude < 5f)
			{
				this.Wander();
			}
		}
	}

	private void Wander()
	{
		if (!this.isParty)
			this.agent.SetDestination(this.wanderer.NewTarget("Hallway"));
		else
			this.agent.SetDestination(this.wanderer.NewTarget("Party"));
			
		this.hugAnnounced = false;
		int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f));
		if (!this.audioDevice.isPlaying & num == 0 & this.coolDown <= 0f)
		{
			int num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
			this.audioDevice.PlayOneShot(this.aud_Random[num2]);
		}
		this.coolDown = 1f;
	}

	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 0.5f;
	}

	public void GoToParty()
	{
		this.isParty = true;
		this.agent.SetDestination(this.gc.partyLocation.position);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && (!this.isDisabled || !this.isParty))
		{
			if (!this.audioDevice.isPlaying & !this.hugAnnounced)
			{
				int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				this.audioDevice.PlayOneShot(this.aud_Hug[num]);
				this.hugAnnounced = true;
			}
			this.agent.autoBraking = false;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			this.autoBrakeCool = 1f;
		}
	}

	public void GoCrazy()
	{
		this.crazyTime = 15f;
	}

	public void GoToAttendance()
	{
		this.isDisabled = true;
		this.currentSpeed = this.normSpeed;
		this.agent.SetDestination(this.gc.attendanceOffice.position);
	}

	[SerializeField] float turnSpeed;
	[SerializeField] float angleDiff;
	[SerializeField] float normSpeed;
	[SerializeField] float runSpeed;
	[SerializeField] float currentSpeed;
	[SerializeField] float autoBrakeCool;
	[SerializeField] float crazyTime;
	[SerializeField] float coolDown;
	[SerializeField] bool playerSeen;
	[SerializeField] bool hugAnnounced;
	[SerializeField] AILocationSelectorScript wanderer;
	[SerializeField] Transform player;
	[SerializeField] AudioClip[] aud_Found = new AudioClip[2];
	[SerializeField] AudioClip[] aud_Lost = new AudioClip[2];
	[SerializeField] AudioClip[] aud_Hug = new AudioClip[2];
	[SerializeField] AudioClip[] aud_Random = new AudioClip[2];
	[SerializeField] AudioClip audBang;
	[SerializeField] AudioClip metalpipe;
	[SerializeField] AudioSource audioDevice;
	[SerializeField] AudioSource motorAudio;
	NavMeshAgent agent;
	public bool isDisabled;
	public bool isParty;
	GameControllerScript gc;
	[SerializeField] float actualSpeed;
}
