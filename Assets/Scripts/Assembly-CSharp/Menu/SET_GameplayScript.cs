using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SET_GameplayScript : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        this.sensitivitySlider.value = data.turnSensitivity;
        this.instantReset.isOn = data.isInstantReset;
        this.notifBoard.isOn = data.isNotifBoard;
    }

    public void SaveData(GameData data)
    {
        data.turnSensitivity = this.sensitivitySlider.value;
        data.isInstantReset = this.instantReset.isOn;
        data.isNotifBoard = this.notifBoard.isOn;
    }

    private void OnEnable()
    {
        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataPersistenceManager>();
        }
        
        Debug.LogWarning("Gameplay Settings");
        dataManager.LoadGame();
    }

    private void Update()
    {
        this.sensitivityText.text = (this.sensitivitySlider.value*10f).ToString("0") + "%";
    }

    [SerializeField] private DataPersistenceManager dataManager;
    [SerializeField] private DelayedSave delayedSave;


    [Header("Sliders")]
    [SerializeField] private Slider sensitivitySlider;

    [Header("Slider Percentage Text")]
    [SerializeField] private TMP_Text sensitivityText;

    [Header("Toggles")]
    [SerializeField] private Toggle instantReset;
    [SerializeField] private Toggle notifBoard;
}
