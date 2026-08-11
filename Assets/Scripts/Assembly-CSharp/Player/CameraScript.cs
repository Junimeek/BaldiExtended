using UnityEngine;

public class CameraScript : MonoBehaviour
{
	private void Start()
	{
		this.cameraObject = base.GetComponent<Camera>();
		this.offset = base.transform.position - this.player.transform.position;
	}

	private void Update()
	{
		if (this.ps.jumpRope) //If the player is jump roping
		{
			this.velocity -= this.gravity * Time.deltaTime; //Decrease the velocity using gravity
			this.jumpHeight += this.velocity * Time.deltaTime; //Increase the jump height based on the velocity
			if (this.jumpHeight <= 0f) //When the player is on the floor, prevent the player from falling through.
			{
				this.jumpHeight = 0f;
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
					this.velocity = this.initVelocity; //Start the jump
			}
			this.jumpHeightV3 = new Vector3(0f, this.jumpHeight, 0f); //Turn the float into a vector
		}
		else if (Input.GetButton("Look Behind"))
			this.lookBehind = 180;
		else
			this.lookBehind = 0;

		if (Input.GetKey(KeyCode.C))
			this.zoom = 15;
		else
			this.zoom = 60;
	}

	private void LateUpdate()
	{
		base.transform.position = this.player.transform.position + this.offset;

		if (this.ps.gameOver)
		{
			if (this.ps.isSecret)
			{
				base.transform.LookAt(new Vector3(this.baldi.position.x, this.baldi.position.y, this.baldi.position.z));
				base.transform.position = this.baldi.transform.position + this.baldi.transform.forward * 2f + new Vector3(0f, 2.5f, 0f);
			}
			else if (this.ps.isNullStyle)
			{
				base.transform.position = this.baldi.transform.position + this.baldi.transform.forward * 2f + new Vector3(0f, 5f, 0f);
				base.transform.LookAt(new Vector3(this.baldi.position.x, this.baldi.position.y + 5f, this.baldi.position.z));
			}
			else
			{
				base.transform.position = this.baldi.transform.position + this.baldi.transform.forward * 2f + new Vector3(0f, 5f, 0f);
				base.transform.LookAt(new Vector3(this.baldi.position.x, this.baldi.position.y + 5f, this.baldi.position.z));
			}
		}
		else if (this.ps.jumpRope)
		{
			base.transform.position = this.player.transform.position + this.offset + this.jumpHeightV3;
			base.transform.rotation = this.player.transform.rotation;
		}
		else
		{
			base.transform.position = this.player.transform.position + this.offset;
			base.transform.rotation = this.player.transform.rotation * Quaternion.Euler(0f, this.lookBehind, 0f);
		}

		this.cameraObject.fieldOfView = this.zoom;
	}

	public Transform baldi;
	public Vector3 offset;
	public float jumpHeight;
	[SerializeField] private GameObject player;
	[SerializeField] private PlayerScript ps;
	[SerializeField] private float initVelocity;
	[SerializeField] private float velocity;
	[SerializeField] private float gravity;
	[SerializeField] private int lookBehind;
	[SerializeField] private Vector3 jumpHeightV3;
	[SerializeField] private Camera cameraObject;
	[SerializeField] private int zoom;

}
