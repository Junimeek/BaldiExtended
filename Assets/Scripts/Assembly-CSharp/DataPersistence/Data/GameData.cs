using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float turnSensitivity;
    public bool isInstantReset;
    public bool isNotifBoard;
    public bool isNewMusic;
    public float volVoice;
    public float volBGM;
    public float volSFX;

    // Default values when no save data is found
    public GameData()
    {
        this.turnSensitivity = 2f;
        this.isInstantReset = true;
        this.isNotifBoard = true;
        this.isNewMusic = false;
        this.volVoice = -1f;
        this.volBGM = -1f;
        this.volSFX = -1f;
    }
}
