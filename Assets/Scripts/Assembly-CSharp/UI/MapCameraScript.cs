using UnityEngine;

public class MapCameraScript : MonoBehaviour
{
    private void Start()
	{
		this.initialOffset = base.transform.position - this.player.transform.position;
        this.isMapOn = true;
        this.ToggleMap();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) this.ToggleMap();
        this.playerIcon.transform.position = this.player.transform.position + this.iconOffset;
    }

    private void ToggleMap()
    {
        if (this.isMapOn)
        {
            this.isMapOn = false;
            this.offset = this.initialOffset - new Vector3(0f, 5f, 0f);
            this.playerIcon.transform.position = new Vector3(player.transform.position.x, this.iconOffset.y, player.transform.position.z);
        }
        else
        {
            this.isMapOn = true;
            this.offset = this.initialOffset;
            this.playerIcon.transform.position = new Vector3(player.transform.position.x, this.iconOffset.y, player.transform.position.z);
        }
    }

    private void LateUpdate()
    {
        base.transform.position = this.player.transform.position + this.offset;
    }

    [SerializeField] private PlayerScript player;
    [SerializeField] private GameObject playerIcon;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 initialOffset;
    [SerializeField] private Vector3 iconOffset;
    private bool isMapOn;
}
