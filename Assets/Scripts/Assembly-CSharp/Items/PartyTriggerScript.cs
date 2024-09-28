using UnityEngine;

public class PartyTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            this.gc.movingPartyLocation = this.partySpawnerLocaion;
            this.wanderer.movingPartyPoints = this.wanderPoints;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player")
            this.gc.movingPartyLocation = null;
    }

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private AILocationSelectorScript wanderer;
    [SerializeField] private Transform partySpawnerLocaion;
    [SerializeField] private Transform[] wanderPoints;
}
