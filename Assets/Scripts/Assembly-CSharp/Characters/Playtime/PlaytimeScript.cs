using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlaytimeScript : MonoBehaviour
{
	private void OnEnable()
	{
		this.isDisabled = false;
		this.gc = FindObjectOfType<GameControllerScript>();
		this.agent = base.GetComponent<NavMeshAgent>(); //Get AI Agent
		this.agent.speed = 15f;
		this.coolDown = 0f;
		this.audioDevice = base.GetComponent<AudioSource>();
		this.music.loop = true;
		this.music.Play();
		this.Wander(); //Start wandering
	}

	private void Update()
	{
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.playCool >= 0f)
		{
			this.playCool -= Time.deltaTime;
		}
		else if (this.animator.GetBool("disappointed"))
		{
			this.playCool = 0f;
			this.animator.SetBool("disappointed", false);
		}
	}

	private void FixedUpdate()
	{
		if (!this.ps.jumpRope)
		{
			Vector3 direction = this.player.position - base.transform.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore)
			& raycastHit.transform.tag == "Player" & (base.transform.position - this.player.position).magnitude <= 80f & this.playCool <= 0f
			&& !this.isParty)
			{
				this.playerSeen = true; //If playtime sees the player, she chases after them
				this.TargetPlayer();
			}
			else if (this.playerSeen & this.coolDown <= 0f)
			{
				this.playerSeen = false; //If the player seen cooldown expires, she will just start wandering again
				this.Wander();
			}
			else if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
			{
				this.Wander();
			}
			this.jumpRopeStarted = false;
		}
		else
		{
			if (!this.jumpRopeStarted)
			{
				this.agent.Warp(base.transform.position - base.transform.forward * 10f); //Teleport back after touching the player
			}
			this.jumpRopeStarted = true;
			this.agent.speed = 0f;
			this.playCool = 15f;
		}
	}

	private void Wander()
	{
		if (this.isDisabled) return;
		if (this.isParty)
			this.agent.SetDestination(this.wanderer.NewTarget("Party"));
		else
			this.agent.SetDestination(this.wanderer.NewTarget("Hallway"));
		this.agent.speed = 15f;
		this.playerSpotted = false;
		this.audVal = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
		if (!this.audioDevice.isPlaying && !this.isParty)
		{
			this.audioDevice.PlayOneShot(this.aud_Random[this.audVal]);
		}
		this.coolDown = 1f;
	}

	public void GoToParty()
	{
		this.isParty = true;
		this.music.loop = false;
		this.music.Stop();
		this.agent.SetDestination(this.gc.partyLocation.position);
	}

	public void LeaveParty()
	{
		this.playCool = 15f;
		this.isParty = false;
		this.music.loop = true;
		this.music.Play();
		this.Wander();
	}

	private void TargetPlayer()
	{
		this.animator.SetBool("disappointed", false); //No longer be sad
		if (this.isDisabled) return;
		this.agent.SetDestination(this.player.position); // Go after the player
		this.agent.speed = 20f; // Speed up
		this.coolDown = 0.2f;
		if (!this.playerSpotted)
		{
			this.playerSpotted = true;
			this.audioDevice.PlayOneShot(this.aud_LetsPlay);
		}
	}

	public void Disappoint()
	{
		this.animator.SetBool("disappointed", true); //Get sad
		this.audioDevice.Stop();
		this.audioDevice.PlayOneShot(this.aud_Sad);
	}

	public void GoToAttendance()
	{
		this.isDisabled = true;
		this.animator.SetBool("disappointed", false);
		this.agent.speed = 30f;
		this.agent.SetDestination(gc.attendanceOffice.position);
	}

	public bool db;
	public bool playerSeen;
	public bool disappointed;
	public int audVal;
	public Animator animator;
	public Transform player;
	public PlayerScript ps;
	public AILocationSelectorScript wanderer;
	[SerializeField] private GameControllerScript gc;
	public float coolDown;
	public float playCool;
	public bool playerSpotted;
	public bool jumpRopeStarted;
	private NavMeshAgent agent;
	public AudioClip[] aud_Numbers = new AudioClip[10];
	public AudioClip[] aud_Random = new AudioClip[2];
	public AudioClip aud_Instrcutions;
	public AudioClip aud_Oops;
	public AudioClip aud_LetsPlay;
	public AudioClip aud_Congrats;
	public AudioClip aud_ReadyGo;
	public AudioClip aud_Sad;
	public AudioSource audioDevice;
	public bool isDisabled;
	public bool isParty;
	[SerializeField] private AudioSource music;
}
