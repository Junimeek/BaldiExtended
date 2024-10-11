using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
	private void Start()
	{
		//Yeah your on your own for this one
		if (PlayerPrefs.GetInt("AnalogMove") == 1)
			this.sensitivityActive = true;
		
		this.mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
		
		this.height = base.transform.position.y;
		this.stamina = this.maxStamina;
		this.playerRotation = base.transform.rotation;
		this.principalBugFixer = 1;
		this.flipaturn = 1f;
		this.isInfiniteStamina = false;
		this.isSpeedShoes = false;
		this.speedSlider.gameObject.SetActive(false);
		this.speedText.SetActive(false);
		this.isSecret = false;
	}

	private void Update()
	{
		if (gc.isSlowmo)
			playerDeltaTimeScale = Time.deltaTime * 2;
		else
			playerDeltaTimeScale = Time.deltaTime;

		if (gc.isSlowmo)
			playerTimeScale = Time.timeScale * 2;
		else
			playerTimeScale = Time.timeScale;

		//base.transform.position = new Vector3(base.transform.position.x, this.height, base.transform.position.z);
		this.MouseMove();
		this.PlayerMove();
		this.StaminaCheck();
		this.GuiltCheck();
		if (this.cc.velocity.magnitude > 0f)
			this.gc.LockMouse();
		
		if (this.jumpRope & (base.transform.position - this.frozenPosition).magnitude >= 1f) // If the player moves, deactivate the jumprope minigame
			this.DeactivateJumpRope();
		
		if (this.sweepingFailsave > 0f)
			this.sweepingFailsave -= Time.deltaTime;
		else
		{
			this.sweeping = false;
			this.hugging = false;
		}
	}

	public void LookAtCharacter(string character)
	{
		Vector3 relativePos = new Vector3(0f, 0f, 0f);

		switch(character)
		{
			case "baldi":
				relativePos = this.baldi.transform.position - this.gameObject.transform.position;
				break;
			case "playtime":
				relativePos = this.playtime.transform.position - this.gameObject.transform.position;
				break;
			case "princey":
				relativePos = this.princeyPos.position - this.gameObject.transform.position;
				break;
		}
		
		Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
		rotation.x = 0f;
		rotation.z = 0f;
		this.playerRotation = rotation;
	}

	private void MouseMove()
	{
		this.playerRotation.eulerAngles = new Vector3(this.playerRotation.eulerAngles.x, this.playerRotation.eulerAngles.y, this.fliparoo);
		this.playerRotation.eulerAngles = this.playerRotation.eulerAngles + Vector3.up * Input.GetAxis("Mouse X") * this.mouseSensitivity * this.playerTimeScale * this.flipaturn;
		base.transform.rotation = this.playerRotation;
	}

	private void PlayerMove()
	{
		Vector3 vector;
		Vector3 vector2;
		vector = base.transform.forward * Input.GetAxis("Forward");
		vector2 = base.transform.right * Input.GetAxis("Strafe");
		if (this.stamina > 0f)
		{
			if (Input.GetButton("Run"))
			{
				this.playerSpeed = this.runSpeed;
				this.sensitivity = 1f;

				if (this.cc.velocity.magnitude > 0.1f & !this.hugging & !this.sweeping)
					this.ResetGuilt("running", 0.1f);
			}
			else
			{
				this.playerSpeed = this.walkSpeed;

				if (this.sensitivityActive)
					this.sensitivity = Mathf.Clamp((vector2 + vector).magnitude, 0f, 1f);
				else
					this.sensitivity = 1f;
			}
		}
		else
		{
			this.playerSpeed = this.walkSpeed;

			if (this.sensitivityActive)
				this.sensitivity = Mathf.Clamp((vector2 + vector).magnitude, 0f, 1f);
			else
				this.sensitivity = 1f;
		}

		if (gc.isSlowmo)
			this.playerSpeed *= this.playerDeltaTimeScale;
		else
			this.playerSpeed *= this.playerDeltaTimeScale;

		this.moveDirection = (vector + vector2).normalized * this.playerSpeed * this.sensitivity;

		if (!(!this.jumpRope & !this.sweeping & !this.hugging))
		{
			if (this.sweeping && !this.bootsActive)
				this.moveDirection = this.gottaSweep.velocity * Time.deltaTime + this.moveDirection * 0.3f;
			else if (this.hugging && !this.bootsActive)
				this.moveDirection = (this.firstPrize.velocity * 1.2f * Time.deltaTime + (new Vector3(this.firstPrizeTransform.position.x, this.height, this.firstPrizeTransform.position.z) + new Vector3((float)Mathf.RoundToInt(this.firstPrizeTransform.forward.x), 0f, (float)Mathf.RoundToInt(this.firstPrizeTransform.forward.z)) * 3f - base.transform.position)) * (float)this.principalBugFixer;
			else if (this.jumpRope)
				this.moveDirection = new Vector3(0f, 0f, 0f);
		}

		this.cc.Move(this.moveDirection);
	}

	private void StaminaCheck()
	{
		if (this.cc.velocity.magnitude > 0.1f && !this.hugging)
		{
			if (this.isSpeedShoes)
			{
				this.runSpeed = speedOverrides.z * 2;
				this.walkSpeed = speedOverrides.y * 2;
				this.slowSpeed = speedOverrides.x * 2;

				if (!Input.GetButton("Run"))
					speedSlider.value -= this.shoeRate * 2.5f * playerDeltaTimeScale;
				else if (Input.GetButton("Run"))
					speedSlider.value -= this.shoeRate * 5 * playerDeltaTimeScale;

				if (speedSlider.value <= 0f && this.isSpeedShoes)
				{
					speedSlider.value = -5f;
					speedSlider.gameObject.SetActive(false);
					speedText.SetActive(false);
					this.isSpeedShoes = false;
				}
			}
			else
			{
				this.runSpeed = speedOverrides.z;
				this.walkSpeed = speedOverrides.y;
				this.slowSpeed = speedOverrides.x;
			}

			if (Input.GetButton("Run") & this.stamina > 0f && !this.isInfiniteStamina)
				this.stamina -= this.staminaRate * playerDeltaTimeScale;
			if (this.stamina < 0f & this.stamina > -5f)
				this.stamina = -5f;
		}
		else if ((this.stamina < this.softStaminaCap) || (this.stamina < this.softStaminaCap && this.hugging))
		{
			if (this.isInfiniteStamina)
				this.stamina += this.staminaRate * 2f * this.playerDeltaTimeScale * 2f;
			else
				this.stamina += this.staminaRate * 2f * this.playerDeltaTimeScale;
		}

		this.staminaBar.value = this.stamina / this.maxStamina * 100f;

		if (this.isInfiniteStamina)
			this.softStaminaCap = 300f;
		else
			this.softStaminaCap = 100f;
	}

	public void IncreaseFightSpeed(int speedPoints)
	{
		float speedValue;

		if (speedPoints == 0)
			speedValue = 10f;
		else
			speedValue = speedPoints * 5f + 15f;

		this.speedOverrides = new Vector3(speedValue, speedValue, speedValue);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "Baldi" || other.transform.name == "NullBoss")
		{
			if (!this.gc.isSafeMode)
			{
				if (other.transform.name == "NullBoss" && this.isInvincible)
					return;

				this.gameOver = true;
				RenderSettings.skybox = this.blackSky; //Sets the skybox black
				base.StartCoroutine(this.KeepTheHudOff()); //Hides the Hud
			}
			else
				this.gc.EndSafeGame();
		}
		else if (other.transform.name == "Playtime" && !this.jumpRope
		&& this.playtime.playCool <= 0f && !this.playtime.isDisabled && !this.playtime.isParty)
			this.ActivateJumpRope();
	}

	public IEnumerator KeepTheHudOff()
	{
		while (this.gameOver)
		{
			this.hud.enabled = false;
			this.mobile1.enabled = false;
			this.mobile2.enabled = false;
			this.jumpRopeScreen.SetActive(false);
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "Gotta Sweep")
		{
			this.sweeping = true;
			this.sweepingFailsave = 1f;
		}
		else if (other.transform.name == "1st Prize" && this.firstPrize.velocity.magnitude > 5f
		&& !firstPrize.GetComponent<FirstPrizeScript>().isDisabled && !firstPrize.GetComponent<FirstPrizeScript>().isParty)
		{
			this.hugging = true;
			this.sweepingFailsave = 0.2f;
		}

		if (other.transform.name == "Stamina Trigger")
			this.isInfiniteStamina = true;
	}

	private void OnTriggerExit(Collider other)
	{
		switch(other.transform.name)
		{
			case "Stamina Trigger":
				this.isInfiniteStamina = false;
				break;
			case "Office Trigger":
				this.ResetGuilt("escape", this.door.lockTime);
				break;
			case "Gotta Sweep":
				this.sweeping = false;
				break;
			case "1st Prize":
				this.hugging = false;
				break;
		}
	}

	public void ResetGuilt(string type, float amount)
	{
		if (amount >= this.guilt)
		{
			this.guilt = amount;
			this.guiltType = type;
		}
	}

	private void GuiltCheck()
	{
		if (this.guilt > 0f)
			this.guilt -= playerDeltaTimeScale;
	}

	public void ActivateJumpRope()
	{
		this.jumpRopeScreen.SetActive(true);
		this.jumpRope = true;
		this.frozenPosition = base.transform.position;
	}

	public void DeactivateJumpRope()
	{
		this.jumpRopeScreen.SetActive(false);
		this.jumpRope = false;
	}

	public void ActivateBoots()
	{
		this.bootsActive = true;
		base.StartCoroutine(this.BootTimer());
	}

	private IEnumerator BootTimer()
	{
		float time = 15f;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		this.bootsActive = false;
		yield break;
	}

	public void ActivateSpeedShoes()
	{
		this.isSpeedShoes = true;
		this.speedText.SetActive(true);
		this.speedSlider.gameObject.SetActive(true);
		this.speedSlider.value = 100f;
	}

	public void DebugAction(int id)
	{
		switch(id)
		{
			case 1:
				this.speedOverrides = new Vector3(20f, 50f, 100f);
				break;
		}
	}

	public GameControllerScript gc;
	public BaldiScript baldi;
	public ClassroomDoorScript door;
	public PlaytimeScript playtime;
	public bool gameOver;
	public bool jumpRope;
	public bool sweeping;
	public bool hugging;
	public bool bootsActive;
	public int principalBugFixer;
	[SerializeField] private Transform princeyPos;
	public float sweepingFailsave;
	[SerializeField] public float fliparoo;
	[SerializeField] public float flipaturn;
	public Quaternion playerRotation;
	public Vector3 frozenPosition;
	private bool sensitivityActive;
	private float sensitivity;
	public float mouseSensitivity;
	[SerializeField] private float playerDeltaTimeScale;
	[SerializeField] private float playerTimeScale;
	public float walkSpeed;
	public float runSpeed;
	public float slowSpeed;
	public float maxStamina;
	public float staminaRate;
	public float guilt;
	public float initGuilt;
	private float moveX;
	private float moveZ;
	private Vector3 moveDirection;
	private float playerSpeed;
	public float stamina;
	public CharacterController cc;
	public NavMeshAgent gottaSweep;
	public NavMeshAgent firstPrize;
	public Transform firstPrizeTransform;
	public Slider staminaBar;
	public float db;
	public string guiltType;
	public GameObject jumpRopeScreen;
	public float height;
	public Material blackSky;
	public Canvas hud;
	public Canvas mobile1;
	public Canvas mobile2;
	public bool isProjectileGrabbed;

	[SerializeField] bool isInfiniteStamina;
	[SerializeField] private float softStaminaCap;
	[SerializeField] bool isSpeedShoes;
	public Slider speedSlider;
	public GameObject speedText;
	[SerializeField] private float shoeRate;
	public bool isSecret;
	public bool isNullStyle;
	public bool isInvincible;
	[Tooltip("X = Slow, Y = Walk, Z = Run")] [SerializeField] private Vector3 speedOverrides;
}
