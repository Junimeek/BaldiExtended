using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossStartTrigger : MonoBehaviour
{
    private void Start()
    {
        this.position = base.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && this.gc.notebooks >= this.gc.daFinalBookCount)
        {
            if (this.gc.exitsReached < 3)
                Destroy(this.gameObject);
            else
            {
                this.entrance.Lower();
                this.gc.ActivateBossFight(this.position);

                this.gc.projectilesInPlay = new GameObject[3];
                for (int i = 0; i < this.points.Length; i++)
                    this.gc.projectilesInPlay[i] = Instantiate(this.projectile, this.points[i].position, Quaternion.identity);
                
                Destroy(this.gameObject);
            }
        }
    }

    [SerializeField] private Vector3 position;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private EntranceScript entrance;
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject projectile;
}
