using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayStylesMenu : MonoBehaviour
{
    private void OnEnable()
    {
        if (container == null) container = FindObjectOfType<SettingsContainer>();

        if (container.safeMode == 1) safeModeToggle.isOn = true;
        else safeModeToggle.isOn = false;

        if (container.difficultMath == 1) difficultMathToggle.isOn = true;
        else difficultMathToggle.isOn = false;

        if (container.curMap == "ClassicExtended")
            this.mapIcon.sprite = this.extendedSprite;
        else if (container.curMap == "JuniperHills")
            this.mapIcon.sprite = this.juniSprite;
        else
            this.mapIcon.sprite = this.classicSprite;
    }

    private void OnDisable()
    {
        if (safeModeToggle.isOn) container.safeMode = 1;
        else container.safeMode = 0;

        if (difficultMathToggle.isOn) container.difficultMath = 1;
        else container.difficultMath = 0;

        container.SaveToRegistry("gamestyles");
    }

    [SerializeField] private SettingsContainer container;
    [SerializeField] private Toggle safeModeToggle;
    [SerializeField] private Toggle difficultMathToggle;
    [SerializeField] private Image mapIcon;
    [SerializeField] private Sprite classicSprite;
    [SerializeField] private Sprite extendedSprite;
    [SerializeField] private Sprite juniSprite;
}