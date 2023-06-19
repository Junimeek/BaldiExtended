using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Token: 0x0200001B RID: 27
public class OptionsManager : MonoBehaviour
{
	// Token: 0x06000061 RID: 97 RVA: 0x000037B4 File Offset: 0x00001BB4
	private void Awake()
	{
		if (PlayerPrefs.HasKey("OptionsSet"))
		{
			this.sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
			this.voiceSlider.value = PlayerPrefs.GetFloat("VolumeVoice");
			this.bgmSlider.value = PlayerPrefs.GetFloat("VolumeBGM");

			if (PlayerPrefs.GetInt("Rumble") == 1) this.rumble.isOn = true;
			else this.rumble.isOn = false;
			
			if (PlayerPrefs.GetInt("AnalogMove") == 1) this.analog.isOn = true;
			else this.analog.isOn = false;

			if (PlayerPrefs.GetInt("InstantReset") == 1) this.instantReset.isOn = true;
			else this.instantReset.isOn = false;

			if (PlayerPrefs.GetInt("AdditionalMusic") == 1) this.additionalMusic.isOn = true;
			else this.additionalMusic.isOn = false;

			if (PlayerPrefs.GetInt("NotifBoard") == 1) this.notifToggle.isOn = true;
			else this.notifToggle.isOn = false;
		}
		else
		{
			PlayerPrefs.SetInt("OptionsSet", 1);
			PlayerPrefs.SetInt("Rumble", 0);
			PlayerPrefs.SetInt("AnalogMove", 0);
			PlayerPrefs.SetInt("InstantReset", 0);
			PlayerPrefs.SetInt("AdditionalMusic", 0);
			PlayerPrefs.SetInt("NotifBoard", 1);
			PlayerPrefs.SetFloat("MouseSensitivity", 0.2f);
			PlayerPrefs.SetFloat("VolumeVoice", 0.9f);
			PlayerPrefs.SetFloat("VolumeBGM", 0.9f);
			PlayerPrefs.SetFloat("VolumeSFX", 0.9f);
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00003850 File Offset: 0x00001C50
	private void Update()
	{
		PlayerPrefs.SetFloat("MouseSensitivity", this.sensitivitySlider.value);
		PlayerPrefs.SetFloat("VolumeVoice", this.voiceSlider.value);
		PlayerPrefs.SetFloat("VolumeBGM", this.bgmSlider.value);

		if (this.rumble.isOn) PlayerPrefs.SetInt("Rumble", 1);
		else PlayerPrefs.SetInt("Rumble", 0);

		if (this.analog.isOn) PlayerPrefs.SetInt("AnalogMove", 1);
		else PlayerPrefs.SetInt("AnalogMove", 0);

		if (this.instantReset.isOn) PlayerPrefs.SetInt("InstantReset", 1);
		else PlayerPrefs.SetInt("InstantReset", 0);

		if (this.additionalMusic.isOn) PlayerPrefs.SetInt("AdditionalMusic", 1);
		else PlayerPrefs.SetInt("AdditionalMusic", 0);

		if (this.notifToggle.isOn) PlayerPrefs.SetInt("NotifBoard", 1);
		else PlayerPrefs.SetInt("NotifBoard", 0);

		this.sensitivityPercent = (int)(sensitivitySlider.value*10);
		this.voiceVolume = (int)(voiceSlider.value*100);
		this.bgmVolume = (int)(bgmSlider.value*100);
		this.sensitivityText.text = sensitivityPercent.ToString() + "%";
		this.voiceText.text = voiceVolume.ToString() + "%";
		this.bgmText.text = bgmVolume.ToString() + "%";
	}

	// Token: 0x0400006F RID: 111
	public Slider sensitivitySlider;
	public Slider voiceSlider;
	public Slider bgmSlider;

	// Token: 0x04000070 RID: 112
	public Toggle rumble;

	// Token: 0x04000071 RID: 113
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
}
