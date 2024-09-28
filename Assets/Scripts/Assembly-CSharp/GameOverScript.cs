using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScript : MonoBehaviour
{
	private void Start()
	{
		this.resetText.text = string.Empty;
		this.image = base.GetComponent<Image>();
		this.audioDevice = base.GetComponent<AudioSource>();

		switch(this.forceSecretItem)
		{
			case 0:
				break;
			case 1:
				StartCoroutine(this.WaitFor99());
				return;
			case 2:
				this.image.color = Color.black;
				this.videoPlayer.SetActive(true);
				return;
			case 3:
				this.image.color = Color.black;
				StartCoroutine(this.WaitForFakeError());
				return;
		}

		float chance = Random.Range(1f, 99f);
		
		if (chance < 98f)
		{
			string curMap = PlayerPrefs.GetString("CurrentMap");
			int num = Mathf.RoundToInt(Random.Range(0f, 4f));
			this.image.sprite = this.images[num];
			
			if (PlayerPrefs.GetInt("InstantReset") == 1 && curMap != "ClassicDark")
				StartCoroutine(this.LoadSceneRoutine(curMap));
			else if (curMap == "ClassicDark")
			{
				this.image.color = new Color(0f, 0f, 0f, 0f);
				StartCoroutine(this.WaitForMenuLoad(0.75f));
			}
			else
				StartCoroutine(this.WaitForMenuLoad(2f));
		}
		else
		{
			int secretChance = Mathf.FloorToInt(Random.Range(0f, 2.9f));
			switch(secretChance)
			{
				case 0:
					StartCoroutine(this.WaitFor99());
					break;
				case 1:
					this.image.color = Color.black;
					this.videoPlayer.SetActive(true);
					break;
				case 2:
					this.image.color = Color.black;
					StartCoroutine(this.WaitForFakeError());
					break;
			}
		}
	}

	private IEnumerator WaitForMenuLoad(float setDelay)
	{
		this.delay = setDelay;

		while (this.delay > 0f)
		{
			this.delay -= Time.unscaledDeltaTime;
			yield return null;
		}

		this.sceneLoader.LoadTheScene("MainMenu", 1);
	}

	private IEnumerator WaitFor99()
	{
		this.image.sprite = this.rare;
		this.delay = 5f;

		while (this.delay > 2f)
		{
			this.delay -= Time.unscaledDeltaTime;
			yield return null;
		}

		this.resetText.text = "oh no";

		while (this.delay > 0f)
		{
			this.delay -= Time.unscaledDeltaTime;
			yield return null;
		}

		this.image.transform.localScale = new Vector3(5f, 5f, 1f);
		this.image.color = Color.red;
		this.audioDevice.Play();
		this.resetText.text = string.Empty;

		while (this.delay > -5f)
		{
			this.delay -= Time.unscaledDeltaTime;
			yield return null;
		}

		Debug.Log("Application Quit");
		Application.Quit();
	}

	private IEnumerator WaitForFakeError()
	{
		float delay = 1f;

		while (delay > 0f)
		{
			delay -= Time.unscaledDeltaTime;
			yield return null;
		}

		this.audioDevice.clip = this.aud_fakeError;
		this.fakeError.SetActive(true);
		this.audioDevice.Play();

		this.fakeErrorPercent.text = "0% Done";
		delay = 1f;
		int curPercent = 0;

		while (curPercent < 100)
		{
			delay -= Time.unscaledDeltaTime;

			if (delay < 0f)
			{
				delay += Random.Range(0.2f, 0.5f);
				curPercent = this.NewPercentage(curPercent);
				this.fakeErrorPercent.text = curPercent + "% Done";
			}

			yield return null;
		}

		this.fakeErrorPercent.text = "100% Done";
		delay = 1f;

		while (delay > 0f)
		{
			delay -= Time.unscaledDeltaTime;
			yield return null;
		}
		
		SceneManager.LoadSceneAsync("Launcher");
	}

	private int NewPercentage(int oldPercentage)
	{
		return oldPercentage += Mathf.FloorToInt(Random.Range(2f, 7f));
	}

	private IEnumerator LoadSceneRoutine(string lescene)
	{
		this.resetText.text = "Reloading the level...";
		AsyncOperation op = SceneManager.LoadSceneAsync(lescene);

		while (!op.isDone)
			yield return null;

		this.sceneLoader.LoadTheScene(lescene, 1);
	}

	[SerializeField] private int forceSecretItem;
	private Image image;
	private float delay;
	public Sprite[] images = new Sprite[5];
	public Sprite rare;
	public TMP_Text resetText;
	private AudioSource audioDevice;
	[SerializeField] private AudioClip aud_fakeError;
	[SerializeField] private DebugSceneLoader sceneLoader;
	[SerializeField] private GameObject videoPlayer;
	[SerializeField] private GameObject fakeError;
	[SerializeField] private TMP_Text fakeErrorPercent;
}
