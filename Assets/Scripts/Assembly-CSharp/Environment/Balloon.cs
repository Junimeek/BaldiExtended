using System.Collections;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	private void Start()
	{
        this.StartCoroutine(this.BalloonRush());
	}

    private IEnumerator BalloonRush()
    {
        this.speed = 50f;
        this.directionTime = 0.5f;

        this.direction.x = Random.Range(-1f, 1f);
		this.direction.z = Random.Range(-1f, 1f);
		this.direction = this.direction.normalized;
        
        while (this.directionTime > 0f)
        {
            this.directionTime -= Time.deltaTime;
            this.rb.velocity = this.direction * this.speed;
            yield return null;
        }
        
        this.ChangeDirection();
    }

    private IEnumerator MoveBalloon()
    {
        while (this.directionTime > 0f)
        {
            this.directionTime -= Time.deltaTime;
            this.rb.velocity = this.direction * this.speed;
            yield return null;
        }

        this.ChangeDirection();
    }

	private void ChangeDirection()
	{
        this.directionTime = Random.Range(2.5f, 10f);
        this.speed = Random.Range(1f, 7f);

		this.direction.x = Random.Range(-1f, 1f);
		this.direction.z = Random.Range(-1f, 1f);
		this.direction = this.direction.normalized;

        this.StartCoroutine(this.MoveBalloon());
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
            this.wallCollider.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player")
            this.wallCollider.SetActive(true);
    }

	[SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject wallCollider;
	[SerializeField] private float directionTime;
	private Vector3 direction;
	public float speed;
}
