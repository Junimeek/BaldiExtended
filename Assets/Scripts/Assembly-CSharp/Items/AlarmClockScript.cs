using UnityEngine;

public class AlarmClockScript : MonoBehaviour
{
	private void Start()
	{
		this.timeLeft = 10f;
		this.SetClock();
	}

	private void SetClock()
	{
		if (this.timeLeft > 45f)
			this.timeLeft = 15f;
		else if (this.timeLeft > 30f)
			this.timeLeft = 60f;
		else if (this.timeLeft > 15f)
			this.timeLeft = 45f;
		else if (this.timeLeft > 0f)
			this.timeLeft = 30f;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(this.player.gameObject.transform.position, base.transform.position) < 10f && raycastHit.transform.name.StartsWith("AlarmClock"))
			{
				this.audioDevice.PlayOneShot(this.wind);
				this.SetClock();
			}
		}

		if (this.timeLeft > 45f)
			this.clockSprite.sprite = this.timeSprites[4];
		else if (this.timeLeft > 30f)
			this.clockSprite.sprite = this.timeSprites[3];
		else if (this.timeLeft > 15f)
			this.clockSprite.sprite = this.timeSprites[2];
		else if (this.timeLeft > 0f)
			this.clockSprite.sprite = this.timeSprites[1];
		else
			this.clockSprite.sprite = this.timeSprites[0];

		if (this.timeLeft >= 0f) //If the time is greater then 0
			this.timeLeft -= Time.deltaTime; //Decrease the time variable
		else if (!this.rang) // If it has not been rang
			this.Alarm(); // Start the alarm function
	}

	private void Alarm()
	{
		this.rang = true;
		this.baldi.alarmClock = base.gameObject;

		if (this.baldi.isActiveAndEnabled)
		{
			this.baldi.isAlarmClock = true;
			this.baldi.AddNewSound(base.transform.position, 5); //Baldi is told to go to this location, with a priority of 5 (above most sounds)
		}

		this.audioDevice.clip = this.ring;
		this.audioDevice.loop = false; // Tells the audio not to loop
		this.audioDevice.Play(); //Play the audio
	}

	public float timeLeft;
	private bool rang;
	public BaldiScript baldi;
	[SerializeField] private AudioClip ring;
	[SerializeField] private AudioClip wind;
	public AudioSource audioDevice;
	public PlayerScript player;
	[SerializeField] private SpriteRenderer clockSprite;
	[SerializeField] private Sprite[] timeSprites;
}
