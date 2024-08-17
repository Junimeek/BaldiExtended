using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuLoadingScreen : MonoBehaviour
{
    private void OnEnable()
    {
        this.map = container.curMap;

        switch(this.map)
        {
            case "Classic":
                this.image.sprite = this.backgrounds[1];
                break;
            case "ClassicExtended":
                this.image.sprite = this.backgrounds[2];
                break;
            case "JuniperHills":
                this.image.sprite = this.backgrounds[3];
                break;
            default:
                this.image.sprite = this.backgrounds[0];
                break;
        }

        this.mapText.text = MapName();
        this.settingsText.text = SelectedSettings();

        this.StartCoroutine(this.LoadMap());
    }

    private IEnumerator LoadMap()
    {
        this.slider.value = 0f;
        float curPercent;

        AsyncOperation op = SceneManager.LoadSceneAsync(this.map);

        while (!op.isDone)
        {
            curPercent = Mathf.RoundToInt(op.progress/0.9f*100f);
            this.slider.value = curPercent;
            this.percentage.text = curPercent + "%";
            yield return null;
        }
    }

    private string MapName()
    {
        switch(this.map)
        {
            case "Classic":
                return "Loading map: CLASSIC";
            case "ClassicExtended":
                return "Loading map: CLASSIC EXTENDED";
            case "JuniperHills":
                return "Loading map: JUNIPER HILLS";
            case "ClassicDark":
                return "Loading challenge: DARK MODE";
            default:
                return "Failed to load map. The game must be closed.";
        }
    }
    private string SelectedSettings()
    {
        if (container.curMap == "ClassicDark")
            return "HARD MODE\nNO CHARACTERS\nN\n\n       ULL";
        else if (container.safeMode == 1 && container.difficultMath == 1)
            return "SAFE MODE\nDIFFICULT MATH";
        else if (container.safeMode == 1)
            return "SAFE MODE";
        else if (container.difficultMath == 1)
            return "DIFFICULT MATH";
        else
            return "None";
    }

    [SerializeField] private SettingsContainer container;
    [SerializeField] private string map;
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private Slider slider;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] backgrounds;
    [SerializeField] private TMP_Text mapText;
    [SerializeField] private TMP_Text settingsText;
}
