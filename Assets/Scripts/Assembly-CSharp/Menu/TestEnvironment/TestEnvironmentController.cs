using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestEnvironmentController : MonoBehaviour
{
    [SerializeField] MenuPlayerController playerScript;
    [SerializeField] Transform playerTransform;
    [SerializeField] Image handIcon;
    [SerializeField] Sprite[] handIconSprites;
    [SerializeField] GameObject upperUIText;
    [SerializeField] SettingsContainer settingsContainer;
    [SerializeField] MenuTestEnvMusic musicDevice;
    public bool isStopped;

    void Start()
    {
        settingsContainer = FindObjectOfType<SettingsContainer>();
    }

    public void ReEnableTestEnvironment(float turnSensitivity)
    {
        this.isStopped = false;
        playerScript.ReEnterTestMove(turnSensitivity);
        this.upperUIText.SetActive(true);
        this.musicDevice.StartLoop();
    }

    void Update()
    {
        if (!this.isStopped)
            CheckRaycast();
        
        if (Input.GetKeyDown(KeyCode.Escape) && !this.isStopped)
        {
            this.isStopped = true;
            playerScript.LeaveTestMove();
            musicDevice.EndLoop();
            this.upperUIText.SetActive(false);
            if (settingsContainer == null)
            {
                SceneManager.LoadSceneAsync("MainMenu");
            }
            else
            {
                settingsContainer.ToggleMenu(true);
            }
            this.ChangeHandIcon(99);
        }
    }

    void CheckRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;
		float distance;

		if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider.name == "_DoorOut" && !raycastHit.collider.CompareTag("SwingingDoor"))
			distance = 15f;
		else
			distance = 15f;

		try
		{
            if (this.isStopped)
                this.ChangeHandIcon(99);
			else if (Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= distance)
			{
				if (raycastHit.collider.name == "_DoorOut" && !(raycastHit.collider.tag == "SwingingDoor"))
				{
					this.ChangeHandIcon(1);
				}
				else
					this.ChangeHandIcon(0);
			}
			else
				this.ChangeHandIcon(0);
		}
		catch
		{
			this.ChangeHandIcon(0);
			return;
		}
    }

    void ChangeHandIcon(int icon)
    {
        if (icon == 99)
            this.handIcon.color = Color.clear;
        else
        {
            this.handIcon.color = Color.white;
            this.handIcon.sprite = this.handIconSprites[icon];
        }
    }
}
