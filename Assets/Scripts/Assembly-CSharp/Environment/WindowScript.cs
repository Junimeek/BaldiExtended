using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WindowScript : MonoBehaviour
{
    private void Start()
    {
        this.gc = FindObjectOfType<GameControllerScript>();
        this.prizeScript = this.gc.firstPrizeScript;
        this.baldiScript = this.gc.baldiScrpt;
        this.isBroken = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "1st Prize" && this.prizeScript.prizeSpeed > 50f && !this.isBroken)
        {
            this.isBroken = true;

            for (int i = 0; i < this.barriers.Length; i++)
                this.barriers[i].enabled = false;
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

        this.agentObstacle.SetActive(false);

        if (this.baldiScript.isActiveAndEnabled)
            this.baldiScript.AddNewSound(this.windows[1].transform.position, 3);
    }

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private BaldiScript baldiScript;
    [SerializeField] private FirstPrizeScript prizeScript;
    [SerializeField] private MeshCollider[] barriers;
    [SerializeField] private MeshRenderer[] windows;
    [SerializeField] private Material brokenMatierial;
    [SerializeField] private GameObject agentObstacle;
    public bool isBroken;
}
