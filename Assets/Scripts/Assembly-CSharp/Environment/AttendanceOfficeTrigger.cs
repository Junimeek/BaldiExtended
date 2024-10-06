using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttendanceOfficeTrigger : MonoBehaviour
{
    private void Start()
    {
        this.isTriggerDisabled = false;
        this.gc = FindObjectOfType<GameControllerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this.isTriggerDisabled)
        {
            other.gameObject.SetActive(false);
            this.StartCoroutine(Countdown());
        }
    }

    private IEnumerator Countdown()
    {
        float timer;
        timer = 1f;
        this.playerScript.ResetGuilt("bully", 99f);
        this.isTriggerDisabled = true;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        
        this.princey.GuiltyAttendance();
    }

    [SerializeField] private PrincipalScript princey;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private bool isTriggerDisabled;
    [SerializeField] private GameControllerScript gc;
}
