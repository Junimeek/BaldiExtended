using UnityEngine;

public class ProjectilePickupScript : MonoBehaviour
{
    private void Start()
    {
        this.gc = FindObjectOfType<GameControllerScript>();
        this.playerScript = FindObjectOfType<PlayerScript>();

        this.rb = base.GetComponent<Rigidbody>();

        int shownObject = Mathf.RoundToInt(Random.Range(0f, this.gameObjects.Length - 1));
        for (int i = 0; i < this.gameObjects.Length; i++)
        {
            if (i != shownObject)
                Destroy(this.gameObjects[i]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !this.isPickedUp && !this.isThrown && !this.playerScript.isProjectileGrabbed)
        {
            this.playerScript.isProjectileGrabbed = true;
            this.isPickedUp = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.isPickedUp)
        {
            this.isThrown = true;
            this.isPickedUp = false;
            this.playerScript.isProjectileGrabbed = false;
            this.lifetime = 5f;
            base.gameObject.name = "Projectile";
            this.gc.CreateProjectile(1);
        }
        
        if (this.isThrown)
        {
            this.rb.velocity = transform.forward * 120f;
        
            this.lifetime -= Time.deltaTime;
            if (this.lifetime < 0f)
                Destroy(base.gameObject);
        }
    }

    private void LateUpdate()
    {
        if (this.isPickedUp)
        {
            base.transform.rotation = this.gc.cameraTransform.rotation;
            base.transform.position = this.playerScript.gameObject.transform.position;
            base.transform.position += base.transform.forward * 5f;
        }
    }

    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private bool isPickedUp;
    [SerializeField] private bool isThrown;
    [SerializeField] private float lifetime;
    [SerializeField] private Rigidbody rb;
}