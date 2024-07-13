using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertMoveTest : MonoBehaviour
{
    private void Start()
    {
        this.trans = this.gameObject.transform;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            trans.position = new Vector3(trans.position.x, trans.position.y, trans.position.z + (moveMagnitude * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.CheckRaycast();
        }
    }

    private void CheckRaycast()
    {
        float snapDistance = this.moveMagnitude*Time.deltaTime;
        RaycastHit hit;
        if(Physics.Raycast(trans.position, transform.TransformDirection(Vector3.down), out hit, 30f))
        {
            trans.position = new Vector3(trans.position.x, trans.position.y-snapDistance, trans.position.z);
        }
    }

    [SerializeField] private Transform trans;
    [SerializeField] private float moveMagnitude;
}
