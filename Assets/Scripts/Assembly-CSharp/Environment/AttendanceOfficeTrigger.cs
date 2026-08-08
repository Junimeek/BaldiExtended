using UnityEngine;

public class AttendanceOfficeTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        AttendanceDetector attendanceDetector = other.gameObject.GetComponent<AttendanceDetector>();
        if (attendanceDetector != null)
        {
            if (attendanceDetector.character == AttendanceDetector.Character.GottaSweep)
            {
                this.DisableCharacter(attendanceDetector.sweepParent);
            }
            else
                this.DisableCharacter(other.gameObject);
        }
    }

    void DisableCharacter(GameObject character)
    {
        character.SetActive(false);
        this.playerScript.ResetGuilt(PlayerScript.GuiltType.Bullying, 99f);
        this.princey.GuiltyAttendance();
    }

    [SerializeField] PrincipalScript princey;
    [SerializeField] PlayerScript playerScript;
}
