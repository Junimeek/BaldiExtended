using System;
using System.Collections;
//using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000D4 RID: 212
public class WarningScreenScript : MonoBehaviour
{
	// Token: 0x060009E5 RID: 2533 RVA: 0x00026753 File Offset: 0x00024B53
	private void Start()
	{
		//this.player = ReInput.players.GetPlayer(0);
		partyAudio.Play();
		loadingManager = FindObjectOfType<LoadingManager>();
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00026766 File Offset: 0x00024B66
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			loadingManager.LoadNewScene("MainMenu", 1);
		}
	}

	// Token: 0x04000719 RID: 1817
	//public Player player;

	[SerializeField] private AudioSource partyAudio;
	[SerializeField] private LoadingManager loadingManager;
}
