using System.Collections;
using UnityEngine;

public class MenuSwingDoors : MonoBehaviour
{
    AudioSource audioDevice;
    bool isOpen;
    float openTime;
    [SerializeField] MeshRenderer inside;
    [SerializeField] MeshRenderer outside;
    [SerializeField] Material closedTexture;
    [SerializeField] Material openTexture;
    [SerializeField] AudioClip aud_open;

    void Start()
    {
        this.audioDevice = base.GetComponent<AudioSource>();
        this.isOpen = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!this.isOpen && other.tag == "Player")
            StartCoroutine(this.DoorRoutine());
    }

    void OnTriggerStay(Collider other)
    {
        this.openTime = 3f;
    }

    IEnumerator DoorRoutine()
    {
        this.isOpen = true;
        this.audioDevice.PlayOneShot(this.aud_open);
        this.inside.material = this.openTexture;
        this.outside.material = this.openTexture;
        this.openTime = 3f;

        while (this.openTime > 0f)
        {
            this.openTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.isOpen = false;
        this.inside.material = this.closedTexture;
        this.outside.material = this.closedTexture;
    }
}
