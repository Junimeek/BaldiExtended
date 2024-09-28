using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image joe;
    [SerializeField] private TMP_Text percentageText;
    [SerializeField] private Slider barPercent;
    [SerializeField] private float curPercent;
    [SerializeField] private string targetScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        loadingCanvas.SetActive(false);
        this.curPercent = 0f;
        this.barPercent.value = 0f;
        this.percentageText.text = string.Empty;
    }

    public void LoadNewScene(string sceneName, int loadType)
    {
        targetScene = sceneName;
        StartCoroutine(LoadSceneRoutine(loadType));
    }

    public IEnumerator LoadSceneRoutine(int loadType)
    {
        barPercent.value = 0f;

        loadingCanvas.SetActive(true);

        if (loadType == 1) this.joe.color = new Color(1f, 1f, 1f, 0f);
        else this.joe.color = new Color(1f, 1f, 1f, 1f);

        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);

        while (!op.isDone)
        {
            curPercent = Mathf.RoundToInt((op.progress/0.9f)*100f);
            barPercent.value = curPercent;
            percentageText.text = curPercent + "%";
            yield return null;
        }

        loadingCanvas.SetActive(false);
        StopCoroutine(LoadSceneRoutine(0));
    }
}
