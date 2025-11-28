using UnityEngine;

public class StealTheMoon : MonoBehaviour
{
    Camera playerCamera;

    void Start()
    {
        this.playerCamera = Camera.main;
        int randomNumber = Mathf.FloorToInt(Random.Range(0f, 40f));
        if (randomNumber == 0)
            base.transform.localScale = new Vector3(300f, 300f, 1f);
    }

    void LateUpdate()
    {
        base.transform.LookAt(base.transform.position + this.playerCamera.transform.rotation * Vector3.forward);
    }
}
