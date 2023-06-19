using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincipalTriggerScript : MonoBehaviour
{
    
    private void Start()
    {
        // this.isTriggerShared = false;
    }

    private void Update()
    {
        /*
        if (this.playerTriggerID == this.princeyTriggerID)
            this.isTriggerShared = true;
        else this.isTriggerShared = false;
        */
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
            this.playerInTrigger = true;
        if (other.gameObject.name == "Principal of the Thing")
            this.princeyInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
            this.playerInTrigger = false;
        if (other.gameObject.name == "Principal of the Thing")
            this.princeyInTrigger = false;
    }
    */

    public bool isTriggerShared;
    public int objectID;
    public int playerTriggerID;
    public int princeyTriggerID;
    [SerializeField] private PlayerScript player;
    [SerializeField] private PrincipalScript princey;
}
