using UnityEngine;
using UnityEngine.AI;

public class PrincipalScript : MonoBehaviour
{
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>(); //Get the agent
		this.audioQueue = base.GetComponent<AudioQueueScript>();
		this.audioDevice = base.GetComponent<AudioSource>();
		notif = FindObjectOfType<NotificationBoard>();
	}

	private void Update()
	{
		if (this.seesRuleBreak)
		{
			this.timeSeenRuleBreak += 1f * Time.deltaTime;
			if ((double)this.timeSeenRuleBreak >= 0.5 & !this.angry) // If the principal sees the player break a rule for more then 1/2 of a second
			{
				this.angry = true;
				this.seesRuleBreak = false;
				this.timeSeenRuleBreak = 0f;
				this.TargetPlayer(); //Target the player
				this.CorrectPlayer();
			}
		}
		else
			this.timeSeenRuleBreak = 0f;
		
		if (this.coolDown > 0f)
			this.coolDown -= 1f * Time.deltaTime;
	}

	private void FixedUpdate()
	{
		if (!this.angry) // If the principal isn't angry
		{
			this.aim = this.player.position - base.transform.position; // If he sees the player and the player has guilt
			if (Physics.Raycast(base.transform.position, this.aim, out this.hit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && this.hit.transform.tag == "Player")
			{
				this.db = true;

				if (this.playerScript.guilt > 0f && !this.inOffice && !this.angry && this.gc.isPrinceyTriggerShared && !this.gc.isPrinceyIgnore)
					this.seesRuleBreak = true;
				else
					this.seesRuleBreak = false;
			}
			else
				this.db = false;

			if (this.db == false)
				this.seesRuleBreak = false;

			this.aim = this.bully.position - base.transform.position;
			if (Physics.Raycast(base.transform.position, this.aim, out this.hit, float.PositiveInfinity, 769) & this.hit.transform.name == "Its a Bully" & this.bullyScript.guilt > 0f & !this.inOffice & !this.angry)
				this.TargetBully();
		}
		else if (this.db)
			this.TargetPlayer(); // If the principal is angry, target the player

		if (this.agent.velocity.magnitude <= 1f && this.coolDown <= 0f)
			this.Wander();
	}

	private void Wander()
	{
		this.playerScript.principalBugFixer = 1;

		if (!this.isParty)
			this.agent.SetDestination(this.wanderer.NewTarget("Princey"));
		else
			this.agent.SetDestination(this.wanderer.NewTarget("Party"));

		if (this.agent.isStopped)
			this.agent.isStopped = false;
		
		this.coolDown = 1f;

		if (UnityEngine.Random.Range(0f, 10f) <= 1f && !this.isParty)
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

	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

	private void TargetBully()
	{
		if (!this.bullySeen)
		{
			this.agent.SetDestination(this.bully.position);
			this.audioQueue.QueueAudio(this.audNoBullying);
			this.bullySeen = true;
		}
	}

	public void GuiltyAttendance()
	{
		this.angry = true;
		this.CorrectPlayer();
	}

	private void CorrectPlayer()
	{
		this.audioQueue.ClearAudioQueue();

		switch(this.playerScript.guiltType)
		{
			case "faculty":
				this.audioQueue.QueueAudio(this.audNoFaculty);
				notif.RuleText(1);
				break;
			case "running":
				this.audioQueue.QueueAudio(this.audNoRunning);
				notif.RuleText(2);
				break;
			case "drink":
				this.audioQueue.QueueAudio(this.audNoDrinking);
				notif.RuleText(3);
				break;
			case "escape":
				this.audioQueue.QueueAudio(this.audNoEscaping);
				notif.RuleText(4);
				break;
			case "bully":
				this.audioQueue.QueueAudio(this.audNoBullying);
				notif.RuleText(5);
				break;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.name == "Office Trigger")
			this.inOffice = true;

		if (other.tag == "Player" & this.angry & !this.inOffice)
		{
			this.inOffice = true;
			this.playerScript.principalBugFixer = 0;
			this.playerScript.guilt = 0f;
			this.agent.Warp(this.gc.detentionPrincipalPos); //Teleport the principal to the office
			this.agent.isStopped = true; //Stop the principal from moving
			this.cc.enabled = false;
			other.transform.position = this.gc.detentionPlayerPos; // Teleport the player to the office
			this.playerScript.LookAtCharacter("princey"); // Get the plaer to look at the principal
			this.cc.enabled = true;

			this.audioQueue.QueueAudio(this.aud_Delay);
			this.audioQueue.QueueAudio(this.audTimes[this.detentions]); //Play the detention time sound
			this.audioQueue.QueueAudio(this.audDetention);
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 3f));
			this.audioQueue.QueueAudio(this.audScolds[num]); // Say one of the other lines

			this.officeDoor.LockDoor((float)this.lockTime[this.detentions]); // Lock the door
			if (this.baldiScript.isActiveAndEnabled) this.baldiScript.AddNewSound(base.transform.position, 3);
			this.coolDown = 5f;
			this.angry = false;
			this.detentions++;
			this.gc.stats.detentions++;
			if (this.detentions > 10)
				this.detentions = 10;

			notif.DetentionBoardRoutine();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Office Trigger")
			this.inOffice = false;

		if (other.name == "Its a Bully")
			this.bullySeen = false;
	}

	public bool seesRuleBreak;
	[SerializeField] private bool db;
	public Transform player;
	public Transform bully;
	public bool bullySeen;
	public PlayerScript playerScript;
	public BullyScript bullyScript;
	public BaldiScript baldiScript;
	public AILocationSelectorScript wanderer;
	public DoorScript officeDoor;
	public float coolDown;
	public float timeSeenRuleBreak;
	public bool angry;
	public bool inOffice;
	public bool isParty;
	private int detentions;
	private int[] lockTime = new int[]
	{
		15,
		20,
		25,
		30,
		35,
		40,
		45,
		50,
		55,
		60,
		99
	};
	public AudioClip[] audTimes = new AudioClip[5];
	public AudioClip[] audScolds = new AudioClip[3];
	public AudioClip audDetention;
	public AudioClip audNoDrinking;
	public AudioClip audNoBullying;
	public AudioClip audNoFaculty;
	public AudioClip audNoLockers;
	public AudioClip audNoRunning;
	public AudioClip audNoStabbing;
	public AudioClip audNoEscaping;
	public AudioClip aud_Whistle;
	public AudioClip aud_Delay;
	private NavMeshAgent agent;
	private AudioQueueScript audioQueue;
	private AudioSource audioDevice;
	public AudioSource quietAudioDevice;
	[SerializeField] private RaycastHit hit;
	private Vector3 aim;
	public CharacterController cc;
	private NotificationBoard notif;
	[SerializeField] private GameControllerScript gc;
}
