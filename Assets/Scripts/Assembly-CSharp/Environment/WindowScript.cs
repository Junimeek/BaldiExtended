using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    private void Start()
    {
        this.prizeScript = this.gc.firstPrizeScript;
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

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private FirstPrizeScript prizeScript;
    [SerializeField] private MeshCollider[] barriers;
    [SerializeField] private bool isBroken;
}
