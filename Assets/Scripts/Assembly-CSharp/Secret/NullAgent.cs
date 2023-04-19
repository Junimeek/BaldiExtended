using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NullAgent : MonoBehaviour
{
    void Start()
    {
        allowMovement = false;
        this.agent = base.GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (allowMovement) TargetPlayer();
    }

    private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
	}

    private NavMeshAgent agent;
    public Transform player;
    public bool allowMovement;
}
