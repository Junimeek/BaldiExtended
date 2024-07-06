using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
	public GameControllerScript()
	{
		int[] array = new int[5];
		array[0] = 109;
		array[1] = 149;
		array[2] = 189;
		array[3] = 229;
		array[4] = 268;
		this.itemSelectOffset = array;
		//base..ctor();
	}

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

		Scene curScene = SceneManager.GetActiveScene();
		string curSceneName = curScene.name;

		if (curSceneName == "SecretMap")
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

		if (PlayerPrefs.GetInt("gps_safemode") == 1) this.isSafeMode = true;
		else this.isSafeMode = false;

		if (PlayerPrefs.GetInt("gps_difficultmath") == 1) this.isDifficultMath = true;
		else this.isDifficultMath = false;

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

		//this.speedrunTimer.allowTime = true;

		//debugScreen.DebugCloseMenu();
	}

	private void Update()
	{
		if (!this.learningActive)
		{
			if (Input.GetButtonDown("Pause"))
			{
				if (!this.gamePaused)
				{
					this.PauseGame();
				}
				else
				{
					this.UnpauseGame();
				}
			}
			if (Input.GetKeyDown(KeyCode.Y) & this.gamePaused)
			{
				this.ExitGame();
			}
			else if (Input.GetKeyDown(KeyCode.N) & this.gamePaused)
			{
				this.UnpauseGame();
			}
			if (!this.gamePaused & Time.timeScale != 1f && !this.isSlowmo)
			{
				Time.timeScale = 1f;
			}
			if (Input.GetMouseButtonDown(1) && Time.timeScale != 0f)
			{
				this.UseItem();
			}
			if ((Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f))
			{
				this.DecreaseItemSelection();
			}
			else if ((Input.GetAxis("Mouse ScrollWheel") < 0f && Time.timeScale != 0f))
			{
				this.IncreaseItemSelection();
			}
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
			if (Time.timeScale != 0f)
			{
				Time.timeScale = 0f;
			}
		}

		/*
		if (this.player.stamina < 0f & !this.warning.activeSelf)
		{
			this.warning.SetActive(true); //Set the warning text to be visible
		}
		else if (this.player.stamina > 0f & this.warning.activeSelf)
		{
			this.warning.SetActive(false); //Set the warning text to be invisible
		}
		*/

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
			if (this.mode == "endless" && this.notebooks > PlayerPrefs.GetInt("HighBooks") && !this.highScoreText.activeSelf)
			{
				this.highScoreText.SetActive(true);
			}
			Time.timeScale = 0f;
			this.gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
			this.camera.farClipPlane = this.gameOverDelay * 400f; //Set camera farClip 
			if (!this.player.isSecret) this.audioDevice.PlayOneShot(this.aud_buzz);

			/*
			int randomScare = UnityEngine.Random.Range(0, this.baldiJumpscareSounds.Length - 1);
			this.audioDevice.PlayOneShot(this.baldiJumpscareSounds[randomScare]);
			*/

			if (PlayerPrefs.GetInt("Rumble") == 1)
			{

			}
			if (this.gameOverDelay <= 0f)
			{
				if (this.mode == "endless")
				{
					if (this.notebooks > PlayerPrefs.GetInt("HighBooks"))
					{
						PlayerPrefs.SetInt("HighBooks", this.notebooks);
					}
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

		/*
		if (this.finaleMode && !this.audioDevice.isPlaying && this.exitsReached == 3)
		{
			this.audioDevice.clip = this.aud_MachineLoop;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
		}
		*/

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

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			DebugStuff();
		}

		if (Input.GetKeyDown(KeyCode.J))
		{
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

		if (this.handIconScript != null) CheckRaycast();
	}

	private void CheckRaycast()
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;
		float distance;

		if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider.name == "_DoorOut" && !(raycastHit.collider.tag == "SwingingDoor"))
			distance = 15f;
		else distance = 10f;

		if (this.player.jumpRope && this.item[this.itemSelected] == 9)
			handIconScript.ChangeIcon(8);
		else if (Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= distance)
		{
			if (raycastHit.collider.name == "1st Prize" && this.item[this.itemSelected] == 9)
				handIconScript.ChangeIcon(8);
			else if (raycastHit.collider.tag == "Notebook")
			{
				if (raycastHit.collider.name == "PayPhone" && this.item[this.itemSelected] == 5)
					handIconScript.ChangeIcon(3);
				else if (raycastHit.collider.name == "TapePlayer" && this.item[this.itemSelected] == 6)
					handIconScript.ChangeIcon(5);
				else if (!(raycastHit.collider.name == "PayPhone" || raycastHit.collider.name == "TapePlayer"))
					handIconScript.ChangeIcon(2);
			}
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
			else if ((raycastHit.collider.name == "BSODAMachine" || raycastHit.collider.name == "ZestyMachine") && this.item[this.itemSelected] == 5)
				handIconScript.ChangeIcon(3);
			else if (raycastHit.collider.tag == "Item")
				handIconScript.ChangeIcon(1);
			else handIconScript.ChangeIcon(0);
		}
		else handIconScript.ChangeIcon(0);
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
			this.ActivateFinaleMode();
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
		this.entrance_0.Lower(); //Lowers all the exits
		this.entrance_1.Lower();
		this.entrance_2.Lower();
		this.entrance_3.Lower();
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
	}

	private void ActivateSafeMode()
	{
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
		this.entrance_0.Raise(); //Raise all the enterances(make them appear)
		this.entrance_1.Raise();
		this.entrance_2.Raise();
		this.entrance_3.Raise();
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
						MusicPlayer(0,0);
						MusicPlayer(2,2);
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
		UnityEngine.Object.Destroy(subject);
		this.LockMouse(); //Prevent the mouse from moving
		if (this.player.stamina < 100f) //Reset Stamina
		{
			this.player.stamina = 100f;
		}
		if (!this.spoopMode) //If it isn't spoop mode, play the school music
		{
			MusicPlayer(0,0);
			MusicPlayer(1,1);
		}

		if (this.isSafeMode && this.notebooks == 2) ActivateSafeMode();

		if (this.spoopMode && notebooks >= 2) // my music stuff
		{
			if (notebooks >= 2 && notebooks < this.daFinalBookCount && this.disableSongInterruption)
			{
				MusicPlayer(3,0);
				return;
			}
			else MusicPlayer(0,0);

			if (audioManager != null) audioManager.SetVolume(0);
			
			if (this.notebooks < this.daFinalBookCount)
			{
				if (this.notebooks == 2) MusicPlayer(1,2);
				else if (this.notebooks == 3) MusicPlayer(1,3);
				else if (this.notebooks == 4) MusicPlayer(1,4);
				else if (this.notebooks == 5) MusicPlayer(1,5);
				else if (this.notebooks == 6) MusicPlayer(1,6);
				else if (this.notebooks == 7) MusicPlayer(1,7);
				else if (this.notebooks == 8) MusicPlayer(1,8);
				else if (this.notebooks == 9) MusicPlayer(1,9);
				else if (this.notebooks == 10) MusicPlayer(1,10);
				else if (this.notebooks == 11) MusicPlayer(1,11);
				else if (this.notebooks == 12) MusicPlayer(1,12);
			}
			else if (this.notebooks >= this.daFinalBookCount)
			{
				MusicPlayer(1,13);
			}
		}
		else if (this.spoopMode && this.notebooks == 1)
		{
			MusicPlayer(2,2);
		}

		if (this.notebooks == 1 && reward) // If this is the players first notebook and they didn't get any questions wrong, reward them with a quarter
		{
			this.quarter.SetActive(true);
			this.tutorBaldi.PlayOneShot(this.aud_Prize);
		}
		else if (this.notebooks == daFinalBookCount & this.mode == "story") // Plays the all 7 notebook sound
		{
			this.spoopLearn.Stop();
			if(!this.isSafeMode) this.audioDevice.PlayOneShot(this.aud_AllNotebooks, 0.8f);
			this.escapeMusic.Play();
		}
		else if (this.notebooks >= this.daFinalBookCount && this.mode == "endless")
		{
			this.spoopLearn.Stop();
			if (PlayerPrefs.GetInt("AdditionalMusic") == 1) this.endlessMusic.Play();
		}
	}

	private void IncreaseItemSelection()
	{
		this.itemSelected++;
		if (this.itemSelected > 4)
		{
			this.itemSelected = 0;
		}
		this.itemSelect.anchoredPosition = new Vector3((float)this.itemSelectOffset[this.itemSelected], 128f, 0f); //Moves the item selector background(the red rectangle)
		this.UpdateItemName();
	}

	private void DecreaseItemSelection()
	{
		this.itemSelected--;
		if (this.itemSelected < 0)
		{
			this.itemSelected = 4;
		}
		this.itemSelect.anchoredPosition = new Vector3((float)this.itemSelectOffset[this.itemSelected], 128f, 0f); //Moves the item selector background(the red rectangle)
		this.UpdateItemName();
	}

	private void UpdateItemSelection()
	{
		this.itemSelect.anchoredPosition = new Vector3((float)this.itemSelectOffset[this.itemSelected], 128f, 0f); //Moves the item selector background(the red rectangle)
		this.UpdateItemName();
	}

	public void CollectItem(int item_ID)
	{
		if (this.item[0] == 0)
		{
			this.item[0] = item_ID; //Set the item slot to the Item_ID provided
			this.itemSlot[0].texture = this.itemTextures[item_ID]; //Set the item slot's texture to a texture in a list of textures based on the Item_ID
		}
		else if (this.item[1] == 0)
		{
			this.item[1] = item_ID; //Set the item slot to the Item_ID provided
            this.itemSlot[1].texture = this.itemTextures[item_ID]; //Set the item slot's texture to a texture in a list of textures based on the Item_ID
        }
		else if (this.item[2] == 0)
		{
			this.item[2] = item_ID; //Set the item slot to the Item_ID provided
            this.itemSlot[2].texture = this.itemTextures[item_ID]; //Set the item slot's texture to a texture in a list of textures based on the Item_ID
        }
		else if (this.item[3] == 0)
		{
			this.item[3] = item_ID;
            this.itemSlot[3].texture = this.itemTextures[item_ID];
        }
		else if (this.item[4] == 0)
		{
			this.item[4] = item_ID;
            this.itemSlot[4].texture = this.itemTextures[item_ID];
        }
		else //This one overwrites the currently selected slot when your inventory is full
		{
			this.item[this.itemSelected] = item_ID;
			this.itemSlot[this.itemSelected].texture = this.itemTextures[item_ID];
		}
		this.UpdateItemName();
	}

	private void UseItem()
	{
		if (this.item[this.itemSelected] != 0)
		{
			if (this.item[this.itemSelected] == 1)
			{
				this.player.stamina = this.player.maxStamina * 2f;
				this.ResetItem();
			}
			else if (this.item[this.itemSelected] == 2)
			{
				Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
				{
					raycastHit.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
					this.ResetItem();
				}
			}
			else if (this.item[this.itemSelected] == 3)
			{
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
			}
			else if (this.item[this.itemSelected] == 4)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.bsodaSpray, this.playerTransform.position, this.cameraTransform.rotation);
				this.ResetItem();
				this.player.ResetGuilt("drink", 1f);
				this.audioDevice.PlayOneShot(this.aud_Soda);
			}
			else if (this.item[this.itemSelected] == 5)
			{
				Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit3;
				if (Physics.Raycast(ray3, out raycastHit3))
				{
					if (raycastHit3.collider.name == "BSODAMachine" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						this.ResetItem();
						this.CollectItem(4);
					}
					else if (raycastHit3.collider.name == "ZestyMachine" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						this.ResetItem();
						this.CollectItem(1);
					}
					else if (raycastHit3.collider.name == "PayPhone" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						raycastHit3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
						this.ResetItem();
					}
				}
			}
			else if (this.item[this.itemSelected] == 6)
			{
				Ray ray4 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit4;
				if (Physics.Raycast(ray4, out raycastHit4) && (raycastHit4.collider.name == "TapePlayer" & Vector3.Distance(this.playerTransform.position, raycastHit4.transform.position) <= 10f))
				{
					raycastHit4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
					this.ResetItem();
				}
			}
			else if (this.item[this.itemSelected] == 7)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.alarmClock, this.playerTransform.position, this.cameraTransform.rotation);
				gameObject.GetComponent<AlarmClockScript>().baldi = this.baldiScrpt;
				this.ResetItem();
			}
			else if (this.item[this.itemSelected] == 8)
			{
				Ray ray5 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit5;
				if (Physics.Raycast(ray5, out raycastHit5) && (raycastHit5.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 10f))
				{
					raycastHit5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
					this.ResetItem();
					this.audioDevice.PlayOneShot(this.aud_Spray);
				}
			}
			else if (this.item[this.itemSelected] == 9)
			{
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
			}
			else if (this.item[this.itemSelected] == 10)
			{
				this.player.ActivateBoots();
				base.StartCoroutine(this.BootAnimation());
				this.ResetItem();
			}
			else if (this.item[this.itemSelected] == 11)
			{
				this.player.ActivateSpeedShoes();
				this.ResetItem();
			}
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
		this.UpdateItemName();
	}

	public void LoseItem(int id)
	{
		this.item[id] = 0;
		this.itemSlot[id].texture = this.itemTextures[0];
		this.UpdateItemName();
	}

	private void UpdateItemName()
	{
		this.itemText.text = this.itemNames[this.item[this.itemSelected]];
	}

	public void ExitReached()
	{
		this.exitsReached++;
		AngrySchoolColors(this.exitsReached);

		/*
		if (this.exitsReached == 1)
		{
			//RenderSettings.ambientLight = Color.red; //Make everything red and start player the weird sound
			//RenderSettings.fog = true;
			
			//this.audioDevice.clip = this.aud_MachineQuiet;
			//this.audioDevice.loop = true;
			//this.audioDevice.Play();
		}
		*/

		if (this.exitsReached != 2) this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);

		if (this.exitsReached == 2) //Play a sound
		{
			this.audioDevice.volume = 0.8f;
			this.audioDevice.clip = this.chaosEarly;
			this.audioDevice.loop = false;
			this.audioDevice.Play();
			this.escapeMusic.volume = 0.32f;
		}
		else if (this.exitsReached == 3) //Play a louder sound
		{
			this.audioDevice.volume = 0.8f;
			this.audioDevice.clip = this.chaosBuildup;
			this.audioDevice.loop = false;
			this.audioDevice.Play();
		}

		/*
		if (this.exitsReached == 12) //Play a sound
		{
			this.audioDevice.volume = 0.8f;
			this.audioDevice.clip = this.aud_MachineStart;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
		}
		if (this.exitsReached == 13) //Play a even louder sound
		{
			this.audioDevice.clip = this.aud_MachineRev;
			this.audioDevice.loop = false;
			this.audioDevice.Play();
		}
		*/
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

	// 0 = Stop All, 1 = school, 2 = learn, 3 = uninterrupted school
	public void MusicPlayer(int songType, int SongId)
	{
		if (songType == 0) // stop all
		{
			this.schoolMusic.Stop();
			this.learnMusic.Stop();
			this.spoopLearn.Stop();
			this.endlessMusic.Stop();
			this.notebook2.Stop();
			this.notebook3.Stop();
			this.notebook4.Stop();
			this.notebook5.Stop();
			this.notebook6.Stop();
			this.notebook7.Stop();
			this.notebook8.Stop();
			this.notebook9.Stop();
			this.notebook10.Stop();
			this.notebook11.Stop();
			this.notebook12.Stop();
		}
		else if (songType == 1 && !this.finaleMode) // schoolhouse
		{
			if (SongId == 1) this.schoolMusic.Play();
			else if (SongId >= 2 && PlayerPrefs.GetInt("AdditionalMusic") == 1)
			{
				if (SongId == 2) this.notebook2.Play();
				else if (SongId == 3) this.notebook3.Play();
				else if (SongId == 4) this.notebook4.Play();
				else if (SongId == 5) this.notebook5.Play();
				else if (SongId == 6) this.notebook6.Play();
				else if (SongId == 7) this.notebook7.Play();
				else if (SongId == 8) this.notebook8.Play();
				else if (SongId == 9) this.notebook9.Play();
				else if (SongId == 10) this.notebook10.Play();
				else if (SongId == 11) this.notebook11.Play();
				else if (SongId == 12) this.notebook12.Play();
			}
		}
		else if (songType == 2 && !(this.notebooks >= this.daFinalBookCount)) // math game
		{
			if (SongId == 1) this.learnMusic.Play();
			else if (SongId == 2 && !this.disableSongInterruption
					&& PlayerPrefs.GetInt("AdditionalMusic") == 1 && !(this.notebooks >= this.daFinalBookCount))
				this.spoopLearn.Play();
		}
		else if (songType == 3)
			if (!this.notebook2.isPlaying && !this.notebook3.isPlaying) StartCoroutine(UninterruptedMusic());
			
	}

	private IEnumerator UninterruptedMusic()
	{
		this.notebook2.Play();

		while (this.notebook2.isPlaying) yield return null;

		this.notebook3.Play();
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

	private void DebugStuff()
	{
		if (debugScreen.isDebugMenuActive == true)
		{
			PauseGame();
		}
		else if (debugScreen.isDebugMenuActive == false)
		{
			UnpauseGame();
		}
	}


	[Header("Game State")]
	[SerializeField] private string curMap;
	public string mode;
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


	[Header("UI")]
	public TMP_Text notebookCount;
	public GameObject pauseMenu;
	public GameObject highScoreText;
	public GameObject warning;
	public TMP_Text staminaPercentText;
	public GameObject reticle;
	public RectTransform itemSelect;
	private int[] itemSelectOffset;
	[SerializeField] private GameObject pointer;


	[Header("Noteboos")]
	public int notebooks;

	public int daFinalBookCount;
	public GameObject[] notebookPickups;
	public int failedNotebooks;


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


	[Header("Items")]
	public int[] item = new int[5];
	public RawImage[] itemSlot = new RawImage[5];
	public UnityEngine.Object[] items = new UnityEngine.Object[10];
	public Texture[] itemTextures = new Texture[10];
	public int itemSelected;
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
		"Speedy Sneakers"
	};
	public GameObject quarter;
	public GameObject bsodaSpray;
	public RectTransform boots;
	public GameObject alarmClock;


	[Header("Detention")]
	public bool isPrinceyTriggerShared;
	public bool isPrinceyIgnore;
	public Vector3 detentionPlayerPos;
	public Vector3 detentionPrincipalPos;
	
	
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
	public AudioClip aud_MachineQuiet;
	public AudioClip aud_MachineStart;
	public AudioClip aud_MachineRev;
	public AudioClip aud_MachineLoop;
	public AudioClip aud_Switch;
	public AudioClip[] baldiJumpscareSounds;
	public AudioClip chaosEarly;
	public AudioClip chaosEarlyLoop;
	public AudioClip chaosBuildup;
	public AudioClip chaosFinalLoop;


	[Header("Music")]
	[SerializeField] private bool disableSongInterruption;
	public AudioSource schoolMusic;
	public AudioSource escapeMusic;
	public AudioSource endlessMusic;
	public AudioSource learnMusic;
	public AudioSource spoopLearn;
	public AudioSource notebook2;
	public AudioSource notebook3;
	public AudioSource notebook4;
	public AudioSource notebook5;
	public AudioSource notebook6;
	public AudioSource notebook7;
	public AudioSource notebook8;
	public AudioSource notebook9;
	public AudioSource notebook10;
	public AudioSource notebook11;
	public AudioSource notebook12;
	public AudioClip LearnQ1;
	public AudioClip LearnQ2;
	public AudioClip LearnQ3;


	[Header("Scripts")]
	public CursorControllerScript cursorController;
	public PlayerScript player;
	public EntranceScript entrance_0;
	public EntranceScript entrance_1;
	public EntranceScript entrance_2;
	public EntranceScript entrance_3;
	public BaldiScript baldiScrpt;
	public PlaytimeScript playtimeScript;
	public FirstPrizeScript firstPrizeScript;
	[SerializeField] private DebugMenuActions debugActions;
	[SerializeField] private DebugScreenSwitch debugScreen;
	[SerializeField] private AudioManager audioManager;
	[SerializeField] private SpeedrunTimer speedrunTimer;
	[SerializeField] private MathMusicScript mathMusicScript;
	[SerializeField] private DebugSceneLoader sceneLoader;
	[SerializeField] private HandIconScript handIconScript;
}