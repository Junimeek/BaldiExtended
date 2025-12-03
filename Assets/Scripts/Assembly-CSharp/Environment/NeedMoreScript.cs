using UnityEngine;

public class NeedMoreScript : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (gc.notebooks < 2 && other.tag == "Player")
			this.audioDevice.PlayOneShot(this.baldiDoor, 1f);
	}

	[SerializeField] GameControllerScript gc;
	[SerializeField] AudioSource audioDevice;
	[SerializeField] AudioClip baldiDoor;
}
