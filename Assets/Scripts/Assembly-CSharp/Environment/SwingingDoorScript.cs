using System.Collections;
using UnityEngine;

public class SwingingDoorScript : MonoBehaviour
{
	private void Start()
    {
        this.requirementMet = false;
        this.audioDevice = base.GetComponent<AudioSource>();
        StartCoroutine(this.WaitForNotebooks());
    }

    private IEnumerator WaitForNotebooks()
    {
        while (this.gc.notebooks < 2)
            yield return null;
        
        this.requirementMet = true;
        this.barrier.enabled = false;
        this.obstacle.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this.requirementMet)
            return;
        
        if (!this.bDoorOpen && !this.bDoorLocked)
        {
            if (other.tag == "Player")
                StartCoroutine(this.DoorRoutine(true));
            else
                StartCoroutine(this.DoorRoutine(false));
        }

        if (this.bDoorOpen && !this.gc.isDoorFix && !this.bDoorLocked)
        {
            this.audioDevice.PlayOneShot(this.doorOpen);

            if (other.tag == "Player")
                this.baldi.AddNewSound(base.transform.position, 1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.requirementMet)
            this.openTime = 3f;
    }

    private IEnumerator DoorRoutine(bool isPlayer)
    {
        this.bDoorOpen = true;
        this.audioDevice.PlayOneShot(this.doorOpen);

        if (isPlayer)
            this.baldi.AddNewSound(base.transform.position, 1);

        this.inside.material = this.open;
        this.outside.material = this.open;
        this.openTime = 3f;

        while (this.openTime > 0f && !this.bDoorLocked)
        {
            this.openTime -= Time.deltaTime;
            yield return null;
        }

        this.bDoorOpen = false;

        if (this.bDoorLocked)
        {
            this.inside.material = this.locked;
            this.outside.material = this.locked;
        }
        else
        {
            this.inside.material = this.closed;
            this.outside.material = this.closed;
        }
    }

    public void LockDoor()
    {
        this.bDoorLocked = true;
        this.lockTime = 30f;
        this.obstacle.SetActive(true);
        this.barrier.enabled = true;
        StartCoroutine(this.LockRoutine());
    }

    private IEnumerator LockRoutine()
    {
        while (this.lockTime > 0f)
        {
            this.lockTime -= Time.deltaTime;
            yield return null;
        }

        this.bDoorLocked = false;
        this.obstacle.SetActive(false);
        this.barrier.enabled = false;
        this.inside.material = this.closed;
        this.outside.material = this.closed;
    }

	[SerializeField] private GameControllerScript gc;
	[SerializeField] private BaldiScript baldi;
	[SerializeField] private MeshCollider barrier;
	[SerializeField] private GameObject obstacle;
	private AudioSource audioDevice;
	[SerializeField] private AudioClip doorOpen;

	private bool requirementMet;
	private bool bDoorLocked;
	private bool bDoorOpen;
	private float openTime;
	private float lockTime;

	[Header("Rendering")]
	[SerializeField] private MeshRenderer outside;
	[SerializeField] private MeshRenderer inside;
	[SerializeField] private Material closed;
	[SerializeField] private Material open;
	[SerializeField] private Material locked;
	
	
	
	
	
	
}
