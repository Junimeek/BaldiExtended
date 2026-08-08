using UnityEngine;

public class AttendanceDetector : MonoBehaviour
{
    public enum Character
    {
        Playtime, GottaSweep, FirstPrize
    }
    public Character character;
    [HideInInspector] public GameObject sweepParent;
}
