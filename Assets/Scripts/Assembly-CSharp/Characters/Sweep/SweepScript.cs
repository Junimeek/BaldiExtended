using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SweepScript : MonoBehaviour
{
	private void Start()
	{
		this.origin = base.transform.position;
		this.gc = FindObjectOfType<GameControllerScript>();
		this.speechCooldown = 0f;
		StartCoroutine(this.WaitRoutine());
	}
	private void OnEnable()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.sweepHitbox = base.GetComponent<CapsuleCollider>();
	}

	IEnumerator WaitRoutine()
	{
		this.waitTime = Random.Range(120f, 180f);
		while (waitTime > 0f && !this.isParty)
		{
			waitTime -= Time.deltaTime;
			yield return null;
		}
		if (!this.hasActivated)
			this.hasActivated = true;
		
		StartCoroutine(this.SweepingRoutine());
	}

	IEnumerator SweepingRoutine()
	{
		this.audioDevice.PlayOneShot(this.aud_Intro);
		this.speechCooldown = 3f;
		this.wanders = 0;
		int curStage;
		if (this.isParty)
			curStage = 11;
		else
			curStage = 1;

		switch(curStage)
		{
			case 0: // Waiting for GS to reach destination
				while (!this.IsDestinationReached() && !this.isParty)
					yield return null;
				if (this.isParty)
					goto case 11;
				else if (this.wanders >= 5)
					goto case 3;
				else
					goto case 1;
			case 1: // Calculate new target destination
				this.Wander();
				while (this.agent.pathPending)
					yield return null;
				this.SetV2Destination(this.agent.destination);
				goto case 2;
			case 2: // Check if the target destination is too close to the current location
				if ((v2Location - v2Target).magnitude < 1.5f)
					goto case 1;
				this.wanders++;
				goto case 0;
			case 3: // Head home
				this.sweepHitbox.enabled = true;
				this.agent.SetDestination(this.origin);
				while (this.agent.pathPending)
					yield return null;
				while (!this.IsDestinationReached() && !this.isParty)
					yield return null;
				if (this.isParty)
					goto case 11;
				break;
			case 11:
				this.agent.SetDestination(gc.partyLocation.position);
				while (this.agent.pathPending)
					yield return null;
				this.SetV2Destination(this.agent.destination);
				while (!this.IsDestinationReached())
					yield return null;
				goto case 12;
			case 12:
				this.sweepHitbox.enabled = false;
				this.agent.SetDestination(wanderer.NewTarget("Party"));
				while (this.agent.pathPending)
					yield return null;
				this.SetV2Destination(this.agent.destination);
				while (!this.IsDestinationReached())
					yield return null;
				if (this.isParty)
					goto case 12;
				else
					goto case 3;
		}
		StartCoroutine(WaitRoutine());
	}

	bool IsDestinationReached()
	{
		if ((v2Target - v2Location).magnitude >= 1.5f)
			return false;
		else
			return true;
	}

	void SetV2Destination(Vector3 target)
	{
		this.v2Target.x = target.x;
		this.v2Target.y = target.z;
	}

	private void Update()
	{
		/*
		if (this.coolDown > 0f)
			this.coolDown -= 1f * Time.deltaTime;

		if (this.waitTime > 0f)
			this.waitTime -= Time.deltaTime;

		else if (!this.active)
		{
			this.active = true;
			this.wanders = 0;
			this.Wander();
			this.audioDevice.PlayOneShot(this.aud_Intro);
		}
		*/
		if (this.speechCooldown > 0f)
			this.speechCooldown -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		this.v2Location.x = base.transform.position.x;
		this.v2Location.y = base.transform.position.z;
		/*
		if ((double)this.agent.velocity.magnitude <= 0.1 & this.coolDown <= 0f & this.wanders < 5 & this.active) // If Gotta Sweep has roamed around the school 5 times
			this.Wander();
		else if (this.wanders >= 5)
			this.GoHome();
		*/
	}

	private void Wander()
	{
		/*
		this.hasActivated = true;

		if (this.isParty)
			this.agent.SetDestination(this.wanderer.NewTarget("Party"));
		else
			this.agent.SetDestination(this.wanderer.NewTarget("Hallway"));

		this.coolDown = 1f;
		*/
		this.agent.SetDestination(this.wanderer.NewTarget("Hallway"));
	}

	public void GoHome()
	{
		this.agent.SetDestination(this.origin);
		//this.waitTime = Random.Range(120f, 180f);
		//this.active = false;
	}

	public void GoToAttendance()
	{
		StopCoroutine(this.SweepingRoutine());
		this.sweepHitbox.enabled = false;
		this.agent.SetDestination(gc.attendanceOffice.position);
	}

	public void GoToParty()
	{
		this.isParty = true;
	}

	public void LeaveParty()
	{
		this.isParty = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.CompareTag("NPC") || other.CompareTag("Player")) && !this.isParty && this.speechCooldown <= 0f)
		{
			this.audioDevice.PlayOneShot(this.aud_Sweep);
			this.speechCooldown = 1.5f;
		}
	}

	/*
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && this.agent.velocity.magnitude > 0.5f)
		{
			this.playerSweepingTime += Time.deltaTime;

			if (this.playerSweepingTime >= 30f && !this.achievementCollected)
			{
				this.achievementCollected = true;
				if (this.achievementMonitor.isActiveAndEnabled && this.achievementMonitor != null)
					this.achievementMonitor.CollectAchievement(4, 0);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			this.playerSweepingTime = 0f;
		}
	}
	*/

	public void EarlyActivate()
	{
		if (!this.hasActivated)
		{
			this.waitTime = 0f;
			this.hasActivated = true;
		}
	}

	[Header("Achievement")]
	[SerializeField] AchievementMonitor achievementMonitor;
	[SerializeField] float playerSweepingTime;
	[SerializeField] bool achievementCollected;

	[Header("Sweep State")]
	[SerializeField] AILocationSelectorScript wanderer;
	float speechCooldown;
	[SerializeField] float waitTime;
	[SerializeField] int wanders;
	[SerializeField] bool active;
	enum CurrentSweepState
	{
		Stationary, Sweeping, Returning
	}
	[SerializeField] Vector2 v2Location;
	[SerializeField] Vector2 v2Target;
	[SerializeField] bool hasActivated;
	[SerializeField] bool isParty;
	Collider sweepHitbox;
	Vector3 origin;
	[SerializeField] AudioClip aud_Sweep;
	[SerializeField] AudioClip aud_Intro;
	NavMeshAgent agent;
	AudioSource audioDevice;
	GameControllerScript gc;
}
