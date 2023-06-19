using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneLoader : MonoBehaviour
{
    [SerializeField] private LoadingManager loadingManager;

    private void Start()
    {
        this.loadingManager = FindObjectOfType<LoadingManager>();
    }

    public void LoadTheScene(string newScene, int sceneType)
    {
        if (loadingManager == null) SceneManager.LoadSceneAsync(newScene);
        else loadingManager.LoadNewScene(newScene, sceneType);
    }
}
