using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            this.gc.partyLocation = this.partySpawnerLocaion;
            this.wanderer.partyPoints = this.wanderPoints;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player")
            this.gc.partyLocation = null;
    }

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private AILocationSelectorScript wanderer;
    [SerializeField] private Transform partySpawnerLocaion;
    [SerializeField] private Transform[] wanderPoints;
}
