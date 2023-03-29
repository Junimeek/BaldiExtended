using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    public string curLevel;
    public bool isDebugActive;

    [SerializeField] TMP_Text boot_DebugText;
    [SerializeField] GameObject debugCanvas;
    [SerializeField] private float textDelay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(debugCanvas.gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (isDebugActive)
        {
            SceneManager.UnloadScene("GlobalState");
            ClearDebugText(7f);
        }
        else
        {
            boot_DebugText.text = string.Empty;
            BootGameNormal();
        }
    }

    private void BootGameNormal()
    {
        curLevel = "Nothing";
        SceneManager.LoadSceneAsync("Warning");
    }

    private void ClearDebugText(float delay)
    {
        textDelay = delay;
    }

    private void Update()
    {
        if (isDebugActive && textDelay > 0f)
        {
            boot_DebugText.text = "Debug mode is ACTIVE! Please deactivate it in the inspector\nbefore shipping this game for release!";
            textDelay -= Time.deltaTime;

            if (this.textDelay <= 0f)
            {
                boot_DebugText.text = string.Empty;
            }
        }

    }
}
