using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class WindowScript : MonoBehaviour
{
    private void Start()
    {
        this.gc = FindObjectOfType<GameControllerScript>();
        this.prizeScript = this.gc.firstPrizeScript;
        this.baldiScript = this.gc.baldiScrpt;
        this.isBroken = false;
        
        Array.Resize(ref this.gc.windowBlockers, this.gc.windowBlockers.Length + 1);
        this.gc.windowBlockers[this.gc.windowBlockers.Length - 1] = this.agentObstacle;
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

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private BaldiScript baldiScript;
    [SerializeField] private FirstPrizeScript prizeScript;
    [SerializeField] private MeshCollider[] barriers;
    [SerializeField] private MeshRenderer[] windows;
    [SerializeField] private Material brokenMatierial;
    [SerializeField] private AudioSource audioDevice;
    public GameObject agentObstacle;
    public bool isBroken;
}
