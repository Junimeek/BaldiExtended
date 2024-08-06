using System;
//using Rewired;
using UnityEngine;

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
			if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(this.player.position, base.transform.position) < 10f && raycastHit.transform.name.StartsWith("Pickup"))
			{
				GameObject curObject = raycastHit.transform.gameObject;
				switch(this.itemPickup)
				{
					case pickup.ZestyBar:
						curObject.SetActive(false);
						this.gc.CollectItem(1);
						break;
					case pickup.DoorLock:
						curObject.SetActive(false);
						this.gc.CollectItem(2);
						break;
					case pickup.PrincipalKeys:
						curObject.SetActive(false);
						this.gc.CollectItem(3);
						break;
					case pickup.BSODA:
						curObject.SetActive(false);
						this.gc.CollectItem(4);
						break;
					case pickup.Quarter:
						curObject.SetActive(false);
						this.gc.CollectItem(5);
						break;
					case pickup.Tape:
						curObject.SetActive(false);
						this.gc.CollectItem(6);
						break;
					case pickup.AlarmClock:
						curObject.SetActive(false);
						this.gc.CollectItem(7);
						break;
					case pickup.NoSquee:
						curObject.SetActive(false);
						this.gc.CollectItem(8);
						break;
					case pickup.SafetyScissors:
						curObject.SetActive(false);
						this.gc.CollectItem(9);
						break;
					case pickup.Boots:
						curObject.SetActive(false);
						this.gc.CollectItem(10);
						break;
					case pickup.SpeedySneakers:
						curObject.SetActive(false);
						this.gc.CollectItem(11);
						break;
					case pickup.AttendanceSlip:
						curObject.SetActive(false);
						this.gc.CollectItem(12);
						break;
					case pickup.DietBSODA:
						curObject.SetActive(false);
						this.gc.CollectItem(13);
						break;
					case pickup.CrystalZesty:
						curObject.SetActive(false);
						this.gc.CollectItem(14);
						break;
					case pickup.PartyPopper:
						curObject.SetActive(false);
						this.gc.CollectItem(15);
						break;
				}
			}
		}
	}

	public GameControllerScript gc;
	public Transform player;
	public SpriteRenderer mapIcon;
	public Sprite mapSprite;
	public pickup itemPickup;
	public enum pickup
	{
		ZestyBar, DoorLock, PrincipalKeys, BSODA, Quarter, Tape, AlarmClock,
		NoSquee, SafetyScissors, Boots, SpeedySneakers, AttendanceSlip, DietBSODA,
		CrystalZesty, PartyPopper
	}
	//private Player playerInput;
}
