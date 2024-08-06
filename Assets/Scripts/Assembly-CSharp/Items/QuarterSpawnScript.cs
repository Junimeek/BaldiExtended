using System;
using UnityEngine;

public class QuarterSpawnScript : MonoBehaviour
{
	private void Start()
	{
		if (base.gameObject.name == "Pickup_Quarter")
			base.transform.position = this.wanderer.NewTarget("Quarter") + Vector3.up * 4f;
		else if (base.gameObject.name == "Pickup_RoomQuarter")
			base.transform.position = this.wanderer.NewTarget("RoomQuarter") + Vector3.up * 4f;
		else if (base.gameObject.name == "Pickup_AttendanceSlip")
			base.transform.position = this.wanderer.NewTarget("Attendance") + Vector3.up * 4f;
	}

	public AILocationSelectorScript wanderer;
}
