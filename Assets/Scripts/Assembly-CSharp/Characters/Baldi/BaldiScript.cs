using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaldiScript : MonoBehaviour
{
	private void Awake()
	{
		gc = FindObjectOfType<GameControllerScript>();
	}

	private void Start()
	{
		this.baldiAudio = base.GetComponent<AudioSource>(); //Get The Baldi Audio Source(Used mostly for the slap sound)
		this.agent = base.GetComponent<NavMeshAgent>(); //Get the Nav Mesh Agent
		this.timeToMove = this.baseTime; //Sets timeToMove to baseTime
		this.Wander(); //Start wandering

		this.ClearSoundList();

		if (speedFactorOverride == 0f)
		{
			float speedFactor = this.gc.daFinalBookCount + 0.5f;
			float bookSquare = speedFactor * speedFactor;
			this.baldiSpeedScale = MathF.Sqrt(15.2f / bookSquare);
		}
		else
			this.baldiSpeedScale = this.speedFactorOverride;

		if (this.gc.isSafeMode)
			this.baldiAnimator.SetTrigger("ghostSlap");
		
		if (this.gc.modeType == "nullStyle")
			this.speechTimer = 30f;
	}

	private void Update()
	{
		if (this.timeToMove > 0f) //If timeToMove is greater then 0, decrease it
			this.timeToMove -= 1f * Time.deltaTime;

		else
		{
			if (this.baldiAnger == 0) this.timeToMove = 3f;
			else this.timeToMove = this.baldiWait - this.baldiTempAnger;
			this.Move(); //Start moving
		}

		if (this.coolDown > 0f) //If coolDown is greater then 0, decrease it
			this.coolDown -= 1f * Time.deltaTime;

		if (this.baldiTempAnger > 0f) //Slowly decrease Baldi's temporary anger over time.
			this.baldiTempAnger -= 0.02f * Time.deltaTime;

		else this.baldiTempAnger = 0f; //Cap its lowest value at 0

		if (this.antiHearingTime > 0f) //Decrease antiHearingTime, then when it runs out stop the effects of the antiHearing tape
			this.antiHearingTime -= Time.deltaTime;

		else this.antiHearing = false;

		if (this.endless) //Only activate if the player is playing on endless mode
		{
			if (this.timeToAnger > 0f) //Decrease the timeToAnger
				this.timeToAnger -= 1f * Time.deltaTime;

			else
			{
				this.timeToAnger = this.angerFrequency; //Set timeToAnger to angerFrequency
				this.GetAngry(this.angerRate); //Get angry based on angerRate
				this.angerRate += this.angerRateRate; //Increase angerRate for next time
			}
		}

		if (this.db)
			this.sightCooldown = 0.3f;
		else if (this.sightCooldown > 0f)
			this.sightCooldown -= Time.deltaTime;
		
		if (this.gc.modeType == "nullStyle")
			this.speechTimer -= Time.deltaTime;

		if (this.gc.modeType == "nullStyle" && this.db && this.playerScript.stamina <= 0f && this.gc.IsNoItems()
		&& this.baldiAnger >= 5 && this.speechTimer < 61f && !this.longAudioDevice.isPlaying)
		{
			this.StartCoroutine(this.NullSight());
			this.longAudioDevice.PlayOneShot(this.nullSpeech[5]);
		}

		if (this.gc.modeType == "nullStyle" && this.speechTimer < 0f)
		{
			if (!this.db && this.currentPriority == 0)
			{
				this.ResetSpeechTimer();
				this.longAudioDevice.PlayOneShot(this.nullSpeech[6]);
			}
			else
			{
				this.ResetSpeechTimer();
				this.baldiAudio.PlayOneShot(this.nullSpeech[this.RandomSpeech()]);
			}
		}
	}

	private void ResetSpeechTimer()
	{
		this.speechTimer = UnityEngine.Random.Range(35f, 75f);
	}

	private int RandomSpeech()
	{
		return Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
	}

	private IEnumerator NullSight()
	{
		while (this.db)
		{
			this.speechTimer = 69f;
			yield return null;
		}
	}

	private void FixedUpdate()
	{
		if (this.moveFrames > 0f) //Move for a certain amount of frames, and then stop moving.(Ruler slapping)
		{
			this.moveFrames -= 1f;
			this.agent.speed = this.speed;
		}
		else
			this.agent.speed = 0f;

		Vector3 direction = this.player.position - base.transform.position; 
		RaycastHit raycastHit;

		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player") //Create a raycast, if the raycast hits the player, Baldi can see the player
		{
			this.db = true;

			if (this.alarmClock != null)
				Destroy(this.alarmClock);

			this.TargetPlayer(); //Start attacking the player
		}
		else
			this.db = false;
	}

	private void Wander()
	{
		if (this.isParty)
			this.agent.SetDestination(this.wanderer.NewTarget("Party"));
		else
			this.agent.SetDestination(this.wanderer.NewTarget("Baldi")); //Head towards the position of the wanderTarget object
		this.coolDown = 1f; //Set the cooldown
	}

	public void TargetPlayer()
	{
		this.AddNewSound(this.player.position, 6); //Target the player
		this.coolDown = 1f;

		if (sightCooldown <= 0f)
		{
			baldicator.ChangeBaldicatorState("Sight");
			Debug.Log("Sighted Player");
		}
	}

	private void Move()
	{
		Vector3 offset = this.agent.destination - base.transform.position;
		float sqrLen = offset.sqrMagnitude;

		if (sqrLen < 4f && this.coolDown < 0f)
			this.DecreasePriority();

		this.moveFrames = 10f;
		this.baldiAudio.PlayOneShot(this.slap); //Play the slap sound

		if (this.gc.modeType != "nullStyle")
		{
			if (this.gc.isSafeMode)
				this.baldiAnimator.SetTrigger("ghostSlap");
			else
				this.baldiAnimator.SetTrigger("slap");
		}
	}

	public void GetAngry(float value)
	{
		this.baldiAnger += value; // Increase Baldi's anger by the value provided

		if (this.baldiAnger < 0.5f) //Cap Baldi anger at a minimum of 0.5
			this.baldiAnger = 0.5f;
		
		this.baldiWait = -3f * this.baldiAnger / (this.baldiAnger + 2f / this.baldiSpeedScale) + 3f; // Some formula I don't understand.
		
		if (this.baldiWait <= 0f)
			this.baldiWait = 3f;
	}

	public void GetTempAngry(float value)
	{
		this.baldiTempAnger += value; //Increase Baldi's Temporary Anger
	}

	public void ActivateAntiHearing(float t)
	{
		this.ClearSoundList();
		this.DecreasePriority();
		this.Wander(); //Start wandering
		this.antiHearing = true; //Set the antihearing variable to true for other scripts
		this.antiHearingTime = t; //Set the time the tape's effect on baldi will last
	}

	public void AddNewSound(Vector3 location, int priority)
	/*
	1 = Door
	2 = Bad Math
	3 = Detention
	4 = Exit
	5 = Alarm Clock, Party
	6 = Sight
	*/
	{
		if (this.db)
			this.StartCoroutine(FollowPlayer());
		if (this.antiHearing)
			return;

		if (priority == 5 && !this.db && this.isAlarmClock)
		{
			this.ClearSoundList();
			this.soundList[priority - 1] = this.alarmClock.transform.position;
			this.currentPriority = priority;
			this.baldicator.ChangeBaldicatorState("Pursuit");
			this.agent.SetDestination(this.alarmClock.transform.position);
			this.isAlarmClock = false;
			this.isParty = false;
			return;
		}

		this.isAlarmClock = false;

		if (priority >= this.currentPriority)
		{
			this.ClearSoundList();
			this.soundList[priority - 1] = location;
			this.currentPriority = priority;
			if (!this.db)
			{
				this.baldicator.ChangeBaldicatorState("Pursuit");
				this.agent.SetDestination(this.soundList[this.currentPriority - 1]);
			}
		}
		else
		{
			this.soundList[priority - 1] = location;
			this.baldicator.ChangeBaldicatorState("Ignore");
		}
	}

	private void ClearSoundList()
	{
		for (int i = 0; i < 6; i++)
			this.soundList[i] = new Vector3(99.9f, -99.9f, 39f);
	}

	private void DecreasePriority()
	{
		if (this.alarmClock != null)
			Destroy(this.alarmClock);
		
		if (this.currentPriority <= 0 || this.isParty)
		{
			if (this.currentPriority == 6 && this.isParty)
			{
				this.soundList[5] = new Vector3(99.9f, -99.9f, 39f);
				this.currentPriority = 5;
			}

			this.Wander();
			return;
		}
		
		this.soundList[this.currentPriority - 1] = new Vector3(99.9f, -99.9f, 39f);
		this.currentPriority--;

		if (this.currentPriority <= 0)
		{
			this.currentPriority = 0;
			this.baldicator.ChangeBaldicatorState("End");
			this.Wander();
			return;
		}

		if (this.soundList[this.currentPriority - 1].x == 99.9f && this.soundList[this.currentPriority - 1].y == -99.9f && this.soundList[this.currentPriority - 1].z == 39f)
		{
			this.DecreasePriority();
			return;
		}
		else
		{
			this.agent.SetDestination(this.soundList[this.currentPriority - 1]);
			this.theNewLocation = agent.destination;
			this.baldicator.ChangeBaldicatorState("Next");
		}
	}

	private IEnumerator FollowPlayer()
	{
		while (this.db)
		{
			this.currentPriority = 6;
			this.ClearSoundList();
			this.soundList[this.currentPriority - 1] = this.player.position;
			this.agent.SetDestination(this.soundList[this.currentPriority - 1]);
			yield return null;
		}
	}

	public void GoToParty()
	{
		if (this.alarmClock != null)
			Destroy(this.alarmClock);
		
		this.StartCoroutine(this.WaitForPartyEnd());
	}

	private IEnumerator WaitForPartyEnd()
	{
		this.AddNewSound(this.gc.partyLocation.position, 5);
		this.isParty = true;

		while (!this.db && this.isParty)
			yield return null;

		this.isParty = false;
	}

	[Header("Priority System")]
	public int currentPriority;
	[SerializeField] private Vector3[] soundList;
	public Baldicator baldicator;
	[SerializeField] private float sightCooldown;
	[SerializeField] private Vector3 theNewLocation;
	public bool isAlarmClock;
	public bool isParty;

	[Header("Null Modifications")]
	[SerializeField] private AudioClip[] nullSpeech;
	[SerializeField] private AudioSource longAudioDevice;
	[SerializeField] private float speechTimer;

	[Space(20f)]
	public bool db;
	public float baseTime;
	public float speed;
	public float timeToMove;
	public float baldiAnger;
	public float baldiTempAnger;
	public float baldiWait;
	public float baldiSpeedScale;
	
	[Tooltip("If this value is set to 0, then the total noteboo count will be used to calculate the speed factor.")]
	[SerializeField] private float speedFactorOverride;
	private float moveFrames;
	public bool antiHearing;
	public float antiHearingTime;
	public float vibrationDistance;
	public float angerRate;
	public float angerRateRate;
	public float angerFrequency;
	public float timeToAnger;
	public bool endless;
	public Transform player;
	[SerializeField] private PlayerScript playerScript;
	public AILocationSelectorScript wanderer;
	private AudioSource baldiAudio;
	public AudioClip slap;
	public Animator baldiAnimator;
	public float coolDown;
	[SerializeField] private Vector3 previous;
	private bool rumble;
	private NavMeshAgent agent;
	private GameControllerScript gc;
	public GameObject alarmClock;
}
