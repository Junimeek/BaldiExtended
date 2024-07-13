using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInformationScript : MonoBehaviour
{
    private void Start()
    {
        this.image = this.gameObject.GetComponent<Image>();
        this.DeHightlightItem();
    }

    public void HighlightItem()
    {
        this.image.color = new Color(0f, 1f, 0f, 1f);
        this.DisplayItemText(this.itemText);
        this.DisplayHintText(this.hintText);
    }

    public void DeHightlightItem()
    {
        this.image.color = new Color(1f, 1f, 1f, 1f);
        this.DisplayItemText(string.Empty);
        this.DisplayHintText(string.Empty);
    }

    private void DisplayItemText(string thetext)
    {
        this.itemTextObject.text = thetext;
    }

    private void DisplayHintText(string thetext)
    {
        this.hintTextObject.text = thetext;
    }

    private Image image;
    [SerializeField] private TMP_Text itemTextObject;
    [SerializeField] private TMP_Text hintTextObject;
    [SerializeField] [TextArea(1,5)] private string itemText;
    [SerializeField] [TextArea(1,5)] private string hintText;
}
