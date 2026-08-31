using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinItemInstance : MonoBehaviour
{
    void Start()
    {
        
    }

    public void SetItemImageSprite(Sprite itemSprite)
    {
        this.image.sprite = itemSprite;
    }

    public Image GetItemImage()
    {
        return this.image;
    }

    public void SetItemCount(int itemsUsed)
    {
        if (itemsUsed <= 1)
            Destroy(this.itemCount);
        else
            this.itemText.text = "x" + itemsUsed;
        
        if (itemsUsed >= 10)
            this.countBG.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70f);
    }

    [SerializeField] Image image;
    [SerializeField] GameObject itemCount;
    [SerializeField] Image countBG;
    [SerializeField] TMP_Text itemText;
}
