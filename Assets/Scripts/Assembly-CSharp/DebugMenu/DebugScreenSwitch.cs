using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScreenSwitch : MonoBehaviour
{
    [Header("Menu stuff")]
    [SerializeField] private GameObject menuBG;
    public bool isDebugMenuActive;

    private void Start()
    {
        isDebugMenuActive = false;
        menuBG.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isDebugMenuActive)
            {
                isDebugMenuActive = true;
                menuBG.SetActive(true);
            }
            else if (isDebugMenuActive)
            {
                isDebugMenuActive = false;
                menuBG.SetActive(false);
            }
        }
    }

    public void DebugCloseMenu()
    {
        isDebugMenuActive = false;
        menuBG.SetActive(false);
    }
}
