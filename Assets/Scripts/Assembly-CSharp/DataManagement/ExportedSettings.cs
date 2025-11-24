using UnityEngine;

public class ExportedSettings : MonoBehaviour
{
    [Header("Opions")]
    public float turnSensitivity;
    public float volumeVoice;
    public float volumeBGM;
    public float volumeSFX;
    public int instantReset;
    public int additionalMusic;
    public int notifBoard;
    public int familyFriendly;

    [Header("Patches")]
    public int doorFix;
    public int framerate;

    [Header("Gameplay Styles")]
    public int safeMode;
    public int difficultMath;
}
