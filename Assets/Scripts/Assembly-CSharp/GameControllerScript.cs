using System;
using System.Collections;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
	private void Awake()
	{
		audioManager = FindObjectOfType<AudioManager>();
		handIconScript = FindObjectOfType<HandIconScript>();

		this.sceneLoader = this.childScripts[0].GetComponent<DebugSceneLoader>();
		this.stats = this.childScripts[1].GetComponent<StatisticsController>();
	}

	private void Start()
	{
		Scene curScene = SceneManager.GetActiveScene();
		string curSceneName = curScene.name;

		Debug.Log("Loaded " + curSceneName);
		Debug.Log("Safe Mode: " + PlayerPrefs.GetInt("gps_safemode"));
		Debug.Log("Difficult Math: " + PlayerPrefs.GetInt("gps_difficultmath"));

		this.LockMouse(); //Prevent the mouse from moving

		this.isParty = false;

		switch(curSceneName)
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

				if (PlayerPrefs.GetInt("gps_safemode") == 1)
					this.isSafeMode = true;
				else
					this.isSafeMode = false;

				if (PlayerPrefs.GetInt("gps_difficultmath") == 1)
					this.isDifficultMath = true;
				else
					this.isDifficultMath = false;

				if (this.mode == "endless")
					this.baldiScrpt.endless = true;

				if (PlayerPrefs.GetInt("AdditionalMusic") == 1)
					this.isAdditionalMusic = true;
				break;
		}

		this.cullingMask = this.camera.cullingMask; // Changes cullingMask in the Camera
		this.audioDevice = base.GetComponent<AudioSource>(); //Get the Audio Source
		this.mode = PlayerPrefs.GetString("CurrentMode"); //Get the current mode

		this.curMap = curSceneName;
		PlayerPrefs.SetString("CurrentMap", this.curMap);

		this.dollarTextTop.color = this.masterTextColor;
		this.speedrunText.color = this.masterTextColor;
		this.itemText.color = this.masterTextColor;
		this.notebookCount.color = this.masterTextColor;

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

		if (PlayerPrefs.GetInt("op_showtimer") == 1)
			this.showTimer = true;

		this.speedrunSeconds = 0f;
		
		if (this.modeType != "nullStyle")
			this.MusicPlayer(1,1); //Play the school music

		this.UpdateNotebookCount(); //Update the notebook count
		this.itemSelected = 0; //Set selection to item slot 0(the first item slot)
		this.UpdateDollarAmount(0f);
		this.StartCoroutine(this.WaitForQuarterDisable(true, false));

		if (this.isSafeMode)
			this.stats.disableSaving = true;
		this.stats.itemsUsed = new int[0];
		this.stats.detentions = 0;

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

	public void InitializeScores()
	{
		this.bestTime = stats.data_bestTime[stats.mapID];
		this.highBooksScore = stats.data_notebooks[stats.mapID];
	}

	private void Update()
	{
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
		else
		{
			if (Time.timeScale != 0f) Time.timeScale = 0f;
		}

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
			{
				this.highScoreText.SetActive(true);
			}
			Time.timeScale = 0f;
			this.gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
			this.camera.farClipPlane = this.gameOverDelay * 400f; //Set camera farClip

			if (!this.player.isSecret)
			{
				this.MusicPlayer(0,0);
				this.endlessMusic.Stop();
			}

			if (!this.player.isSecret && !this.isScareStarted)
			{
				if (this.modeType == "nullStyle")
					this.StartCoroutine(this.NullKill());
				
				if (this.mode != "endless")
					this.stats.SaveAllData(null);
				else if (this.notebooks > this.highBooksScore && !this.isScareStarted)
				{
					this.stats.notebooks = this.notebooks;
					this.stats.SaveAllData("notebooks");
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

		if (this.finaleMode && !this.audioDevice.isPlaying && this.exitsReached == 2 && this.modeType != "nullStyle")
		{
			this.audioDevice.clip = this.chaosEarlyLoop;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
		}
		else if (this.finaleMode && !this.audioDevice.isPlaying && this.exitsReached == 3 && this.modeType != "nullStyle")
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

		if (this.modeType == "nullStyle" && this.isDynamicColor)
		{
			Vector3 distance = this.baldi.transform.position - this.playerTransform.position;
			float sqrLen = distance.sqrMagnitude;
			this.darkLevel = Mathf.Sqrt(sqrLen / 300000f);
			
			if (this.darkLevel >= 0.4f)
				RenderSettings.ambientLight = new Color(0.4f, 0.4f, 0.4f);
			else
				RenderSettings.ambientLight = new Color(this.darkLevel, this.darkLevel, this.darkLevel);
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

	private IEnumerator NullKill()
	{
		if (this.isDynamicColor)
			RenderSettings.ambientLight = Color.black;
			
		this.isDynamicColor = false;
		RenderSettings.ambientIntensity = 0f;

		float remTime = 10f;

		while (remTime > 6f)
		{
			RenderSettings.ambientIntensity += Time.unscaledDeltaTime  / 7f;
			RenderSettings.ambientLight += new Color(Time.unscaledDeltaTime / 8f, Time.unscaledDeltaTime  / 8f, Time.unscaledDeltaTime  / 8f);
			remTime -= Time.unscaledDeltaTime;
			yield return null;
		}

		RenderSettings.ambientLight = this.RandomColor();

		while (remTime > 4f)
		{
			RenderSettings.ambientIntensity += Time.unscaledDeltaTime  / 7f;
			RenderSettings.ambientLight += new Color(Time.unscaledDeltaTime / 4f, Time.unscaledDeltaTime  / 4f, Time.unscaledDeltaTime  / 3f);
			remTime -= Time.unscaledDeltaTime;
			yield return null;
		}

		RenderSettings.ambientLight = this.RandomColor();

		while (remTime > 3f)
		{
			RenderSettings.ambientIntensity += Time.unscaledDeltaTime  / 7f;
			RenderSettings.ambientLight += new Color(Time.unscaledDeltaTime / 4f, Time.unscaledDeltaTime  / 4f, Time.unscaledDeltaTime  / 3f);
			remTime -= Time.unscaledDeltaTime;
			yield return null;
		}

		RenderSettings.ambientLight = this.RandomColor();

		while (remTime > 2f)
		{
			RenderSettings.ambientIntensity += Time.unscaledDeltaTime  / 7f;
			RenderSettings.ambientLight += new Color(Time.unscaledDeltaTime / 4f, Time.unscaledDeltaTime  / 4f, Time.unscaledDeltaTime  / 3f);
			remTime -= Time.unscaledDeltaTime;
			yield return null;
		}

		RenderSettings.ambientLight = this.RandomColor();

		while (remTime > 1f)
		{
			RenderSettings.ambientIntensity += Time.unscaledDeltaTime  / 7f;
			RenderSettings.ambientLight += new Color(Time.unscaledDeltaTime / 4f, Time.unscaledDeltaTime  / 4f, Time.unscaledDeltaTime  / 3f);
			remTime -= Time.unscaledDeltaTime;
			yield return null;
		}

		RenderSettings.ambientLight = new Color(0.8f, 0f, 0f);

		while (remTime > 0f)
		{
			RenderSettings.ambientIntensity += Time.unscaledDeltaTime  / 7f;
			remTime -= Time.unscaledDeltaTime;
			yield return null;
		}
	}

	private UnityEngine.Color RandomColor()
	{
		float r = UnityEngine.Random.Range(0f, 1f);
		float g = UnityEngine.Random.Range(0f, 1f);
		float b = UnityEngine.Random.Range(0f, 1f);

		return new Color(r, g, b);
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
		if (this.mode == "story" || this.mode == "challenge")
			this.notebookCount.text = this.notebooks.ToString() + "/" + daFinalBookCount.ToString() + " Notebooks";
		else
			this.notebookCount.text = this.notebooks.ToString() + " Notebooks";

		if (this.notebooks == daFinalBookCount && (this.mode == "story" || this.mode == "challenge"))
		{
			this.exitCountGroup.SetActive(true);
			this.notebookCount.text = string.Empty;
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
				if (this.notebooks == 2)
				{
					this.baldi.SetActive(true);
					this.ModifyExits("lower");
				}

				if (this.notebooks >= 2)
				{
					this.baldiScrpt.GetAngry(1f);
					if (this.baldiScrpt.isActiveAndEnabled)
						this.baldiScrpt.AddNewSound(this.player.transform.position, 2);
				}
				break;
			default:
				GameObject gameObject = Instantiate(this.mathGameUI);
				gameObject.GetComponent<MathGameScript>().gc = this;
				gameObject.GetComponent<MathGameScript>().baldiScript = this.baldiScrpt;
				gameObject.GetComponent<MathGameScript>().playerPosition = this.player.GetComponent<Transform>().position;
				break;
		}
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
		this.mathMusicScript.StopSong();
		this.audioDevice.PlayOneShot(this.aud_Hang);
	}

	public void ActivateSafeMode()
	{
		if (!this.isGameFail)
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
	}

	private void ActivateFinaleMode()
	{
		this.finaleMode = true;
		this.ModifyExits("raise");
	}

	public void ActivateBossFight(Vector3 nullPos)
	{
		this.speedrunText.color = Color.black;
		this.audioDevice.Stop();
		this.nullBoss.SetActive(true);
		this.audioDevice.PlayOneShot(this.aud_BigClose, 0.6f);
		this.nullBoss.GetComponent<NullBoss>().WarpToExit(nullPos);
		RenderSettings.ambientLight = new Color(1f, 1f, 1f);
		this.playerFlashlight[0].intensity = 0f;
		this.playerFlashlight[1].intensity = 0f;
		this.mainHud.renderMode = RenderMode.WorldSpace;
		this.mapScript.DisableAllItems();

		this.createdProjectiles = 3;
		this.allowedProjectiles = 10;

		this.dollarAmount = 0f;
		for (int i = 0; i < this.totalSlotCount; i++)
			this.item[i] = 0;
	}

	public void CreateProjectile(int number)
	{
		if (this.createdProjectiles == 0)
		{
			this.createdProjectiles = 3;
			this.projectilesInPlay = new GameObject[3];
			for (int i = 0; i < this.projectilesInPlay.Length; i++)
				this.projectilesInPlay[i] = Instantiate(this.projectile, this.wanderer.NewTarget("Projectile"), Quaternion.identity);
		}
		else
		{
			this.createdProjectiles += number;
			Array.Resize(ref this.projectilesInPlay, this.projectilesInPlay.Length + 1);
			this.projectilesInPlay[this.projectilesInPlay.Length - 1] = Instantiate(this.projectile, this.wanderer.NewTarget("Projectile"), Quaternion.identity);
		}
	}

	public IEnumerator WaitForProjectile()
	{
		NullBoss nullBoss = this.nullBoss.GetComponent<NullBoss>();
		int curHit = nullBoss.hits;
		float remTime = 5.1f;

		while (remTime > 0f && !(nullBoss.hits == curHit))
		{
			remTime -= Time.deltaTime;
			yield return null;
		}

		if (this.createdProjectiles <= this.allowedProjectiles)
		{
			this.CreateProjectile(1);
		}
	}

	public void DeleteProjectiles()
	{
		foreach (GameObject i in this.projectilesInPlay)
			Destroy(i);
	}

	public void DeactivateBossFight()
	{
		this.ModifyExits("raise");
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
			if (this.isAdditionalMusic)
			{
				if (notebooks > 2)
				{
					if (audioManager != null)
						audioManager.SetVolume(1);
					
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
				else if (this.notebooks == 1 && this.audioManager != null)
					audioManager.SetVolume(0);
			}
			else if (this.audioManager != null)
				this.audioManager.SetVolume(0);
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

			if (this.isAdditionalMusic && !this.isEndlessSong)
			{
				if (!this.isParty && !this.isSafeMode)
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
			this.curItem.GetComponent<PickupScript>().ChangeItem(0);
			this.UpdateDollarAmount(0.25f);
			return;
		}
		if (item_ID == 16)
		{
			this.curItem.GetComponent<PickupScript>().ChangeItem(0);
			this.UpdateDollarAmount(1f);
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
						this.ResetItem(1);
					break;
				case 2: // Yellow Door Lock
					Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
					{
						raycastHit.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
						this.ResetItem(2);
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
							component.OpenDoor(3f);
							this.ResetItem(3);
						}
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
						raycastHit5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
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
					this.ResetItem(14);
					break;
				case 15: // Party Popper
					if (!(this.movingPartyLocation == null))
					{
						this.partyLocation = this.movingPartyLocation;
						this.wanderer.partyPoints = this.wanderer.movingPartyPoints;
						this.MusicPlayer(0,0);
						Instantiate(this.party, this.partyLocation.position, this.cameraTransform.rotation);
						this.audioDevice.PlayOneShot(this.aud_BalloonPop);
						this.ResetItem(15);
						this.MusicPlayer(4,0);
						this.StartCoroutine(PlayPartyMusic());
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
		if (this.mode == "endless" && this.isAdditionalMusic)
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

	private void ResetItem(int item_ID)
	{
		this.item[this.itemSelected] = 0;
		this.itemSlot[this.itemSelected].texture = this.itemTextures[0];
		this.UpdateItemName(false);

		Array.Resize(ref this.stats.itemsUsed, this.stats.itemsUsed.Length + 1);
		this.stats.itemsUsed[this.stats.itemsUsed.Length - 1] = item_ID;
		this.stats.lifetimeItems[item_ID]++;
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

		if (this.exitsReached == 1 && this.modeType == "nullStyle")
		{
			this.audioDevice.loop = true;
			this.audioDevice.clip = this.quietNoiseLoop;
			this.audioDevice.Play();
		}

		if (this.exitsReached != 2 || this.modeType == "nullStyle")
			this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);

		if (this.exitsReached == 2) //Play a sound
		{
			if (this.modeType == "nullStyle")
			{
				this.audioDevice.volume = 0.85f;
				this.audioDevice.loop = true;
				this.audioDevice.clip = this.glambience;
				this.audioDevice.Play();
			}
			else
			{
				this.audioDevice.volume = 0.8f;
				this.audioDevice.clip = this.chaosEarly;
				this.audioDevice.loop = false;
				this.audioDevice.Play();
				this.escapeMusic.volume = 0.5f;
			}
		}
		else if (this.exitsReached == 3 && this.modeType != "nullStyle") //Play a louder sound
		{
			this.audioDevice.volume = 0.8f;
			this.audioDevice.clip = this.chaosBuildup;
			this.audioDevice.loop = false;
			this.audioDevice.Play();
		}
		else if (this.exitsReached == 3 && this.modeType == "nullStyle")
		{
			this.baldi.SetActive(false);
			this.isDynamicColor = false;
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
			else if (SongId >= 2 && this.isAdditionalMusic && this.mode != "endless")
			{
				if (this.notebooks < this.daFinalBookCount)
					this.schoolhouseTroublePlaylist[SongId - 2].Play();
			}
		}
		else if (songType == 2) // math game
		{
			if (SongId == 1) this.learnMusic.Play();
			else if (SongId == 2 && !this.disableSongInterruption && this.isAdditionalMusic)
			{
				this.spoopLearn.Play();
			}
		}
		else if (songType == 3)
		{
			if (!this.schoolhouseTroublePlaylist[0].isPlaying && !this.schoolhouseTroublePlaylist[1].isPlaying)
				StartCoroutine(UninterruptedMusic());
		}
		else if (songType == 4 && !this.partyMusic.isPlaying)
			this.partyMusic.Play();
	}

	private IEnumerator PlayPartyMusic()
	{
		this.isParty = true;
		this.ActivateParty();
		if (this.mode == "endless")
			this.remainingPartyTime = 84.65f;
		else
			this.remainingPartyTime = (-1.5f / this.daFinalBookCount * this.notebooks + 2f) * 60f;
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

		while (this.schoolhouseTroublePlaylist[0].isPlaying)
			yield return null;

		this.schoolhouseTroublePlaylist[1].Play();
	}

	private void AngrySchoolColors(int phase)
	{
		if (!this.isSafeMode && this.modeType != "nullStyle")
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
	public bool spoopMode;
	public bool finaleMode;
	public bool learningActive;
	public bool isSlowmo;
	public bool isAdditionalMusic;
	public bool isSafeMode;
	public bool isDifficultMath;
	public bool mouseLocked;
	private bool gamePaused;
	public bool showTimer;
	[SerializeField] private bool forceQuarter;
	[SerializeField] private bool isLookingAtVendingMachine;
	[SerializeField] private bool isItemUpgrade;
	public bool isParty;
	public bool isGameFail;
	[SerializeField] private bool isScareStarted;
	public bool ignoreInitializationChecks;
	public bool isDynamicColor;
	public int notebooks;
	[SerializeField] private int highBooksScore;
	[SerializeField] private float bestTime;
	[SerializeField] private float speedrunSeconds;
	[SerializeField] private int speedrunMinutes;
	[SerializeField] private int speedrunHours;
	public int exitsReached;
	[SerializeField] private float dollarAmount;
	[SerializeField] private float remainingPartyTime;
	public float gameOverDelay;
	[SerializeField] private float darkLevel;
	public int createdProjectiles;
	public int allowedProjectiles;
	public string mode;
	public string modeType;
	public string curMap;
	[SerializeField] private string charInAttendance;
	public Transform partyLocation;
	public Transform movingPartyLocation;
	[SerializeField] private GameObject curItem;
	[SerializeField] private GameObject[] projectilesInPlay;
	private Color masterTextColor;


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
	public TMP_Text speedrunText;
	[SerializeField] private Canvas mainHud;
	[SerializeField] private Light[] playerFlashlight;


	[Header("Noteboos")]
	public int failedNotebooks;
	public int notebookCharReturn;
	[SerializeField] private GameObject mathGameUI;


	[Header("Player")]
	public Transform playerTransform;
	public Transform cameraTransform;
	public Camera camera;
	private int cullingMask;
	[SerializeField] private Material redSky;


	[Header("Characters")]
	public GameObject baldiTutor;
	public GameObject baldi;
	[SerializeField] private GameObject nullBoss;
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
		for (int i = 0; i < this.totalSlotCount; i++)
		{
			if (this.item[i] == 0)
				return true;
		}
		return false;
	}


	[Header("Items")]
	public int[] item;
	public RawImage[] itemSlot;
	public Texture[] itemTextures;
	public TMP_Text itemText;
	public Sprite[] pickup_itemSprites;
	public Sprite[] pickup_itemMapSprites;
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
		"Party Popper",
		"Dollar Bill",
		"Hammer"
	};
	public GameObject quarter;
	public GameObject bsodaSpray;
	public GameObject dietBsodaSpray;
	[SerializeField] private GameObject projectile;
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
	public AudioClip aud_Hang;
	[SerializeField] private AudioClip aud_Error;
	public AudioClip aud_Switch;
	[SerializeField] private AudioClip aud_BigClose;
	public AudioClip[] baldiJumpscareSounds;
	public AudioClip chaosEarly;
	public AudioClip chaosEarlyLoop;
	public AudioClip chaosBuildup;
	public AudioClip chaosFinalLoop;
	[SerializeField] private AudioClip quietNoiseLoop;
	[SerializeField] private AudioClip glambience;
	[SerializeField] private AudioClip aud_BalloonPop;
	[SerializeField] private AudioClip aud_GlassBreak;


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
	public AudioSource partyMusic;
	public AudioSource[] schoolhouseTroublePlaylist;


	[Header("Scripts")]
	public GameObject[] childScripts;
	[HideInInspector] public StatisticsController stats;
	private DebugSceneLoader sceneLoader;
	[SerializeField] private MathMusicScript mathMusicScript;
	public CursorControllerScript cursorController;
	public PlayerScript player;
	public BaldiScript baldiScrpt;
	public PlaytimeScript playtimeScript;
	public FirstPrizeScript firstPrizeScript;
	[SerializeField] private AILocationSelectorScript wanderer;
 	[SerializeField] private SweepScript sweepScript;
	[SerializeField] private AudioManager audioManager;
	[SerializeField] private HandIconScript handIconScript;
}