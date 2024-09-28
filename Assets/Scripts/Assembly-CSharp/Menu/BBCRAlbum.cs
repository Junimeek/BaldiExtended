using UnityEngine;
using UnityEngine.UI;

public class BBCRAlbum : MonoBehaviour
{
    private void OnEnable()
    {
        int isActive = PlayerPrefs.GetInt("AdditionalMusic");
        if (isActive == 1) image.sprite = activeSprite;
        else image.sprite = inactiveSprite;
    }

    public void UpdateAlbum()
    {
        if (musicButton.isOn) image.sprite = activeSprite;
        else image.sprite = inactiveSprite;
    }

    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Toggle musicButton;
    [SerializeField] private Image image;
}