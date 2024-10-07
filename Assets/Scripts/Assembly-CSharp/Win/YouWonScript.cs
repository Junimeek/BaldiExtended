using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class YouWonScript : MonoBehaviour
{
	private void Start()
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

	private void SetPlaceholderStatistics()
	{
		this.lastTime = 99.39f;
		this.items = new int[]
		{
			5, 4, 9, 1, 11, 1, 1
		};
		this.detentions = 2;
	}

	private void SetStatistics()
	{
		this.lastTime = this.stats.finalSeconds;

		if (this.lastTime < this.stats.data_bestTime[this.GetMapID()])
		{
			this.stats.SaveAllData("time");
			this.isNewBest = true;
		}
		else
			this.stats.SaveAllData(null);

		this.items = this.stats.itemsUsed;
		this.detentions = this.stats.detentions;

		Destroy(this.stats.gameObject);
	}

	private int GetMapID()
	{
		switch(PlayerPrefs.GetString("CurrentMap"))
		{
			case "Classic":
				return 0;
			case "ClassicExtended":
				return 1;
			case "JuniperHills":
				return 2;
			case "ClassicDark":
				return 0;
			default:
				return 0;
		}
	}

	private IEnumerator BeginBaldiDance()
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

	private void Update()
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

	private void ChangeAnimState()
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

		this.wowDevice.Play();
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

	private void GetStatistics()
	{
		if (this.isFetched)
			return;
		else
			this.isFetched = true;

		this.timeText.text = this.FinalTime();

		if (this.isNewBest)
			this.newBestText.SetActive(true);

		this.detentionsText.text = this.detentions.ToString();

		int loops = -1;
		for (int i = 0; i < this.items.Length; i++)
		{
			GameObject gameObject = Instantiate(new GameObject(this.items[i].ToString()), new Vector2(0f, 0f), Quaternion.identity, this.itemParent);
			Image image = gameObject.AddComponent<Image>();
			image.rectTransform.localScale = new Vector2(0.5f, 0.5f);
			image.raycastTarget = false;

			if (i % 6 == 0)
				loops++;

			image.rectTransform.anchoredPosition = new Vector2((i * 50f) - (loops * 300f), loops * -50f);

			image.sprite = this.itemSprites[this.items[i]];
		}
		//this.itemParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80, -10 + (loops * 25f));

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

	public void WinReturn()
	{
		SceneManager.LoadSceneAsync("MainMenu");
	}

	private string FinalTime()
	{
		float seconds = this.lastTime % 60f;
		int minutes =  Mathf.FloorToInt(this.lastTime / 60f);
		int hours = Mathf.FloorToInt(minutes / 60f);
		minutes -= hours * 60;
		
		return hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00.000");
	}

	[SerializeField] private StatisticsController stats;
	[SerializeField] private TMP_Text timeText;
	[SerializeField] private GameObject newBestText;
	[SerializeField] private TMP_Text detentionsText;
	[SerializeField] private bool isFetched;
	[SerializeField] private Transform itemParent;

	[Header("Baldi Dance")]
	[SerializeField] private MetronomeScript metronome;
	[SerializeField] private Animator animator;
	[SerializeField] private int curState;
	[SerializeField] private Transform cameraObject;
	[SerializeField] private GameObject canvasRenderer;
	[SerializeField] private GameObject skipCanvas;
	[SerializeField] private GameObject cover;
	[SerializeField] private bool isDanceFinished;

	[Header("Audio")]
	[SerializeField] private AudioSource musicDevice;
	[SerializeField] private AudioSource baldiDevice;
	[SerializeField] private AudioClip[] baldiClips;
	[SerializeField] private AudioSource wowDevice;
	[SerializeField] private int[] bpmTargets;
	[SerializeField] private float animationTimer;

	[Header("Statistics")]
	[SerializeField] private bool isNewBest;
	[SerializeField] private float lastTime;
	[SerializeField] private int[] items;
	[SerializeField] private int detentions;

	[Header("Screens")]
	[SerializeField] private GameObject mainCanvas;
	[SerializeField] private GameObject statsCanvas;

	[Header("Sprites")]
	[SerializeField] private Sprite[] itemSprites;
	[SerializeField] private Sprite[] mapSprites;

	[Header("Icons")]
	[SerializeField] private Image mapIcon;
}
