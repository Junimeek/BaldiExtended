using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScreenTrigger : MonoBehaviour
{
    private void Start()
    {
        hasStarted = false;
        leaveScreen.SetActive(false);
        killScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !hasStarted)
        {
            hasStarted = true;
            badSum.Play();
            leaveScreen.SetActive(true);
            StartCoroutine(WaitForAudio());
        }
    }

    private IEnumerator WaitForAudio()
    {
        while (badSum.isPlaying)
            yield return null;
        StartCoroutine(FinalCountdown());
        StopCoroutine(WaitForAudio());
    }

    private IEnumerator FinalCountdown()
    {
        leaveScreen.SetActive(false);
        killScreen.SetActive(true);
        baldloon.Play();
        float time = 8f;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.gc.UnlockMouse();
        Debug.LogWarning("Game Quit");
        Application.Quit();
    }
    
    [SerializeField] private Collider audioCollider;
    [SerializeField] private GameObject leaveScreen;
    [SerializeField] private GameObject killScreen;
    [SerializeField] private AudioSource badSum;
    [SerializeField] private AudioSource baldloon;
    [SerializeField] private bool hasStarted;
    [SerializeField] private GameControllerScript gc;
}
