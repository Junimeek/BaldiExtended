using UnityEngine;

public class FacultyTriggerScript : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (this.player == null)
				this.player = other.GetComponent<PlayerScript>();
			this.player.ResetGuilt(PlayerScript.GuiltType.Faculty, 1f);
		}
	}
	PlayerScript player;
}