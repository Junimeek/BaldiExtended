using UnityEngine;
using UnityEngine.UI;

public class ItemImageScript : MonoBehaviour
{
	private void Update()
	{
		if (this.gc != null) {
			Texture texture = this.gc.itemSlot[this.gc.itemSelected].texture;
			if (texture == this.blankSprite)
				this.sprite.texture = this.noItemSprite;
			else
				this.sprite.texture = texture;
		}
		else
			this.sprite.texture = this.noItemSprite;
	}

	public RawImage sprite;
	[SerializeField] private Texture noItemSprite;
	[SerializeField] private Texture blankSprite;
	public GameControllerScript gc;
}
