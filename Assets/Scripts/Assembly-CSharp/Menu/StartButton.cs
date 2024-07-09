using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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

	public void StartGame()
	{
		container = FindObjectOfType<SettingsContainer>();
		this.baldiLoadScreen.SetActive(true);
		this.gpMenu.SetActive(false);
		this.sceneLoader.LoadTheScene(container.curMap, 1);
	}

	public void SetGamePref()
	{
		if (this.currentMode == StartButton.Mode.Story)
		{
			PlayerPrefs.SetString("CurrentMode", "story");
		}
		else
		{
			PlayerPrefs.SetString("CurrentMode", "endless");
		}
		this.mapMenu.SetActive(true);
		this.storyMenu.SetActive(false);
	}

	public StartButton.Mode currentMode;
	[SerializeField] private DebugSceneLoader sceneLoader;
	[SerializeField] private SettingsContainer container;
	[SerializeField] private GameObject baldiLoadScreen;
	[SerializeField] private GameObject storyMenu;
	[SerializeField] private GameObject gpMenu;
	[SerializeField] private GameObject mapMenu;
	public enum Mode
	{
		Story,
		Endless
	}
}
