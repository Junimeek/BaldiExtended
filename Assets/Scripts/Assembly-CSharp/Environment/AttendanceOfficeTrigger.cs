using UnityEngine;

public class AttendanceOfficeTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            switch(other.gameObject.name)
            {
                case "1st Prize":
                    this.DisableCharacter(other.gameObject);
                    break;
                case "Gotta Sweep":
                    this.DisableCharacter(other.gameObject);
                    break;
                case "Playtime":
                    this.DisableCharacter(other.gameObject);
                    break;
            }
        }
    }

    void DisableCharacter(GameObject character)
    {
        character.SetActive(false);
        this.playerScript.ResetGuilt(PlayerScript.GuiltType.Bullying, 99f);
        this.princey.GuiltyAttendance();
    }

    [SerializeField] private PrincipalScript princey;
    [SerializeField] private PlayerScript playerScript;
}
