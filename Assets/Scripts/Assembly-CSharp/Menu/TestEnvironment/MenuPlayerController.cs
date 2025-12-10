using UnityEngine;

public class MenuPlayerController : MonoBehaviour
{
    [SerializeField] bool isMouseLocked;
    [Header("Player")]
    [SerializeField] float sensitivity;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float height;
    [SerializeField] Quaternion playerRotation;
    [SerializeField] CharacterController cc;
    [SerializeField] Vector3 moveDirection;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float playerSpeed;

    [Header("Camera")]
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] int cameraLookBehind;
    [SerializeField] new Camera camera;
    [SerializeField] GameObject cameraObject;
    [SerializeField] int cameraZoom;

    void Start()
    {
        this.mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");

        this.TogglePlayerControl(true);

        this.height = base.transform.position.y;
		this.playerRotation = base.transform.rotation;
        this.cameraOffset = cameraObject.transform.position - base.transform.position;
    }

    void Update()
    {
        if (this.isMouseLocked)
        {
            this.MouseMove();
            this.PlayerMove();
            if (this.cc.velocity.magnitude > 0f)
                this.LockMouse();
        
            if (Input.GetButton("Look Behind"))
                this.cameraLookBehind = 180;
            else
                this.cameraLookBehind = 0;

            if (Input.GetKey(KeyCode.C))
			    this.cameraZoom = 15;
    		else
	    		this.cameraZoom = 60;
        }
    }

    void LateUpdate()
    {
        if (this.isMouseLocked)
        {
            cameraObject.transform.position = base.transform.position + cameraOffset;
            cameraObject.transform.rotation = base.transform.rotation * Quaternion.Euler(0f, this.cameraLookBehind, 0f);
            this.camera.fieldOfView = this.cameraZoom;
        }
    }

    void MouseMove()
    {
        this.playerRotation.eulerAngles = new Vector3(this.playerRotation.eulerAngles.x, this.playerRotation.eulerAngles.y, 0f);
		this.playerRotation.eulerAngles = this.playerRotation.eulerAngles + Vector3.up * Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.timeScale;
		base.transform.rotation = this.playerRotation;
    }

    void PlayerMove()
    {
        Vector3 vector = base.transform.forward * Input.GetAxis("Forward");
		Vector3 vector2 = base.transform.right * Input.GetAxis("Strafe");
        this.sensitivity = 1f;
        if (Input.GetButton("Run"))
        {
            this.playerSpeed = this.runSpeed;
        }
        else
        {
            this.playerSpeed = this.walkSpeed;
        }

        this.playerSpeed *= Time.unscaledDeltaTime;
        this.moveDirection = (vector + vector2).normalized * this.playerSpeed * this.sensitivity;
        this.cc.Move(moveDirection);
    }

    public void TogglePlayerControl(bool toggleThing)
    {
        if (toggleThing)
            this.LockMouse();
        else
            this.UnlockMouse();
    }

    public void LeaveTestMove()
    {
        this.UnlockMouse();
        this.camera.enabled = false;
    }

    public void ReEnterTestMove(float newSensitivity)
    {
        this.camera.enabled = true;
        this.LockMouse();
        this.mouseSensitivity = newSensitivity;
    }

    void LockMouse()
    {
        this.isMouseLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockMouse()
    {
        this.isMouseLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
