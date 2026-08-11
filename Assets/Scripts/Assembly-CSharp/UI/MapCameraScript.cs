using System.Collections;
using UnityEngine;

public class MapCameraScript : MonoBehaviour
{
    void Start()
	{
        gc = FindObjectOfType<GameControllerScript>();
		this.initialOffset = base.transform.position - this.player.transform.position;
        this.isMapOn = true;
        this.ToggleMap();

        PickupScript[] fetchedItems = itemParent.GetComponentsInChildren<PickupScript>();
        this.itemList = new PickupScript[fetchedItems.Length];

        for (int i = 0; i < this.itemList.Length; i++)
            this.itemList[i] = fetchedItems[i];

        this.HideCharacters();
	}

    void HideCharacters()
    {
        if (!FindObjectOfType<GameControllerScript>().ignoreInitializationChecks)
        {
            this.baldiSprite.color = Color.clear;
            this.playtimeSprite.color = Color.clear;
            this.craftersSprite.color = Color.clear;
            this.sweepSprite.color = Color.clear;
            this.princeySprite.color = Color.clear;
            this.prizeSprite.color = Color.clear;
            this.bullySprite.color = Color.clear;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (gc.isQuickMapToggle)
                this.ToggleMap();
            else
                StartCoroutine(this.HoldMapDisplay());
        }
        this.playerIcon.transform.position = this.player.transform.position + this.iconOffset;
    }

    void LateUpdate()
    {
        base.transform.position = this.player.transform.position + this.offset;
    }

    void ToggleMap()
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
            this.offset = this.initialOffset - new Vector3(0f, 25f, 0);
            this.playerIcon.transform.position = new Vector3(player.transform.position.x, this.iconOffset.y, player.transform.position.z);
        }
    }

    IEnumerator HoldMapDisplay()
    {
        this.ToggleMap();

        while (Input.GetKey(KeyCode.Tab))
            yield return null;
        
        this.ToggleMap();
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
                this.baldiSprite.color = Color.white;
                this.playtimeSprite.color = Color.white;
                this.craftersSprite.color = Color.white;
                this.sweepSprite.color = Color.white;
                this.princeySprite.color = Color.white;
                this.prizeSprite.color = Color.white;
                this.bullySprite.color = Color.white;
                break;
        }
    }

    public void DisableAllItems()
    {
        foreach (PickupScript i in this.itemList)
            i.gameObject.SetActive(false);
    }

    [SerializeField] PlayerScript player;
    GameControllerScript gc;
    [SerializeField] GameObject playerIcon;
    [SerializeField] Vector3 offset;
    Vector3 initialOffset;
    [SerializeField] Vector3 iconOffset;
    [SerializeField] GameObject itemParent;
    [SerializeField] PickupScript[] itemList;
    [SerializeField] SpriteRenderer baldiSprite;
    [SerializeField] SpriteRenderer playtimeSprite;
    [SerializeField] SpriteRenderer craftersSprite;
    [SerializeField] SpriteRenderer sweepSprite;
    [SerializeField] SpriteRenderer princeySprite;
    [SerializeField] SpriteRenderer prizeSprite;
    [SerializeField] SpriteRenderer bullySprite;
    bool isMapOn;
}
