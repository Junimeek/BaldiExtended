﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class SweepScript : MonoBehaviour
{
	private void Start()
	{
		this.origin = base.transform.position;
		this.gc = FindObjectOfType<GameControllerScript>();
	}
	private void OnEnable()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.waitTime = UnityEngine.Random.Range(120f, 180f);
	}

	private void Update()
	{
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.waitTime > 0f)
		{
			this.waitTime -= Time.deltaTime;
		}
		else if (!this.active)
		{
			this.active = true;
			this.wanders = 0;
			this.Wander(); // Start wandering
			this.audioDevice.PlayOneShot(this.aud_Intro); // "Looks like its sweeping time!"
		}
	}

	private void FixedUpdate()
	{
		if ((double)this.agent.velocity.magnitude <= 0.1 & this.coolDown <= 0f & this.wanders < 5 & this.active) // If Gotta Sweep has roamed around the school 5 times
		{
			this.Wander(); // Wander
		}
		else if (this.wanders >= 5)
		{
			this.GoHome(); // Go back to the closet
		}
	}

	private void Wander()
	{
		this.agent.SetDestination(this.wanderer.NewTarget("Hallway"));
		this.coolDown = 1f;
		this.wanders++;
	}

	public void GoHome()
	{
		this.agent.SetDestination(this.origin);
		this.waitTime = UnityEngine.Random.Range(120f, 180f);
		this.wanders = 0;
		this.active = false;
	}

	public void GoToAttendance()
	{
		this.waitTime = 199f;
		this.agent.SetDestination(gc.attendanceOffice.position);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC" || other.tag == "Player")
		{
			this.audioDevice.PlayOneShot(this.aud_Sweep);
		}
	}

	public AILocationSelectorScript wanderer;
	public float coolDown;
	public float waitTime;
	public int wanders;
	public bool active;
	private Vector3 origin;
	public AudioClip aud_Sweep;
	public AudioClip aud_Intro;
	private NavMeshAgent agent;
	private AudioSource audioDevice;
	private GameControllerScript gc;
}
