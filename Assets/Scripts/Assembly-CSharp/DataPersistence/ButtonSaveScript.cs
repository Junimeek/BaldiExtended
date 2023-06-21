using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSaveScript : MonoBehaviour
{
    public SET_GameplayScript gpScript;
    public SET_AudioScript audScript;
    [SerializeField] private DelayedSave delayedSave;

    [SerializeField] private GameObject gpPage;
    [SerializeField] private GameObject audPage;
    [SerializeField] private GameObject dataPage;

    public void NextPage(int page)
    {
        StartCoroutine(PageDelay(page));
    }

    private IEnumerator PageDelay(int page)
    {
        delayedSave.DelayedSaveFunction();

        while (delayedSave.isSaving)
        {
            yield return null;
        }

        SwitchPage(page);
    }

    private void SwitchPage(int page)
    {
        if (page == 1)
        {
            gpPage.SetActive(true);
            audPage.SetActive(false);
        }
        else if (page == 2)
        {
            audPage.SetActive(true);
            gpPage.SetActive(false);
            dataPage.SetActive(false);
        }
        else if (page == 3)
        {
            dataPage.SetActive(true);
            audPage.SetActive(false);
        }
    }
}
