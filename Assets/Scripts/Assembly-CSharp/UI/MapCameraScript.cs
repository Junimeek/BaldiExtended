using System;
using UnityEngine;

public class MapCameraScript : MonoBehaviour
{
    private void Start()
	{
		this.initialOffset = base.transform.position - this.player.transform.position;
        this.isMapOn = true;
        this.ToggleMap();

        PickupScript[] fetchedItems = itemParent.GetComponentsInChildren<PickupScript>();
        Array.Resize(ref this.itemList, fetchedItems.Length);

        for (int i = 0; i < this.itemList.Length; i++)
        {
            this.itemList[i] = fetchedItems[i];
        }

        this.HideCharacters();
	}

    private void HideCharacters()
    {
        if (!FindObjectOfType<GameControllerScript>().ignoreInitializationChecks)
        {
            this.baldiSprite.color = new Color(1f, 1f, 1f, 0f);
            this.playtimeSprite.color = new Color(1f, 1f, 1f, 0f);
            this.craftersSprite.color = new Color(1f, 1f, 1f, 0f);
            this.sweepSprite.color = new Color(1f, 1f, 1f, 0f);
            this.princeySprite.color = new Color(1f, 1f, 1f, 0f);
            this.prizeSprite.color = new Color(1f, 1f, 1f, 0f);
            this.bullySprite.color = new Color(1f, 1f, 1f, 0f);
        }
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
            this.offset = this.initialOffset - new Vector3(0f, 40f, 0f);
            this.playerIcon.transform.position = new Vector3(player.transform.position.x, this.iconOffset.y, player.transform.position.z);
        }
        else
        {
            this.isMapOn = true;
            this.offset = this.initialOffset - new Vector3(0f, 30f, 0);
            this.playerIcon.transform.position = new Vector3(player.transform.position.x, this.iconOffset.y, player.transform.position.z);
        }
    }

    public void UpgradeMap(int upgrade)
    {
        switch(upgrade)
        {
            case 1:
                this.gameObject.GetComponent<Camera>().orthographicSize = 100f;
                break;
            case 2:
                for (int i = 0; i < this.itemList.Length; i++)
                    this.itemList[i].mapIcon.sprite = itemList[i].mapSprite;
                break;
            case 3:
                this.baldiSprite.color = new Color(1f, 1f, 1f, 1f);
                this.playtimeSprite.color = new Color(1f, 1f, 1f, 1f);
                this.craftersSprite.color = new Color(1f, 1f, 1f, 1f);
                this.sweepSprite.color = new Color(1f, 1f, 1f, 1f);
                this.princeySprite.color = new Color(1f, 1f, 1f, 1f);
                this.prizeSprite.color = new Color(1f, 1f, 1f, 1f);
                this.bullySprite.color = new Color(1f, 1f, 1f, 1f);
                break;
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
    [SerializeField] private GameObject itemParent;
    [SerializeField] private PickupScript[] itemList;
    [SerializeField] private SpriteRenderer baldiSprite;
    [SerializeField] private SpriteRenderer playtimeSprite;
    [SerializeField] private SpriteRenderer craftersSprite;
    [SerializeField] private SpriteRenderer sweepSprite;
    [SerializeField] private SpriteRenderer princeySprite;
    [SerializeField] private SpriteRenderer prizeSprite;
    [SerializeField] private SpriteRenderer bullySprite;
    private bool isMapOn;
}
