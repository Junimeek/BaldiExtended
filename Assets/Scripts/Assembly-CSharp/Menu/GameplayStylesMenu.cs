using UnityEngine;
using UnityEngine.UI;

public class GameplayStylesMenu : MonoBehaviour
{
    private void OnEnable()
    {
        if (container == null)
            container = FindObjectOfType<SettingsContainer>();

        if (container.safeMode == 1)
            safeModeToggle.isOn = true;
        else
            safeModeToggle.isOn = false;

        if (container.difficultMath == 1)
            difficultMathToggle.isOn = true;
        else
            difficultMathToggle.isOn = false;

        switch (container.curMap)
        {
            case "ClassicExtended":
                this.mapIcon.sprite = this.extendedSprite;
                break;
            case "JuniperHills":
                this.mapIcon.sprite = this.juniSprite;
                break;
            case "Mitakihara":
                this.mapIcon.sprite = this.mitaSprite;
                break;
            default:
                this.mapIcon.sprite = this.classicSprite;
                break;
        }
    }

    private void OnDisable()
    {
        if (safeModeToggle.isOn)
            container.safeMode = 1;
        else
            container.safeMode = 0;

        if (difficultMathToggle.isOn)
            container.difficultMath = 1;
        else
            container.difficultMath = 0;

        container.SaveToRegistry("gamestyles");
    }

    [SerializeField] SettingsContainer container;
    [SerializeField] Toggle safeModeToggle;
    [SerializeField] Toggle difficultMathToggle;
    [SerializeField] Image mapIcon;
    [SerializeField] Sprite classicSprite;
    [SerializeField] Sprite extendedSprite;
    [SerializeField] Sprite juniSprite;
    [SerializeField] Sprite mitaSprite;
}