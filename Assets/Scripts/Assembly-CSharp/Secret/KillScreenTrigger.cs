using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScreenTrigger : MonoBehaviour
{
    private void Start()
    {
        canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            canvas.SetActive(true);
            StartCoroutine(countdown());
        }
    }

    private IEnumerator countdown()
    {
        float time = 2f;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        Debug.LogWarning("Game Quit");
        Application.Quit();
    }
    
    [SerializeField] private Collider audioCollider;
    [SerializeField] private GameObject canvas;
}
