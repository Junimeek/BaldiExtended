using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Token: 0x0200001B RID: 27
public class OptionsManager : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerPrefs.HasKey("OptionsSet"))
		{
			if (PlayerPrefs.GetInt("Rumble") == 1) this.rumble.isOn = true;
			else this.rumble.isOn = false;
			
			if (PlayerPrefs.GetInt("AnalogMove") == 1) this.analog.isOn = true;
			else this.analog.isOn = false;
		}
		else
		{
			PlayerPrefs.SetInt("OptionsSet", 1);
			PlayerPrefs.SetInt("Rumble", 0);
			PlayerPrefs.SetInt("AnalogMove", 0);
		}
	}

	private void Update()
	{
		if (this.rumble.isOn) PlayerPrefs.SetInt("Rumble", 1);
		else PlayerPrefs.SetInt("Rumble", 0);

		if (this.analog.isOn) PlayerPrefs.SetInt("AnalogMove", 1);
		else PlayerPrefs.SetInt("AnalogMove", 0);

		/*
		this.sensitivityPercent = (int)(sensitivitySlider.value*10);
		this.voiceVolume = (int)(((25*voiceSlider.value)/6)+100);
		this.bgmVolume = (int)(((25*bgmSlider.value)/6)+100);
		this.sfxVolume = (int)(((25*sfxSlider.value)/6)+100);
		this.sensitivityText.text = sensitivityPercent.ToString() + "%";
		this.voiceText.text = voiceVolume.ToString() + "%";
		this.bgmText.text = bgmVolume.ToString() + "%";
		this.sfxText.text = sfxVolume.ToString() + "%";
		*/
	}

	public Slider sensitivitySlider;
	public Slider voiceSlider;
	public Slider bgmSlider;
	public Slider sfxSlider;

	public Toggle rumble;

	public Toggle analog;

	public Toggle instantReset;
	public Toggle additionalMusic;
	public Toggle notifToggle;

	[SerializeField] private float sensitivityPercent;
	[SerializeField] private TMP_Text sensitivityText;
	[SerializeField] private float voiceVolume;
	[SerializeField] private TMP_Text voiceText;
	[SerializeField] private float bgmVolume;
	[SerializeField] private TMP_Text bgmText;
	[SerializeField] private float sfxVolume;
	[SerializeField] private TMP_Text sfxText;
}
