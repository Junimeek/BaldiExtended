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
		this.modifiedSpeedDivBy2 = this.modifiedSpeed / 2f;

		if (this.randomizeStart)
			this.sinPosition = Random.Range(0f, modifiedSpeedDivBy2);
	}

	void Update()
	{
		yPosition = amplitude * Mathf.Sin(4f * Mathf.PI / modifiedSpeed * sinPosition) + yOffset;
		itemTransform.localPosition = new Vector3(xPosition, yPosition, zPosition);
		sinPosition += Time.deltaTime;

		if (sinPosition > modifiedSpeedDivBy2)
			sinPosition -= modifiedSpeedDivBy2;
	}

	Transform itemTransform;
	float xPosition;
	float yPosition;
	float yOffset;
	float zPosition;
	float sinPosition;
	[SerializeField] bool randomizeStart;
	[SerializeField] float speed;
	float modifiedSpeed;
	float modifiedSpeedDivBy2;
	[SerializeField] float amplitude;
}
