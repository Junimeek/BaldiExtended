using UnityEngine;

public class CeilingFanScript : MonoBehaviour
{
    private void Update()
    {
        this.yAngle += this.speed * 50f * Time.deltaTime;
        this.currentEulerAngles = new Vector3(180f, this.yAngle, 0f);
        this.currentRotation.eulerAngles = this.currentEulerAngles;
        this.fan.rotation = this.currentRotation;
    }

    [SerializeField] private float speed;
    [SerializeField] private Transform fan;
    private Vector3 currentEulerAngles;
    private float yAngle;
    private Quaternion currentRotation;
}
