using System;
using System.Collections;
using UnityEngine;

public class TapePlayerScript : MonoBehaviour
{

	private void Start()
	{
		this.audioDevice = base.GetComponent<AudioSource>();
		this.sprite.sprite = this.openSprite;
	}

	private void Update()
	{
		if (this.audioDevice.isPlaying & Time.timeScale == 0f)
		{
			this.audioDevice.Pause();
		}
		else if (Time.timeScale > 0f & this.baldi.antiHearingTime > 0f)
		{
			this.audioDevice.UnPause();
		}
	}

	public void Play()
	{
		this.sprite.sprite = this.closedSprite;
		this.audioDevice.Play();
		if (this.baldi.isActiveAndEnabled) this.baldi.ActivateAntiHearing(30f);
		StartCoroutine(ClosePlayer());
	}

	private IEnumerator ClosePlayer()
	{
		if (this.baldi.isActiveAndEnabled)
		{
			while (this.baldi.antiHearing == true)
				yield return null;

			this.sprite.sprite = this.openSprite;
		}
	}

	public Sprite closedSprite;
	[SerializeField] private Sprite openSprite;
	public SpriteRenderer sprite;
	public BaldiScript baldi;
	private AudioSource audioDevice;
}
