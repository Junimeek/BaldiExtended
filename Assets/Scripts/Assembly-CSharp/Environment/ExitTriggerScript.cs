using UnityEngine;

public class ExitTriggerScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (this.gc.notebooks >= this.gc.daFinalBookCount && other.CompareTag("Player"))
			this.gc.CompleteGame();
	}

	[SerializeField] private GameControllerScript gc;
}
