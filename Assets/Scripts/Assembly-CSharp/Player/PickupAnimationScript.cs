using UnityEngine;

public class PickupAnimationScript : MonoBehaviour
{
	void Start()
	{
		this.speed = Mathf.Clamp(speed, 0.1f, 5f);
		if (this.amplitude <= 0)
			this.amplitude = 1;

		this.itemTransform = GetComponent<Transform>();
		this.xPosition = itemTransform.localPosition.x;
		this.yPosition = itemTransform.localPosition.y;
		this.yOffset = yPosition;
		this.zPosition = itemTransform.localPosition.z;

		this.modifiedSpeed = 4f / speed;
	}

	void Update()
	{
		yPosition = amplitude * Mathf.Sin(4f * Mathf.PI / modifiedSpeed * sinPosition) + yOffset;
		itemTransform.localPosition = new Vector3(xPosition, yPosition, zPosition);
		sinPosition += Time.deltaTime;

		if (sinPosition > modifiedSpeed / 2f)
			sinPosition -= modifiedSpeed / 2f;
	}

	Transform itemTransform;
	float xPosition;
	float yPosition;
	float yOffset;
	float zPosition;
	float sinPosition;
	[SerializeField] float speed;
	float modifiedSpeed;
	[SerializeField] float amplitude;
}
