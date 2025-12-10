using UnityEngine;

public class CraftersWarpPoints : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] int chosenPlayerPoint;

    void Start()
    {
        Transform[] initialPoints = GetComponentsInChildren<Transform>();
        this.points = new Transform[initialPoints.Length - 1];

        for (int i = 0; i < initialPoints.Length; i++)
        {
            try
            {
                this.points[i] = initialPoints[i + 1];
                this.points[i].GetComponent<SpriteRenderer>().color = Color.clear;
            }
            catch
            {
                break;
            }
        }
    }
    
    public Vector3[] GetCraftersWarpPoints()
    {
        Vector3 baldiPoint = points[GetPointID()].position;
        this.chosenPlayerPoint = GetPointID();
        Vector3 playerPoint = points[chosenPlayerPoint].position;
        int rerolls = 0;

        while (playerPoint == baldiPoint)
        {
            rerolls++;
            this.chosenPlayerPoint = GetPointID();
            playerPoint = points[chosenPlayerPoint].position;
            if (rerolls > 10) // failsave in case the array has just one point
            {
                PlayerScript player = FindObjectOfType<PlayerScript>();
                playerPoint = new Vector3(5f, player.height, 5f);
                break;
            }
        }
        Vector3[] theArray = {
            baldiPoint, playerPoint
        };
        return theArray;
    }

    public Quaternion GetPlayerRotation(Vector3 playerPosition)
    {
        Vector3 relativePos = this.points[chosenPlayerPoint].position + Vector3.forward - playerPosition;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        return rotation;
    }

    int GetPointID()
    {
        int id = Mathf.FloorToInt(Random.Range(0f, points.Length - 0.05f));
        return id;
    }
}
