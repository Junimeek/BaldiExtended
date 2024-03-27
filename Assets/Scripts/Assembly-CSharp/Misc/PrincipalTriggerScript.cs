using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrincipalTriggerScript : MonoBehaviour
{
    
    private void Start()
    {
        this.isTriggerShared = false;
        this.gc = FindObjectOfType<GameControllerScript>();
    }

    private void FixedUpdate()
    {
        if (this.isPlayer && this.isPrincey)
            this.isTriggerShared = true;
        else this.isTriggerShared = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !this.isProtected)
        {
            if (this.isPrincey == true) gc.UpdatePrinceyTrigger(true);
            this.isPlayer = true;
        }
        if (other.gameObject.name == "Principal of the Thing")
        {
            if (this.isPlayer == true) gc.UpdatePrinceyTrigger(true);
            this.isPrincey = true;
        }
        if (other.gameObject.name == "Player" && this.isProtected)
            gc.UpdatePrinceyTrigger(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            gc.UpdatePrinceyTrigger(false);
            this.isPlayer = false;
        }
            
        if (other.gameObject.name == "Principal of the Thing")
        {
            gc.UpdatePrinceyTrigger(false);
            this.isPrincey = false;
        }
    }

    public bool isProtected;
    public bool isTriggerShared;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isPrincey;
    [SerializeField] private GameControllerScript gc;
}
