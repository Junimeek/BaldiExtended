using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSave : MonoBehaviour
{
    [SerializeField] private DataPersistenceManager dataManager;
    [SerializeField] private GameObject saveCanvas;
    public bool isSaving;

    private void Start()
    {
        // this.dataManager = FindObjectOfType<DataPersistenceManager>();
        saveCanvas.SetActive(false);
        isSaving = false;
    }

    private IEnumerator AssignDataManager()
    {
        while (this.dataManager == null)
        {
            this.dataManager = FindObjectOfType<DataPersistenceManager>();
            yield return null;
        }
    }

    public void DelayedSaveFunction()
    {
        isSaving = true;
        StartCoroutine(SaveThis());
    }

    private IEnumerator SaveThis()
    {
        saveCanvas.SetActive(true);
        dataManager.SaveGame();

        float delay = 0.2f;

        while (delay > 0f)
        {
            delay -= Time.unscaledDeltaTime;
            yield return null;
        }

        saveCanvas.SetActive(false);
        isSaving = false;
    }
}
