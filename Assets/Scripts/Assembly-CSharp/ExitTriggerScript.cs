﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000BF RID: 191
public class ExitTriggerScript : MonoBehaviour
{
	private void Start()
	{
		speedrunTimer = FindObjectOfType<SpeedrunTimer>();
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x000219A0 File Offset: 0x0001FDA0
	private void OnTriggerEnter(Collider other)
	{
		if (this.gc.notebooks >= this.gc.daFinalBookCount & other.tag == "Player")
		{
			speedrunTimer.allowTime = false;
			PlayerPrefs.SetFloat("LastTime", this.speedrunTimer.totalTime);

			if (this.gc.failedNotebooks >= this.gc.daFinalBookCount) //If the player got all the problems wrong on all the 7 notebooks
			{
				//SceneManager.LoadSceneAsync("SecretMap"); //Go to the secret ending
				sceneLoader.LoadTheScene("SecretMap", 0);
			}
			else
			{
				sceneLoader.LoadTheScene("Results", 0);
				//SceneManager.LoadSceneAsync("Results"); //Go to the win screen
			}
		}
	}

	// Token: 0x040005F6 RID: 1526
	public GameControllerScript gc;
	[SerializeField] private DebugSceneLoader sceneLoader;
	[SerializeField] private SpeedrunTimer speedrunTimer;
}
