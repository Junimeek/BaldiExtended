using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DelayGameStart : MonoBehaviour
{
    private void OnEnable()
    {
        this.StartCoroutine(this.GameLoadDelay());
    }

    private IEnumerator GameLoadDelay()
    {
		float timeRem = 0.6f;

		while (timeRem > 0f)
		{
			timeRem -= Time.unscaledDeltaTime;
			yield return null;
		}

        this.loadingScreen.SetActive(true);
		this.StartCoroutine(this.FadeOverlay());
    }

    private IEnumerator FadeOverlay()
	{
		float fadeTime = 1f;

		while (imageOverlay.color.a > 0f)
		{
			fadeTime -= Time.unscaledDeltaTime * 2f;
			imageOverlay.color = new Color(0f, 0f, 0f, fadeTime);
			yield return null;
		}
	}

    [SerializeField] private RawImage imageOverlay;
    [SerializeField] private GameObject loadingScreen;
}
