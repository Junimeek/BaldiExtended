using UnityEngine;

public class QuarterSpawnScript : MonoBehaviour
{
	private void Start()
	{
		switch(this.pickupTargetLocation)
		{
			case PickupType.Hallway:
				base.transform.position = this.wanderer.GetNewItemTarget(AILocationSelectorScript.ItemTargetType.Hallway) + Vector3.up * 4f;
				break;
			case PickupType.Classroom:
				base.transform.position = this.wanderer.GetNewItemTarget(AILocationSelectorScript.ItemTargetType.Classroom) + Vector3.up * 4f;
				break;
			case PickupType.FacultyRoom:
				base.transform.position = this.wanderer.GetNewItemTarget(AILocationSelectorScript.ItemTargetType.FacultyRoom) + Vector3.up * 4f;
				break;
		}
	}

	public AILocationSelectorScript wanderer;
	enum PickupType
	{
		Hallway, Classroom, FacultyRoom
	}
	[SerializeField] PickupType pickupTargetLocation;
}
