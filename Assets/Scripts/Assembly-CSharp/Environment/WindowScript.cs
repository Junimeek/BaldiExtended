using UnityEngine;

public class WindowScript : MonoBehaviour
{
    void Start()
    {
        this.isBroken = false;
        GameControllerScript gc = FindObjectOfType<GameControllerScript>();
        this.player = gc.player;
        this.baldi = gc.baldiScrpt;
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player.CheckPlayerWindowState() && !this.isBroken)
        {
            BreakWindow();
            if (baldi.isActiveAndEnabled)
                baldi.AddNewSound(agentObstacle.transform.position, 3);
        }
    }

    public void FirstPrizeHit()
    {
        if (!this.isBroken)
        {
            if (baldi.isActiveAndEnabled)
                baldi.AddNewSound(agentObstacle.transform.position, 3);
            BreakWindow();
        }
    }

    public void BreakWindow()
    {
        this.isBroken = true;

        for (int i = 0; i < 2; i++)
        {
            this.barriers[i].enabled = false;
            this.windows[i].material = this.brokenMatierial;
        }

        if (this.poster.gameObject)
            this.poster.SetActive(false);
        this.audioDevice.Play();
        this.agentObstacle.transform.position += new Vector3(0f, 20f, 0f);
        base.gameObject.layer = 2;
    }

    [SerializeField] MeshCollider[] barriers;
    [SerializeField] MeshRenderer[] windows;
    [SerializeField] GameObject poster;
    [SerializeField] Material brokenMatierial;
    [SerializeField] AudioSource audioDevice;
    public GameObject agentObstacle;
    [HideInInspector] public bool isBroken;
    PlayerScript player;
    BaldiScript baldi;
}
