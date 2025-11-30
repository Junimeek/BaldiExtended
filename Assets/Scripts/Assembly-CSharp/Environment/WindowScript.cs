using System;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    private void Start()
    {
        this.challengeController = FindObjectOfType<ChallengeController>();
        this.isBroken = false;
        
        Array.Resize(ref this.challengeController.windowBlockers, this.challengeController.windowBlockers.Length + 1);
        this.challengeController.windowBlockers[this.challengeController.windowBlockers.Length - 1] = this.agentObstacle;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Baldi" && !this.isBroken)
            this.BreakWindow();
    }

    public void BreakWindow()
    {
        this.isBroken = true;

        for (int i = 0; i < 2; i++)
        {
            this.barriers[i].enabled = false;
            this.windows[i].material = this.brokenMatierial;
        }

        this.audioDevice.Play();
        this.agentObstacle.transform.position += new Vector3(0f, 20f, 0f);
    }

    ChallengeController challengeController;
    [SerializeField] private MeshCollider[] barriers;
    [SerializeField] private MeshRenderer[] windows;
    [SerializeField] private Material brokenMatierial;
    [SerializeField] private AudioSource audioDevice;
    public GameObject agentObstacle;
    [HideInInspector] public bool isBroken;
}
