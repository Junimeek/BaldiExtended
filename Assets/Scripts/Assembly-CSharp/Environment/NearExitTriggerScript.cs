using UnityEngine;

public class NearExitTriggerScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (this.gc.exitsReached < this.gc.entranceList.Length - 1 & this.gc.finaleMode & other.tag == "Player")
		{
			this.gc.ExitReached();
			this.es.Lower();
			if (this.gc.baldiScrpt.isActiveAndEnabled)
			{
				this.gc.baldiScrpt.AddNewSound(base.transform.position, 4);
				this.gc.baldiScrpt.GetAngry(0.5f);
			}
		}
	}

	public GameControllerScript gc;
	public EntranceScript es;
}
