using UnityEngine;

public class AILocationSelectorScript : MonoBehaviour
{
	public Vector3 GetNewNPCTarget(NPCTargetType type)
	{
		int randomID;
		int randomID2;
		Vector3 newLocation;

		switch(type)
		{
			case NPCTargetType.Hallways:
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, hallwayPoints.Length-1));
				newLocation = this.hallwayPoints[randomID].position;
				break;
			case NPCTargetType.Bully:
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, bullyPoints.Length-1));
				newLocation = this.bullyPoints[randomID].position;
				break;
			case NPCTargetType.PartyWanderPoints:
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, partyPoints.Length-1));
				newLocation = this.partyPoints[randomID].position;
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

	public Vector3 GetNewItemTarget(ItemTargetType type)
	{
		int randomID;
		Vector3 newLocation;

		switch(type)
		{
			case ItemTargetType.Hallway:
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, quarterPoints.Length-1));
				newLocation = this.quarterPoints[randomID].position;
				break;
			case ItemTargetType.Classroom:
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, roomQuarterPoints.Length-1));
				newLocation = this.roomQuarterPoints[randomID].position;
				break;
			case ItemTargetType.FacultyRoom:
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, attendancePoints.Length-1));
				newLocation = this.attendancePoints[randomID].position;
				break;
			case ItemTargetType.BossProjectile:
				randomID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, projectilePoints.Length-1));
				newLocation = this.projectilePoints[randomID].position;
				break;
			default:
				newLocation = this.GetNewNPCTarget(NPCTargetType.AllWanderPoints);
				break;
		}
		return newLocation;
	}

	public AmbienceScript ambience;
	public enum NPCTargetType
	{
		AllWanderPoints, Hallways, Bully, PartyWanderPoints
	}
	public NPCTargetType npcTargetType;
	public enum ItemTargetType
	{
		Hallway, Classroom, FacultyRoom, BossProjectile
	}
	public ItemTargetType itemTargetType;
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
