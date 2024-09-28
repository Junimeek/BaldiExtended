using System;
//using Rewired;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PickupScript : MonoBehaviour
{
	private void Start()
	{
		//this.playerInput = ReInput.players.GetPlayer(0);
	}

	public void ChangeItem(int newItem)
	{
		switch(newItem)
		{
			case 0:
				base.gameObject.SetActive(false);
				break;
			case 1:
				this.itemPickup = pickup.ZestyBar;
				break;
			case 2:
				this.itemPickup = pickup.DoorLock;
				break;
			case 3:
				this.itemPickup = pickup.PrincipalKeys;
				break;
			case 4:
				this.itemPickup = pickup.BSODA;
				break;
			case 5:
				this.itemPickup = pickup.Quarter;
				break;
			case 6:
				this.itemPickup = pickup.Tape;
				break;
			case 7:
				this.itemPickup = pickup.AlarmClock;
				break;
			case 8:
				this.itemPickup = pickup.NoSquee;
				break;
			case 9:
				this.itemPickup = pickup.SafetyScissors;
				break;
			case 10:
				this.itemPickup = pickup.Boots;
				break;
			case 11:
				this.itemPickup = pickup.SpeedySneakers;
				break;
			case 12:
				this.itemPickup = pickup.AttendanceSlip;
				break;
			case 13:
				this.itemPickup = pickup.DietBSODA;
				break;
			case 14:
				this.itemPickup = pickup.CrystalZesty;
				break;
			case 15:
				this.itemPickup = pickup.PartyPopper;
				break;
			case 16:
				this.itemPickup = pickup.DollarBill;
				break;
			case 17:
				this.itemPickup = pickup.Hammer;
				break;
		}

		this.itemSprite.sprite = this.gc.pickup_itemSprites[newItem];
		this.mapSprite = this.gc.pickup_itemMapSprites[newItem];
		this.mapIcon.sprite = this.mapSprite;
	}

	public int ItemID()
	{
		switch(this.itemPickup)
		{
			case pickup.ZestyBar:
				return 1;
			case pickup.DoorLock:
				return 2;
			case pickup.PrincipalKeys:
				return 3;
			case pickup.BSODA:
				return 4;
			case pickup.Quarter:
				return 5;
			case pickup.Tape:
				return 6;
			case pickup.AlarmClock:
				return 7;
			case pickup.NoSquee:
				return 8;
			case pickup.SafetyScissors:
				return 9;
			case pickup.Boots:
				return 10;
			case pickup.SpeedySneakers:
				return 11;
			case pickup.AttendanceSlip:
				return 12;
			case pickup.DietBSODA:
				return 13;
			case pickup.CrystalZesty:
				return 14;
			case pickup.PartyPopper:
				return 15;
			case pickup.DollarBill:
				return 16;
			case pickup.Hammer:
				return 17;
			default:
				return 5;
		}
	}

	public GameControllerScript gc;
	public Transform player;
	public SpriteRenderer mapIcon;
	public Sprite mapSprite;
	[SerializeField] private SpriteRenderer itemSprite;
	public pickup itemPickup;
	public enum pickup
	{
		ZestyBar, DoorLock, PrincipalKeys, BSODA, Quarter, Tape, AlarmClock,
		NoSquee, SafetyScissors, Boots, SpeedySneakers, AttendanceSlip, DietBSODA,
		CrystalZesty, PartyPopper, DollarBill, Hammer
	}
	//private Player playerInput;
}
