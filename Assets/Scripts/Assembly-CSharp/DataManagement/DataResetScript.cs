using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class DataResetScript : MonoBehaviour
{
    private void OnEnable()
    {
        this.curAction = 0;
        promptBackground.color = new Color(1f, 1f, 1f, 0f);
        promptBackground.raycastTarget = false;
        promptText.text = string.Empty;
        this.noButton.SetActive(false);
        this.yesButton.SetActive(false);
    }

    public void ResetPrompt(int id)
    {
        promptBackground.color = new Color(1f, 1f, 1f, 1f);
        promptBackground.raycastTarget = true;
        this.curAction = id;
        this.noButton.SetActive(true);
        this.yesButton.SetActive(true);

        switch(id)
        {
            case 1:
                promptText.text = "This will reset all Endless Mode scores.\n\nReset all High Scores?";
            break;
            case 3:
                promptText.text = "This will reset all achievements.\n\nReset achievements?";
            break;
            case 99:
                promptText.text = "This will reset ALL DATA.\n\nAre you sure?";
            break;
            default:
                this.curAction = 0;
                promptBackground.color = new Color(1f, 1f, 1f, 0f);
                promptBackground.raycastTarget = false;
                promptText.text = string.Empty;
                this.noButton.SetActive(false);
                this.yesButton.SetActive(false);
            break;
        }
    }

    public void Confirm()
    {
        switch(curAction)
        {
            case 3:
                try
                {
                    achievementManager = FindObjectOfType<AchievementManager>();
                    Array.Resize<int>(ref achievementManager.ach_Maps, achievementManager.ach_Maps.Length-achievementManager.ach_Maps.Length);
                    achievementManager.SaveAchievementData();
                    ResetPrompt(0);
                }
                catch { Debug.LogError("Failed to clear achievements."); }
            break;
            case 99:
                SceneManager.LoadSceneAsync("Launcher");
            break;
        }
    }

    public void Cancel()
    {
        ResetPrompt(0);
    }

    [SerializeField] private int curAction;
    [SerializeField] private AchievementManager achievementManager;
    [SerializeField] private Image promptBackground;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private GameObject noButton;
    [SerializeField] private GameObject yesButton;
}
