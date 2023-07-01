using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SET_AudioScript : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        this.voiceSlider.value = data.volVoice;
        this.bgmSlider.value = data.volBGM;
        this.sfxSlider.value = data.volSFX;
        this.newMusic.isOn = data.isNewMusic;
    }

    public void SaveData(GameData data)
    {
        data.volVoice = this.voiceSlider.value;
        data.volBGM = this.bgmSlider.value;
        data.volSFX = this.sfxSlider.value;
        data.isNewMusic = this.newMusic.isOn;
    }

    private void OnEnable()
    {
        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataPersistenceManager>();
        }
        
        Debug.LogWarning("Audio Settings");
        dataManager.LoadGame();
    }

    private void Update()
    {
        this.voiceVolume = PercentConversion(voiceSlider.value);
        this.bgmVolume = PercentConversion(bgmSlider.value);
        this.sfxVolume = PercentConversion(sfxSlider.value);
        this.voiceText.text = voiceVolume.ToString() + "%";
        this.bgmText.text = bgmVolume.ToString() + "%";
        this.sfxText.text = sfxVolume.ToString() + "%";
    }

    private int PercentConversion(float input)
    {
        int final = 0;
        final = (int)(((25*input)/6)+100);
        return final;
    }

    [SerializeField] private DataPersistenceManager dataManager;
    [SerializeField] private DelayedSave delayedSave;

    [Header("Sliders")]
    [SerializeField] private Slider voiceSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Slider Percentage Text")]
    [SerializeField] private TMP_Text voiceText;
    [SerializeField] private TMP_Text bgmText;
    [SerializeField] private TMP_Text sfxText;
    private int voiceVolume;
    private int bgmVolume;
    private int sfxVolume;

    [Header("Toggles")]
    [SerializeField] private Toggle newMusic;
}
