using System;
using UnityEngine;
using UnityEngine.XR.WSA;

public class AILocationSelectorScript : MonoBehaviour
{
	public Vector3 NewTarget(string type)
	{
		//if (!this.LocationCheck()) this.InitializeWanderPoints();

		int randomID;
		int randomID2;
		Vector3 newLocation;

		switch(type)
		{
			case "Quarter":
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, quarterPoints.Length-1));
				newLocation = this.quarterPoints[randomID].position;
				break;
			case "RoomQuarter":
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, roomQuarterPoints.Length-1));
				newLocation = this.roomQuarterPoints[randomID].position;
				break;
			case "Bully":
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, bullyPoints.Length-1));
				newLocation = this.bullyPoints[randomID].position;
				break;
			case "Hallway":
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, hallwayPoints.Length-1));
				newLocation = this.hallwayPoints[randomID].position;
				break;
			default:
				randomID2 = Mathf.RoundToInt(UnityEngine.Random.Range(1f, 2f));
				switch(randomID2)
				{
					case 1:
						randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, hallwayPoints.Length-1));
						newLocation = this.hallwayPoints[randomID].position;
						break;
					default:
						randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, roomPoints.Length-1));
						newLocation = this.roomPoints[randomID].position;
						break;
				}
				break;
			case "Attendance":
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, attendancePoints.Length-1));
				newLocation = this.attendancePoints[randomID].position;
				break;
			case "Party":
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, partyPoints.Length-1));
				newLocation = this.partyPoints[randomID].position;
				break;
			case "Projectile":
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, projectilePoints.Length-1));
				newLocation = this.projectilePoints[randomID].position;
				break;
		}

		this.ambience.PlayAudio();
		return newLocation;
	}

	private bool LocationCheck()
	{
		if (!(this.bullyPoints.Length > 0 /*&& this.quarterPoints.Length > 0 && this.hallwayPoints.Length > 0
		&& this.roomPoints.Length > 0 && this.attendancePoints.Length > 0*/))
			return true;
		else
		{
			Debug.Log(false);
			return false;
		}
			
	}

	private void InitializeWanderPoints()
	{
		Transform[] fetchedPoints = bullyParent.GetComponentsInChildren<Transform>();
		Array.Resize(ref this.bullyPoints, fetchedPoints.Length);
		for (int i = 0; i < this.bullyPoints.Length; i++)
			this.bullyPoints[i] = fetchedPoints[i];

		/*
		fetchedPoints = quarterParent.GetComponentsInChildren<Transform>();
		Array.Resize(ref this.quarterPoints, fetchedPoints.Length);
		for (int i = 0; i < this.quarterPoints.Length; i++)
			this.quarterPoints[i] = fetchedPoints[i];
		*/

		fetchedPoints = hallwayParent.GetComponentsInChildren<Transform>();
		Array.Resize(ref this.hallwayPoints, fetchedPoints.Length);
		for (int i = 0; i < this.hallwayPoints.Length; i++)
			this.hallwayPoints[i] = fetchedPoints[i];

		fetchedPoints = roomParent.GetComponentsInChildren<Transform>();
		Array.Resize(ref this.roomPoints, fetchedPoints.Length);
		for (int i = 0; i < this.roomPoints.Length; i++)
			this.roomPoints[i] = fetchedPoints[i];

		fetchedPoints = attendanceParent.GetComponentsInChildren<Transform>();
		Array.Resize(ref this.attendancePoints, fetchedPoints.Length);
		for (int i = 0; i < this.attendancePoints.Length; i++)
			this.attendancePoints[i] = fetchedPoints[i];
	}

	public AmbienceScript ambience;
	[SerializeField] private GameObject bullyParent;
	[SerializeField] private GameObject quarterParent;
	[SerializeField] private GameObject hallwayParent;
	[SerializeField] private GameObject roomParent;
	[SerializeField] private GameObject attendanceParent;
	public Transform[] bullyPoints;
	public Transform[] quarterPoints;
	public Transform[] roomQuarterPoints;
	public Transform[] hallwayPoints;
	public Transform[] roomPoints;
	public Transform[] attendancePoints;
	public Transform[] partyPoints;
	public Transform[] movingPartyPoints;
	public Transform[] projectilePoints;
}
