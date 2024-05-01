using UnityEngine;

public class EntranceScript : MonoBehaviour
{
	public void Lower()
	{
		base.transform.position = base.transform.position - new Vector3(0f, 10f, 0f);
		
		if (this.gc.finaleMode && gc.exitsReached >= 1)
		{
			if (allowMap) this.wall.material = this.map;
			if (this.exitSign != null)
			{
				SpriteRenderer sprite = this.exitSign.GetComponent<SpriteRenderer>();
				sprite.sprite = this.greenExit;
			}
		}
	}

	public void Raise()
	{
		if (!gc.isSafeMode)
			base.transform.position = base.transform.position + new Vector3(0f, 10f, 0f);
	}

	[SerializeField] private bool allowMap;
	public GameControllerScript gc;

	public Material map;

	public MeshRenderer wall;
	[SerializeField] private GameObject exitSign;
	[SerializeField] private Sprite greenExit;
}
