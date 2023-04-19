using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000C0 RID: 192
public class GameControllerScript : MonoBehaviour
{
	// Token: 0x06000963 RID: 2403 RVA: 0x00021A00 File Offset: 0x0001FE00
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
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00021AC4 File Offset: 0x0001FEC4
	private void Start()
	{
		this.cullingMask = this.camera.cullingMask; // Changes cullingMask in the Camera
		this.audioDevice = base.GetComponent<AudioSource>(); //Get the Audio Source
		this.mode = PlayerPrefs.GetString("CurrentMode"); //Get the current mode
		if (this.mode == "endless") //If it is endless mode
		{
			this.baldiScrpt.endless = true; //Set Baldi use his slightly changed endless anger system
		}
		MusicPlayer(1,1); //Play the school music
		this.LockMouse(); //Prevent the mouse from moving
		this.UpdateNotebookCount(); //Update the notebook count
		this.itemSelected = 0; //Set selection to item slot 0(the first item slot)
		this.gameOverDelay = 0.5f;

		//debugScreen.DebugCloseMenu();
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00021B5C File Offset: 0x0001FF5C
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
			if (!this.gamePaused & Time.timeScale != 1f)
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


		//if (this.finaleMode && !this.audioDevice.isPlaying && this.exitsReached == 3)
		//{
		//	this.audioDevice.clip = this.aud_MachineLoop;
		//	this.audioDevice.loop = true;
		//	this.audioDevice.Play();
		//}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			DebugStuff();
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00021F8C File Offset: 0x0002038C
	private void UpdateNotebookCount()
	{
		if (this.mode == "story")
		{
			this.notebookCount.text = this.notebooks.ToString() + "/" + daFinalBookCount.ToString() + " Notebooks";
		}
		else
		{
			this.notebookCount.text = this.notebooks.ToString() + " Notebooks";
		}
		if (this.notebooks == daFinalBookCount & this.mode == "story")
		{
			this.ActivateFinaleMode();
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00022024 File Offset: 0x00020424
	public void CollectNotebook()
	{
		this.notebooks++;
		this.UpdateNotebookCount();
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0002203A File Offset: 0x0002043A
	public void LockMouse()
	{
		if (!this.learningActive)
		{
			this.cursorController.LockCursor(); //Prevent the cursor from moving
			this.mouseLocked = true;
			this.reticle.SetActive(true);
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00022065 File Offset: 0x00020465
	public void UnlockMouse()
	{
		this.cursorController.UnlockCursor(); //Allow the cursor to move
		this.mouseLocked = false;
		this.reticle.SetActive(false);
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00022085 File Offset: 0x00020485
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

	// Token: 0x0600096B RID: 2411 RVA: 0x000220C5 File Offset: 0x000204C5
	public void ExitGame()
	{
		SceneManager.LoadScene("MainMenu");
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x000220D1 File Offset: 0x000204D1
	public void UnpauseGame()
	{
		Time.timeScale = 1f;
		this.gamePaused = false;
		this.pauseMenu.SetActive(false);
		this.LockMouse();
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x000220F8 File Offset: 0x000204F8
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
		this.audioDevice.PlayOneShot(this.aud_Hang); //Plays the hang sound
		MusicPlayer(0,0);
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x000221BF File Offset: 0x000205BF
	private void ActivateFinaleMode()
	{
		this.finaleMode = true;
		this.entrance_0.Raise(); //Raise all the enterances(make them appear)
		this.entrance_1.Raise();
		this.entrance_2.Raise();
		this.entrance_3.Raise();
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x000221F4 File Offset: 0x000205F4
	public void GetAngry(float value) //Make Baldi get angry
	{
		if (!this.spoopMode)
		{
			this.ActivateSpoopMode();
		}
		this.baldiScrpt.GetAngry(value);
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00022214 File Offset: 0x00020614
	public void ActivateLearningGame()
	{
		//this.camera.cullingMask = 0; //Sets the cullingMask to nothing
		this.learningActive = true;
		this.UnlockMouse(); //Unlock the mouse
		this.tutorBaldi.Stop(); //Make tutor Baldi stop talking
		if (!this.spoopMode) //If the player hasn't gotten a question wrong
		{
			MusicPlayer(0,0); //Start playing the learn music
			MusicPlayer(2,1);
		}
		else if (this.spoopMode)
		{
			if (PlayerPrefs.GetInt("AdditionalMusic") == 1)
			{
				if (notebooks > 2)
				{
					audioManager.SetVolume(1);
					MusicPlayer(0,0);
					MusicPlayer(2,2);
				}
				else if (notebooks == 1)
				{
					audioManager.SetVolume(0);
				}
			}
			else
			{
				audioManager.SetVolume(0);
			}
		}
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x00022278 File Offset: 0x00020678
	public void DeactivateLearningGame(GameObject subject)
	{
		audioManager.SetVolume(0);
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

		if (this.spoopMode && notebooks >= 2) // my music stuff
		{
			MusicPlayer(0,0);
			audioManager.SetVolume(0);
			
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
			else if (this.notebooks > 12) MusicPlayer(1,13);
		}
		else if (this.spoopMode && this.notebooks == 1)
		{
			MusicPlayer(2,2);
		}

		if (this.notebooks == 1 & !this.spoopMode) // If this is the players first notebook and they didn't get any questions wrong, reward them with a quarter
		{
			this.quarter.SetActive(true);
			this.tutorBaldi.PlayOneShot(this.aud_Prize);
		}
		else if (this.notebooks == daFinalBookCount & this.mode == "story") // Plays the all 7 notebook sound
		{
			this.spoopLearn.Stop();
			this.audioDevice.PlayOneShot(this.aud_AllNotebooks, 0.8f);
			
			if (PlayerPrefs.GetInt("AdditionalMusic") == 1) this.escapeMusic.Play();
		}
		else if (this.notebooks > 12 && this.mode == "endless")
		{
			this.spoopLearn.Stop();
			this.endlessMusic.Play();
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00022360 File Offset: 0x00020760
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

	// Token: 0x06000973 RID: 2419 RVA: 0x000223C4 File Offset: 0x000207C4
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

	// Token: 0x06000974 RID: 2420 RVA: 0x00022425 File Offset: 0x00020825
	private void UpdateItemSelection()
	{
		this.itemSelect.anchoredPosition = new Vector3((float)this.itemSelectOffset[this.itemSelected], 128f, 0f); //Moves the item selector background(the red rectangle)
		this.UpdateItemName();
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0002245C File Offset: 0x0002085C
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

	// Token: 0x06000976 RID: 2422 RVA: 0x00022528 File Offset: 0x00020928
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

	// Token: 0x06000977 RID: 2423 RVA: 0x00022B40 File Offset: 0x00020F40
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

	// Token: 0x06000978 RID: 2424 RVA: 0x00022B5B File Offset: 0x00020F5B
	private void ResetItem()
	{
		this.item[this.itemSelected] = 0;
		this.itemSlot[this.itemSelected].texture = this.itemTextures[0];
		this.UpdateItemName();
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00022B8B File Offset: 0x00020F8B
	public void LoseItem(int id)
	{
		this.item[id] = 0;
		this.itemSlot[id].texture = this.itemTextures[0];
		this.UpdateItemName();
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00022BB1 File Offset: 0x00020FB1
	private void UpdateItemName()
	{
		this.itemText.text = this.itemNames[this.item[this.itemSelected]];
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00022BD4 File Offset: 0x00020FD4
	public void ExitReached()
	{
		this.exitsReached++;
		if (this.exitsReached == 1)
		{
			RenderSettings.ambientLight = Color.red; //Make everything red and start player the weird sound
			//RenderSettings.fog = true;
			
			//this.audioDevice.clip = this.aud_MachineQuiet;
			//this.audioDevice.loop = true;
			//this.audioDevice.Play();
		}
		if (this.exitsReached >= 0)
		{
			this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
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

	// Token: 0x0600097C RID: 2428 RVA: 0x00022CC1 File Offset: 0x000210C1
	public void DespawnCrafters()
	{
		this.crafters.SetActive(false); //Make Arts And Crafters Inactive
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00022CD0 File Offset: 0x000210D0
	public void Fliparoo()
	{
		this.player.height = 6f;
		this.player.fliparoo = 180f;
		this.player.flipaturn = -1f;
		Camera.main.GetComponent<CameraScript>().offset = new Vector3(0f, -1f, 0f);
	}

	public void MusicPlayer(int songType, int SongId) // 0 = Stop All, 1 = school, 2 = learn
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
			if (SongId == 1)
			{
				this.schoolMusic.Play();
			}
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
		else if (songType == 2) // math game
		{
			if (SongId == 1)
			{
				this.learnMusic.Play();
			}
			else if (SongId == 2)
			{
				if (PlayerPrefs.GetInt("AdditionalMusic") == 1) this.spoopLearn.Play();
			}
		}
	}

	private void MathMusic()
	{
		this.audioDevice.clip = this.LearnQ1;
		if (!this.audioDevice.isPlaying && this.learningActive)
		{
			if (mathScript.problem == 1)
			this.audioDevice.PlayOneShot(this.LearnQ1);
			if (mathScript.problem == 2)
			this.audioDevice.PlayOneShot(this.LearnQ2);
			if (mathScript.problem == 3)
			this.audioDevice.PlayOneShot(this.LearnQ3);
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

	

	// Token: 0x040005F7 RID: 1527
	

	// Token: 0x040005F8 RID: 1528
	

	// Token: 0x040005F9 RID: 1529
	public Transform playerTransform;

	// Token: 0x040005FA RID: 1530
	public Transform cameraTransform;

	// Token: 0x040005FB RID: 1531
	public Camera camera;

	// Token: 0x040005FC RID: 1532
	private int cullingMask;

	// Token: 0x040005FD RID: 1533
	

	// Token: 0x04000601 RID: 1537
	public GameObject baldiTutor;

	// Token: 0x04000602 RID: 1538
	public GameObject baldi;

	// Token: 0x04000603 RID: 1539
	

	// Token: 0x04000604 RID: 1540
	

	// Token: 0x04000607 RID: 1543
	public GameObject principal;

	// Token: 0x04000608 RID: 1544
	public GameObject crafters;

	// Token: 0x04000609 RID: 1545
	public GameObject playtime;

	// Token: 0x0400060A RID: 1546
	

	// Token: 0x0400060B RID: 1547
	public GameObject gottaSweep;

	// Token: 0x0400060C RID: 1548
	public GameObject bully;

	// Token: 0x0400060D RID: 1549
	public GameObject firstPrize;

	// Token: 0x0400060D RID: 1549
	public GameObject TestEnemy;

	// Token: 0x0400060E RID: 1550
	

	// Token: 0x0400060F RID: 1551
	public GameObject quarter;

	// Token: 0x04000610 RID: 1552
	

	// Token: 0x04000611 RID: 1553
	public RectTransform boots;

	// Token: 0x04000612 RID: 1554
	public string mode;

	// Token: 0x04000613 RID: 1555
	public int notebooks;

	public int daFinalBookCount;

	// Token: 0x04000614 RID: 1556
	public GameObject[] notebookPickups;

	// Token: 0x04000615 RID: 1557
	public int failedNotebooks;

	// Token: 0x04000616 RID: 1558
	public bool spoopMode;

	// Token: 0x04000617 RID: 1559
	public bool finaleMode;

	// Token: 0x04000618 RID: 1560
	public bool debugMode;

	// Token: 0x04000619 RID: 1561
	public bool mouseLocked;

	// Token: 0x0400061A RID: 1562
	public int exitsReached;

	// Token: 0x0400061B RID: 1563
	public int itemSelected;

	// Token: 0x0400061C RID: 1564
	public int[] item = new int[5];

	// Token: 0x0400061D RID: 1565
	public RawImage[] itemSlot = new RawImage[5];

	// Token: 0x0400061E RID: 1566
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

	// Token: 0x0400061F RID: 1567
	public TMP_Text itemText;

	// Token: 0x04000620 RID: 1568
	public UnityEngine.Object[] items = new UnityEngine.Object[10];

	// Token: 0x04000621 RID: 1569
	public Texture[] itemTextures = new Texture[10];

	// Token: 0x04000622 RID: 1570
	public GameObject bsodaSpray;

	// Token: 0x04000623 RID: 1571
	public GameObject alarmClock;

	// Token: 0x04000624 RID: 1572
	public TMP_Text notebookCount;

	// Token: 0x04000625 RID: 1573
	public GameObject pauseMenu;

	// Token: 0x04000626 RID: 1574
	public GameObject highScoreText;

	// Token: 0x04000627 RID: 1575
	public GameObject warning;
	public TMP_Text staminaPercentText;

	// Token: 0x04000628 RID: 1576
	public GameObject reticle;

	// Token: 0x04000629 RID: 1577
	public RectTransform itemSelect;
	private int[] itemSelectOffset;
	private bool gamePaused;
	public bool learningActive;
	public float gameOverDelay;
	//private Player playerInput;
	
	
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


	[Header("Music")]
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
	[SerializeField] private MathGameScript mathScript;
	[SerializeField] private DebugMenuActions debugActions;
	[SerializeField] private DebugScreenSwitch debugScreen;
	[SerializeField] private AudioManager audioManager;
}