using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Token: 0x020000D5 RID: 213
public class YouWonScript : MonoBehaviour
{
	// Token: 0x060009E8 RID: 2536 RVA: 0x0002678A File Offset: 0x00024B8A
	private void Start()
	{
		//StartCoroutine(waitUntilDelay());
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		CalculateFinalTime();
	}

	public void WinReturn()
	{
		SceneManager.LoadSceneAsync("MainMenu");
	}

	private void CalculateFinalTime()
	{
		lastTime = PlayerPrefs.GetFloat("LastTime");

		daSeconds = (Mathf.FloorToInt(lastTime));
		daMinutes = (Mathf.FloorToInt(daSeconds/60));
		daMiliseconds = MathF.Round((daSeconds - lastTime), 2);

		bestText.text = daMinutes + ":" + daSeconds + "." + daMiliseconds;
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00026797 File Offset: 0x00024B97
	
	/*
	private void Update()
	{
		this.delay -= Time.deltaTime;
		if (this.delay <= 0f)
		{
			Application.Quit();
		}
	}
	*/

	// Token: 0x0400071A RID: 1818
	private float delay;

	[SerializeField] private TMP_Text bestText;
	[SerializeField] private float lastTime;
	[SerializeField] private int daMinutes;
	[SerializeField] private int daSeconds;
	[SerializeField] private float daMiliseconds;

	/*
	private IEnumerator waitUntilDelay()
	{
		this.delay = 11f;

		while (this.delay > 0f)
		{
			this.delay -= Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene("MainMenu");
		StopCoroutine(waitUntilDelay());
	}
	*/
}
