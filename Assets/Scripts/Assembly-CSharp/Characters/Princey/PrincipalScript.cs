using UnityEngine;
using UnityEngine.AI;

public class PrincipalScript : MonoBehaviour
{
	void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioQueue = base.GetComponent<AudioQueueScript>();
		notif = FindObjectOfType<NotificationBoard>();
	}

	void Update()
	{
		if (!this.inOffice)
		{
			if (this.angry && this.db)
				this.TargetPlayer();
			else
				this.CheckForRuleBreak();
		}

		if (this.seesRuleBreak && !this.angry && !this.inOffice)
		{
			if (this.timeSeenRuleBreak >= 0.5f)
			{
				this.angry = true;
				this.timeSeenRuleBreak = 0f;
				this.CorrectPlayer();
			}
			else
				this.timeSeenRuleBreak += Time.deltaTime;
		}
		else if (this.timeSeenRuleBreak > 0f)
			this.timeSeenRuleBreak -= Time.deltaTime;
		
		if (this.coolDown > 0f)
			this.coolDown -= Time.deltaTime;
	}

	void FixedUpdate()
	{
		this.aim = this.player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position, this.aim, out this.raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && this.raycastHit.transform.CompareTag("Player"))
			this.db = true;
		else
			this.db = false;
		
		if (!this.angry)
		{
			this.aim = this.bully.position - base.transform.position;
			if (Physics.Raycast(base.transform.position, this.aim, out this.raycastHit, float.PositiveInfinity, 769) && this.raycastHit.transform.name == "Its a Bully" && this.bullyScript.guilt > 0f)
				this.TargetBully();
		}

		if (this.agent.velocity.magnitude <= 1f && this.coolDown <= 0f)
			this.Wander();
	}

	void CheckForRuleBreak()
	{
		if (!this.db)
			this.seesRuleBreak = false;
		else if (this.playerScript.guilt > 0f && this.gc.isPrinceyTriggerShared && !this.gc.isPrinceyIgnore)
			this.seesRuleBreak = true;
		else
			this.seesRuleBreak = false;
	}

	void Wander()
	{
		this.playerScript.principalBugFixer = 1;

		if (!this.isParty)
			this.agent.SetDestination(this.wanderer.GetNewNPCTarget(AILocationSelectorScript.NPCTargetType.AllWanderPoints));
		else
			this.agent.SetDestination(this.wanderer.GetNewNPCTarget(AILocationSelectorScript.NPCTargetType.PartyWanderPoints));

		if (this.agent.isStopped)
			this.agent.isStopped = false;
		
		this.coolDown = 1f;

		if (Random.Range(0f, 10f) <= 1f && !this.isParty)
			this.quietAudioDevice.PlayOneShot(this.aud_Whistle);
	}

	public void GoToParty()
	{
		this.isParty = true;
		this.agent.SetDestination(this.gc.partyLocation.position);
	}

	public void LeaveParty()
	{
		this.isParty = false;
		this.Wander();
	}

	void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

	void TargetBully()
	{
		if (!this.bullySeen)
		{
			this.agent.SetDestination(this.bully.position);
			this.bullySeen = true;
			if (this.db)
				this.audioQueue.QueueAudio(this.audNoBullying);
			else
				this.audioQueue.QueueAudio(this.audDistantBullying);
		}
	}

	public void GuiltyAttendance()
	{
		this.angry = true;
		this.CorrectPlayer();
	}

	void CorrectPlayer()
	{
		this.audioQueue.ClearAudioQueue();

		switch(this.playerScript.guiltType)
		{
			case PlayerScript.GuiltType.Faculty:
				this.audioQueue.QueueAudio(this.audNoFaculty);
				notif.RuleText(1);
				break;
			case PlayerScript.GuiltType.Running:
				this.audioQueue.QueueAudio(this.audNoRunning);
				notif.RuleText(2);
				break;
			case PlayerScript.GuiltType.Drinking:
				this.audioQueue.QueueAudio(this.audNoDrinking);
				notif.RuleText(3);
				break;
			case PlayerScript.GuiltType.Escaping:
				this.audioQueue.QueueAudio(this.audNoEscaping);
				notif.RuleText(4);
				break;
			case PlayerScript.GuiltType.Bullying:
				this.audioQueue.QueueAudio(this.audNoBullying);
				notif.RuleText(5);
				break;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.name == "Office Trigger")
			this.inOffice = true;

		if (other.CompareTag("Player") && this.angry && !this.inOffice)
		{
			this.inOffice = true;
			this.playerScript.principalBugFixer = 0;
			this.playerScript.guilt = 0f;
			this.agent.Warp(this.gc.detentionPrincipalPos);
			this.agent.isStopped = true;
			this.playerScript.WarpPlayer("detention");
			this.playerScript.LookAtCharacter("princey");

			this.audioQueue.QueueAudio(this.aud_Delay);
			this.audioQueue.QueueAudio(this.audTimes[this.detentions]);
			this.audioQueue.QueueAudio(this.audDetention);
			int num = Mathf.RoundToInt(Random.Range(0f, 3f));
			this.audioQueue.QueueAudio(this.audScolds[num]);

			this.officeDoor.LockDoor(this.lockTime[this.detentions]);
			this.gc.remainingDetentionTime = this.lockTime[this.detentions];
			if (this.baldiScript.isActiveAndEnabled)
				this.baldiScript.AddNewSound(base.transform.position, 3);
			this.coolDown = 5f;
			this.angry = false;
			this.detentions++;
			this.gc.stats.detentions++;
			if (this.detentions > 10)
				this.detentions = 10;

			notif.DetentionBoardRoutine();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.name == "Office Trigger")
			this.inOffice = false;

		if (other.name == "Its a Bully")
			this.bullySeen = false;
	}

	[SerializeField] bool seesRuleBreak;
	[SerializeField] bool db;
	[SerializeField] Transform player;
	[SerializeField] Transform bully;
	[SerializeField] bool bullySeen;
	[SerializeField] PlayerScript playerScript;
	[SerializeField] BullyScript bullyScript;
	[SerializeField] BaldiScript baldiScript;
	[SerializeField] AILocationSelectorScript wanderer;
	[SerializeField] ClassroomDoorScript officeDoor;
	[SerializeField] float coolDown;
	[SerializeField] float timeSeenRuleBreak;
	[SerializeField] bool angry;
	[SerializeField] bool inOffice;
	[SerializeField] bool isParty;
	int detentions;
	readonly int[] lockTime = new int[]
	{
		15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 99
	};
	[SerializeField] AudioClip[] audTimes = new AudioClip[5];
	[SerializeField] AudioClip[] audScolds = new AudioClip[3];
	[SerializeField] AudioClip audDetention;
	[SerializeField] AudioClip audNoDrinking;
	[SerializeField] AudioClip audNoBullying;
	[SerializeField] AudioClip audDistantBullying;
	[SerializeField] AudioClip audNoFaculty;
	[SerializeField] AudioClip audNoRunning;
	[SerializeField] AudioClip audNoEscaping;
	[SerializeField] AudioClip aud_Whistle;
	[SerializeField] AudioClip aud_Delay;
	NavMeshAgent agent;
	AudioQueueScript audioQueue;
	[SerializeField] AudioSource quietAudioDevice;
	RaycastHit raycastHit;
	Vector3 aim;
	[SerializeField] private NotificationBoard notif;
	[SerializeField] private GameControllerScript gc;
}
