using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeControllerScript : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Hello World");
    }


    [Header("Game State")]
    [SerializeField] private string curMap;
    public bool spoopMode;
    public bool finaleMode;
    public int exitsReached;
    private bool gamePaused;
    public bool learningActive;
	public float gameOverDelay;
    public bool isDifficultMath;
    
    [Header("UI")]
    [Header("Noteboos")]
    public int notebooks;
	public int daFinalBookCount;
    
    [Header("Player")]
    public Transform playerTransform;

    [Header("Characters")]
    public GameObject baldi;

    [Header("Items")]
    public int[] item;

    [Header("SFX and Voices")]
    public AudioSource audioDevice;

    [Header("Music")]
    [SerializeField] private AudioSource schoolMusic;

    [Header("Scripts")]
    [SerializeField] private EntranceScript entrance_0;
}
