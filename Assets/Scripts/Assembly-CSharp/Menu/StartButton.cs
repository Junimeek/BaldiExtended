using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.MPE;

public class StartButton : MonoBehaviour
{
	public void SelectMap(string themap)
	{
		container = FindObjectOfType<SettingsContainer>();
		container.curMap = themap;
		container.SaveToRegistry("map");
		this.gpMenu.SetActive(true);
		this.mapMenu.SetActive(false);
	}

	public void SelectChallengeMap(string themap)
	{
		container = FindObjectOfType<SettingsContainer>();
		container.curMap = themap;
		container.SaveToRegistry("map");
		this.mapMenu.SetActive(false);
		this.blankOverlay.SetActive(true);
	}

	public void StartGame()
	{
		container = FindObjectOfType<SettingsContainer>();
		this.gpMenu.SetActive(false);
		this.blankOverlay.SetActive(true);
	}

	public void SetGamePref()
	{
		switch(this.currentMode)
		{
			case Mode.Endless:
				PlayerPrefs.SetString("CurrentMode", "endless");
				this.EnableMenu(1);
				break;
			case Mode.Challenge:
				PlayerPrefs.SetString("CurrentMode", "challenge");
				this.EnableMenu(2);
				break;
			default:
				PlayerPrefs.SetString("CurrentMode", "story");
				this.EnableMenu(1);
				break;
		}
		
	}

	private void EnableMenu(int menuID)
	{
		switch(menuID)
		{
			case 2:
				this.storyMenu.SetActive(false);
				this.challengeMenu.SetActive(true);
				break;
			default:
				this.mapMenu.SetActive(true);
				this.storyMenu.SetActive(false);
				break;
		}
	}

	public Mode currentMode;
	[SerializeField] private SettingsContainer container;
	[SerializeField] private GameObject storyMenu;
	[SerializeField] private GameObject gpMenu;
	[SerializeField] private GameObject mapMenu;
	[SerializeField] private GameObject challengeMenu;
	[SerializeField] private GameObject blankOverlay;
	public enum Mode
	{
		Story, Endless, Challenge
	}
}
