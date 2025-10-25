using System.Collections;
using UnityEngine;

public class AchievementLoader : MonoBehaviour
{
    private void OnEnable()
    {
        if (this.isFetched)
            return;
        
        this.locks = new GameObject[4,20];

        for (int i = 0; i < this.storyBoxes.Length; i++)
        {
            this.locks[0,i] = this.storyBoxes[i].Find("Lock").gameObject;
            
            if (this.storyData[i] == true)
                this.locks[0,i].SetActive(false);
        }
    }

    [Header("Configuration")]
    [SerializeField] private Transform[] storyBoxes;
    [SerializeField] private Transform[] endlessBoxes;
    [SerializeField] private Transform[] challengeBoxes;
    [SerializeField] private Transform[] miniChallengeBoxes;

    [Header("Loader State")]
    [SerializeField] private bool isFetched;
    private GameObject[,] locks;

    [Header("Achievement Data")]
    [SerializeField] private bool[] storyData;
    [SerializeField] private bool[] endlessData;
    [SerializeField] private bool[] challengeData;
    [SerializeField] private bool[] miniChallengeData;
}
