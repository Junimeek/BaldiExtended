using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000BE RID: 190
public class ExitButtonScript : MonoBehaviour
{
	private void Start()
	{
		waitForDialog = false;
		exitText.SetActive(false);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0002198E File Offset: 0x0001FD8E
	public void ExitGame()
	{
		if (!quitSound.isPlaying)
		{
			balIntro.Stop();
			quitSound.Play();
			exitText.SetActive(true);
			waitForDialog = true;
		}
	}

	private void Update()
	{
		if (waitForDialog)
		{
			if (!quitSound.isPlaying)
			{
				Debug.Log("Quit Game");
				Application.Quit();
			}
		}
	}

	[SerializeField] private bool waitForDialog;
	[SerializeField] private AudioSource quitSound;
	[SerializeField] private AudioSource balIntro;
	[SerializeField] private GameObject exitText;
}
