using UnityEngine;

public class ProjectilePickupScript : MonoBehaviour
{
    private void Start()
    {
        this.gc = FindObjectOfType<GameControllerScript>();
        this.playerScript = FindObjectOfType<PlayerScript>();
        this.nullBoss = FindObjectOfType<NullBoss>();

        this.rb = base.GetComponent<Rigidbody>();

        int shownObject = Mathf.RoundToInt(Random.Range(0f, this.gameObjects.Length - 1));
        for (byte i = 0; i < this.gameObjects.Length; i++)
        {
            if (i != shownObject)
                Destroy(this.gameObjects[i]);
            else
                this.pickupID = i;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !this.isPickedUp && !this.isThrown && !this.playerScript.isProjectileGrabbed)
        {
            this.playerScript.isProjectileGrabbed = true;
            this.isPickedUp = true;

            if (this.pickupID == 2 || this.pickupID == 3)
            {
                this.meshRenderer = this.gameObjects[this.pickupID].GetComponentInChildren<MeshRenderer>();
                this.meshRenderer.material = this.transparent;
            }
            else
            {
                this.sprite = this.gameObjects[this.pickupID].GetComponent<SpriteRenderer>();
                this.sprite.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.isPickedUp && !this.gc.gamePaused)
        {
            this.isThrown = true;
            this.isPickedUp = false;
            this.playerScript.isProjectileGrabbed = false;
            this.lifetime = 3.1f;
            base.gameObject.name = "Projectile";
            this.gc.createdProjectiles--;
            //this.gc.StartCoroutine(this.gc.WaitForProjectile());
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
            this.mapIcon.color = new Color(0f, 0f, 0f, 0f);
            base.transform.rotation = this.gc.cameraTransform.rotation;
            base.transform.position = this.playerScript.gameObject.transform.position;
            base.transform.position += base.transform.forward * 5f;
        }
    }

    private void OnDestroy()
    {
        this.gc.CheckProjectileCount(this.nullBoss);
    }

    [SerializeField] private NullBoss nullBoss;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private bool isPickedUp;
    [SerializeField] private bool isThrown;
    [SerializeField] private float lifetime;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer mapIcon;
    [SerializeField] private byte pickupID;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material transparent;
    [SerializeField] private SpriteRenderer sprite;
}