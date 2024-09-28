using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneLoader : MonoBehaviour
{
    private void Start()
    {
        this.loadingManager = FindObjectOfType<LoadingManager>();

        if (this.loadingManager != null)
            this.isSceneLoaderFound = true;
        else
            this.isSceneLoaderFound = false;
    }

    public void LoadTheScene(string newScene, int sceneType)
    {
        if (!this.isSceneLoaderFound)
            SceneManager.LoadSceneAsync(newScene);
        else
            this.loadingManager.LoadNewScene(newScene, sceneType);
    }

    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private bool isSceneLoaderFound;
}
