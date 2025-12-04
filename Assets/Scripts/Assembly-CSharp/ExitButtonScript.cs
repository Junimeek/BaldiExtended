using System.Collections;
using UnityEngine;

public class ExitButtonScript : MonoBehaviour
{
	private void Start()
	{
		exitText.SetActive(false);
	}

	public void ExitGame()
	{
		theNumber++;

		if (theNumber == 1)
        {
            balIntro.Stop();
			quitSound.Play();
			exitText.SetActive(true);
			StartCoroutine(QuitRoutine());
        }
	}

	IEnumerator QuitRoutine()
    {
        while (theNumber == 1 && quitSound.isPlaying)
			yield return null;
		
		Explode();
    }

	void Explode()
    {
        Debug.Log("Quit Game");
		Application.Quit();
    }

	int theNumber;
	[SerializeField] AudioSource quitSound;
	[SerializeField] AudioSource balIntro;
	[SerializeField] GameObject exitText;
}
