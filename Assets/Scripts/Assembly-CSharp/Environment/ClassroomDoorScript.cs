using System.Collections;
using UnityEngine;

public class ClassroomDoorScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		    RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider == this.raycastTrigger && Vector3.Distance(this.player.position, base.transform.position) <= 15f)
            {
                if (this.isLocked || this.permanentLock)
                {
                    this.audioDevice.PlayOneShot(this.aud_click);
                    return;
                }

                this.openTime = 3f;

                if (this.isOpen && !this.gc.isDoorFix && this.baldiScript.isActiveAndEnabled && this.remainingSilentOpens == 0)
                {
                    this.audioDevice.PlayOneShot(this.aud_openDoor);
                    this.baldiScript.AddNewSound(base.transform.position, 1);
                }
                else if (!this.isOpen)
                {
                    StartCoroutine(this.DoorRoutine(true));

                    if (this.sweepDoor && this.sweepScript.isActiveAndEnabled && this.sweepScript != null)
                        this.sweepScript.EarlyActivate();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            this.openTime = 3f;
            StartCoroutine(this.DoorRoutine(false));
        }
    }

    private IEnumerator DoorRoutine(bool isPlayer)
    {
        this.isOpen = true;

        if (isPlayer && this.baldiScript.isActiveAndEnabled && this.remainingSilentOpens == 0)
            this.baldiScript.AddNewSound(base.transform.position, 1);
        
        this.inside.material = this.openTexture;
        this.outside.material = this.openTexture;
        this.barrier.enabled = false;
        this.invisibleBarrier.enabled = false;

        if (this.remainingSilentOpens > 0)
        {
            this.nosqueeOverlays[0].material = this.nosqueeTextures[2];
            this.nosqueeOverlays[1].material = this.nosqueeTextures[2];

            if (isPlayer)
                this.remainingSilentOpens--;
        }
        else
        {
            this.audioDevice.PlayOneShot(this.aud_openDoor);
            this.nosqueeOverlays[0].material = this.nosqueeTextures[0];
            this.nosqueeOverlays[1].material = this.nosqueeTextures[0];
        }

        while (this.openTime > 0f && !this.isLocked)
        {
            this.openTime -= Time.deltaTime;
            yield return null;
        }

        this.isOpen = false;
        this.inside.material = this.closedTexture;
        this.outside.material = this.closedTexture;
        this.barrier.enabled = true;
        this.invisibleBarrier.enabled = true;

        if (this.remainingSilentOpens > 0)
        {
            this.nosqueeOverlays[0].material = this.nosqueeTextures[1];
            this.nosqueeOverlays[1].material = this.nosqueeTextures[1];
        }
        else
        {
            this.audioDevice.PlayOneShot(this.aud_closeDoor);
            this.nosqueeOverlays[0].material = this.nosqueeTextures[0];
            this.nosqueeOverlays[1].material = this.nosqueeTextures[0];
        }
    }

    public void LockDoor(float time)
    {
        this.isLocked = true;
        this.lockTime = time;
        StartCoroutine(this.LockRoutine());
    }

    public bool TryUnLock()
    {
        if (this.isLocked)
        {
            this.isLocked = false;
            this.openTime = 3f;
            return true;
        }
        else
            return false;
    }

    private IEnumerator LockRoutine()
    {
        this.audioDevice.PlayOneShot(this.aud_lock);

        while (this.lockTime > 0f && this.isLocked)
        {
            this.lockTime -= Time.deltaTime;
            yield return null;
        }

        if (this.isLocked)
        {
            this.isLocked = false;
            this.audioDevice.PlayOneShot(this.aud_unlock);
        }
        else
            StartCoroutine(this.DoorRoutine(true));
    }

    public void SilenceDoor()
    {
        this.remainingSilentOpens += 6;
        this.nosqueeOverlays[0].material = this.nosqueeTextures[1];
        this.nosqueeOverlays[1].material = this.nosqueeTextures[1];
    }

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private Transform player;
    [SerializeField] private BaldiScript baldiScript;
    [SerializeField] private SweepScript sweepScript;
    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private MeshCollider barrier;
    [SerializeField] private MeshCollider invisibleBarrier;
    [SerializeField] private MeshCollider raycastTrigger;

    [Header("Door configuration")]
    [SerializeField] private bool sweepDoor;
    [SerializeField] private bool permanentLock;

    [Header("Door State")]
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isOpen;
    [SerializeField] private byte remainingSilentOpens;
    [SerializeField] private float openTime;
    public float lockTime;

    [Header("Audio")]
    [SerializeField] private AudioClip aud_openDoor;
    [SerializeField] private AudioClip aud_closeDoor;
    [SerializeField] private AudioClip aud_lock;
    [SerializeField] private AudioClip aud_unlock;
    [SerializeField] private AudioClip aud_click;

    [Header("Rendering")]
    [SerializeField] private MeshRenderer inside;
    [SerializeField] private MeshRenderer outside;
    [SerializeField] private MeshRenderer[] nosqueeOverlays;
    [SerializeField] private Material openTexture;
    [SerializeField] private Material closedTexture;
    [SerializeField] private Material[] nosqueeTextures;
}
