using UnityEngine;

public class PickupAnimationScript : MonoBehaviour
{
	private void Start()
	{
		this.itemPosition = base.GetComponent<Transform>();
	}

	private void Update()
	{
		if (this.isFast)
			this.itemPosition.localPosition = new Vector3(0f, Mathf.Sin(Time.frameCount * 0.02f) / 2f + 0.5f, 0f);
		else
			this.itemPosition.localPosition = new Vector3(0f, Mathf.Sin((float)Time.frameCount * 0.017453292f) / 2f + 1f, 0f);
	}

	private Transform itemPosition;
	[SerializeField] private bool isFast;
}
