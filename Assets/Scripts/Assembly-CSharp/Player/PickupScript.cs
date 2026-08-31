using UnityEngine;

public class PickupScript : MonoBehaviour
{
	void Start()
	{
		switch (this.randomizeItem)
		{
			case RandomSetting.AnyItem:
				this.ChangeItem(this.GetRandomItem());
				break;
			case RandomSetting.CommonItem:
				this.ChangeItem(this.GetRandomCommonItem());
				break;
			case RandomSetting.StrongItem:
				this.ChangeItem(this.GetRandomStrongItem());
				break;
		}
		this.mapIcon.sprite = gc.mysteryItemSprite;
	}

	Pickup GetRandomItem()
	{
		int randy = Mathf.RoundToInt(Random.Range(1f, 17f));
		return this.GetNewItemPickupByID(randy);
	}

	Pickup GetRandomCommonItem()
	{
		int randy = Mathf.RoundToInt(Random.Range(0f, 7f));
        int newItemID = randy switch
        {
            0 => 1,
            1 => 5,
			2 => 8,
			3 => 9,
			4 => 10,
			5 => 13,
			6 => 14,
			7 => 17,
            _ => 5
        };
        return this.GetNewItemPickupByID(newItemID);
	}

	Pickup GetRandomStrongItem()
	{
		int randy = Mathf.RoundToInt(Random.Range(0f, 9f));
        int newItemID = randy switch
        {
            0 => 1,
			1 => 2,
			2 => 3,
			3 => 4,
			4 => 6,
			5 => 7,
			6 => 11,
			7 => 12,
			8 => 15,
			9 => 16,
            _ => 1
        };
        return this.GetNewItemPickupByID(newItemID);
	}

	public void ChangeItem(int newItemID)
	{
		if (newItemID == 0)
			base.gameObject.SetActive(false);
		else
			this.itemPickup = this.GetNewItemPickupByID(newItemID);

		this.SetItemSprites(newItemID);
	}

	void ChangeItem(Pickup newPickup)
	{
		this.itemPickup = newPickup;
		this.SetItemSprites(GetItemID());
	}

	void SetItemSprites(int newItemID)
	{
		this.itemSprite.sprite = gc.GetNewItemSprite(newItemID);
		this.mapSprite = gc.GetNewItemMapSprite(newItemID);
		this.mapIcon.sprite = this.mapSprite;
	}

	public int GetItemID()
	{
        return this.itemPickup switch
        {
            Pickup.ZestyBar => 1,
            Pickup.DoorLock => 2,
            Pickup.PrincipalKeys => 3,
            Pickup.BSODA => 4,
            Pickup.Quarter => 5,
            Pickup.Tape => 6,
            Pickup.AlarmClock => 7,
            Pickup.NoSquee => 8,
            Pickup.SafetyScissors => 9,
            Pickup.Boots => 10,
            Pickup.SpeedySneakers => 11,
            Pickup.AttendanceSlip => 12,
            Pickup.DietBSODA => 13,
            Pickup.CrystalZesty => 14,
            Pickup.PartyPopper => 15,
            Pickup.DollarBill => 16,
            Pickup.Hammer => 17,
            _ => 5,
        };
    }

	Pickup GetNewItemPickupByID(int itemID)
	{
		return itemID switch
		{
			1 => Pickup.ZestyBar,
			2 => Pickup.DoorLock,
			3 => Pickup.PrincipalKeys,
			4 => Pickup.BSODA,
			5 => Pickup.Quarter,
			6 => Pickup.Tape,
			7 => Pickup.AlarmClock,
			8 => Pickup.NoSquee,
			9 => Pickup.SafetyScissors,
			10 => Pickup.Boots,
			11 => Pickup.SpeedySneakers,
			12 => Pickup.AttendanceSlip,
			13 => Pickup.DietBSODA,
			14 => Pickup.CrystalZesty,
			15 => Pickup.PartyPopper,
			16 => Pickup.DollarBill,
			17 => Pickup.Hammer,
			_ => Pickup.Quarter
		};
	}

	public GameControllerScript gc;
	public Transform player;
	public SpriteRenderer mapIcon;
	public Sprite mapSprite;
	[SerializeField] SpriteRenderer itemSprite;
	public enum Pickup
	{
		ZestyBar, DoorLock, PrincipalKeys, BSODA, Quarter, Tape, AlarmClock,
		NoSquee, SafetyScissors, Boots, SpeedySneakers, AttendanceSlip, DietBSODA,
		CrystalZesty, PartyPopper, DollarBill, Hammer
	}
	public Pickup itemPickup;
	enum RandomSetting
	{
		DontRandomize, AnyItem, CommonItem, StrongItem
	}
	[SerializeField] RandomSetting randomizeItem;
}
