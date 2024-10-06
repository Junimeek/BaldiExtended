using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorPrompt : MonoBehaviour
{
    private void Start()
    {
        this.ChangeText(0);
        this.promptColor = this.promptBG.GetComponent<Image>();
    }

    private void LateUpdate()
    {
        this.mousePos = Input.mousePosition;
        this.promptParent.anchoredPosition = mousePos;
    }

    public void ChangeText(float id)
    {
        if (id == 0)
        {
            this.positionMultiplier = Screen.height / 550f;
            this.text.text = string.Empty;
            this.promptParent.pivot = new Vector2(-0.3f * this.positionMultiplier, 0.4f * this.positionMultiplier);
            this.promptColor.color = new Color(0f, 0f, 0f, 0f);
            this.promptBG.localScale = this.GetBoxScale();
            this.text.fontSize = this.GetFontSize();
            this.text.rectTransform.anchoredPosition = this.GetTextOffset();
            return;
        }
        
        this.promptParent.pivot = new Vector2(-0.3f * this.positionMultiplier, 0.4f * this.positionMultiplier);
        this.promptColor.color = Color.white;

        switch(id)
        {
            case 1.1f:
                this.text.text = "Start playing the game!";
                break;
            case 1.2f:
                this.promptParent.pivot = new Vector2(4f * this.positionMultiplier, 0.4f * this.positionMultiplier);
                this.text.text = "Access the main menu";
                break;
            case 1.3f:
                this.promptParent.pivot = new Vector2(2.6f * this.positionMultiplier, 1.1f * this.positionMultiplier);
                this.text.text = "Quit the game";
                break;
            case 2.1f:
                this.text.text = "If this is enabled and Baldi catches you, the game will immediately restart instead of returning to the main menu.";
                break;
            case 2.2f:
                this.text.text = "A chalkboard will appear in the bottom right corner to tell you:\nDetention reason, Endless Notebook respawning";
                break;
            case 2.3f:
                this.text.text = "Censors swearing in minor details";
                break;
            case 2.4f:
                this.text.text = "If this is enabled, Schoolhouse Trouble will play in the background during normal Story Mode gameplay.";
                break;
            case 4.1f:
                this.promptParent.pivot = new Vector2(3f * this.positionMultiplier, -2f * this.positionMultiplier);
                this.text.text = "DEVELOPMENT NOTE: Eventually, this challenge will be locked behind requiring getting the secret ending. Beating this before then, however, will keep this challenge unlocked.";
                break;
            
        }

        Vector2 boxSize = this.GetBoxSize(this.text.text);
        this.promptBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, boxSize.x);
        this.promptBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, boxSize.y);
        this.text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.GetTextWidth());
    }

    private Vector3 GetBoxSize(string inputText)
    {
        return new Vector3(Mathf.Clamp(inputText.Length, 12f, 35f) * 12f, Mathf.FloorToInt(inputText.Length / 35f) * 27f + 43f, 1f);
    }

    private Vector3 GetBoxScale()
    {
        return new Vector3(this.positionMultiplier * 1.1f, this.positionMultiplier, 1);
    }

    private float GetFontSize()
    {
        float size = Screen.height / 27.5f * 1.1f;
        return size;
    }

    private float GetTextWidth()
    {
        float width = (this.promptBG.rect.width - 50f) * this.GetBoxScale().x;
        return width;
    }

    private Vector3 GetTextOffset()
    {
        return new Vector3(this.positionMultiplier * 5.8f + 6f, this.positionMultiplier * 0.7f - 6f, 1f);
    }

    [SerializeField] private Vector2 mousePos;
    [SerializeField] private RectTransform promptParent;
    [SerializeField] private RectTransform promptBG;
    [SerializeField] private Image promptColor;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float positionMultiplier;
}
