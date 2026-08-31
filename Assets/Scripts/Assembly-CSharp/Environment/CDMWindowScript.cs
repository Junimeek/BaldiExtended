using UnityEngine;

public class CDMWindowScript : MonoBehaviour
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
        if (other.gameObject.name == "Baldi" && !this.isBroken)
            BreakWindow();
    }

    public void BreakWindow()
    {
        this.isBroken = true;

        for (int i = 0; i < 2; i++)
        {
            this.barriers[i].enabled = false;
            this.windows[i].material = this.brokenMatierial;
        }

        Destroy(this.agentObstacle);
        this.audioDevice.Play();
    }

    public void ToggleAgentObstacle(bool isTurnOn)
    {
        if (this.agentObstacle != null)
            this.agentObstacle.SetActive(isTurnOn);
    }

    [SerializeField] MeshCollider[] barriers;
    [SerializeField] MeshRenderer[] windows;
    [SerializeField] Material brokenMatierial;
    [SerializeField] AudioSource audioDevice;
    public GameObject agentObstacle;
    [HideInInspector] public bool isBroken;
    PlayerScript player;
    BaldiScript baldi;
}
