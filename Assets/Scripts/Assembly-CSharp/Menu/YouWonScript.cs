using System;
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

		this.stats = FindObjectOfType<StatisticsController>();

		if (this.stats != null)
			this.SetStatistics();
		else
			this.SetPlaceholderStatistics();
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
		this.items = this.stats.itemsUsed;
		this.detentions = this.stats.detentions;

		Destroy(this.stats.gameObject);
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
	[SerializeField] private TMP_Text detentionsText;
	[SerializeField] private bool isFetched;
	[SerializeField] private Transform itemParent;

	[Header("Statistics")]
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
