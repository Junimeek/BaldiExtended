using UnityEngine;

public class EntranceScript : MonoBehaviour
{
	private void Start()
	{
		this.isMapOn = true;
		this.ToggleMapIcon();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
			this.ToggleMapIcon();
	}

	public void Lower()
	{
		base.transform.position = base.transform.position - new Vector3(0f, 10f, 0f);
		
		if (this.gc.finaleMode && gc.exitsReached >= 1)
		{
			this.gc.UpdateExitCount();

			if (this.allowMap)
				this.wall.material = this.map;

			if (this.exitSign != null)
			{
				SpriteRenderer sprite = this.exitSign.GetComponent<SpriteRenderer>();
				sprite.sprite = this.greenExit;
			}
		}

		if (!this.gc.finaleMode)
			this.mapSprite.color = new Color(1f, 0f, 0f, this.mapSprite.color.a);
		else
		{
			this.mapSprite.color = new Color(1f, 0f, 0f, this.mapSprite.color.a);
			this.mapSprite.sprite = this.closedSprite;
		}
	}

	public void Raise()
	{
		if (!gc.isSafeMode)
			base.transform.position = base.transform.position + new Vector3(0f, 10f, 0f);
			
		this.mapSprite.color = new Color(0f, 1f, 0f, this.mapSprite.color.a);
	}

	private void ToggleMapIcon()
	{
		if (this.isMapOn)
		{
			this.isMapOn = false;
			this.mapSprite.color = new Color(this.mapSprite.color.r, this.mapSprite.color.g, this.mapSprite.color.b, 0f);
		}
		else
		{
			this.isMapOn = true;
			this.mapSprite.color = new Color(this.mapSprite.color.r, this.mapSprite.color.g, this.mapSprite.color.b, 1f);
		}
	}

	[SerializeField] private bool allowMap;
	public GameControllerScript gc;
	public Material map;
	public MeshRenderer wall;
	[SerializeField] private GameObject exitSign;
	[SerializeField] private Sprite greenExit;
	[SerializeField] private SpriteRenderer mapSprite;
	[SerializeField] private Sprite closedSprite;
	private bool isMapOn;
}
