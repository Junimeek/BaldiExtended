using UnityEngine;

public class ExitTriggerScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (this.gc.notebooks >= this.gc.daFinalBookCount && other.tag == "Player")
			this.gc.CompleteGame();
	}

	[SerializeField] private GameControllerScript gc;
}
