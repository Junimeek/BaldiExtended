using UnityEngine;

public class BossStartTrigger : MonoBehaviour
{
    void Start()
    {
        this.position = base.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && this.gc.notebooks >= this.gc.daFinalBookCount)
        {
            if (this.gc.exitsReached < 3)
                Destroy(this.gameObject);
            else
            {
                this.entrance.Lower();
                this.darkModeController.ActivateNullBossFight(this.position);

                this.darkModeController.projectilesInPlay = new GameObject[3];
                for (int i = 0; i < this.points.Length; i++)
                    this.darkModeController.projectilesInPlay[i] = Instantiate(this.projectile, this.points[i].position, Quaternion.identity);
                
                Destroy(this.gameObject);
            }
        }
    }

    Vector3 position;
    [SerializeField] GameControllerScript gc;
    [SerializeField] CCDarkMode darkModeController;
    [SerializeField] EntranceScript entrance;
    [SerializeField] Transform[] points;
    [SerializeField] GameObject projectile;
}
