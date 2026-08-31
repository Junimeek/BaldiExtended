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
            case "ClassicDark":
                this.image.sprite = this.backgrounds[4];
                foreach (TMP_Text i in this.staticTexts)
                    i.color = Color.white;
                break;
            case "Mitakihara":
                this.image.sprite = this.backgrounds[5];
                break;
            default:
                this.image.sprite = this.backgrounds[0];
                break;
        }

        this.mapText.text = MapName();
        this.settingsText.text = SelectedSettings();

        StartCoroutine(this.LoadMap());
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
            case "Mitakihara":
                return "Loading map: MITAKIHARA";
            default:
                return "Failed to load map. The game must be closed.";
        }
    }
    private string SelectedSettings()
    {
        if (container.curMap == "ClassicDark")
            return "HARD MODE\nNO CHARACTERS\n\nN\n       ULL";
        else
        {
            string returnedSettings = string.Empty;

            if (container.safeMode == 1)
                returnedSettings = returnedSettings + "SAFE MODE\n";
            if (container.difficultMath == 1)
                returnedSettings = returnedSettings + "DIFFICULT MATH\n";

            if (returnedSettings == string.Empty)
                return "None";
            else
                return returnedSettings;
        }
    }

    [SerializeField] SettingsContainer container;
    [SerializeField] string map;
    [SerializeField] TMP_Text percentage;
    [SerializeField] Slider slider;
    [SerializeField] Image image;
    [SerializeField] Sprite[] backgrounds;
    [SerializeField] TMP_Text mapText;
    [SerializeField] TMP_Text settingsText;
    [SerializeField] TMP_Text[] staticTexts;
}
