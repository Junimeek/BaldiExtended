using UnityEngine;

public class BsodaSparyScript : MonoBehaviour
{
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>(); //Get the RigidBody
		this.rb.velocity = base.transform.forward * this.speed; //Move forward
		if (this.gameObject.name == "DietBSODA_Spray(Clone)") //Set the lifespan
		{
			this.sprite.sprite = this.cyanSpray;
			this.lifeSpan = 5f;
		}
		else
		{
			this.sprite.sprite = this.blueSpray;
			this.lifeSpan = 30f;
		}
	}

	private void Update()
	{
		this.rb.velocity = base.transform.forward * this.speed; //Move forward
		this.lifeSpan -= Time.deltaTime; // Decrease the lifespan variable
		if (this.lifeSpan < 0f) //When the lifespan timer ends, destroy the BSODA
		{
			UnityEngine.Object.Destroy(base.gameObject, 0f);
		}
	}

	public float speed;
	private float lifeSpan;
	private Rigidbody rb;
	[SerializeField] private Sprite blueSpray;
	[SerializeField] private Sprite cyanSpray;
	[SerializeField] private SpriteRenderer sprite;
}
