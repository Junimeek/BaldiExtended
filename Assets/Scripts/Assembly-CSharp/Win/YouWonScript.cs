using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class YouWonScript : MonoBehaviour
{
	void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		this.canvasRenderer.SetActive(false);
		this.skipCanvas.SetActive(true);
		this.isDanceFinished = true;

		this.stats = FindObjectOfType<StatisticsController>();

		if (this.stats != null)
			this.SetStatistics();
		else
			this.SetPlaceholderStatistics();
		
		StartCoroutine(this.BeginBaldiDance());
	}

	void SetPlaceholderStatistics()
	{
		this.lastTime = 690420;
		this.items = new int[]
		{
			5,4,9,9,1,11,1,5,5,5,5,1,1,1,14,
			2,2,2,2,2,2,2,2,2,2,2,7,0,0,0,4,0,4
		};
		this.detentions = 2;
	}

	void SetStatistics()
	{
		this.lastTime = stats.finalMS;

		if (this.lastTime < stats.prevBestTime)
			this.isNewBest = true;

		this.items = stats.itemUsageOrder;
		this.detentions = stats.detentions;

		Destroy(this.stats.gameObject);
	}

	IEnumerator BeginBaldiDance()
	{
		this.cover.SetActive(true);
		
		float delay = 0.5f;
		while (delay > 0f)
		{
			delay -= Time.unscaledDeltaTime;
			yield return null;
		}

		this.cover.SetActive(false);
		this.musicDevice.loop = false;
		this.musicDevice.Play();
		this.metronome.StartMetronome();

		while(this.metronome.curBeat < this.bpmTargets[0])
			yield return null;
		this.isDanceFinished = false;
		this.baldiDevice.PlayOneShot(this.baldiClips[0]);

		while (this.metronome.curBeat < this.bpmTargets[1])
			yield return null;
		this.baldiDevice.PlayOneShot(this.baldiClips[1]);

		while (this.metronome.curBeat < this.bpmTargets[2])
			yield return null;
		this.baldiDevice.PlayOneShot(this.baldiClips[2]);

		while (this.metronome.curBeat < this.bpmTargets[3])
			yield return null;
		this.baldiDevice.PlayOneShot(this.baldiClips[3]);

		while (this.metronome.curBeat < this.bpmTargets[4])
			yield return null;
		this.baldiDevice.PlayOneShot(this.baldiClips[4]);

		while (this.metronome.curBeat < this.bpmTargets[5])
			yield return null;
		this.baldiDevice.PlayOneShot(this.baldiClips[5]);

		while (this.metronome.curBeat < this.bpmTargets[6])
			yield return null;
		this.baldiDevice.PlayOneShot(this.baldiClips[6]);

		while (this.metronome.curBeat < this.bpmTargets[7])
			yield return null;
		this.baldiDevice.PlayOneShot(this.baldiClips[7]);

		while (this.metronome.curBeat < this.bpmTargets[7])
			yield return null;
		this.isDanceFinished = true;

		while (this.musicDevice.isPlaying)
			yield return null;

		this.metronome.StopMetronome();
		this.EnableCanvas(false);
	}

	void Update()
	{
		if (!this.isDanceFinished)
		{
			this.animationTimer -= Time.unscaledDeltaTime;

			if (this.animationTimer <= 0f)
			{
				this.animationTimer += 0.51282f;
				this.ChangeAnimState();
			}
		}
		else
		{
			this.animationTimer = 0.512f;
			this.animator.Play("Baldi_Dance");
		}
	}

	void ChangeAnimState()
	{
		if (this.curState == 1)
		{
			this.curState = 0;
			this.animator.Play("Baldi_Dance");
		}
		else
		{
			this.curState = 1;
			this.animator.Play("Baldi_Dance2");
		}
	}

	public void EnableCanvas(bool forceDanceEnd)
	{
		this.isDanceFinished = true;
		this.cameraObject.position = new Vector3(20f, 5f, 25f);
		this.cameraObject.eulerAngles = new Vector3(0f, 90f, 0f);

		Destroy(this.balloons);

		if (forceDanceEnd)
		{
			this.baldiDevice.clip = null;
			this.baldiDevice.volume = 0f;
			this.metronome.StopMetronome();
			this.StopCoroutine(this.BeginBaldiDance());
		}

		if (!this.musicDevice.isPlaying)
			this.musicDevice.Play();
		
		this.musicDevice.loop = true;
		this.canvasRenderer.SetActive(true);
		this.skipCanvas.SetActive(false);

		this.wowDevice.clip = this.GetRandomPraise();
		this.wowDevice.Play();
	}

	AudioClip GetRandomPraise()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, this.praises.Length - 1));
		return praises[num];
	}

	public void ChangeScreen(int newScreen)
	{
		switch(newScreen)
		{
			case 1:
				this.mainCanvas.SetActive(false);
				this.statsCanvas.SetActive(true);
				this.GetStatistics();
				break;
			default:
				this.mainCanvas.SetActive(true);
				this.statsCanvas.SetActive(false);
				break;
		}
	}

	void GetStatistics()
	{
		if (this.isFetched)
			return;
		else
			this.isFetched = true;

		this.timeText.text = this.FinalTime();

		if (this.isNewBest)
			this.newBestText.SetActive(true);

		this.detentionsText.text = this.detentions.ToString();

		int lastItemID = 0;
		int actualItemNumber = -1;
		int itemsInARow = 1;
		WinItemInstance currentItemInstance = null;

		for (int i = 0; i < this.items.Length; i++)
		{
			if (lastItemID == this.items[i])
			{
				itemsInARow++;
				continue;
			}
			if (this.items[i] == 0)
				continue;
			if (currentItemInstance != null)
				currentItemInstance.SetItemCount(itemsInARow);
			itemsInARow = 1;
			currentItemInstance = Instantiate(this.winItemObject, Vector2.zero, Quaternion.identity, this.itemParent).GetComponent<WinItemInstance>();
			currentItemInstance.SetItemImageSprite(this.itemSprites[this.items[i]]);
			Image image = currentItemInstance.GetItemImage();
			actualItemNumber++;
			image.rectTransform.anchoredPosition = this.GetItemPosition(actualItemNumber);
			lastItemID = this.items[i];
		}
		if (currentItemInstance != null)
			currentItemInstance.SetItemCount(itemsInARow);

		switch (PlayerPrefs.GetString("CurrentMap"))
		{
			case "Classic":
				this.mapIcon.sprite = this.mapSprites[1];
				break;
			case "ClassicExtended":
				this.mapIcon.sprite = this.mapSprites[2];
				break;
			case "JuniperHills":
				this.mapIcon.sprite = this.mapSprites[3];
				break;
			case "ClassicDark":
				this.mapIcon.sprite = this.mapSprites[4];
				break;
			default:
				this.mapIcon.sprite = this.mapSprites[0];
				break;
		}
	}

	Vector2 GetItemPosition(int itemNumber)
	{
		int loops = Mathf.FloorToInt(itemNumber / 6f);
		return new Vector2((itemNumber * 50f) - (loops * 300f), loops * -50f);
	}

	public void WinReturn()
	{
		SceneManager.LoadSceneAsync("MainMenu");
	}

	string FinalTime()
	{
		float timeInSeconds = this.lastTime / 1000f;
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        timeInSeconds -= minutes * 60f;

        return minutes.ToString() + ":" + timeInSeconds.ToString("00.000");
	}

	[SerializeField] StatisticsController stats;
	[SerializeField] TMP_Text timeText;
	[SerializeField] GameObject newBestText;
	[SerializeField] TMP_Text detentionsText;
	[SerializeField] bool isFetched;
	[SerializeField] Transform itemParent;

	[Header("Baldi Dance")]
	[SerializeField] MetronomeScript metronome;
	[SerializeField] Animator animator;
	[SerializeField] int curState;
	[SerializeField] Transform cameraObject;
	[SerializeField] GameObject canvasRenderer;
	[SerializeField] GameObject skipCanvas;
	[SerializeField] GameObject cover;
	[SerializeField] bool isDanceFinished;
	[SerializeField] GameObject balloons;

	[Header("Audio")]
	[SerializeField] AudioSource musicDevice;
	[SerializeField] AudioSource baldiDevice;
	[SerializeField] AudioClip[] baldiClips;
	[SerializeField] AudioClip[] praises;
	[SerializeField] AudioSource wowDevice;
	[SerializeField] int[] bpmTargets;
	[SerializeField] float animationTimer;

	[Header("Statistics")]
	[SerializeField] bool isNewBest;
	[SerializeField] int lastTime;
	[SerializeField] int[] items;
	[SerializeField] int detentions;

	[Header("Screens")]
	[SerializeField] GameObject mainCanvas;
	[SerializeField] GameObject statsCanvas;

	[Header("Sprites")]
	[SerializeField] Sprite[] itemSprites;
	[SerializeField] Sprite[] mapSprites;
	[SerializeField] GameObject winItemObject;

	[Header("Icons")]
	[SerializeField] Image mapIcon;
}
