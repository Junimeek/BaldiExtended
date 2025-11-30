using UnityEngine;

public class NullSpawnerScript : MonoBehaviour
{
    [SerializeField] GameControllerScript gc;
    [SerializeField] GameObject baldi;
    [SerializeField] AudioSource baldiSource;
    [SerializeField] AudioClip theAudioClip;

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && gc.notebooks == 2)
        {
            gc.SpawnNullBaldi();
            baldiSource.PlayOneShot(theAudioClip);
        }
    }
}
