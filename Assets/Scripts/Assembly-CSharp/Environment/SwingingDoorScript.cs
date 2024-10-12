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
        
        if (!this.isOpen && !this.isLocked)
        {
            if (other.tag == "Player")
                StartCoroutine(this.DoorRoutine(true));
            else
                StartCoroutine(this.DoorRoutine(false));
        }

        if (this.isOpen && !this.gc.isDoorFix && !this.isLocked)
        {
            this.audioDevice.PlayOneShot(this.aud_openDoor);

            if (other.tag == "Player" && this.baldiScript.isActiveAndEnabled)
                this.baldiScript.AddNewSound(base.transform.position, 1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.requirementMet)
            this.openTime = 3f;
    }

    private IEnumerator DoorRoutine(bool isPlayer)
    {
        this.isOpen = true;
        this.audioDevice.PlayOneShot(this.aud_openDoor);

        if (isPlayer && this.baldiScript.isActiveAndEnabled)
            this.baldiScript.AddNewSound(base.transform.position, 1);

        this.inside.material = this.openTexture;
        this.outside.material = this.openTexture;
        this.openTime = 3f;

        while (this.openTime > 0f && !this.isLocked)
        {
            this.openTime -= Time.deltaTime;
            yield return null;
        }

        this.isOpen = false;

        if (!this.isLocked)
        {
            this.inside.material = this.closedTexture;
            this.outside.material = this.closedTexture;
        }
    }

    public void LockDoor()
    {
        this.isLocked = true;
        this.lockTime = 30f;
        this.obstacle.SetActive(true);
        this.barrier.enabled = true;
        this.inside.material = this.lockedTexture;
        this.outside.material = this.lockedTexture;
        StartCoroutine(this.LockRoutine());
    }

    private IEnumerator LockRoutine()
    {
        while (this.lockTime > 0f)
        {
            this.lockTime -= Time.deltaTime;
            yield return null;
        }

        this.isLocked = false;
        this.obstacle.SetActive(false);
        this.barrier.enabled = false;
        this.inside.material = this.closedTexture;
        this.outside.material = this.closedTexture;
    }

    private AudioSource audioDevice;
	[SerializeField] private GameControllerScript gc;
	[SerializeField] private BaldiScript baldiScript;
	[SerializeField] private MeshCollider barrier;
	[SerializeField] private GameObject obstacle;
	[SerializeField] private AudioClip aud_openDoor;

	private bool requirementMet;
	private bool isLocked;
	private bool isOpen;
	private float openTime;
	private float lockTime;

	[Header("Rendering")]
	[SerializeField] private MeshRenderer outside;
	[SerializeField] private MeshRenderer inside;
	[SerializeField] private Material closedTexture;
	[SerializeField] private Material openTexture;
	[SerializeField] private Material lockedTexture;
	
	
	
	
	
	
}
