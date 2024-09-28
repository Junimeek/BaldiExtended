using System;
using UnityEngine;
using UnityEngine.AI;

public class CraftersScript : MonoBehaviour
{
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>(); // Defines the nav mesh agent
		this.audioDevice = base.GetComponent<AudioSource>(); //Gets the audio source
		this.sprite.SetActive(false); // Set arts and crafters sprite to be invisible
	}

	private void Update()
	{
		if (this.forceShowTime > 0f && !this.isParty)
			this.forceShowTime -= Time.deltaTime;
		else if (this.isParty)
			this.forceShowTime = 1f;

		if (this.gettingAngry) //If arts is getting agry
		{
			this.anger += Time.deltaTime; // Increase anger
			if (this.anger >= 1f & !this.angry) //If anger is greater then 1 and arts isn't angry
			{
				this.angry = true; // Get angry
				this.audioDevice.PlayOneShot(this.aud_Intro); // Do the woooosoh sound
				this.spriteImage.sprite = this.angrySprite; // Switch to the angry sprite
			}
		}
		else if (this.anger > 0f) // If anger is greater then 0, decrease.
		{
			this.anger -= Time.deltaTime;
		}
		if (!this.angry) // If not angry
		{
			if (((base.transform.position - this.agent.destination).magnitude <= 20f & (base.transform.position - this.player.position).magnitude >= 60f) || this.forceShowTime > 0f) //If close to the player and force showtime is less then 0
			{
				this.sprite.SetActive(true); // Become visible
			}
			else
			{
				this.sprite.SetActive(false); // Become invisible
			}
		}
		else
		{
			this.agent.speed = this.agent.speed + 60f * Time.deltaTime; // Increase the speed
			this.TargetPlayer(); // Target the player
			if (!this.audioDevice.isPlaying) //If the sound is not already playing
			{
				this.audioDevice.PlayOneShot(this.aud_Loop); //Play the full wooooosh sound
			}
		}
	}

	private void FixedUpdate()
	{
		if (this.gc.notebooks >= this.gc.daFinalBookCount) // If the player has more then 7 notebooks
		{
			Vector3 direction = this.player.position - base.transform.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore)
			&& raycastHit.transform.tag == "Player" && this.craftersRenderer.isVisible && this.sprite.activeSelf && !this.isParty) // If Arts is Visible, and active and sees the player
			{
				this.gettingAngry = true; // Start to get angry
			}
			else
			{
				this.gettingAngry = false; // Stop getting angry
			}
		}
	}

	public void GiveLocation(Vector3 location, bool flee)
	{
		if (!this.angry && this.agent.isActiveAndEnabled && !this.isParty)
		{
			this.agent.SetDestination(location);
			if (flee)
			{
				this.forceShowTime = 3f; // Make arts appear in 3 seconds
			}
		}
		else if (this.isParty && this.agent.isActiveAndEnabled)
		{
			this.agent.SetDestination(this.wanderer.NewTarget("Party"));
		}
	}

	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position); // Set destination to the player
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" & this.angry) // If arts is angry and is touching the player
		{
			this.cc.enabled = false;
			this.player.position = new Vector3(this.playerTeleLocation.x, this.player.position.y, this.playerTeleLocation.z); // Teleport the player to X: 5, their current Y position, Z: 80
			this.baldiAgent.Warp(new Vector3(this.baldiTeleLocation.x, this.baldi.position.y, this.baldiTeleLocation.z)); // Teleport Baldi to X: 5, baldi's Y, Z: 125
			this.playerScript.LookAtCharacter("baldi"); // Make the player look at baldi
			this.cc.enabled = true;
			this.gc.DespawnCrafters(); // Despawn Arts And Crafters
		}
	}

	public bool db;
	public bool angry;
	public bool gettingAngry;
	public float anger;
	private float forceShowTime;
	public Transform player;
	[SerializeField] private PlayerScript playerScript;
	public CharacterController cc;
	public Transform playerCamera;
	public Transform baldi;
	public NavMeshAgent baldiAgent;
	public GameObject sprite;
	public GameControllerScript gc;
	[SerializeField] private NavMeshAgent agent;
	public Renderer craftersRenderer;
	public SpriteRenderer spriteImage;
	public Sprite angrySprite;
	private AudioSource audioDevice;
	public AudioClip aud_Intro;
	public AudioClip aud_Loop;
	[SerializeField] private Vector3 playerTeleLocation;
	[SerializeField] private Vector3 baldiTeleLocation;
	[SerializeField] private AILocationSelectorScript wanderer;
	public bool isParty;
}
