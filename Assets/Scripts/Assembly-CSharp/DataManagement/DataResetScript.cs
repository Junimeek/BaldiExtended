using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DataResetScript : MonoBehaviour
{
    private void OnEnable()
    {
        this.ResetPrompt(0);
    }

    public void ResetPrompt(int id)
    {
        this.curAction = id;

        if (id != 1)
        {
            this.ShowPrompt();
            this.HideScorePrompt();
        }

        switch(id)
        {
            case 1:
                this.ShowScorePrompt();
                break;
            case 2:
                this.promptText.text = "This will reset all settings\nto their default values.\n\nReset settings?";
                break;
            case 21:
                this.promptText.text = "This will reset all Story Mode scores\nfor your best time and item usage.\n\nReset Story Mode high scores?";
                break;
            case 22:
                this.promptText.text = "This will reset all Endless Mode scores\nfor your notebook count and item usage.\n\nReset Endless Mode high scores?";
                break;
            case 23:
                this.promptText.text = "This will reset all Challenge Mode scores\nfor your best time and item usage.\n\nReset Challenge Mode high scores?";
                break;
            case 98:
                this.promptText.text = "This will reset all achievements.\n\nReset achievements?";
                break;
            case 99:
                this.promptText.text = "WARNING:\nThis will delete ALL save data.\n\nAre you absolutely sure you want to\nproceed with deleting everything?";
                break;
            default:
                this.curAction = 0;
                this.HidePrompt();
                this.HideScorePrompt();
                break;
        }
    }

    public void Confirm()
    {
        this.HidePrompt();
        this.HideScorePrompt();

        switch(this.curAction)
        {
            case 2:
                FindObjectOfType<SettingsContainer>().ResetSettings();
                break;
            case 21:
                SaveDataController.SaveStoryData(null);
                break;
            case 22:
                SaveDataController.SaveEndlessData(null);
                break;
            case 23:
                SaveDataController.SaveChallengeData(null);
                break;
            case 98:
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
                string path = Application.persistentDataPath;
                File.Copy(path + "/BaldiData/story.sav", Application.persistentDataPath + "/BaldiData_Backup/story.sav", true);
                File.Copy(path + "/BaldiData/endless.sav", Application.persistentDataPath + "/BaldiData_Backup/endless.sav", true);
                File.Copy(path + "/BaldiData/challenge.sav", Application.persistentDataPath + "/BaldiData_Backup/challenge.sav", true);
                File.Delete(path + "/BaldiData/story.sav");
                File.Delete(path + "/BaldiData/endless.sav");
                File.Delete(path + "/BaldiData/challenge.sav");
                File.Delete(Application.persistentDataPath + "/settings.sav");
                Directory.Delete(path + "/BaldiData");
                SceneManager.LoadSceneAsync("Launcher");
                break;
        }
    }

    public void Cancel()
    {
        this.ResetPrompt(0);
    }

    private void ShowPrompt()
    {
        this.promptBackground.color = new Color(1f, 1f, 1f, 1f);
        this.promptBackground.raycastTarget = true;
        this.backButton.interactable = false;
        this.scoreBackButton.SetActive(false);
        this.noButton.SetActive(true);
        this.yesButton.SetActive(true);
    }

    private void ShowScorePrompt()
    {
        this.scorePromptBackground.color = new Color(1f, 1f, 1f, 1f);
        this.scorePromptBackground.raycastTarget = true;
        this.backButton.interactable = false;
        this.scoreBackButton.SetActive(true);
        this.storyButton.SetActive(true);
        this.endlessButton.SetActive(true);
        this.challengeButton.SetActive(true);
        this.scoreText.text = "Select which high scores to reset:";
    }

    private void HidePrompt()
    {
        this.promptBackground.color = new Color(1f, 1f, 1f, 0f);
        this.promptBackground.raycastTarget = false;
        this.backButton.interactable = true;
        this.promptText.text = string.Empty;
        this.noButton.SetActive(false);
        this.yesButton.SetActive(false);
    }

    private void HideScorePrompt()
    {
        this.scorePromptBackground.color = new Color(1f, 1f, 1f, 0f);
        this.scorePromptBackground.raycastTarget = false;
        this.storyButton.SetActive(false);
        this.endlessButton.SetActive(false);
        this.challengeButton.SetActive(false);
        this.scoreBackButton.SetActive(false);
        this.scoreText.text = string.Empty;
    }

    [SerializeField] private int curAction;
    [SerializeField] private AchievementManager achievementManager;
    [SerializeField] private Image promptBackground;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private GameObject noButton;
    [SerializeField] private GameObject yesButton;
    [SerializeField] private Image scorePromptBackground;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject storyButton;
    [SerializeField] private GameObject endlessButton;
    [SerializeField] private GameObject challengeButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject scoreBackButton;
}
