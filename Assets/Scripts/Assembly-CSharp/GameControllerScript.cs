using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Threading;
using System.Runtime.CompilerServices;

public class GameControllerScript : MonoBehaviour
{
	/*
	public GameControllerScript()
	{
		float[] array = new float[5];
		array[0] = 107.5f;
		array[1] = 147.5f;
		array[2] = 187.5f;
		array[3] = 227.5f;
		array[4] = 267.5f;
		this.itemSelectOffset = array;
		//base..ctor();
	}
	*/
	

	private void Awake()
	{
		debugActions = FindObjectOfType<DebugMenuActions>();
		debugScreen = FindObjectOfType<DebugScreenSwitch>();
		audioManager = FindObjectOfType<AudioManager>();
		handIconScript = FindObjectOfType<HandIconScript>();
	}

	private void Start()
	{
		Debug.Log("Loaded " + PlayerPrefs.GetString("CurrentMap"));
		Debug.Log("Safe Mode: " + PlayerPrefs.GetInt("gps_safemode"));
		Debug.Log("Difficult Math: " + PlayerPrefs.GetInt("gps_difficultmath"));

		this.InitializeItemSlots();
		this.exitCountText.text = "0/" + this.entranceList.Length;
		this.exitCountGroup.SetActive(false);

		this.isParty = false;

		if (PlayerPrefs.GetInt("gps_safemode") == 1) this.isSafeMode = true;
		else this.isSafeMode = false;

		if (PlayerPrefs.GetInt("gps_difficultmath") == 1) this.isDifficultMath = true;
		else this.isDifficultMath = false;

		Scene curScene = SceneManager.GetActiveScene();
		string curSceneName = curScene.name;

		if (curSceneName == "SecretMap" && !this.isSafeMode)
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = new Color(0.79f, 0.79f, 0.79f);
			RenderSettings.ambientLight = new Color(0.86f, 0.86f, 0.86f);
		}
		else
		{
			RenderSettings.fog = false;
			RenderSettings.fogColor = new Color(1f, 1f, 1f);
			RenderSettings.ambientLight = new Color(1f, 1f, 1f);
		}

		this.cullingMask = this.camera.cullingMask; // Changes cullingMask in the Camera
		this.audioDevice = base.GetComponent<AudioSource>(); //Get the Audio Source
		this.mode = PlayerPrefs.GetString("CurrentMode"); //Get the current mode
		if (this.mode == "endless") //If it is endless mode
		{
			this.baldiScrpt.endless = true; //Set Baldi use his slightly changed endless anger system
		}
		this.curMap = PlayerPrefs.GetString("CurrentMap");
		MusicPlayer(1,1); //Play the school music
		this.LockMouse(); //Prevent the mouse from moving
		this.UpdateNotebookCount(); //Update the notebook count
		this.itemSelected = 0; //Set selection to item slot 0(the first item slot)
		this.gameOverDelay = 0.5f;
		this.UpdateDollarAmount(0f);
		StartCoroutine(this.WaitForQuarterDisable(true));

		this.InitializeScores();

		//this.speedrunTimer.allowTime = true;

		//debugScreen.DebugCloseMenu();
	}

	private void InitializeItemSlots() // investigate why this shit wont work
	{
		//this.itemSlotOffset = 10 - this.totalSlotCount;

		this.item = new int[this.totalSlotCount];
		this.slotForeground.texture = this.slotForegroundList[this.totalSlotCount - 1];

		float[] slotList = new float[10];
		slotList[0] = -92.5f;
		slotList[1] = -52.5f;
		slotList[2] = -12.5f;
		slotList[3] = 27.5f;
		slotList[4] = 67.5f;
		slotList[5] = 107.5f;
		slotList[6] = 147.5f;
		slotList[7] = 187.5f;
		slotList[8] = 227.5f;
		slotList[9] = 267f;

		this.itemSelect.anchoredPosition = new Vector3(slotList[10 - this.totalSlotCount], 128.5f, 0f);
		this.trashOverlay.anchoredPosition = new Vector3(slotList[10 - this.totalSlotCount], 128.5f, 0f);

		this.slotOffsetArray = new float[this.totalSlotCount];
		RawImage[] slotImages = new RawImage[this.totalSlotCount];

		for (int i = 10 - this.totalSlotCount; i < 10; i++)
		{
			this.slotOffsetArray[i - (10 - this.totalSlotCount)] = slotList[i];
			slotImages[i - (10 - this.totalSlotCount)] = this.itemSlot[i];
		}

		this.itemSelectOffset = this.slotOffsetArray;
		this.itemSlot = slotImages;

		float[] bgList = new float[10];
		bgList[0] = 392.5f;
		bgList[1] = 352.5f;
		bgList[2] = 312.5f;
		bgList[3] = 272.5f;
		bgList[4] = 232.5f;
		bgList[5] = 192.5f;
		bgList[6] = 152.5f;
		bgList[7] = 112.5f;
		bgList[8] = 72.5f;
		bgList[9] = 33f;

		this.itemBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bgList[10 - this.totalSlotCount]);
		this.itemSelected = 9 - this.totalSlotCount;

		if (this.forceQuarterPickup)
		{
			this.trashOverlay.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
			this.walletUnderlay.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
			this.walletForeground.SetActive(false);
			this.walletBackground.SetActive(false);
			this.dollarTextCenter.text = string.Empty;
			this.dollarTextTop.text = string.Empty;
			this.itemText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(267f, 97f, 0f);
		}
	}

	private void InitializeScores()
	{
		switch(this.mode)
		{
			default:
				switch(this.curMap)
				{
					case "ClassicExtended":
						this.highBooksDirectory = "highbooks_ClassicExtended";
						break;
					case "JuniperHills":
						this.highBooksDirectory = "highbooks_JuniperHills";
						break;
					default:
						this.highBooksDirectory = "highbooks_Classic";
						break;
				}
				this.highBooksScore = PlayerPrefs.GetInt(this.highBooksDirectory);
				break;
		}
	}

	private void Update()
	{
		if (!this.learningActive)
		{
			if (Input.GetButtonDown("Pause"))
			{
				if (!this.gamePaused) this.PauseGame();
				else this.UnpauseGame();
			}

			if (Input.GetKeyDown(KeyCode.Y) & this.gamePaused) this.ExitGame();
			else if (Input.GetKeyDown(KeyCode.N) & this.gamePaused) this.UnpauseGame();

			if (!this.gamePaused & Time.timeScale != 1f && !this.isSlowmo) Time.timeScale = 1f;

			if (Input.GetMouseButtonDown(1) && Time.timeScale != 0f) this.UseItem();

			if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f) this.DecreaseItemSelection();
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Time.timeScale != 0f) this.IncreaseItemSelection();

			if (Time.timeScale != 0f)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					this.itemSelected = 0;
					this.UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					this.itemSelected = 1;
					this.UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					this.itemSelected = 2;
					this.UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					this.itemSelected = 3;
					this.UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					this.itemSelected = 4;
					this.UpdateItemSelection();
				}
			}
		}
		else
		{
			if (Time.timeScale != 0f) Time.timeScale = 0f;
		}

		if (this.player.stamina > 0f)
		{
			this.staminaPercentText.text = this.player.stamina.ToString("0") + "%";
			this.staminaPercentText.color = Color.black;
		}
		else if (this.player.stamina <= 0f)
		{
			this.staminaPercentText.text = "YOU NEED REST!";
			this.staminaPercentText.color = Color.red;
		}

		if (this.player.gameOver)
		{
			this.cursorController.UnlockCursor();
			if (this.mode == "endless" && this.notebooks > this.highBooksScore && !this.highScoreText.activeSelf)
			{
				this.highScoreText.SetActive(true);
			}
			Time.timeScale = 0f;
			this.gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
			this.camera.farClipPlane = this.gameOverDelay * 400f; //Set camera farClip
			this.MusicPlayer(0,0);
			this.endlessMusic.Stop();
			if (!this.player.isSecret && !this.isScareStarted)
			{
				//this.audioDevice.PlayOneShot(this.aud_buzz);
				this.isScareStarted = true;
				int randomScare = Mathf.RoundToInt(UnityEngine.Random.Range(0, this.baldiJumpscareSounds.Length - 1));
				this.audioDevice.PlayOneShot(this.baldiJumpscareSounds[randomScare]);
			}

			if (PlayerPrefs.GetInt("Rumble") == 1)
			{

			}
			if (this.gameOverDelay <= 0f)
			{
				if (this.mode == "endless" && !this.isSafeMode)
				{
					if (this.notebooks > this.highBooksScore)
						PlayerPrefs.SetInt(this.highBooksDirectory, this.notebooks);

					PlayerPrefs.SetInt("CurrentBooks", this.notebooks);
				}

				if (!player.isSecret)
				{
					Time.timeScale = 1f;
					SceneManager.LoadScene("GameOver");
				}
				else if (player.isSecret)
				{
					Debug.Log("Game Quit");
					this.cursorController.UnlockCursor();
					Application.Quit();
				}
			}
		}

		if (this.finaleMode && !this.audioDevice.isPlaying && this.exitsReached == 2)
		{
			this.audioDevice.clip = this.chaosEarlyLoop;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
		}
		else if (this.finaleMode && !this.audioDevice.isPlaying && this.exitsReached == 3)
		{
			this.audioDevice.clip = this.chaosFinalLoop;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
		}

		if (Input.GetKeyDown(KeyCode.J))
		{
			return; // debug function. might use for a potential new item later
			if (!this.isSlowmo)
			{
				Time.timeScale = 0.5f;
				this.isSlowmo = true;
				Debug.Log("time is now " + Time.timeScale.ToString());
			}
			else
			{
				Time.timeScale = 1f;
				this.isSlowmo = false;
				Debug.Log("time is now " + Time.timeScale.ToString());
			}
		}

		if (this.handIconScript != null)
			CheckRaycast();
	}

	private void CheckRaycast()
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;
		float distance;

		if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider.name == "_DoorOut" && !(raycastHit.collider.tag == "SwingingDoor"))
			distance = 15f;
		else if (Physics.Raycast(ray, out raycastHit) && this.item[this.itemSelected] == 12
				&& (raycastHit.collider.name == "Playtime" || raycastHit.collider.name == "Gotta Sweep" || raycastHit.collider.name == "1st Prize"))
				distance = 20f;
		else distance = 10f;

		try
		{
			if (this.player.jumpRope)
			{
				if (this.item[this.itemSelected] == 9)
					handIconScript.ChangeIcon(8);
				else if (this.item[this.itemSelected] == 12)
					handIconScript.ChangeIcon(9);
			}
				
			else if (Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= distance)
			{
				if (this.itemSelected == 15 && this.partyLocation != null)
					handIconScript.ChangeIcon(10);
				else if (raycastHit.collider.name == "1st Prize" && this.item[this.itemSelected] == 9)
					handIconScript.ChangeIcon(8);
				else if (raycastHit.collider.name.Contains("Notebook") && raycastHit.collider.name != "Notebooks")
					handIconScript.ChangeIcon(2);
				else if (raycastHit.collider.name == "Payphone")
				{
					if (this.forceQuarterPickup)
						handIconScript.ChangeIcon(11);
					else
					{
						StartCoroutine(this.WaitForQuarterDisable(false));
						handIconScript.ChangeIcon(3);
					}
				}
				else if (raycastHit.collider.name == "TapePlayer")
				{
					if (this.item[this.itemSelected] == 6)
						handIconScript.ChangeIcon(5);
				}
				else if (raycastHit.collider.name.StartsWith("AlarmClock"))
					handIconScript.ChangeIcon(1);
				else if (raycastHit.collider.name == "_DoorOut")
				{
					if (raycastHit.collider.tag == "SwingingDoor"  && this.item[this.itemSelected] == 2)
						handIconScript.ChangeIcon(6);
					else if (raycastHit.collider.tag == "PrincipalDoor" && this.item[this.itemSelected] == 3)
						handIconScript.ChangeIcon(4);
					else if (this.item[this.itemSelected] == 8)
						handIconScript.ChangeIcon(7);
					else if (!(raycastHit.collider.tag == "SwingingDoor"))
						handIconScript.ChangeIcon(1);
				}
				else if (raycastHit.collider.name.StartsWith("VendingMachine"))
				{
					if (this.forceQuarterPickup)
						handIconScript.ChangeIcon(11);
					else
					{
						StartCoroutine(this.WaitForQuarterDisable(true));
						handIconScript.ChangeIcon(3);
					}
				}
				else if ((raycastHit.collider.name == "Playtime" || raycastHit.collider.name == "Gotta Sweep" || raycastHit.collider.name == "1st Prize")
				&& this.item[this.itemSelected] == 12)
					handIconScript.ChangeIcon(9);
				else if (raycastHit.collider.tag == "Item")
					handIconScript.ChangeIcon(1);
				else
					handIconScript.ChangeIcon(0);
			}
			else
				handIconScript.ChangeIcon(0);
		}
		catch
		{
			handIconScript.ChangeIcon(0);
			return;
		}
	}

	private IEnumerator WaitForQuarterDisable(bool isVendingMachine)
	{
		if (this.forceQuarterPickup)
			yield break;

		this.forceQuarter = true;
		this.itemSelect.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

		if (isVendingMachine && !this.IsNoItems())
			this.trashOverlay.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

		this.walletUnderlay.SetActive(true);
		this.dollarTextCenter.gameObject.SetActive(true);

		while (handIconScript.icon.sprite == handIconScript.dollar)
		{
			this.UpdateItemName(true);
			yield return null;
		}

		this.forceQuarter = false;
		this.itemSelect.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		this.trashOverlay.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		this.walletUnderlay.SetActive(false);
		this.dollarTextCenter.gameObject.SetActive(false);
		this.UpdateItemName(false);
	}

	public void UpdatePrinceyTrigger(int type, bool triggerSetting)
	{
		if (type == 1) this.isPrinceyTriggerShared = triggerSetting;
		else if (type == 2) this.isPrinceyIgnore = triggerSetting;
	}

	private void UpdateNotebookCount()
	{
		if (this.mode == "story")
			this.notebookCount.text = this.notebooks.ToString() + "/" + daFinalBookCount.ToString() + " Notebooks";
		else
			this.notebookCount.text = this.notebooks.ToString() + " Notebooks";

		if (this.notebooks == daFinalBookCount & this.mode == "story")
		{
			this.exitCountGroup.SetActive(true);
			this.notebookCount.text = string.Empty;
			this.ActivateFinaleMode();
		}
	}

	public void CollectNotebook()
	{
		this.notebooks++;
		this.UpdateNotebookCount();
	}

	public void LockMouse()
	{
		if (!this.learningActive)
		{
			this.cursorController.LockCursor(); //Prevent the cursor from moving
			this.mouseLocked = true;
		}
	}

	public void UnlockMouse()
	{
		this.cursorController.UnlockCursor(); //Allow the cursor to move
		this.mouseLocked = false;
	}

	public void PauseGame()
	{
		if (!this.learningActive)
		{
			{
				this.UnlockMouse();
			}
			Time.timeScale = 0f;
			this.gamePaused = true;
			this.pauseMenu.SetActive(true);
		}
	}

	public void ExitGame()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void UnpauseGame()
	{
		Time.timeScale = 1f;
		this.gamePaused = false;
		this.pauseMenu.SetActive(false);
		this.LockMouse();
	}

	public void ActivateSpoopMode()
	{
		this.spoopMode = true; //Tells the game its time for spooky
		this.ModifyExits("lower"); //Lowers all the exits
		this.baldiTutor.SetActive(false); //Turns off Baldi(The one that you see at the start of the game)
		this.baldi.SetActive(true); //Turns on Baldi
        this.principal.SetActive(true); //Turns on Principal
        this.crafters.SetActive(true); //Turns on Crafters
        this.playtime.SetActive(true); //Turns on Playtime
        this.gottaSweep.SetActive(true); //Turns on Gotta Sweep
        this.bully.SetActive(true); //Turns on Bully
        this.firstPrize.SetActive(true); //Turns on First-Prize
		//this.TestEnemy.SetActive(true); //Turns on Test-Enemy
		MusicPlayer(0,0);
		mathMusicScript.StopSong();
		this.audioDevice.PlayOneShot(this.aud_Hang);
	}

	public void ActivateSafeMode()
	{
		if (!this.isGameFail)
			this.baldi.SetActive(true);
		this.principal.SetActive(true); //Turns on Principal
        this.crafters.SetActive(true); //Turns on Crafters
        this.playtime.SetActive(true); //Turns on Playtime
        this.gottaSweep.SetActive(true); //Turns on Gotta Sweep
        this.bully.SetActive(true); //Turns on Bully
        this.firstPrize.SetActive(true); //Turns on First-Prize
	}

	private void ActivateFinaleMode()
	{
		this.finaleMode = true;
		ModifyExits("raise");
	}

	public void EndSafeGame()
	{
		this.isGameFail = true;
		this.baldi.SetActive(false);
		this.audioDevice.PlayOneShot(this.aud_Hang);
	}

	public void UpdateExitCount()
	{
		this.exitCountText.text = this.exitsReached + "/" + this.entranceList.Length;
	}

	private void ModifyExits(string mode)
	{
		switch(mode)
		{
			case "lower":
				foreach(EntranceScript i in entranceList)
					i.Lower();
				break;
			case "raise":
				foreach(EntranceScript i in entranceList)
					i.Raise();
				break;
		}
	}

	public void GetAngry(float value) //Make Baldi get angry
	{
		if (!this.spoopMode) this.ActivateSpoopMode();
			
		this.baldiScrpt.GetAngry(value);
	}

	public void ActivateLearningGame()
	{
		//this.camera.cullingMask = 0; //Sets the cullingMask to nothing
		this.learningActive = true;
		this.UnlockMouse(); //Unlock the mouse
		this.tutorBaldi.Stop(); //Make tutor Baldi stop talking
		if (!this.spoopMode || this.isSafeMode) //If the player hasn't gotten a question wrong
		{
			MusicPlayer(0,0); //Start playing the learn music
			this.mathMusicScript.TheAwakening();
		}
		else if (this.spoopMode)
		{
			if (PlayerPrefs.GetInt("AdditionalMusic") == 1)
			{
				if (notebooks > 2)
				{
					if (audioManager != null) audioManager.SetVolume(1);
					
					if (!this.disableSongInterruption)
					{
						if (!(this.mode == "endless"))
						{
							MusicPlayer(0,0);
							MusicPlayer(2,2);
						}
						else if (this.notebooks <= this.daFinalBookCount && !this.isEndlessSong)
						{
							MusicPlayer(0,0);
							MusicPlayer(2,2);
						}
					}
				}
				else if (notebooks == 1)
					if (audioManager != null) audioManager.SetVolume(0);
			}
			else
			{
				if (audioManager != null)  audioManager.SetVolume(0);
			}
		}
	}

	public void DeactivateLearningGame(GameObject subject, bool reward)
	{
		this.mathMusicScript.StopSong();
		if (audioManager != null) audioManager.SetVolume(0);
		this.camera.cullingMask = this.cullingMask; //Sets the cullingMask to Everything
		this.learningActive = false;
		Destroy(subject);
		this.LockMouse(); //Prevent the mouse from moving
		if (this.player.stamina < 100f) //Reset Stamina
			this.player.stamina = 100f;

		if (!this.spoopMode) //If it isn't spoop mode, play the school music
		{
			MusicPlayer(0,0);

			if (!this.isParty)
				MusicPlayer(1,1);
			else
				MusicPlayer(4,0);
		}

		if (this.isSafeMode && this.notebooks == 2)
			ActivateSafeMode();

		if (this.spoopMode && notebooks >= 2) // my music stuff
		{
			if (this.notebooks >= 2 && this.notebooks < this.daFinalBookCount && this.disableSongInterruption)
			{
				MusicPlayer(3,0);
				return;
			}
			else if (!this.isEndlessSong) 
				MusicPlayer(0,0);

			if (audioManager != null)
				audioManager.SetVolume(0);
			
			if (this.notebooks < this.daFinalBookCount)
			{
				if (this.isParty)
					MusicPlayer(4,0);
				else
					MusicPlayer(1, this.notebooks);
			}
		}
		else if (this.spoopMode && this.notebooks == 1)
			MusicPlayer(2,2);

		if (this.notebooks == 1 && reward) // If this is the players first notebook and they didn't get any questions wrong, reward them with a quarter
		{
			this.quarter.SetActive(true);
			this.tutorBaldi.PlayOneShot(this.aud_Prize);
		}
		else if (this.notebooks >= this.daFinalBookCount && this.mode == "story") // Plays the all 7 notebook sound
		{
			this.spoopLearn.Stop();

			if (!this.isSafeMode)
				this.audioDevice.PlayOneShot(this.aud_AllNotebooks, 0.8f);
			if (!this.isParty)
				this.escapeMusic.Play();
			else
				this.partyMusic.Play();
		}
		else if (this.mode == "endless")
		{
			if (this.spoopLearn.isPlaying)
				this.spoopLearn.Stop();

			if (PlayerPrefs.GetInt("AdditionalMusic") == 1 && !this.isEndlessSong)
			{
				if (!this.isParty)
					this.endlessMusic.Play();
				
				this.isEndlessSong = true;
			}
		}
	}

	private void IncreaseItemSelection()
	{
		this.itemSelected++;
		if (this.itemSelected > this.totalSlotCount - 1)
		{
			this.itemSelected = 0;
		}
		this.itemSelect.anchoredPosition = new Vector3(this.itemSelectOffset[this.itemSelected], 128.5f, 0f); //Moves the item selector background(the red rectangle)
		this.trashOverlay.anchoredPosition = new Vector3(this.itemSelectOffset[this.itemSelected], 128.5f, 0f);
		this.UpdateItemName(false);
	}

	private void DecreaseItemSelection()
	{
		this.itemSelected--;
		if (this.itemSelected < 0)
		{
			this.itemSelected = this.totalSlotCount - 1;
		}
		this.itemSelect.anchoredPosition = new Vector3(this.itemSelectOffset[this.itemSelected], 128.5f, 0f); //Moves the item selector background(the red rectangle)
		this.trashOverlay.anchoredPosition = new Vector3(this.itemSelectOffset[this.itemSelected], 128.5f, 0f);
		this.UpdateItemName(false);
	}

	private void UpdateItemSelection()
	{
		this.itemSelect.anchoredPosition = new Vector3(this.itemSelectOffset[this.itemSelected], 128.5f, 0f); //Moves the item selector background(the red rectangle)
		this.trashOverlay.anchoredPosition = new Vector3(this.itemSelectOffset[this.itemSelected], 128.5f, 0f);
		this.UpdateItemName(false);
	}

	public void CollectItem(int item_ID)
	{
		if (item_ID == 5 && !this.forceQuarterPickup)
		{
			this.UpdateDollarAmount(0.25f);
			return;
		}
		if (item_ID == 16)
		{
			this.UpdateDollarAmount(1f);
			return;
		}

		for (int i = 0; i < this.totalSlotCount; i++)
		{
			if (this.item[i] == 0)
			{
				this.item[i] = item_ID;
				this.itemSlot[i].texture = this.itemTextures[item_ID];
				break;
			}
			else if (!(this.item[i] == 0) && i == this.totalSlotCount - 1)
			{
				this.item[this.itemSelected] = item_ID;
				this.itemSlot[this.itemSelected].texture = this.itemTextures[item_ID];
			}
		}
		this.UpdateItemName(false);
	}

	private void UseItem()
	{
		if (this.item[this.itemSelected] != 0 && !this.forceQuarter)
		{
			switch(this.item[this.itemSelected])
			{
				case 1: // Zesty Bar
					if (this.player.stamina < 100f)
						this.player.stamina = this.player.maxStamina * 2f;
					else this.player.stamina += 100f;
						this.ResetItem();
					break;
				case 2: // Yellow Door Lock
					Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
					{
						raycastHit.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
						this.ResetItem();
					}
					break;
				case 3: // Principal's Keys
					Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit2;
					if (Physics.Raycast(ray2, out raycastHit2) && (raycastHit2.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit2.transform.position) <= 10f))
					{
						DoorScript component = raycastHit2.collider.gameObject.GetComponent<DoorScript>();
						if (component.DoorLocked)
						{
							component.UnlockDoor();
							component.OpenDoor();
							this.ResetItem();
						}
					}
					break;
				case 4: // BSODA
					Instantiate(this.bsodaSpray, this.playerTransform.position, this.cameraTransform.rotation);
					this.ResetItem();
					this.player.ResetGuilt("drink", 1f);
					this.audioDevice.PlayOneShot(this.aud_Soda);
					break;
				case 5: // Quarter
					Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit3;
					if (Physics.Raycast(ray3, out raycastHit3) && Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						if (raycastHit3.collider.name.StartsWith("VendingMachine"))
						{
							this.ResetItem();
							VendingMachineScript curVendingMachine = raycastHit3.collider.gameObject.GetComponent<VendingMachineScript>();
							curVendingMachine.UseQuarter();
							this.CollectItem(curVendingMachine.DispensedItem());
						}
						else if (raycastHit3.collider.name == "PayPhone" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
						{
							this.ResetItem();
							raycastHit3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
						}
					}
					break;
				case 6: // Tape
					Ray ray4 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit4;
					if (Physics.Raycast(ray4, out raycastHit4) && (raycastHit4.collider.name == "TapePlayer" & Vector3.Distance(this.playerTransform.position, raycastHit4.transform.position) <= 10f))
					{
						raycastHit4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
						this.ResetItem();
					}
					break;
				case 7: // Alarm Clock
					GameObject gameObject = Instantiate(this.alarmClock, this.playerTransform.position, this.cameraTransform.rotation);
					gameObject.GetComponent<AlarmClockScript>().baldi = this.baldiScrpt;
					gameObject.GetComponent<AlarmClockScript>().player = this.player.GetComponent<PlayerScript>();
					this.ResetItem();
					break;
				case 8: // NoSquee
					Ray ray5 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit5;
					if (Physics.Raycast(ray5, out raycastHit5) && (raycastHit5.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 10f))
					{
						raycastHit5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
						this.ResetItem();
						this.audioDevice.PlayOneShot(this.aud_Spray);
					}
					break;
				case 9: // Safety Scissors
					Ray ray6 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit6;
					if (this.player.jumpRope)
					{
						this.player.DeactivateJumpRope();
						this.playtimeScript.Disappoint();
						this.ResetItem();
					}
					else if (Physics.Raycast(ray6, out raycastHit6) && raycastHit6.collider.name == "1st Prize")
					{
						this.firstPrizeScript.GoCrazy();
						this.ResetItem();
					}
					break;
				case 10: // Bigass boots
					this.player.ActivateBoots();
					base.StartCoroutine(this.BootAnimation());
					this.ResetItem();
					break;
				case 11: // Speedy Sneakers
					this.player.ActivateSpeedShoes();
					this.ResetItem();
					break;
				case 12: // Attendance Slip
					Ray ray7 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit7;
					if (Physics.Raycast(ray7, out raycastHit7))
					{
						this.SendCharacterHome(raycastHit7.collider.name);
					}
					break;
				case 13: // Diet BSODA
					Instantiate(this.dietBsodaSpray, this.playerTransform.position, this.cameraTransform.rotation);
					this.ResetItem();
					this.player.ResetGuilt("drink", 1f);
					this.audioDevice.PlayOneShot(this.aud_Soda);
					break;
				case 14: // Crystal Zest
					this.player.stamina += 70f;
					this.ResetItem();
					break;
				case 15: // Party Popper
					if (!(this.movingPartyLocation == null))
					{
						this.partyLocation = this.movingPartyLocation;
						this.wanderer.partyPoints = this.wanderer.movingPartyPoints;
						this.MusicPlayer(0,0);
						Instantiate(this.party, this.partyLocation.position, this.cameraTransform.rotation);
						this.audioDevice.PlayOneShot(this.aud_BalloonPop);
						this.ResetItem();
						this.MusicPlayer(4,0);
						this.StartCoroutine(PlayPartyMusic());
					}
					else
						this.audioDevice.PlayOneShot(this.aud_Error);
					break;
			}
		}
		else if (this.forceQuarter)
		{
			Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit3;
			if (Physics.Raycast(ray3, out raycastHit3) && Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
			{
				if (raycastHit3.collider.name.StartsWith("VendingMachine"))
				{
					VendingMachineScript curVendingMachine = raycastHit3.collider.gameObject.GetComponent<VendingMachineScript>();

					if (curVendingMachine.vendingMachineType == VendingMachineScript.machineType.Map_Upgrade && curVendingMachine.curQuarterCount == 0)
					{
						this.audioDevice.PlayOneShot(this.aud_Error);
						StopCoroutine(this.MoneyWarning(2));
						StartCoroutine(this.MoneyWarning(2));
						return;
					}
					else if (this.dollarAmount <= 0f)
					{
						this.audioDevice.PlayOneShot(this.aud_Error);
						StopCoroutine(this.MoneyWarning(1));
						StartCoroutine(this.MoneyWarning(1));
						return;
					}

					this.UpdateDollarAmount(-0.25f);
					curVendingMachine.UseQuarter();

					if (curVendingMachine.curQuarterCount == 0 && !(curVendingMachine.vendingMachineType == VendingMachineScript.machineType.Map_Upgrade))
					{
						this.CollectItem(curVendingMachine.DispensedItem());
						curVendingMachine.ResetQuarterCount();
					}
				}
				else if (raycastHit3.collider.name == "PayPhone" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
				{
					if (this.dollarAmount <= 0f)
					{
						this.audioDevice.PlayOneShot(this.aud_Error);
						StopCoroutine(this.MoneyWarning(1));
						StartCoroutine(this.MoneyWarning(1));
						return;
					}
					
					this.UpdateDollarAmount(-0.25f);
					raycastHit3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
				}
			}
		}
	}

	private void UpdateDollarAmount(float amount)
	{
		if (this.dollarAmount == 0f && amount < 0f)
		{
			StartCoroutine(this.MoneyWarning(1));
			return;
		}
			
		this.dollarAmount += amount;
		this.PrintDollarAmount();

		if (this.dollarAmount == 0f) this.walletSlot.SetActive(false);
		else this.walletSlot.SetActive(true);
	}

	private void PrintDollarAmount()
	{
		if (this.forceQuarterPickup)
			return;
		
		if (this.dollarAmount % 1 == 0f)
		{
			this.dollarTextCenter.text = "$" + this.dollarAmount + ".00";
			this.dollarTextTop.text = "$" + this.dollarAmount + ".00";
		}
		else if (this.dollarAmount % 1 == 0.5f)
		{
			this.dollarTextCenter.text = "$" + this.dollarAmount + "0";
			this.dollarTextTop.text = "$" + this.dollarAmount + "0";
		}
		else
		{
			this.dollarTextCenter.text = "$" + this.dollarAmount;
			this.dollarTextTop.text = "$" + this.dollarAmount;
		}
	}

	private IEnumerator MoneyWarning(int warning)
	{
		float remTime = 1.5f;

		switch(warning)
		{
			case 1:
				this.dollarTextCenter.text = "Not enough money!";
				break;
			case 2:
				this.dollarTextCenter.text = "All upgrades reached!";
				break;
		}
		
		this.dollarTextCenter.color = Color.red;

		while (remTime > 0f && (this.dollarAmount == 0f || warning == 2))
		{
			remTime -= Time.unscaledDeltaTime;
			yield return null;
		}

		this.dollarTextCenter.color = Color.black;
		this.PrintDollarAmount();
	}

	public void UpgradeMap(int upgrade)
	{
		switch(upgrade)
		{
			case 3: // 1 quarter inserted
				this.mapScript.UpgradeMap(1);
				break;
			case 1: // 3 quarters inserted
				this.mapScript.UpgradeMap(2);
				break;
			case 0: // 4 quarters inserted
				this.mapScript.UpgradeMap(3);
				break;
			case 99:
				this.audioDevice.PlayOneShot(this.aud_Error);
				this.StartCoroutine(this.MoneyWarning(2));
				break;
		}
	}

	public void ActivateParty()
	{
		this.principal.GetComponent<PrincipalScript>().GoToParty();
		this.firstPrizeScript.GoToParty();

		if (this.baldiScrpt.isActiveAndEnabled)
			this.baldiScrpt.GoToParty();
		if (this.playtimeScript.isActiveAndEnabled)
			this.playtimeScript.GoToParty();
		if (this.sweepScript.isActiveAndEnabled)
			this.sweepScript.GoToParty();
		if (this.crafters.GetComponent<CraftersScript>().isActiveAndEnabled)
			this.crafters.GetComponent<CraftersScript>().isParty = true;
	}

	public void DeactivateParty()
	{
		if (this.finaleMode)
		{
			this.escapeMusic.Play();

			if (this.exitsReached >= 2)
				this.escapeMusic.volume = 0.5f;
		}
		if (this.mode == "endless" && PlayerPrefs.GetInt("AdditionalMusic") == 1)
			this.endlessMusic.Play();

		this.principal.GetComponent<PrincipalScript>().LeaveParty();
		this.firstPrizeScript.isParty = false;

		if (this.baldiScrpt.isActiveAndEnabled)
			this.baldiScrpt.isParty = false;
		if (this.playtimeScript.isActiveAndEnabled)
			this.playtimeScript.LeaveParty();
		if (this.sweepScript.isActiveAndEnabled)
			this.sweepScript.LeaveParty();
		if (this.crafters.GetComponent<CraftersScript>().isActiveAndEnabled)
			this.crafters.GetComponent<CraftersScript>().isParty = false;
	}

	private void SendCharacterHome(string character)
	{
		this.charInAttendance = character;
		switch(character)
		{
			case "Playtime":
				this.playtimeScript.GoToAttendance();
				this.player.DeactivateJumpRope();
				this.ResetItem();
			break;
			case "Gotta Sweep":
				this.sweepScript.GoToAttendance();
				this.ResetItem();
			break;
			case "1st Prize":
				this.firstPrizeScript.GoToAttendance();
				this.ResetItem();
			break;
		}
	}

	private void ReactivateAttendanceCharacter()
	{
		switch (this.charInAttendance)
		{
			case "Playtime":
				this.playtime.SetActive(true);
			break;
			case "Gotta Sweep":
				this.gottaSweep.SetActive(true);
				this.sweepScript.GoHome();
			break;
			case "1st Prize":
				this.firstPrize.SetActive(true);
			break;
		}
	}

	private IEnumerator BootAnimation()
	{
		float time = 15f;
		float height = 375f;
		Vector3 position = default(Vector3);
		this.boots.gameObject.SetActive(true);
		while (height > -375f)
		{
			height -= 375f * Time.deltaTime;
			time -= Time.deltaTime;
			position = this.boots.localPosition;
			position.y = height;
			this.boots.localPosition = position;
			yield return null;
		}
		position = this.boots.localPosition;
		position.y = -375f;
		this.boots.localPosition = position;
		this.boots.gameObject.SetActive(false);
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		this.boots.gameObject.SetActive(true);
		while (height < 375f)
		{
			height += 375f * Time.deltaTime;
			position = this.boots.localPosition;
			position.y = height;
			this.boots.localPosition = position;
			yield return null;
		}
		position = this.boots.localPosition;
		position.y = 375f;
		this.boots.localPosition = position;
		this.boots.gameObject.SetActive(false);
		yield break;
	}

	private void ResetItem()
	{
		this.item[this.itemSelected] = 0;
		this.itemSlot[this.itemSelected].texture = this.itemTextures[0];
		this.UpdateItemName(false);
	}

	public void LoseItem(int id)
	{
		this.item[id] = 0;
		this.itemSlot[id].texture = this.itemTextures[0];
		this.UpdateItemName(false);
	}

	private void UpdateItemName(bool isWallet)
	{
		if (isWallet) this.itemText.text = "Wallet";
		else this.itemText.text = this.itemNames[this.item[this.itemSelected]];
	}

	public void ExitReached()
	{
		this.exitsReached++;
		AngrySchoolColors(this.exitsReached);

		if (this.exitsReached != 2) this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);

		if (this.exitsReached == 2) //Play a sound
		{
			this.audioDevice.volume = 0.8f;
			this.audioDevice.clip = this.chaosEarly;
			this.audioDevice.loop = false;
			this.audioDevice.Play();
			this.escapeMusic.volume = 0.5f;
		}
		else if (this.exitsReached == 3) //Play a louder sound
		{
			this.audioDevice.volume = 0.8f;
			this.audioDevice.clip = this.chaosBuildup;
			this.audioDevice.loop = false;
			this.audioDevice.Play();
		}
	}

	public void DespawnCrafters()
	{
		this.crafters.SetActive(false); //Make Arts And Crafters Inactive
	}

	public void Fliparoo()
	{
		this.player.height = 6f;
		this.player.fliparoo = 180f;
		this.player.flipaturn = -1f;
		Camera.main.GetComponent<CameraScript>().offset = new Vector3(0f, -1f, 0f);
	}

	// 0 = Stop All, 1 = school, 2 = learn, 3 = uninterrupted school, 4 = party
	public void MusicPlayer(int songType, int SongId)
	{
		if (songType == 0) // stop all
		{
			this.schoolMusic.Stop();
			this.learnMusic.Stop();
			this.spoopLearn.Stop();
			this.partyMusic.Stop();
			this.escapeMusic.Stop();
			for (int i = 0; i < this.daFinalBookCount - 2; i++)
				this.schoolhouseTroublePlaylist[i].Stop();
		}
		else if (songType == 1 && !this.finaleMode) // schoolhouse
		{
			if (SongId == 1) this.schoolMusic.Play();
			else if (SongId >= 2 && PlayerPrefs.GetInt("AdditionalMusic") == 1 && !(this.mode == "endless"))
			{
				if (this.notebooks < this.daFinalBookCount)
					this.schoolhouseTroublePlaylist[SongId - 2].Play();
			}
		}
		else if (songType == 2) // math game
		{
			if (SongId == 1) this.learnMusic.Play();
			else if (SongId == 2 && !this.disableSongInterruption && PlayerPrefs.GetInt("AdditionalMusic") == 1)
			{
				this.spoopLearn.Play();
			}
		}
		else if (songType == 3)
		{
			if (!this.schoolhouseTroublePlaylist[0].isPlaying && !this.schoolhouseTroublePlaylist[1].isPlaying)
				StartCoroutine(UninterruptedMusic());
		}
		else if (songType == 4)
		{
			this.partyMusic.Play();
		}
	}

	private IEnumerator PlayPartyMusic()
	{
		this.isParty = true;
		this.ActivateParty();
		this.remainingPartyTime = 84.65f;
		MusicPlayer(4,0);

		while (this.remainingPartyTime > 0f)
		{
			this.remainingPartyTime -= Time.deltaTime;
			yield return null;
		}

		this.MusicPlayer(0,0);
		this.isParty = false;
		this.DeactivateParty();
		
		if (this.spoopMode)
			this.MusicPlayer(1,this.notebooks);
		else if (this.isSafeMode)
			this.MusicPlayer(1,1);
	}

	private IEnumerator UninterruptedMusic()
	{
		this.schoolhouseTroublePlaylist[0].Play();

		while (this.schoolhouseTroublePlaylist[0].isPlaying) yield return null;

		this.schoolhouseTroublePlaylist[1].Play();
	}

	private void AngrySchoolColors(int phase)
	{
		if (!this.isSafeMode)
		{
			StartCoroutine(ChangeSchoolColor(phase));
			StartCoroutine(ChangeFogColor(phase));
		}
	}

	private IEnumerator ChangeSchoolColor(int phase)
	{
		float curValue;
		
		switch(phase)
		{
			case 2:
				curValue = 1f;
				
				while (curValue > 0f)
				{
					curValue -= Time.deltaTime/5f;
					RenderSettings.ambientLight = new Color(1f, curValue, curValue);
					yield return null;
				}
			break;
			case 3:
				curValue = 1f;
				while (curValue > 0.388f)
				{
					curValue -= Time.deltaTime/5f;
					RenderSettings.ambientLight = new Color(curValue, 0f, 0f);
					yield return null;
				}
			break;
		}
	}

	private IEnumerator ChangeFogColor(int phase)
	{
		float curValue;
		switch(phase)
		{
			case 1:
				curValue = 0f;
				RenderSettings.fogDensity = 0f;
				RenderSettings.fog = true;
				while (curValue < 0.01f)
				{
					curValue += Time.deltaTime/100f;
					RenderSettings.fogDensity = curValue;
					yield return null;
				}
				RenderSettings.fogDensity = 0.01f;
				curValue = 1f;
				while (curValue > 0f)
				{
					curValue -= Time.deltaTime/5f;
					RenderSettings.fogColor = new Color(1f, curValue, curValue);
					yield return null;
				}
			break;
			case 2:
				curValue = 2f;
				while (curValue > 0f)
				{
					curValue -= Time.deltaTime;
					yield return null;
				}
				curValue = 0.01f;
				while (curValue > 0f)
				{
					curValue -= Time.deltaTime/100f;
					RenderSettings.fogDensity = curValue;
					yield return null;
				}
			break;
			case 3:
				curValue = 0f;
				RenderSettings.fogColor = new Color(1f, 0f, 0f);
				while (curValue < 0.01f)
				{
					curValue += Time.deltaTime/100f;
					RenderSettings.fogDensity = curValue;
					yield return null;
				}
				RenderSettings.fogDensity = 0.01f;
			break;
		}
	}


	[Header("Game Configuration")]
	public int daFinalBookCount;
	public int totalSlotCount;
	public EntranceScript[] entranceList;
	public Transform attendanceOffice;
	public Vector3 detentionPlayerPos;
	public Vector3 detentionPrincipalPos;
	[SerializeField] private bool disableSongInterruption;
	[SerializeField] private bool forceQuarterPickup;


	[Header("Game State")]
	[SerializeField] private string curMap;
	public string mode;
	public int notebooks;
	public bool spoopMode;
	public bool finaleMode;
	public bool debugMode;
	public bool mouseLocked;
	public int exitsReached;
	private bool gamePaused;
	public bool learningActive;
	public float gameOverDelay;
	public bool isSlowmo;
	public bool isSafeMode;
	public bool isDifficultMath;
	[SerializeField] private string charInAttendance;
	[SerializeField] private float dollarAmount;
	[SerializeField] private bool forceQuarter;
	public Transform partyLocation;
	public Transform movingPartyLocation;
	public bool isParty;
	[SerializeField] private float remainingPartyTime;
	public bool isGameFail;
	[SerializeField] private string highBooksDirectory;
	[SerializeField] private int highBooksScore;
	[SerializeField] private bool isScareStarted;


	[Header("UI")]
	public TMP_Text notebookCount;
	public GameObject pauseMenu;
	public GameObject highScoreText;
	public GameObject warning;
	public TMP_Text staminaPercentText;
	public GameObject reticle;
	public RectTransform itemSelect;
	[SerializeField] private float[] itemSelectOffset;
	[SerializeField] private GameObject pointer;
	[SerializeField] private TMP_Text dollarTextCenter;
	[SerializeField] private TMP_Text dollarTextTop;
	[SerializeField] private GameObject exitCountGroup;
	[SerializeField] private TMP_Text exitCountText;
	[SerializeField] private MapCameraScript mapScript;


	[Header("Noteboos")]
	public GameObject[] notebookPickups;
	public int failedNotebooks;
	public int notebookCharReturn;


	[Header("Player")]
	public Transform playerTransform;
	public Transform cameraTransform;
	public Camera camera;
	private int cullingMask;
	[SerializeField] private Material redSky;


	[Header("Characters")]
	public GameObject baldiTutor;
	public GameObject baldi;
	public GameObject principal;
	public GameObject crafters;
	public GameObject playtime;
	public GameObject gottaSweep;
	public GameObject bully;
	public GameObject firstPrize;
	public GameObject TestEnemy;


	[Header("Item Slots")]
	[SerializeField] private RectTransform itemBG;
	[SerializeField] private float[] slotOffsetArray;
	[SerializeField] private RawImage slotForeground;
	[SerializeField] private Texture[] slotForegroundList;
	public int itemSelected;
	[SerializeField] private GameObject walletSlot;
	[SerializeField] private GameObject walletUnderlay;
	[SerializeField] private GameObject walletForeground;
	[SerializeField] private GameObject walletBackground;
	[SerializeField] private RectTransform trashOverlay;
	private bool IsNoItems()
	{
		for (int i = 0; i < this.totalSlotCount; i++)
		{
			if (this.item[i] == 0)
				return true;
		}
		return false;
	}


	[Header("Items")]
	public int[] item = new int[5];
	public RawImage[] itemSlot = new RawImage[5];
	public Texture[] itemTextures = new Texture[10];
	public TMP_Text itemText;
	private string[] itemNames = new string[]
	{
		"Nothing",
		"Energy flavored Zesty Bar",
		"Yellow Door Lock",
		"Principal's Keys",
		"BSODA",
		"Quarter",
		"Baldi Anti Hearing and Disorienting Tape",
		"Alarm Clock",
		"WD-NoSquee (Door Type)",
		"Safety Scissors",
		"Big Ol' Boots",
		"Speedy Sneakers",
		"Attendance Slip",
		"Diet BSODA",
		"Crystal flavored Zesty Bar",
		"Party Popper"
	};
	public GameObject quarter;
	public GameObject bsodaSpray;
	public GameObject dietBsodaSpray;
	public RectTransform boots;
	public GameObject alarmClock;
	[SerializeField] private GameObject party;


	[Header("Detention")]
	public bool isPrinceyTriggerShared;
	public bool isPrinceyIgnore;
	
	
	[Header("SFX and Voices")]
	public AudioClip aud_Prize;
	private AudioSource audioDevice;
	private AudioSource mathAudioDevice;
	public AudioClip aud_PrizeMobile;
	public AudioClip aud_AllNotebooks;
	public AudioSource tutorBaldi;
	public AudioClip aud_Soda;
	public AudioClip aud_Spray;
	public AudioClip aud_buzz;
	public AudioClip aud_Hang;
	[SerializeField] private AudioClip aud_Error;
	public AudioClip aud_Switch;
	public AudioClip[] baldiJumpscareSounds;
	public AudioClip chaosEarly;
	public AudioClip chaosEarlyLoop;
	public AudioClip chaosBuildup;
	public AudioClip chaosFinalLoop;
	[SerializeField] private AudioClip aud_BalloonPop;


	[Header("Music")]
	[SerializeField] private bool isEndlessSong;
	public AudioSource schoolMusic;
	public AudioSource escapeMusic;
	public AudioSource endlessMusic;
	public AudioSource learnMusic;
	public AudioSource spoopLearn;
	public AudioClip LearnQ1;
	public AudioClip LearnQ2;
	public AudioClip LearnQ3;
	[SerializeField] private AudioSource partyMusic;
	public AudioSource[] schoolhouseTroublePlaylist;


	[Header("Scripts")]
	public CursorControllerScript cursorController;
	public PlayerScript player;
	public BaldiScript baldiScrpt;
	public PlaytimeScript playtimeScript;
	public FirstPrizeScript firstPrizeScript;
	[SerializeField] private AILocationSelectorScript wanderer;
 	[SerializeField] private SweepScript sweepScript;
	[SerializeField] private DebugMenuActions debugActions;
	[SerializeField] private DebugScreenSwitch debugScreen;
	[SerializeField] private AudioManager audioManager;
	[SerializeField] private SpeedrunTimer speedrunTimer;
	[SerializeField] private MathMusicScript mathMusicScript;
	[SerializeField] private DebugSceneLoader sceneLoader;
	[SerializeField] private HandIconScript handIconScript;
}