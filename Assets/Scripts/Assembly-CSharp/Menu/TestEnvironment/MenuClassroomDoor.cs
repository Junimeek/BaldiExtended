using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClassroomDoor : MonoBehaviour
{
    [SerializeField] AudioSource audioDevice;
    [SerializeField] TestEnvironmentController gc;
    [SerializeField] AudioClip aud_open;
    [SerializeField] AudioClip aud_close;
    [SerializeField] bool isOpen;
    [SerializeField] float openTime;
    [SerializeField] MeshRenderer inside;
    [SerializeField] MeshRenderer outside;
    [SerializeField] Material openTexture;
    [SerializeField] Material closedTexture;
    [SerializeField] MeshCollider barrier;
    [SerializeField] MeshCollider invisibleBarrier;
    [SerializeField] MeshCollider raycastTrigger;
    [SerializeField] Transform player;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gc.isStopped)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider == this.raycastTrigger && Vector3.Distance(this.player.position, base.transform.position) <= 15f)
            {
                this.openTime = 3f;

                if (!this.isOpen)
                    StartCoroutine(this.DoorRoutine());
            }
        }
    }

    IEnumerator DoorRoutine()
    {
        this.isOpen = true;
        this.inside.material = this.openTexture;
        this.outside.material = this.openTexture;
        this.barrier.enabled = false;
        this.invisibleBarrier.enabled = false;
        this.audioDevice.PlayOneShot(this.aud_open);

        while (this.openTime > 0f)
        {
            this.openTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.isOpen = false;
        this.inside.material = this.closedTexture;
        this.outside.material = this.closedTexture;
        this.barrier.enabled = true;
        this.invisibleBarrier.enabled = true;
        this.audioDevice.PlayOneShot(this.aud_close);
    }
}
