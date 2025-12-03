using UnityEngine;

public class EvilNeedMoreScript : MonoBehaviour
{
    [SerializeField] AudioSource audioDevice;
    [SerializeField] AudioClip audioClip;
    [SerializeField] GameControllerScript gc;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && gc.notebooks == 2)
            audioDevice.PlayOneShot(this.audioClip);
    }
}
