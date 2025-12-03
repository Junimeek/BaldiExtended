using UnityEngine;

public class NullSpawnerScript : MonoBehaviour
{
    [SerializeField] GameControllerScript gc;
    [SerializeField] GameObject[] evilColliders;
    [SerializeField] AudioSource baldiSource;
    [SerializeField] AudioClip theAudioClip;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && gc.notebooks == 2)
        {
            gc.SpawnNullBaldi();
            baldiSource.PlayOneShot(theAudioClip);
            foreach (GameObject i in evilColliders)
            {
                Destroy(i);
            }
        }
    }
}
