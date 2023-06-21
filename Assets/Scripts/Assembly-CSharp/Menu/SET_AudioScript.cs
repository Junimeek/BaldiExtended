using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SET_AudioScript : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {

    }

    public void SaveData(GameData data)
    {

    }

    private void OnEnable()
    {
        dataManager = FindObjectOfType<DataPersistenceManager>();
        Debug.LogWarning("Audio Settings");
        dataManager.LoadGame();
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

    [Header("Toggles")]
    [SerializeField] private Toggle newMusic;
}
