using System;
using UnityEngine;

public class CDMWindowScript : MonoBehaviour
{
    void Start()
    {
        this.challengeController = FindObjectOfType<ChallengeController>();
        this.isBroken = false;
        GameControllerScript gc = FindObjectOfType<GameControllerScript>();
        this.player = gc.player;
        this.baldi = gc.baldiScrpt;

        if (gc.modeType == "nullStyle")
        {
            Array.Resize(ref this.challengeController.windowBlockers, this.challengeController.windowBlockers.Length + 1);
            this.challengeController.windowBlockers[^1] = this.agentObstacle;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Baldi" && !this.isBroken)
            BreakWindow();
        else if (other.gameObject.CompareTag("Player") && player.CheckPlayerWindowState() && !this.isBroken)
        {
            BreakWindow();
            if (baldi.isActiveAndEnabled)
                baldi.AddNewSound(agentObstacle.transform.position, 3);
        }
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
    [SerializeField] MeshCollider[] barriers;
    [SerializeField] MeshRenderer[] windows;
    [SerializeField] Material brokenMatierial;
    [SerializeField] AudioSource audioDevice;
    public GameObject agentObstacle;
    [HideInInspector] public bool isBroken;
    PlayerScript player;
    BaldiScript baldi;
}
