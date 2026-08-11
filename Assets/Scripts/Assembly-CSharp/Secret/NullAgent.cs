using UnityEngine;
using UnityEngine.AI;

public class NullAgent : MonoBehaviour
{
    void Start()
    {
        this.allowMovement = false;
        this.agent = base.GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (this.allowMovement)
            this.TargetPlayer();
    }

    void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
	}

    public void DisableAgent()
    {
        this.agent.enabled = false;
    }

    NavMeshAgent agent;
    public Transform player;
    public bool allowMovement;
}
