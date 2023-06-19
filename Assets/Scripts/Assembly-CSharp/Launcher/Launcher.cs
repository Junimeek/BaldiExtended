using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    private void Start()
    {
        logoCanvas.SetActive(false);
        launcherCanvas.SetActive(false);
        StartCoroutine(WaitForStart());
    }

    private IEnumerator WaitForStart()
    {
        float time = 0.3f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        launcherCanvas.SetActive(true);
        audioDevice.PlayOneShot(music);
    }

    public void DaClick(int id)
    {
        if (id == 1 && !isLaunching) // Generic
            audioDevice.PlayOneShot(acceptSound);

        else if (id == 2) // Github
        {
            audioDevice.PlayOneShot(acceptSound);
            Application.OpenURL("https://github.com/Junimeek/BaldiExtended");
        }

        else if (id == 3 && !isLaunching) // Quit Game
        {
            audioDevice.PlayOneShot(quitSound);
            StartCoroutine(WaitForQuit());
        }

        else if (id == 99 && !isLaunching) // Launch Game
        {
            audioDevice.volume = 0.5f;
            audioDevice.PlayOneShot(splap);
            StartCoroutine(WaitForLaunch());
        }
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    private IEnumerator WaitForLaunch()
    {
        float time = 0.5f;
        isLaunching = true;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        logoCanvas.SetActive(true);
        launcherCanvas.SetActive(false);
        audioDevice.volume = 0f;
        StartCoroutine(WaitForLogo());
    }

    private IEnumerator WaitForQuit()
    {
        float time = 1.2f;
        isLaunching = true;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        Debug.LogWarning("Game Quit");
        Application.Quit();
    }

    private IEnumerator WaitForLogo()
    {
        basicallyLogo.SetActive(true);
        juniLogo.SetActive(false);

        float time1 = 2.5f;

        while (time1 > 0f)
        {
            time1 -= Time.unscaledDeltaTime;
            yield return null;
        }

        basicallyLogo.SetActive(false);
        juniLogo.SetActive(true);

        float time2 = 1.7f;

        while (time2 > 0f)
        {
            time2 -= Time.unscaledDeltaTime;
            yield return null;
        }

        loadingManager.LoadNewScene("Warning", 1);
    }

    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip splap;
    [SerializeField] private AudioClip quitSound;
    [SerializeField] private AudioClip acceptSound;
    [SerializeField] private bool isLaunching;
    [SerializeField] private GameObject launcherCanvas;
    [SerializeField] private GameObject logoCanvas;
    [SerializeField] private GameObject basicallyLogo;
    [SerializeField] private GameObject juniLogo;
    [SerializeField] private LoadingManager loadingManager;
}
