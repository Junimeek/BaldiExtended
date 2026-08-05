using UnityEngine;

public class TutorBaldi : MonoBehaviour
{
    void OnEnable()
    {
        audioDevice = base.GetComponent<AudioSource>();
        int num = Mathf.RoundToInt(Random.Range(0f, this.clipList.Length - 1));
        audioDevice.PlayOneShot(clipList[num]);
    }

    AudioSource audioDevice;
    [SerializeField] AudioClip[] clipList;
}
