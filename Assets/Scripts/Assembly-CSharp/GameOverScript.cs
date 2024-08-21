using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScript : MonoBehaviour
{
	private void Start()
	{
		resetText.text = string.Empty;
		this.image = base.GetComponent<Image>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.delay = 2f;
		this.chance = UnityEngine.Random.Range(1f, 99f);
		if (this.chance < 98f)
		{
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
			this.image.sprite = this.images[num];
			
			if (PlayerPrefs.GetInt("InstantReset") == 1)
			{
				StartCoroutine(LoadSceneRoutine(PlayerPrefs.GetString("CurrentMap")));
			}
		}
		else
		{
			this.delay = 5f;
			this.image.sprite = this.rare;
		}
	}

	private void Update()
	{
		this.delay -= 1f * Time.deltaTime;

		if (this.delay <= 0f)
		{
			if (this.chance < 98f)
			{
				if (PlayerPrefs.GetInt("InstantReset") != 1)
				{
					sceneLoader.LoadTheScene("MainMenu", 1);
				}
			}
			else
			{
				this.image.transform.localScale = new Vector3(5f, 5f, 1f);
				this.image.color = Color.red;
				if (!this.audioDevice.isPlaying)
				{
					this.audioDevice.Play();
					resetText.text = string.Empty;
				}
				if (this.delay <= -5f)
				{
					Application.Quit();
				}
			}
		}
		if (this.delay <= 2 && this.delay > 0 && this.chance >= 98f)
		{
			resetText.text = "oh no";
		}
	}

	private IEnumerator LoadSceneRoutine(string lescene)
	{
		resetText.text = "Reloading the level...";
		AsyncOperation op = SceneManager.LoadSceneAsync(lescene);

		while (!op.isDone)
		{
			yield return null;
		}

		sceneLoader.LoadTheScene(lescene, 1);
		StopCoroutine(LoadSceneRoutine(lescene));
	}

	private Image image;
	private float delay;
	public Sprite[] images = new Sprite[5];
	public Sprite rare;
	public TMP_Text resetText;
	private float chance;
	private AudioSource audioDevice;
	[SerializeField] private DebugSceneLoader sceneLoader;
}
