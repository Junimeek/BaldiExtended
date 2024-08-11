﻿using System;
//using Rewired;
using UnityEngine;

public class NotebookScript : MonoBehaviour
{
	private void Start()
	{
		//this.playerInput = ReInput.players.GetPlayer(0);
		this.up = true;
		notif = FindObjectOfType<NotificationBoard>();
	}

	private void Update()
	{
		if (this.gc.mode == "endless")
		{
			if (this.respawnTime > 0f && (base.transform.position - this.player.position).magnitude > 60f)
				this.respawnTime -= Time.deltaTime;
			else if (!this.up)
			{
				base.transform.position = new Vector3(base.transform.position.x, 4f, base.transform.position.z);
				this.up = true;
				this.audioDevice.Play();
				notif.StartNotebooRoutine(this.gameObject.name);
			}
		}
		if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && (raycastHit.transform.tag == "Notebook" & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance))
			{
				base.transform.position = new Vector3(base.transform.position.x, 200f, base.transform.position.z);
				this.up = false;
				this.respawnTime = 120f;
				this.gc.CollectNotebook();
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.learningGame);
				gameObject.GetComponent<MathGameScript>().gc = this.gc;
				gameObject.GetComponent<MathGameScript>().baldiScript = this.bsc;
				gameObject.GetComponent<MathGameScript>().playerPosition = this.player.position;
			}
		}
	}

	public float openingDistance;
	public GameControllerScript gc;
	public BaldiScript bsc;
	public float respawnTime;
	public bool up;
	public Transform player;
	public GameObject learningGame;
	public AudioSource audioDevice;
	//private Player playerInput;
	[SerializeField] private NotificationBoard notif;
}
