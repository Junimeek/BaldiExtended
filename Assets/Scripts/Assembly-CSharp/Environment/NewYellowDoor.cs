using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NewYellowDoor : MonoBehaviour
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
        this.obstacle.enabled = false;
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
            this.audioDevice.PlayOneShot(this.aud_doorOpen);

            if (other.tag == "Player")
                this.baldiScript.AddNewSound(base.transform.position, 1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.requirementMet)
            this.remainingOpenTime = 3f;
    }

    private IEnumerator DoorRoutine(bool isPlayer)
    {
        this.isOpen = true;
        this.audioDevice.PlayOneShot(this.aud_doorOpen);

        if (isPlayer)
            this.baldiScript.AddNewSound(base.transform.position, 1);

        this.inside.material = this.openTexture;
        this.outside.material = this.openTexture;
        this.remainingOpenTime = 3f;

        while (this.remainingOpenTime > 0f && !this.isLocked)
        {
            this.remainingOpenTime -= Time.deltaTime;
            yield return null;
        }

        this.isOpen = false;

        if (this.isLocked)
        {
            this.inside.material = this.lockedTexture;
            this.outside.material = this.lockedTexture;
        }
        else
        {
            this.inside.material = this.closedTexture;
            this.outside.material = this.closedTexture;
        }
    }

    public void LockDoor()
    {
        this.isLocked = true;
        this.remainingLockTime = 5f;
        this.obstacle.enabled = true;
        this.barrier.enabled = true;
        StartCoroutine(this.LockRoutine());
    }

    private IEnumerator LockRoutine()
    {
        while (this.remainingLockTime > 0f)
        {
            this.remainingLockTime -= Time.deltaTime;
            yield return null;
        }

        this.isLocked = false;
        this.obstacle.enabled = false;
        this.barrier.enabled = false;
        this.inside.material = this.closedTexture;
        this.outside.material = this.openTexture;
    }

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private BaldiScript baldiScript;
    [SerializeField] private MeshCollider barrier;
    [SerializeField] private NavMeshObstacle obstacle;
    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private AudioClip aud_doorOpen;

    [Header("Door State")]
    [SerializeField] private bool requirementMet;
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isOpen;
    [SerializeField] private float remainingOpenTime;
    [SerializeField] private float remainingLockTime;
    
    [Header("Rendering")]
    [SerializeField] private MeshRenderer outside;
    [SerializeField] private MeshRenderer inside;
    [SerializeField] private Material closedTexture;
    [SerializeField] private Material openTexture;
    [SerializeField] private Material lockedTexture;
}
