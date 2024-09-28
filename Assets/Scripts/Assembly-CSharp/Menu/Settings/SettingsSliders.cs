using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsSliders : MonoBehaviour
{
    public void UpdateSensitivityText()
    {
        StartCoroutine(ChangeSensitivityText());
    }

    public void UpdateVolumeText()
    {
        StartCoroutine(ChangeVolumeText());
    }

    private IEnumerator ChangeSensitivityText()
    {
        while (settingsScript.curSetting == "menuGP")
        {
            sensitivityText.text = (sensitivitySlider.value*10f).ToString("0") + "%";
            yield return null;
        }
    }

    private IEnumerator ChangeVolumeText()
    {
        while (settingsScript.curSetting == "menuAudio")
        {
            voiceText.text = ((int)((25f*voiceSlider.value/6f)+100f)).ToString("0") + "%";
            bgmText.text = ((int)((25f*bgmSlider.value/6f)+100f)).ToString("0") + "%";
            sfxText.text = ((int)((25f*sfxSlider.value/6f)+100f)).ToString("0") + "%";
            yield return null;
        }
    }

    [SerializeField] private SettingsLoader settingsScript;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider voiceSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Text sensitivityText;
    [SerializeField] private TMP_Text voiceText;
    [SerializeField] private TMP_Text bgmText;
    [SerializeField] private TMP_Text sfxText;
}
