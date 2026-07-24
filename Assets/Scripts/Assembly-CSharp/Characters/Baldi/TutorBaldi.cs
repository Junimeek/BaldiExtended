using UnityEngine;

public class TutorBaldi : MonoBehaviour
{
    void OnEnable()
    {
        audioDevice = base.GetComponent<AudioSource>();
        int num = Mathf.RoundToInt(Random.Range(0f, 1f));
        audioDevice.PlayOneShot(clipList[num]);
    }

    AudioSource audioDevice;
    [SerializeField] AudioClip[] clipList;
}
