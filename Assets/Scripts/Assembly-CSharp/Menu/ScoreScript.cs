﻿using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
	private void Start()
	{
		if (PlayerPrefs.GetString("CurrentMode") == "endless")
		{
			this.scoreText.SetActive(true);
			this.text.text = "Score:\n" + PlayerPrefs.GetInt("CurrentBooks") + " Notebooks";
		}
	}

	public GameObject scoreText;
	public TMP_Text text;
}
