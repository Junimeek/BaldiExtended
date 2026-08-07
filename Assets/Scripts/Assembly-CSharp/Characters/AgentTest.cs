using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.Wander();
	}

	private void Update()
	{
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity, 3, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player") //Check if its the player
		{
			this.db = true;
			this.TargetPlayer();
		}
		else
		{
			this.db = false;
			if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
			{
				this.Wander();
			}
		}
	}

	private void Wander()
	{
		this.agent.SetDestination(this.wanderer.GetNewNPCTarget(AILocationSelectorScript.NPCTargetType.AllWanderPoints));
		this.coolDown = 1f;
	}

	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

	public bool db;
	public Transform player;
	public AILocationSelectorScript wanderer;
	public float coolDown;
	private NavMeshAgent agent;
	[SerializeField] private GameObject attendanceRoom;
}
