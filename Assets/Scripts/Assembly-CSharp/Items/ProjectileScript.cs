using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private void Start()
    {
        this.lifetime = 5f;
    }
    private void Update()
    {
        this.rb.velocity = transform.forward * 120f;
        
        this.lifetime -= Time.deltaTime;
        if (this.lifetime < 0f)
            Destroy(base.gameObject);
    }

    [SerializeField] private Rigidbody rb;
    private float lifetime;
}
