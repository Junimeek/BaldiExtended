using UnityEngine;

public class AILocationSelectorScript : MonoBehaviour
{
	public Vector3 NewTarget(string type)
	{
		int randomID;
		int randomID2;
		Vector3 newLocation;

		switch(type)
		{
			case "Quarter":
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, quarterPoints.Length-1));
				newLocation = this.quarterPoints[randomID].position;
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
		}

		this.ambience.PlayAudio();
		return newLocation;
	}
	public AmbienceScript ambience;
	public Transform[] bullyPoints;
	public Transform[] quarterPoints;
	public Transform[] hallwayPoints;
	public Transform[] roomPoints;
}
