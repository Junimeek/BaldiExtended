using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengeControllerScript : MonoBehaviour
{
    private void Start()
    {
        this.curMode = PlayerPrefs.GetString("challengetype");
        this.audioDevice = base.GetComponent<AudioSource>();

        this.cullingMask = this.camera.cullingMask; // Changes cullingMask in the Camera

        switch(this.curMode)
        {
            case "longmath":
                this.InitializeMathChallenge();
                break;
        }
    }

    private void InitializeMathChallenge()
    {

    }

    public void ActivateLearningGame()
	{
		//this.camera.cullingMask = 0; //Sets the cullingMask to nothing
		this.learningActive = true;
		this.UnlockMouse(); //Unlock the mouse
		this.tutorBaldi.Stop(); //Make tutor Baldi stop talking
		if (!this.spoopMode) //If the player hasn't gotten a question wrong
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
			if (notebooks >= 2 && notebooks < this.daFinalBookCount && this.disableSongInterruption)
			{
				MusicPlayer(3,0);
				return;
			}

			if (audioManager != null) audioManager.SetVolume(0);
			
            if (this.notebooks >= this.daFinalBookCount)
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
		else if (this.notebooks >= daFinalBookCount) // Plays the all 7 notebook sound
		{
			//this.spoopLearn.Stop();
			this.audioDevice.PlayOneShot(this.aud_AllNotebooks, 0.8f);
			this.escapeMusic.Play();
		}
	}

    public void ActivateSpoopMode()
    {
        ModifyExits("lower");

        switch(this.curMode)
        {
            case "longmath":
                this.baldiTutor.SetActive(false);
                this.baldi.SetActive(true);
                break;
        }
    }

    private void ActivateFinaleMode()
    {
        this.finaleMode = true;
		ModifyExits("raise");
    }

    public void CollectNotebook()
	{
		this.notebooks++;
		this.UpdateNotebookCount();
	}

    private void UpdateNotebookCount()
	{
		this.notebookCount.text = this.notebooks.ToString() + "/" + daFinalBookCount.ToString() + " Notebooks";

		if (this.notebooks == daFinalBookCount)
		{
			this.exitCountGroup.SetActive(true);
			this.notebookCount.text = string.Empty;
			this.ActivateFinaleMode();
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

    // 0 = Stop All, 1 = school, 2 = learn, 3 = uninterrupted school
	public void MusicPlayer(int songType, int SongId)
	{
		if (songType == 0) // stop all
		{
			this.schoolMusic.Stop();
			this.learnMusic.Stop();
		}
		else if (songType == 2) // math game
		{
			if (SongId == 1) this.learnMusic.Play();
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

    [Header("Game Config")]
    public int daFinalBookCount;
    public int totalSlotCount;
    public EntranceScript[] entranceList;
    [SerializeField] private bool disableSongInterruption;


    [Header("Game State")]
    [SerializeField] private string curMap;
    [SerializeField] private string curMode;
    public bool spoopMode;
    public bool finaleMode;
    public int exitsReached;
    public bool mouseLocked;
    private bool gamePaused;
    public bool learningActive;
	public float gameOverDelay;
    public bool isDifficultMath;

    
    [Header("UI")]
    [SerializeField] private TMP_Text notebookCount;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private RectTransform itemSelect;
	[SerializeField] private GameObject exitCountGroup;
	[SerializeField] private TMP_Text exitCountText;


    [Header("Noteboos")]
    public int notebooks;

    
    [Header("Player")]
    public Transform playerTransform;
    public Camera camera;
    private int cullingMask;


    [Header("Characters")]
    [SerializeField] private GameObject baldiTutor;
    public GameObject baldi;


    [Header("Item Slots")]
    [SerializeField] private RectTransform itemBG;
	[SerializeField] private float[] slotOffsetArray;
	[SerializeField] private RawImage slotForeground;
	[SerializeField] private Texture[] slotForegroundList;
	public int itemSelected;
	[SerializeField] private GameObject walletSlot;
	[SerializeField] private GameObject walletOverlay;
	[SerializeField] private RectTransform trashOverlay;
	[SerializeField] private bool IsNoItems()
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
    [SerializeField] private GameObject quarter;


    [Header("SFX and Voices")]
    private AudioSource audioDevice;
    [SerializeField] private AudioSource tutorBaldi;
    [SerializeField] private AudioClip aud_Prize;
    [SerializeField] private AudioClip aud_AllNotebooks;


    [Header("Music")]
    [SerializeField] private AudioSource schoolMusic;
    [SerializeField] private AudioSource learnMusic;
    [SerializeField] private AudioSource notebook2;
    [SerializeField] private AudioSource notebook3;
    [SerializeField] private AudioSource escapeMusic;


    [Header("Scripts")]
    public CursorControllerScript cursorController;
    public PlayerScript player;
    public BaldiScript baldiScript;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private MathMusicScript mathMusicScript;
    [SerializeField] private HandIconScript handIconScript;
}
