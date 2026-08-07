using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemy2 : MonoBehaviour
{
    void Start()
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.wanderer = FindObjectOfType<AILocationSelectorScript>();
        this.InitialWander();
    }

    void Update()
    {
        this.v2Location = new Vector2(transform.position.x, transform.position.z);
    }

    void FixedUpdate()
    {
        if (this.agent.velocity.magnitude <= 0.1f)
            this.stillTime += 1;
        else
            this.stillTime = 0;
    }

    void InitialWander()
    {
        //this.agent.SetDestination(wanderer.NewTarget("Kyoko"));
        //this.v2Destination = new Vector2(this.agent.destination.x, this.agent.destination.z);

        //this.cornerList = this.agent.path.corners;
        NavMeshPath newPath = new NavMeshPath();
        NavMesh.CalculatePath(base.transform.position, wanderer.GetNewNPCTarget(AILocationSelectorScript.NPCTargetType.AllWanderPoints), NavMesh.AllAreas, newPath);
        this.cornerList = newPath.corners;

        float valueLeft;
        for (int i = 0; i < this.cornerList.Length; i++)
        {
            valueLeft = this.cornerList[i].x % 5f;
            if (valueLeft < 3f) {
                this.cornerList[i].x -= valueLeft;
            }
            else {
                this.cornerList[i].x += 5f - valueLeft;
            }
            newPath.corners[i].x = this.cornerList[i].x; 

            valueLeft = this.cornerList[i].z % 5f;
            if (valueLeft < 3f) {
                this.cornerList[i].z -= valueLeft;
            }
            else {
                this.cornerList[i].z += 5f - valueLeft;
            }
            newPath.corners[i].z = this.cornerList[i].z;
        }

        this.agent.SetPath(newPath);
        StartCoroutine(MoveToDestination());
    }

    void Wander()
    {
        NavMeshPath newPath = this.GetNewPath();
        this.agent.SetPath(newPath);
    }

    IEnumerator MoveToDestination()
    {
        Debug.Log("starte wanade");
        while (this.agent.velocity.magnitude >= 0.1f)
        {
            yield return null;
        }
        //this.InitialWander();
    }

    NavMeshPath GetNewPath()
    {
        NavMeshPath newPath = new NavMeshPath();
        NavMesh.CalculatePath(base.transform.position, wanderer.GetNewNPCTarget(AILocationSelectorScript.NPCTargetType.AllWanderPoints), NavMesh.AllAreas, newPath);
        this.cornerList = newPath.corners;
        return newPath;
    }

    [SerializeField] NavMeshAgent agent;
    [SerializeField] AILocationSelectorScript wanderer;
    [SerializeField] int stillTime;
    [SerializeField] Vector2 v2Destination;
    [SerializeField] Vector2 v2Location;
    [SerializeField] NavMeshPath nextDestinationPath;

    [SerializeField] Vector3[] cornerList;
}
