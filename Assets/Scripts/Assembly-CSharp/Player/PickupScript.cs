﻿using System;
//using Rewired;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class PickupScript : MonoBehaviour
{
	private void Start()
	{
		//this.playerInput = ReInput.players.GetPlayer(0);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit))
			{
				if (raycastHit.transform.name == "Pickup_EnergyFlavoredZestyBar" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(1);
				}
				else if (raycastHit.transform.name == "Pickup_YellowDoorLock" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(2);
				}
				else if (raycastHit.transform.name == "Pickup_Key" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(3);
				}
				else if (raycastHit.transform.name == "Pickup_BSODA" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(4);
				}
				else if (raycastHit.transform.name == "Pickup_Quarter" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(5);
				}
				else if (raycastHit.transform.name == "Pickup_Tape" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(6);
				}
				else if (raycastHit.transform.name == "Pickup_AlarmClock" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(7);
				}
				else if (raycastHit.transform.name == "Pickup_WD-3D" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(8);
				}
				else if (raycastHit.transform.name == "Pickup_SafetyScissors" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(9);
				}
				else if (raycastHit.transform.name == "Pickup_BigBoots" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(10);
				}
				else if (raycastHit.transform.name == "Pickup_SpeedySneakers" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(11);
				}
				else if (raycastHit.transform.name == "Pickup_AttendanceSlip" & Vector3.Distance(this.player.position, base.transform.position) < 10f)
				{
					raycastHit.transform.gameObject.SetActive(false);
					this.gc.CollectItem(12);
				}
			}
		}
	}

	public GameControllerScript gc;
	public Transform player;
	//private Player playerInput;
}
