using UnityEngine;

public class MenuPlayerController : MonoBehaviour
{
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

        this.LockMouse();

        this.height = base.transform.position.y;
		this.playerRotation = base.transform.rotation;
        this.cameraOffset = cameraObject.transform.position - base.transform.position;
    }

    void Update()
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

    void LateUpdate()
    {
        cameraObject.transform.position = base.transform.position + cameraOffset;
        cameraObject.transform.rotation = base.transform.rotation * Quaternion.Euler(0f, this.cameraLookBehind, 0f);
        this.camera.fieldOfView = this.cameraZoom;
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

    public void LeaveTestMove()
    {
        this.UnlockMouse();
    }

    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
