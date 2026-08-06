using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshAgent))]
public class BaldiScript : MonoBehaviour
{
	private void Awake()
	{
		gc = FindObjectOfType<GameControllerScript>();
	}

	private void Start()
	{
		this.baldiAudio = GetComponent<AudioSource>();
		this.agent = GetComponent<NavMeshAgent>();
		this.baseTime = 3f;
		this.timeToMove = this.baseTime;
		this.Wander();

		this.ClearSoundList();

		if (!this.isSpeedFactorSet)
		{
			if (speedFactorOverride <= 0f)
				this.SetSpeedFactor();
        	else
			{
				this.baldiSpeedScale = this.speedFactorOverride;
				this.isSpeedFactorSet = true;
			}
		}

		if (gc.isSafeMode)
		{
			this.baldiAnimator.SetTrigger("ghostSlap");
			this.niceMode = true;
			StartCoroutine(this.DelayedSpeechStart());
			this.ResetSpeechTimer();
		}
		
		if (gc.modeType == "nullStyle")
		{
			this.isNullMode = true;
			this.agent.SetDestination(this.player.position);
			this.ResetSpeechTimer();
		}
	}

	private void Update()
	{
		if (this.timeToMove > 0f)
			this.timeToMove -= 1f * Time.deltaTime;

		else
		{
			if (this.baldiAnger == 0)
				this.timeToMove = 3f;
			else
				this.timeToMove = this.baldiWait - this.baldiTempAnger;
			this.Move();
		}

		if (this.coolDown > 0f)
			this.coolDown -= 1f * Time.deltaTime;

		if (this.baldiTempAnger > 0f)
			this.baldiTempAnger -= 0.02f * Time.deltaTime;
		else
			this.baldiTempAnger = 0f;

		if (this.antiHearingTime > 0f)
			this.antiHearingTime -= Time.deltaTime;
		else
			this.antiHearing = false;

		if (this.endless)
		{
			if (this.timeToAnger > 0f)
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
		
		if (this.isNullMode || this.niceMode)
			this.speechTimer -= Time.deltaTime;

		if (this.isNullMode && this.db && this.playerScript.stamina <= 0f && gc.IsNoItems()
		&& this.baldiAnger >= 5 && this.speechTimer < 61f && !this.longAudioDevice.isPlaying)
		{
			this.StartCoroutine(this.NullSight());
			this.longAudioDevice.PlayOneShot(this.randomSpeechList[5]);
		}

		if (this.speechTimer < 0f)
		{
			if (this.isNullMode)
			{
				if (!this.db && this.currentPriority == 0)
				{
					this.ResetSpeechTimer();
					this.longAudioDevice.PlayOneShot(this.randomSpeechList[6]);
				}
				else
				{
					this.ResetSpeechTimer();
					this.baldiAudio.PlayOneShot(this.randomSpeechList[this.RandomSpeech()]);
				}
			}
			else if (this.niceMode && !this.db && this.currentPriority == 0)
			{
				this.ResetSpeechTimer();
				this.longAudioDevice.PlayOneShot(this.randomSpeechList[3]);
			}
		}
	}

	private void ResetSpeechTimer()
	{
		this.speechTimer = Random.Range(35f, 75f);
	}

	private int RandomSpeech()
	{
		return Mathf.RoundToInt(Random.Range(0f, 4f));
	}

	IEnumerator DelayedSpeechStart()
	{
		float tempTimer = 0.2f;
		while (tempTimer > 0f)
		{
			tempTimer -= Time.deltaTime;
			yield return null;
		}
		this.baldiAudio.PlayOneShot(this.randomSpeechList[0]);
	}

	private IEnumerator NullSight()
	{
		while (this.db)
		{
			this.speechTimer = 69f;
			yield return null;
		}
	}

	private void OnDisable()
	{
		if (this.isNullMode && !this.isDisabled && challengeController != null)
			challengeController.EnableAllWindowBlockers();
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
			Debug.DrawLine(base.transform.position, raycastHit.transform.position, Color.cyan);
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
			baldicator.ChangeBaldicatorState("Sight");
	}

	private void Move()
	{
		Vector3 offset = this.agent.destination - base.transform.position;
		float sqrLen = offset.sqrMagnitude;

		if (sqrLen < 4f && this.coolDown < 0f)
			this.DecreasePriority();

		this.moveFrames = 10f;
		this.baldiAudio.PlayOneShot(this.slap); //Play the slap sound

		if (!this.isNullMode)
		{
			if (this.gc.isSafeMode)
				this.baldiAnimator.SetTrigger("ghostSlap");
			else
				this.baldiAnimator.SetTrigger("slap");
		}
	}

	public void GetAngry(float value)
	{
		this.baldiAnger += value;

		if (this.baldiAnger < 0.5f)
			this.baldiAnger = 0.5f;
		
		if (!this.isSpeedFactorSet)
			this.SetSpeedFactor();
		
		this.baldiWait = -3f * this.baldiAnger / (this.baldiAnger + this.baldiSpeedScale / 0.65f) + 3f;
		
		if (this.baldiWait <= 0f)
			this.baldiWait = 3f;
	}

	void SetSpeedFactor()
	{
		float nb = gc.daFinalBookCount;
        float speedFactor = 0.2f * nb * nb / nb + 0.6f;
        this.baldiSpeedScale = Mathf.Clamp(speedFactor, 1f, Mathf.Infinity);
		this.isSpeedFactorSet = true;
	}

	public void GetTempAngry(float value)
	{
		this.baldiTempAnger += value; //Increase Baldi's Temporary Anger
	}

	public void WarpToCrafterPoint(Vector3 point)
    {
        this.agent.Warp(point);
    }

	public void ActivateAntiHearing(float t)
	{
		this.ClearSoundList();
		this.DecreasePriority();
		this.Wander();
		this.antiHearing = true;
		this.antiHearingTime = t;
	}

	public void NullOffset()
	{
		this.isDisabled = true;
		this.agent.enabled = false;
		Transform baldiSprite = base.transform.Find("BaldiSprite");
		Vector3 curPosition = baldiSprite.position;
		baldiSprite.position = new Vector3(curPosition.x, curPosition.y - 0.5f, curPosition.z);
	}

	public void AddNewSound(Vector3 location, int priority)
	/*
	1 = Door
	2 = Bad Math
	3 = Detention, window breaking
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

		if (!this.allowWindowBreaking && this.currentPriority > 1 && gc.gameMode == GameControllerScript.GameMode.Challenge)
			this.gc.StartCoroutine(this.challengeController.ToggleWindowBlockers());

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
		if (!this.allowWindowBreaking && this.isNullMode)
			gc.StartCoroutine(this.challengeController.ToggleWindowBlockers());

		while (this.db && !this.isDisabled)
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
		this.AddNewSound(gc.partyLocation.position, 5);
		this.isParty = true;

		while (!this.db && this.isParty)
			yield return null;

		this.isParty = false;
	}

	[Header("Priority System")]
	public int currentPriority;
	[SerializeField] Vector3[] soundList;
	[SerializeField] Baldicator baldicator;
	[SerializeField] float sightCooldown;
	[SerializeField] Vector3 theNewLocation;
	public bool isAlarmClock;
	public bool isParty;

	[Header("Null Modifications")]
	[SerializeField] ChallengeController challengeController;
	[SerializeField] bool isNullMode;
	public bool allowWindowBreaking;
	[SerializeField] AudioClip[] randomSpeechList;
	[SerializeField] AudioSource longAudioDevice;
	[SerializeField] float speechTimer;

	[Header("Endless Mode")]
	public bool endless;
	[SerializeField] float angerRate;
	[SerializeField] float angerRateRate;
	[SerializeField] float angerFrequency;
	[SerializeField] float timeToAnger;

	[Header("Other stuff")]
	[SerializeField] bool niceMode;

	[Space(20f)]
	[SerializeField] bool db;
	float baseTime;
	[SerializeField] float speed;
	float timeToMove;
	float baldiAnger;
	float baldiTempAnger;
	float baldiWait;
	float baldiSpeedScale;
	bool isSpeedFactorSet;

	[Tooltip("If this value is set to 0, then the total noteboo count will be used to calculate the speed factor.")]
	[SerializeField] float speedFactorOverride;
	float moveFrames;
	public bool antiHearing;
	public float antiHearingTime;
	[SerializeField] bool isDisabled;
	[SerializeField] Transform player;
	[SerializeField] PlayerScript playerScript;
	[SerializeField] AILocationSelectorScript wanderer;
	AudioSource baldiAudio;
	[SerializeField] AudioClip slap;
	[SerializeField] Animator baldiAnimator;
	float coolDown;
	[SerializeField] Vector3 previous;
	NavMeshAgent agent;
	GameControllerScript gc;
	public GameObject alarmClock;
}
