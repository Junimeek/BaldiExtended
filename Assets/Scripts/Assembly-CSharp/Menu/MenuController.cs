using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	private void Start()
	{
		AudioManager audioManager = FindObjectOfType<AudioManager>();
		if (audioManager != null)
			audioManager.SetVolume(0);
	}

	public void OnEnable()
	{
		this.uc.firstButton = this.firstButton;
		this.uc.dummyButtonPC = this.dummyButtonPC;
		this.uc.dummyButtonElse = this.dummyButtonElse;
		this.uc.SwitchMenu();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Cancel") && this.back != null)
		{
			this.back.SetActive(true);
			base.gameObject.SetActive(false);
		}
	}

	public UIController uc;
	public Selectable firstButton;
	public Selectable dummyButtonPC;
	public Selectable dummyButtonElse;
	public GameObject back;

}
