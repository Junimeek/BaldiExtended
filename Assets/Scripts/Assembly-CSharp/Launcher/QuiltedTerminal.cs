using System.Collections;
using UnityEngine;
using TMPro;

public class QuiltedTerminal : MonoBehaviour
{
    public void BeginGameBoot()
    {
        terminalText.text = string.Empty;
        StartCoroutine(DisplayStartupString());
    }

    public void BeginGameShutdown()
    {
        terminalText.text = string.Empty;

    }

    IEnumerator DisplayStartupString()
    {
        float delay = 0.2f;
        int remainingCharacters = startupString.Length;
        
        switch(remainingCharacters)
        {
            case 0:
                delay = 0.3f;
                while (delay > 0f)
                {
                    delay -= Time.unscaledDeltaTime;
                    yield return null;
                }
                terminalText.text = string.Empty;
                launcherScript.FinalBoot();
                break;
            case 99:
                for (int i = 0; i < startupString.Length; i++)
                {
                    delay = 0.03f;
                    while (delay > 0f)
                    {
                        delay -= Time.unscaledDeltaTime;
                        yield return null;
                    }
                    remainingCharacters--;
                    terminalText.text += startupString[i];
                }
                goto case 0;
            default:
                goto case 99;

        }
    }

    public void QuitGame()
    {
        terminalText.text = string.Empty;
        StartCoroutine(ExplodeGame());
    }

    IEnumerator ExplodeGame()
    {
        float delay = 0.2f;
        int remainingCharacters = terminationString.Length;

        switch(remainingCharacters)
        {
            case 0:
                Debug.Log("Game Quit");
                Application.Quit();
                break;
            case 99:
                for (int i = 0; i < terminationString.Length; i++)
                {
                    delay = 0.03f;
                    while (delay > 0f)
                    {
                        delay -= Time.unscaledDeltaTime;
                        yield return null;
                    }
                    remainingCharacters--;
                    terminalText.text += terminationString[i];
                }
                goto case 0;
            default:
                goto case 99;
        }
    }

    [SerializeField] Launcher launcherScript;
    [SerializeField] TMP_Text terminalText;
    readonly string[] startupString =
    {
        ">", " ", "S", "t", "a", "r", "t", "i", "n", "g", " ", "p", "r", "o", "g", "r", "a", "m", ".", ".", "."
    };
    readonly string[] terminationString =
    {
        ">", " ", "T", "e", "r", "m", "i", "n", "a", "t", "i", "n", "g",  " ", "p", "r", "o", "g", "r", "a", "m", ".", ".", "."
    };
}
