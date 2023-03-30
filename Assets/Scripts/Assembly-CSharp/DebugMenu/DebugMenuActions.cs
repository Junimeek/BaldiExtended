using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DebugMenuActions : MonoBehaviour
{
    [Header("Baldi")]
    public int debug_baldiBehavior;
    [SerializeField] private TMP_Text slapRateText;

    private void Start()
    {
        ResetDebugValues();
    }

    private void ResetDebugValues()
    {
        debug_baldiBehavior = 1;
        slapRateText.text = "Normal";
    }

    public void DebugBaldiModification(int buttonID)
    {
        /*
            ID 1 : Movement type (1 = normal ; 2 = force 3-seconds)
        */

        if (buttonID == 1)
        {
            if (debug_baldiBehavior == 1)
            {
                slapRateText.text = "Force 3s";
                debug_baldiBehavior++;
            }
            else if (debug_baldiBehavior == 2)
            {
                slapRateText.text = "Normal";
                debug_baldiBehavior = 1;
            }
        }
    }

    public void DebugLoadScene(string sceneToLoad)
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
