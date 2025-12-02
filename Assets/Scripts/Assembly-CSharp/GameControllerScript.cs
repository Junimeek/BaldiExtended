using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
	private void Awake()
	{
		audioManager = FindObjectOfType<AudioManager>();

		this.sceneLoader = this.childScripts[0].GetComponent<DebugSceneLoader>();
		this.stats = this.childScripts[1].GetComponent<StatisticsController>();
	}

	private void Start()
	{
		Scene curScene = SceneManager.GetActiveScene();
		string curSceneName = curScene.name;

		Debug.Log(this.LogMapStart(curSceneName));

		this.LockMouse(); //Prevent the mouse from moving

		this.isParty = false;

		this.isDoorFix = this.ReadBoolFromRegistry("doorFix");

		switch (curSceneName)
		{
			case "ClassicDark":
				this.ignoreInitializationChecks = true;
				this.masterTextColor = Color.white;
				this.gameOverDelay = 5f;
				this.modeType = "nullStyle";
				this.isSafeMode = false;
				this.isDifficultMath = false;
				this.player.isNullStyle = true;
				break;
			default:
				this.ignoreInitializationChecks = false;
				this.masterTextColor = Color.black;
				this.gameOverDelay = 0.5f;

				this.isSafeMode = this.ReadBoolFromRegistry("safeMode");
				this.isDifficultMath = this.ReadBoolFromRegistry("difficultMath");
				this.isAdditionalMusic = this.ReadBoolFromRegistry("additionalMusic");
				break;
		}

		this.cullingMask = this.playerCamera.cullingMask; // Changes cullingMask in the Camera
		this.audioDevice = base.GetComponent<AudioSource>(); //Get the Audio Source
		this.mode = PlayerPrefs.GetString("CurrentMode"); //Get the current mode

		this.curMap = curSceneName;
		PlayerPrefs.SetString("CurrentMap", this.curMap);

		this.dollarTextTop.color = this.masterTextColor;
		this.speedrunText.color = this.masterTextColor;
		this.itemText.color = this.masterTextColor;
		this.fpsCounter.color = this.masterTextColor;

		if (curSceneName == "SecretMap" && !this.isSafeMode)
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = new Color(0.79f, 0.79f, 0.79f);
			RenderSettings.ambientLight = new Color(0.86f, 0.86f, 0.86f);
		}
		else if (curSceneName == "ClassicDark")
		{
			this.isDynamicColor = true;
			RenderSettings.fog = false;
			RenderSettings.ambientLight = new Color(0.1f, 0.1f, 0.1f);
		}
		else
		{
			RenderSettings.fog = false;
			RenderSettings.fogColor = new Color(1f, 1f, 1f);
			RenderSettings.ambientLight = new Color(1f, 1f, 1f);
		}

		this.InitializeItemSlots();
		this.exitCountText.text = "0/" + this.entranceList.Length;
		this.exitCountGroup.SetActive(false);

		this.showTimer = this.ReadBoolFromRegistry("showTimer");

		this.speedrunSeconds = 0f;

		if (this.modeType != "nullStyle")
			StartCoroutine(this.MusicRoutine(0)); //Play the school music

		this.UpdateNotebookCount(); //Update the notebook count
		this.UpdateDollarAmount(0f);
		this.StartCoroutine(this.WaitForQuarterDisable(true, false));

		if (this.isSafeMode)
			this.stats.disableSaving = true;
		this.stats.itemsUsed = new int[0];
		this.stats.detentions = 0;
		
		this.itemSelected = 0; //Set selection to item slot 0(the first item slot)

		//debugScreen.DebugCloseMenu();
	}

	private string LogMapStart(string curSceneName)
	{
		string finalLog = "GAME STARTED IN MAP ";
		finalLog += curSceneName;
		finalLog += " IN ";
		finalLog += PlayerPrefs.GetString("CurrentMode");
		finalLog += " mode WITH SETTINGS:";
		finalLog += "\nSAFE MODE = ";
		finalLog += this.ReadBoolFromRegistry("safeMode");
		finalLog += "\nDIFFICULT MATH = ";
		finalLog += this.ReadBoolFromRegistry("difficultMath");
		finalLog += "\nDOOR FIX SETTING = ";
		finalLog += this.ReadBoolFromRegistry("doorFix");
		return finalLog;
	}

	private bool ReadBoolFromRegistry(string entry)
	{
		string entryDirectory;
		int defaultValue = 0;
		int value = 0;
		
		switch(entry)
		{
			case "safeMode":
				entryDirectory = "gps_safemode";
				break;
			case "difficultMath":
				entryDirectory = "gps_difficultmath";
				break;
			case "additionalMusic":
				entryDirectory = "AdditionalMusic";
				break;
			case "showTimer":
				entryDirectory = "op_showtimer";
				break;
			case "doorFix":
				entryDirectory = "pat_doorFix";
				defaultValue = 1;
				break;
			default:
				entryDirectory = "gps_difficultmath";
				break;
		}

		value = PlayerPrefs.GetInt(entryDirectory, defaultValue);

		if (value == 0)
			return false;
		else
			return true;
	}

	private void InitializeItemSlots()
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

	public void InitializeScores()
	{
		this.bestTime = stats.data_bestTime[stats.mapID];

		if (this.mode != "challenge")
			this.highBooksScore = stats.data_notebooks[stats.mapID];
	}

	private void Update()
	{
		this.fpsTimer -= Time.unscaledDeltaTime;

		if (this.fpsTimer < 0f)
		{
			this.fpsTimer += 0.25f;
			this.fpsCounter.text = (1 / Time.unscaledDeltaTime).ToString("0") + "fps";
		}

		if (!this.learningActive)
		{
			if (Input.GetButtonDown("Pause"))
			{
				if (!this.gamePaused)
					this.PauseGame();
				else
					this.UnpauseGame();
			}

			if (Input.GetKeyDown(KeyCode.Y) && this.gamePaused)
				this.ExitGame();
			else if (Input.GetKeyDown(KeyCode.N) && this.gamePaused)
				this.UnpauseGame();

			if (!this.gamePaused & Time.timeScale != 1f && !this.isSlowmo)
				Time.timeScale = 1f;

			if (Input.GetMouseButtonDown(1) && Time.timeScale != 0f)
				this.UseItem();

			if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f)
				this.DecreaseItemSelection();
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Time.timeScale != 0f)
				this.IncreaseItemSelection();

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
		else if (Time.timeScale != 0f)
			Time.timeScale = 0f;

		if (this.player.stamina > 0f)
		{
			this.staminaPercentText.text = this.player.stamina.ToString("0") + "%";
			this.staminaPercentText.color = this.masterTextColor;
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
				this.highScoreText.SetActive(true);

			Time.timeScale = 0f;
			this.gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
			this.playerCamera.farClipPlane = Mathf.Clamp(this.gameOverDelay * 400f, -1f, 400f); //Set camera farClip

			if (!this.player.isSecret)
			{
				this.MusicPlayer(0);
				this.endlessMusic.Stop();
			}

			if (!this.player.isSecret && !this.isScareStarted)
			{
				if (this.modeType == "nullStyle")
					challengeController.EndChallengeGame(1);
				
				if (this.mode != "endless")
					this.stats.SaveAllData(null);
				else if (!this.isScareStarted)
				{
					this.isScareStarted = true;
					PlayerPrefs.SetInt("CurrentBooks", this.notebooks);

					if (this.notebooks > this.highBooksScore)
					{
						this.stats.notebooks = this.notebooks;
						this.stats.SaveAllData("notebooks");
					}
					else
						this.stats.SaveAllData(null);
				}

				this.isScareStarted = true;
				int randomScare = Mathf.RoundToInt(UnityEngine.Random.Range(0, this.baldiJumpscareSounds.Length - 1));
				this.audioDevice.PlayOneShot(this.baldiJumpscareSounds[randomScare]);
			}

			if (this.gameOverDelay <= 0f)
			{
				if (!this.player.isSecret)
				{
					Time.timeScale = 1f;
					SceneManager.LoadScene("GameOver");
				}
				else
				{
					Debug.Log("Game Quit");
					this.cursorController.UnlockCursor();
					Application.Quit();
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			switch(this.showTimer)
			{
				case false:
					this.showTimer = true;
					PlayerPrefs.SetInt("op_showtimer", 1);
					break;
				case true:
					this.showTimer = false;
					PlayerPrefs.SetInt("op_showtimer", 0);
					break;
			}
		}

		if (Time.timeScale != 0f && this.remainingDetentionTime > 0f)
			this.remainingDetentionTime -= Time.deltaTime;

		if (this.modeType == "nullStyle" && this.exitsReached < 3)
		{
			Vector3 distance = this.baldi.transform.position - this.playerTransform.position;
			float sqrLen = distance.sqrMagnitude;
			this.darkLevel = Mathf.Sqrt(sqrLen / 300000f);

			if (this.darkLevel >= 0.4f)
				RenderSettings.ambientLight = new Color(0.4f, 0.4f, 0.4f);
			else
				RenderSettings.ambientLight = new Color(this.darkLevel, this.darkLevel, this.darkLevel);
		}
		else if (this.modeType == "nullStyle" && this.isDynamicColor)
		{
			Vector3 distance = this.entranceDarkSources[0].position - this.playerTransform.position;
			float sqrLen = distance.sqrMagnitude;
			this.darkLevel = Mathf.Clamp(Mathf.Sqrt(sqrLen / 50000f), 0.02f, 1f);

			if (this.darkLevel >= 0.4f)
				RenderSettings.ambientLight = new Color(0.4f, 0.4f, 0.4f);
			else
				RenderSettings.ambientLight = new Color(this.darkLevel, this.darkLevel, this.darkLevel);
		}
		else if (this.finaleMode && this.isDynamicColor)
		{
			Vector3 distance = this.entranceDarkSources[0].position - this.playerTransform.position;
			float sqrLen = distance.sqrMagnitude;
			this.darkLevel = Mathf.Clamp(Mathf.Sqrt(sqrLen / 50000f), 0.02f, 1f);

			RenderSettings.ambientLight = new Color(this.darkLevel, 0f, 0f);
		}

		this.speedrunSeconds += Time.unscaledDeltaTime;
		
		if (this.speedrunSeconds >= 60f)
		{
			this.speedrunMinutes++;
			this.speedrunSeconds -= 60f;
		}
		if (this.speedrunMinutes >= 60)
		{
			this.speedrunHours++;
			this.speedrunMinutes = 0;
		}

		if (this.showTimer)
			this.speedrunText.text = this.speedrunHours + ":" + this.speedrunMinutes.ToString("00") + ":" + this.speedrunSeconds.ToString("00.000");
		else
			this.speedrunText.text = string.Empty;

		this.CheckItemRaycast();

		if (this.handIconScript != null)
			this.CheckRaycast();
	}

	public void CompleteGame()
	{
		DontDestroyOnLoad(this.stats.gameObject);
		
		this.stats.finalSeconds = this.FinalTime();

		if (this.failedNotebooks >= this.daFinalBookCount)
			this.sceneLoader.LoadTheScene("SecretMap", 0);
		else
			this.sceneLoader.LoadTheScene("Results", 0);
	}

	private float FinalTime()
	{
		float roundedTime = MathF.Round(this.speedrunSeconds, 3);
		return (this.speedrunHours * 3600f) + (this.speedrunMinutes * 60f) + roundedTime;
	}

	private void CheckItemRaycast()
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

		if (Physics.Raycast(ray, out raycastHit))
		{
			Debug.DrawLine(this.playerTransform.position, raycastHit.transform.position, Color.red);
			if (raycastHit.collider.tag == "Item")
			{
				this.curItem = raycastHit.collider.gameObject;
				this.isLookingAtVendingMachine = false;
			}
			else if (raycastHit.collider.name.StartsWith("VendingMachine"))
			{
				this.curItem = null;
				this.isLookingAtVendingMachine = true;
			}
			else
			{
				this.curItem = null;
				this.isLookingAtVendingMachine = false;
			}
		}

		if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f && this.curItem != null
		&& Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f
		&& !this.isLookingAtVendingMachine)
		{
			this.CollectItem(this.curItem.GetComponent<PickupScript>().ItemID());
		}
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
		else
			distance = 10f;

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
				else if (raycastHit.collider.name == "PayPhone")
				{
					if (this.forceQuarterPickup && this.item[itemSelected] == 5)
						handIconScript.ChangeIcon(11);
					else if (!this.forceQuarterPickup)
					{
						StartCoroutine(this.WaitForQuarterDisable(false, true));
						handIconScript.ChangeIcon(3);
					}
				}
				else if (raycastHit.collider.name == "TapePlayer" && this.item[this.itemSelected] == 6)
				{
					handIconScript.ChangeIcon(5);
				}
				else if (raycastHit.collider.name.StartsWith("AlarmClock"))
					handIconScript.ChangeIcon(1);
				else if (raycastHit.collider.tag == "SwingingDoor" && this.item[this.itemSelected] == 2)
					handIconScript.ChangeIcon(6);
				else if (raycastHit.collider.name == "_DoorOut" && !(raycastHit.collider.tag == "SwingingDoor"))
				{
					if (raycastHit.collider.tag == "PrincipalDoor" && this.item[this.itemSelected] == 3)
						handIconScript.ChangeIcon(4);
					else if (this.item[this.itemSelected] == 8)
						handIconScript.ChangeIcon(7);
					else
						handIconScript.ChangeIcon(1);
				}
				else if (raycastHit.collider.name.StartsWith("VendingMachine"))
				{
					if (this.forceQuarterPickup && this.item[this.itemSelected] == 5)
						handIconScript.ChangeIcon(11);
					else if (!this.forceQuarterPickup)
					{
						if (raycastHit.collider.name == "VendingMachine_MapUpgrade")
							StartCoroutine(this.WaitForQuarterDisable(true, true));
						else
							StartCoroutine(this.WaitForQuarterDisable(true, false));
						handIconScript.ChangeIcon(3);
					}
				}
				else if ((raycastHit.collider.name == "Playtime" || raycastHit.collider.name == "Gotta Sweep" || raycastHit.collider.name == "1st Prize")
				&& this.item[this.itemSelected] == 12)
					handIconScript.ChangeIcon(9);
				else if (raycastHit.collider.tag == "BreakableWindow" && this.item[this.itemSelected] == 17)
					handIconScript.ChangeIcon(12);
				else if (raycastHit.collider.tag == "Item")
				{
					this.curItem = raycastHit.collider.gameObject;
					handIconScript.ChangeIcon(1);
				}
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

	private IEnumerator WaitForQuarterDisable(bool isVendingMachine, bool isUpgrade)
	{
		if (this.forceQuarterPickup)
			yield break;

		this.forceQuarter = true;
		this.itemSelect.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

		if (isVendingMachine && !this.IsNoItems() && !isUpgrade)
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
		this.notebookCountText.text = this.notebooks.ToString();

		if (this.mode != "endless")
			this.notebookCountText.text += "/" + this.daFinalBookCount.ToString();

		if (this.notebooks == daFinalBookCount && this.mode != "endless")
		{
			if (this.mode == "story")
            {
                this.exitCountGroup.SetActive(true);
				this.notebookObject.SetActive(false);
            }
			else if (this.mode == "challenge")
				this.ActivateFinaleMode();
		}
	}

	public void CollectNotebook()
	{
		if (this.player.stamina < 100f)
			this.player.stamina = 100f;

		this.notebooks++;
		this.UpdateNotebookCount();

		switch(this.modeType)
		{
			case "nullStyle":
				if (notebookCountScript.isActiveAndEnabled)
					notebookCountScript.FlipNotebooks();
				this.audioDevice.PlayOneShot(this.aud_NotebookCollect);

				if (this.notebooks == 2)
				{
					this.ModifyExits("lower");
				}

				if (this.notebooks >= 3)
				{
					this.baldiScrpt.GetAngry(1f);
					if (this.baldiScrpt.isActiveAndEnabled)
						this.baldiScrpt.AddNewSound(this.player.transform.position, 2);
				}
				break;
			default:
				GameObject gameObject = Instantiate(this.mathGameUI);
				MathGameScript mathScript = gameObject.GetComponent<MathGameScript>();
				mathScript.gc = this;
				mathScript.baldiScript = this.baldiScrpt;
				mathScript.playerPosition = this.player.GetComponent<Transform>().position;
				break;
		}
	}

	public void SpawnNullBaldi()
    {
		this.baldi.SetActive(true);
        this.baldiScrpt.GetAngry(1f);
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
		DontDestroyOnLoad(this.stats.gameObject);

		if (this.curMap != "SecretMap")
			this.stats.SaveAllData(null);

		SceneManager.LoadSceneAsync("MainMenu");
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
		MusicPlayer(0);
		this.mathMusicScript.StopSong();
		this.audioDevice.PlayOneShot(this.aud_Hang);
		this.crafterScript = FindObjectOfType<CraftersScript>();
		this.principalScript = FindObjectOfType<PrincipalScript>();
	}

	public void ActivateSafeMode()
	{
		if (!this.isGameFail && this.notebooks < 3)
			this.baldi.SetActive(true);

		this.principal.SetActive(true);
        this.crafters.SetActive(true);
        this.bully.SetActive(true);

		if (this.charInAttendance != "Playtime")
			this.playtime.SetActive(true);
		if (this.charInAttendance != "Gotta Sweep")
        	this.gottaSweep.SetActive(true);
		if (this.charInAttendance != "1st Prize")
			this.firstPrize.SetActive(true);
		
		this.crafterScript = FindObjectOfType<CraftersScript>();
		this.principalScript = FindObjectOfType<PrincipalScript>();
	}

	private void ActivateFinaleMode()
	{
		this.finaleMode = true;
		this.escapeMusicStage = 1;
		this.ModifyExits("raise");
	}

	void ToggleFlashlight(float lightChange)
    {
		this.playerFlashlights[0].intensity = lightChange;
		this.playerFlashlights[1].intensity = lightChange;
    }

	public void TemporaryBossActivation()
    {
        this.audioDevice.Stop();
		this.audioDevice.PlayOneShot(this.aud_BigClose, 0.6f);
		this.ToggleFlashlight(0f);
		this.mapScript.DisableAllItems();
    }

	public void SwitchToMinimalUI()
    {
		this.minimalUI.SetActive(true);
        this.speedrunText = this.minimalSpeedrunText;
		this.fpsCounter = this.minimalFpsCounter;
		handIconScript.icon = this.minimalCenterIcon;
		this.speedrunText.color = Color.black;
		this.fpsCounter.color = Color.black;
		this.mainHud.renderMode = RenderMode.WorldSpace;

		this.dollarAmount = 0f;
		for (int i = 0; i < this.totalSlotCount; i++)
			this.item[i] = 0;
    }

	public Vector3 GetNewWanderLocation(string wanderType)
    {
        Vector3 newLocation = this.wanderer.NewTarget(wanderType);
		return newLocation;
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
		
		if (this.escapeMusicStage == 1)
			this.escapeMusicStage = 2;
		else
			this.escapeMusicStage = 3;
	}

	public void ModifyExits(string mode)
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
		if (!this.spoopMode)
			this.ActivateSpoopMode();
			
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
			MusicPlayer(0); //Start playing the learn music
			this.mathMusicScript.TheAwakening();
		}
		else if (this.spoopMode && audioManager != null)
		{
			this.audioManager.SetVolume(1);
		}
	}

	public void DeactivateLearningGame(GameObject subject, bool reward)
	{
		this.learningActive = false;
		Destroy(subject);
		this.LockMouse(); //Prevent the mouse from moving
		this.mathMusicScript.StopSong();
		if (audioManager != null)
			audioManager.SetVolume(0);
		this.playerCamera.cullingMask = this.cullingMask; //Sets the cullingMask to Everything
		
		if (notebookCountScript.isActiveAndEnabled)
			notebookCountScript.FlipNotebooks();
		this.audioDevice.PlayOneShot(this.aud_NotebookCollect);

		if (audioManager != null)
			audioManager.SetVolume(0);

		if (this.notebooks == 1 && reward) // If this is the players first notebook and they didn't get any questions wrong, reward them with a quarter
		{
			this.quarter.SetActive(true);
			this.tutorBaldi.PlayOneShot(this.aud_Prize);
		}
		else if (this.notebooks >= this.daFinalBookCount && this.mode == "story") // Plays the all 7 notebook sound
		{
			this.ActivateFinaleMode();

			if (!this.isSafeMode)
				this.audioDevice.PlayOneShot(this.aud_AllNotebooks, 0.8f);
		}
		else if (this.mode == "endless")
		{
			if (this.baldiScrpt.isActiveAndEnabled)
				this.baldiScrpt.endless = true;
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
			this.curItem.GetComponent<PickupScript>().ChangeItem(0);
			this.UpdateDollarAmount(0.25f);
			this.audioDevice.PlayOneShot(this.aud_CoinCollect);
			return;
		}
		if (item_ID == 16)
		{
			this.curItem.GetComponent<PickupScript>().ChangeItem(0);
			this.UpdateDollarAmount(1f);
			this.audioDevice.PlayOneShot(this.aud_CoinCollect);
			return;
		}

		for (int i = 0; i < this.totalSlotCount; i++)
		{
			if (this.item[i] == 0)
			{
				if (!this.isLookingAtVendingMachine)
					this.curItem.GetComponent<PickupScript>().ChangeItem(0);
				this.item[i] = item_ID;
				this.itemSlot[i].texture = this.itemTextures[item_ID];
				break;
			}
			else if (!(this.item[i] == 0) && i == this.totalSlotCount - 1)
			{
				if (!this.isLookingAtVendingMachine)
				{
					this.curItem.GetComponent<PickupScript>().ChangeItem(this.item[this.itemSelected]);
					if (this.isItemUpgrade)
						this.UpgradeMap(1);
				}
				this.item[this.itemSelected] = item_ID;
				this.itemSlot[this.itemSelected].texture = this.itemTextures[item_ID];
			}
		}
		this.audioDevice.PlayOneShot(this.aud_ItemCollect);
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
					else
						this.player.stamina += 100f;
					this.audioDevice.PlayOneShot(this.aud_EatFood);
					this.ResetItem(1);
					break;
				case 2: // Yellow Door Lock
					Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
					{
						raycastHit.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor();
						this.ResetItem(2);
					}
					break;
				case 3: // Principal's Keys
					Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit2;
					if (Physics.Raycast(ray2, out raycastHit2) && (raycastHit2.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit2.transform.position) <= 10f))
					{
						ClassroomDoorScript component = raycastHit2.collider.gameObject.GetComponent<ClassroomDoorScript>();
						if (component.TryUnLock())
							this.ResetItem(3);
					}
					break;
				case 4: // BSODA
					Instantiate(this.bsodaSpray, this.playerTransform.position, this.cameraTransform.rotation);
					this.ResetItem(4);
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
							this.ResetItem(5);
							this.handIconScript.ChangeIcon(0);
							VendingMachineScript curVendingMachine = raycastHit3.collider.gameObject.GetComponent<VendingMachineScript>();
							curVendingMachine.UseQuarter();
							this.CollectItem(curVendingMachine.DispensedItem());
						}
						else if (raycastHit3.collider.name == "PayPhone" && Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
						{
							this.ResetItem(5);
							this.handIconScript.ChangeIcon(0);
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
						this.ResetItem(6);
						this.handIconScript.ChangeIcon(0);
					}
					break;
				case 7: // Alarm Clock
					GameObject gameObject = Instantiate(this.alarmClock, this.playerTransform.position, this.cameraTransform.rotation);
					gameObject.GetComponent<AlarmClockScript>().baldi = this.baldiScrpt;
					gameObject.GetComponent<AlarmClockScript>().player = this.player.GetComponent<PlayerScript>();
					this.ResetItem(7);
					break;
				case 8: // NoSquee
					Ray ray5 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit5;
					if (Physics.Raycast(ray5, out raycastHit5) && (raycastHit5.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 10f))
					{
						raycastHit5.collider.gameObject.GetComponent<ClassroomDoorScript>().SilenceDoor();
						this.ResetItem(8);
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
						this.ResetItem(9);
					}
					else if (Physics.Raycast(ray6, out raycastHit6) && raycastHit6.collider.name == "1st Prize")
					{
						this.firstPrizeScript.GoCrazy();
						this.ResetItem(9);
					}
					break;
				case 10: // Bigass boots
					this.player.ActivateBoots();
					base.StartCoroutine(this.BootAnimation());
					this.ResetItem(10);
					break;
				case 11: // Speedy Sneakers
					this.player.ActivateSpeedShoes();
					this.ResetItem(11);
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
					this.ResetItem(13);
					this.player.ResetGuilt("drink", 1f);
					this.audioDevice.PlayOneShot(this.aud_Soda);
					break;
				case 14: // Crystal Zest
					this.player.stamina += 70f;
					this.audioDevice.PlayOneShot(this.aud_EatFood);
					this.ResetItem(14);
					break;
				case 15: // Party Popper
					if (this.movingPartyLocation != null)
					{
						this.ActivateParty();
						this.ResetItem(15);
					}
					else
						this.audioDevice.PlayOneShot(this.aud_Error);
					break;
				case 17:
					Ray ray8 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit8;
					if (Physics.Raycast(ray8, out raycastHit8) && Vector3.Distance(this.playerTransform.position, raycastHit8.transform.position) <= 10f
					&& raycastHit8.collider.tag == "BreakableWindow")
					{
						WindowScript window = raycastHit8.collider.gameObject.GetComponent<WindowScript>();
						if (!window.isBroken)
						{
							if (this.baldiScrpt.isActiveAndEnabled)
								this.baldiScrpt.AddNewSound(window.agentObstacle.transform.position, 3);
								
							window.BreakWindow();
							this.audioDevice.PlayOneShot(this.aud_GlassBreak, 0.8f);
							this.ResetItem(17);
							this.handIconScript.ChangeIcon(0);
						}
					}
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

					Array.Resize(ref this.stats.itemsUsed, this.stats.itemsUsed.Length + 1);
					this.stats.itemsUsed[this.stats.itemsUsed.Length - 1] = 5;
					this.stats.lifetimeItems[5]++;
				}
				else if (raycastHit3.collider.name == "PayPhone" && Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
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

					Array.Resize(ref this.stats.itemsUsed, this.stats.itemsUsed.Length + 1);
					this.stats.itemsUsed[this.stats.itemsUsed.Length - 1] = 5;
					this.stats.lifetimeItems[5]++;
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

		if (this.dollarAmount == 0f)
			this.walletSlot.SetActive(false);
		else
			this.walletSlot.SetActive(true);
	}

	private void PrintDollarAmount()
	{
		if (!this.forceQuarterPickup)
		{
			this.dollarTextCenter.text = "$" + this.dollarAmount.ToString("0.00");
			this.dollarTextTop.text = "$" + this.dollarAmount.ToString("0.00");
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

		this.dollarTextCenter.color = this.masterTextColor;
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
				this.isItemUpgrade = true;
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
		this.isParty = true;
		this.audioDevice.PlayOneShot(this.aud_BalloonPop);
		this.partyLocation = this.movingPartyLocation;
		this.wanderer.partyPoints = this.wanderer.movingPartyPoints;
		Instantiate(this.party, this.partyLocation.position, this.cameraTransform.rotation);
		
		if (principalScript.isActiveAndEnabled)
			principalScript.GoToParty();
		if (firstPrizeScript.isActiveAndEnabled)
			firstPrizeScript.GoToParty();
		if (baldiScrpt.isActiveAndEnabled)
			baldiScrpt.GoToParty();
		if (playtimeScript.isActiveAndEnabled)
			playtimeScript.GoToParty();
		if (sweepScript.isActiveAndEnabled)
			sweepScript.GoToParty();
		if (crafterScript.isActiveAndEnabled)
			crafterScript.isParty = true;
		
		StartCoroutine(PartyRoutine());
	}

	IEnumerator PartyRoutine()
    {
        if (this.mode == "endless")
			this.remainingPartyTime = 60f;
		else
			this.remainingPartyTime = (-1.5f / this.daFinalBookCount * this.notebooks + 2f) * 60f;
		
		while (this.remainingPartyTime > 0f)
        {
            this.remainingPartyTime -= Time.deltaTime;
			yield return null;
        }
		this.DeactivateParty();
    }

	public void DeactivateParty()
	{
		this.isParty = false;

		if (principalScript.isActiveAndEnabled)
			principalScript.LeaveParty();
		if (firstPrizeScript.isActiveAndEnabled)
			firstPrizeScript.isParty = false;
		if (baldiScrpt.isActiveAndEnabled)
			baldiScrpt.isParty = false;
		if (playtimeScript.isActiveAndEnabled)
			playtimeScript.LeaveParty();
		if (sweepScript.isActiveAndEnabled)
			sweepScript.LeaveParty();
		if (crafterScript.isActiveAndEnabled)
			crafterScript.isParty = false;
	}

	private void SendCharacterHome(string character)
	{
		this.charInAttendance = character;
		this.attendanceBlocker.SetActive(false);
		switch(character)
		{
			case "Playtime":
				this.playtimeScript.GoToAttendance();
				this.player.DeactivateJumpRope();
				this.ResetItem(12);
				break;
			case "Gotta Sweep":
				this.sweepScript.GoToAttendance();
				this.ResetItem(12);
				break;
			case "1st Prize":
				this.firstPrizeScript.GoToAttendance();
				this.ResetItem(12);
				break;
		}
	}

	private IEnumerator BootAnimation()
	{
		float time = 30f;
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

	private void ResetItem(ushort item_ID)
	{
		this.item[this.itemSelected] = 0;
		this.itemSlot[this.itemSelected].texture = this.itemTextures[0];
		this.UpdateItemName(false);

		Array.Resize(ref this.stats.itemsUsed, this.stats.itemsUsed.Length + 1);
		this.stats.itemsUsed[this.stats.itemsUsed.Length - 1] = item_ID;

		try {
			this.stats.lifetimeItems[item_ID]++;
		}
		catch {
			Array.Resize(ref this.stats.lifetimeItems, this.itemNames.Length);
			this.stats.lifetimeItems[item_ID]++;
		}
	}

	public void LoseItem(int id)
	{
		this.item[id] = 0;
		this.itemSlot[id].texture = this.itemTextures[0];
		this.UpdateItemName(false);
	}

	private void UpdateItemName(bool isWallet)
	{
		if (isWallet)
			this.itemText.text = "Wallet";
		else
			this.itemText.text = this.itemNames[this.item[this.itemSelected]];
	}

	public void ExitReached()
	{
		this.exitsReached++;
		this.AngrySchoolColors(this.exitsReached);

		if (this.exitsReached == 1)
			StartCoroutine(this.ChaosAudio());

		if (this.exitsReached != 2 || this.isSafeMode)
			this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);

		if (this.modeType == "nullStyle")
		{
			switch (this.exitsReached)
			{
				case 1:
					this.audioDevice.loop = true;
					this.audioDevice.clip = this.quietNoiseLoop;
					this.audioDevice.Play();
					break;
				case 2:
					this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
					this.audioDevice.volume = 0.85f;
					this.audioDevice.loop = true;
					this.audioDevice.clip = this.glambience;
					this.audioDevice.Play();
					break;
				case 3:
					this.baldi.SetActive(false);
					this.entranceDarkSources[0] = this.GetFinalDarkSource();
					break;
			}
		}
		else if ((this.exitsReached == this.entranceList.Length - 1) && !this.isSafeMode)
		{
			this.entranceDarkSources[0] = this.GetFinalDarkSource();
			this.isDynamicColor = true;
			this.ToggleFlashlight(0.5f);
		}
	}
	
	Transform GetFinalDarkSource()
	{
		Transform finalSource = null;
		for (int i = 0; i < this.entranceList.Length; i++)
		{
			if (entranceDarkSources[i] != null)
			{
				finalSource = entranceDarkSources[i];
				break;
			}
		}
		return finalSource;
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

	// 0 = Stop All, 1 = schoolhouse, 2 = learn, 3 = party
	public void MusicPlayer(int songType)
	{
		switch (songType)
        {
            case 0:
				this.schoolMusic.Stop();
				this.learnMusic.Stop();
				this.escapeDevice.Stop();
				break;
        }
	}

	IEnumerator MusicRoutine(int initialCase)
    {
		if (this.isMusicStarted)
			yield break;
		
		this.isMusicStarted = true;
        int[] lastSongs = {
            0, 0
        };

		switch (initialCase)
        {
            case 0:
				if (this.isSafeMode || !this.spoopMode)
					goto case 30;
				else if (!this.isAdditionalMusic)
					goto case 2; 
				else
                {
                    int nextSong = GetRandomSong(lastSongs[0], lastSongs[1]);
					lastSongs[0] = lastSongs[1];
					lastSongs[1] = nextSong;
					Debug.LogWarning("Now playing: Song " + nextSong);
					this.musicDevice.clip = this.backgroudMusicPlaylist[lastSongs[1]];
					this.musicDevice.loop = false;
					this.musicDevice.Play();
					goto case 1;
                }
			case 1: // Regular music with Additional Music enabled
                while (!this.isParty && !this.finaleMode && this.musicDevice.isPlaying)
					yield return null;
				if (!this.musicDevice.isPlaying)
					goto case 0;
				else
					goto case 3;
			case 2: // No additional music
				while (!this.finaleMode && !this.isParty)
					yield return null;
				goto case 3;
			case 3: // Check for usage of the party popper
				this.musicDevice.Stop();
				if (this.isParty)
					goto case 11;
				else
					goto case 20;
			case 7:
				while (!this.finaleMode && !this.isParty)
					yield return null;
				if (this.finaleMode)
					goto case 24;
				else
                {
					this.schoolMusic.Stop();
                    goto case 11;
                }
			case 11: // Party Popper
				this.musicDevice.loop = true;
				this.musicDevice.clip = this.partyMusic;
				this.musicDevice.Play();
				while (this.isParty)
					yield return null;
				this.musicDevice.Stop();
				if (this.finaleMode)
					goto case 20;
				else
					goto case 0;
			case 20: // Check whether the player has reached any exit or not
				if (this.escapeMusicStage == 1 && !this.isSafeMode)
                {
					this.musicDevice.loop = true;
                    this.musicDevice.clip = this.escapeMusic[0];
					this.musicDevice.Play();
					goto case 21;
                }
				else if (this.isSafeMode)
					goto case 24;
				else
					goto case 23;
			case 21: // Play the initial escape music
				while (this.escapeMusicStage == 1 && !this.isParty)
					yield return null;
				this.musicDevice.Stop();
				if (this.isParty)
					goto case 11;
				else if (this.escapeMusicStage == 2)
					goto case 22;
				else
					goto case 23;
			case 22: // Play the transition to the slow loop
				this.escapeMusicStage = 2;
				this.musicDevice.loop = false;
				this.musicDevice.clip = this.escapeMusic[1];
				this.musicDevice.Play();
				while (this.musicDevice.isPlaying && !this.isParty)
					yield return null;
				this.escapeMusicStage = 3;
				if (this.isParty)
					goto case 11;
				else
					goto case 23;
			case 23: // Play the slow loop
				this.musicDevice.loop = true;
				this.musicDevice.clip = this.escapeMusic[2];
				this.musicDevice.Play();
				while (!this.isParty)
					yield return null;
				if (this.isParty)
					goto case 11;
				else
					yield break;
			case 24: // Escape music in safe mode
				this.musicDevice.loop = true;
				this.musicDevice.clip = this.escapeMusic[0];
				this.musicDevice.Play();
				while (!this.isParty)
					yield return null;
				goto case 11;
			case 30: // Normal school music
				if (this.finaleMode)
					goto case 24;
				this.musicDevice.clip = this.schoolMusic.clip;
				this.musicDevice.loop = true;
				this.musicDevice.Play();
				while (!this.learningActive && !this.isParty)
					yield return null;
				if (this.isParty)
                {
					this.musicDevice.Stop();
                    goto case 11;
                }
				else
					goto case 31;
			case 31:
				this.musicDevice.Stop();
				while (this.learningActive)
					yield return null;
				goto case 0;
        }
    }

	int GetRandomSong(int song1, int song2)
    {
		int songID = 0;
		if (song1 == song2)
        {
            songID = Mathf.FloorToInt(UnityEngine.Random.Range(0f, (float)backgroudMusicPlaylist.Length - 0.1f));
			return songID;
        }

		while (songID == song1 || songID == song2)
        {
            songID = Mathf.FloorToInt(UnityEngine.Random.Range(0f, (float)backgroudMusicPlaylist.Length - 0.1f));
			Debug.Log("Rerolled song: " + songID);
        }
		return songID;
    }

	private IEnumerator ChaosAudio()
	{
		string state = "start";

		switch(state)
		{
			case "start":
				if (this.modeType == "nullStyle" || this.isSafeMode)
					yield break;
				this.chaosDevice.loop = false;
				this.chaosDevice.volume = 0.8f;
				this.chaosDevice.clip = this.chaosEarly;
				while (this.exitsReached == 1)
					yield return null;
				goto case "earlyLoop";

			case "earlyLoop":
				this.chaosDevice.Play();
				while (this.exitsReached == 2 && this.chaosDevice.isPlaying)
					yield return null;
				if (this.exitsReached > 2)
				{
					this.chaosDevice.Stop();
					goto case "finalLoop";
				}
				else
				{
					this.chaosDevice.clip = this.chaosEarlyLoop;
					this.chaosDevice.loop = true;
					this.chaosDevice.Play();
					while (this.exitsReached == 2)
						yield return null;
					this.chaosDevice.Stop();
					goto case "finalLoop";
				}
			
			case "finalLoop":
				this.chaosDevice.loop = false;
				this.chaosDevice.clip = this.chaosBuildup;
				this.chaosDevice.Play();
				while (this.chaosDevice.isPlaying)
					yield return null;
				this.chaosDevice.clip = this.chaosFinalLoop;
				this.chaosDevice.loop = true;
				this.chaosDevice.Play();
				break;
		}
	}

	private void AngrySchoolColors(int phase)
	{
		if (!this.isSafeMode && this.modeType != "nullStyle")
		{
			RenderSettings.skybox = this.redSky;
			StartCoroutine(ChangeSchoolColor(phase));
			StartCoroutine(ChangeFogColor(phase));
		}
	}

	private IEnumerator ChangeSchoolColor(int phase)
	{
		float curValue;

		if (phase == 2)
		{
			curValue = 1f;
			while (curValue > 0f)
			{
				curValue -= Time.deltaTime/5f;
				RenderSettings.ambientLight = new Color(1f, curValue, curValue);
				yield return null;
			}
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
				while (curValue < 0.02f)
				{
					curValue += Time.deltaTime/100f;
					RenderSettings.fogDensity = curValue;
					yield return null;
				}
				RenderSettings.fogDensity = 0.02f;
				curValue = 1f;
				while (curValue > 0f)
				{
					curValue -= Time.deltaTime/5f;
					RenderSettings.fogColor = new Color(1f, curValue, curValue);
					yield return null;
				}
				break;
		}
	}

	[HideInInspector] public int daFinalBookCount;
	[HideInInspector] public int totalSlotCount;
	[HideInInspector] public EntranceScript[] entranceList;
	[HideInInspector] public Transform attendanceOffice;
	[HideInInspector] public Vector3 detentionPlayerPos;
	[HideInInspector] public Vector3 detentionPrincipalPos;
	[HideInInspector] public bool forceQuarterPickup;


	[Header("Game State")]
	public bool spoopMode;
	public bool finaleMode;
	public bool learningActive;
	public bool isSlowmo;
	public bool isAdditionalMusic;
	public bool isSafeMode;
	public bool isDifficultMath;
	public bool isDoorFix;
	public bool mouseLocked;
	public bool gamePaused;
	public bool showTimer;
	[SerializeField] bool forceQuarter;
	[SerializeField] bool isLookingAtVendingMachine;
	[SerializeField] bool isItemUpgrade;
	public bool isParty;
	public bool isGameFail;
	[SerializeField] private bool isScareStarted;
	public bool ignoreInitializationChecks;
	public bool isDynamicColor;
	[SerializeField] bool isMusicStarted;
	public byte notebooks;
	[SerializeField] private int highBooksScore;
	[SerializeField] private float bestTime;
	[SerializeField] private float speedrunSeconds;
	[SerializeField] private uint speedrunMinutes;
	[SerializeField] private uint speedrunHours;
	public byte exitsReached;
	[SerializeField] byte escapeMusicStage;
	[SerializeField] private float dollarAmount;
	[SerializeField] private float remainingPartyTime;
	public float gameOverDelay;
	[SerializeField] private float darkLevel;
	public string mode;
	public string modeType;
	public string curMap;
	[SerializeField] private string charInAttendance;
	public Transform partyLocation;
	public Transform movingPartyLocation;
	[SerializeField] private GameObject curItem;
	private Color masterTextColor;
	public Transform[] entranceDarkSources;

	[Header("UI")]
	[HideInInspector] public Light[] playerFlashlights;
	[HideInInspector] public GameObject notebookObject;
	[HideInInspector] public TMP_Text notebookCountText;
	[HideInInspector] public NotebookCountScript notebookCountScript;
	[SerializeField] GameObject pauseMenu;
	[SerializeField] GameObject highScoreText;
	[HideInInspector] public TMP_Text staminaPercentText;
	[HideInInspector] public RectTransform itemSelect;
	[SerializeField] private float[] itemSelectOffset;
	[HideInInspector] public TMP_Text dollarTextCenter;
	[HideInInspector] public TMP_Text dollarTextTop;
	[HideInInspector] public GameObject exitCountGroup;
	[HideInInspector] public TMP_Text exitCountText;
	[HideInInspector] public TMP_Text fpsCounter;
	[SerializeField] private float fpsTimer;
	[HideInInspector] public MapCameraScript mapScript;
	[HideInInspector] public TMP_Text speedrunText;
	[SerializeField] private Canvas mainHud;


	[Header("Noteboos")]
	public byte failedNotebooks;
	[SerializeField] private GameObject mathGameUI;


	[Header("Player")]
	public Transform playerTransform;
	public Transform cameraTransform;
	[SerializeField] private Camera playerCamera;
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
	public bool IsNoItems()
	{
		byte emptySlots = 0;

		for (byte i = 0; i < this.totalSlotCount; i++)
		{
			if (this.item[i] == 0)
				emptySlots++;
		}

		if (emptySlots == this.totalSlotCount)
			return true;
		else
			return false;
	}


	[Header("Items")]
	public int[] item;
	public RawImage[] itemSlot;
	public Texture[] itemTextures;
	public TMP_Text itemText;
	public Sprite[] pickup_itemSprites;
	public Sprite[] pickup_itemMapSprites;
	readonly string[] itemNames = new string[]
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
		"Party Popper",
		"Dollar Bill",
		"Dangerous Hammer"
	};
	public GameObject quarter;
	public GameObject bsodaSpray;
	public GameObject dietBsodaSpray;
	public RectTransform boots;
	public GameObject alarmClock;
	[SerializeField] private GameObject party;
	[SerializeField] private GameObject attendanceBlocker;


	[Header("Detention")]
	public bool isPrinceyTriggerShared;
	public bool isPrinceyIgnore;
	public float remainingDetentionTime;
	[SerializeField] GameObject minimalUI;
	[SerializeField] TMP_Text minimalSpeedrunText;
	[SerializeField] TMP_Text minimalFpsCounter;
	[SerializeField] Image minimalCenterIcon;


	[Header("SFX and Voices")]
	[SerializeField] private AudioClip aud_Prize;
	[SerializeField] private AudioSource audioDevice;
	[SerializeField] private AudioClip aud_AllNotebooks;
	[SerializeField] private AudioSource tutorBaldi;
	[SerializeField] private AudioClip aud_Soda;
	[SerializeField] private AudioClip aud_Spray;
	[SerializeField] private AudioClip aud_EatFood;
	[SerializeField] private AudioClip aud_Hang;
	[SerializeField] private AudioClip aud_Error;
	[SerializeField] private AudioClip aud_Switch;
	[SerializeField] private AudioClip aud_BigClose;
	[SerializeField] private AudioClip[] baldiJumpscareSounds;
	[SerializeField] private AudioSource chaosDevice;
	[SerializeField] private AudioClip chaosEarly;
	[SerializeField] private AudioClip chaosEarlyLoop;
	[SerializeField] private AudioClip chaosBuildup;
	[SerializeField] private AudioClip chaosFinalLoop;
	[SerializeField] private AudioClip quietNoiseLoop;
	[SerializeField] private AudioClip glambience;
	[SerializeField] private AudioClip aud_BalloonPop;
	[SerializeField] private AudioClip aud_GlassBreak;
	[SerializeField] private AudioClip aud_NotebookCollect;
	[SerializeField] private AudioClip aud_ItemCollect;
	[SerializeField] private AudioClip aud_CoinCollect;


	[Header("Music")]
	[SerializeField] AudioSource schoolMusic;
	[SerializeField] private AudioSource escapeDevice;
	public AudioClip[] escapeMusic;
	public AudioSource endlessMusic;
	public AudioSource learnMusic;
	public AudioSource spoopLearn;
	public AudioClip LearnQ1;
	public AudioClip LearnQ2;
	public AudioClip LearnQ3;
	[SerializeField] AudioClip partyMusic;
	[SerializeField] AudioClip[] backgroudMusicPlaylist;
	[SerializeField] AudioSource musicDevice;
	[SerializeField] AudioSource[] schoolhouseTroublePlaylist;


	[Header("Scripts")]
	public GameObject[] childScripts;
	[SerializeField] ChallengeController challengeController;
	[HideInInspector] public StatisticsController stats;
	private DebugSceneLoader sceneLoader;
	[SerializeField] private MathMusicScript mathMusicScript;
	public CursorControllerScript cursorController;
	public PlayerScript player;
	public BaldiScript baldiScrpt;
	public PlaytimeScript playtimeScript;
	public FirstPrizeScript firstPrizeScript;
	[SerializeField] PrincipalScript principalScript;
	[SerializeField] CraftersScript crafterScript;
	[SerializeField] private AILocationSelectorScript wanderer;
 	[SerializeField] private SweepScript sweepScript;
	[SerializeField] private AudioManager audioManager;
	[HideInInspector] public HandIconScript handIconScript;
}