using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationBoard : MonoBehaviour
{
    private void Start()
    {
        this.gc = FindObjectOfType<GameControllerScript>();
        
        try
        {
            this.ruleText.text = string.Empty;
            this.detentionText.text = string.Empty;
            this.notebooText.text = string.Empty;

            this.notebooGroup.SetActive(false);
            this.detentionGroup.SetActive(false);
            this.ruleGroup.SetActive(false);
        }
        catch
        {
            Debug.LogError("Failed to disable the notif board");
        }
    }

    private void Update()
    {
        if (this.gc.remainingDetentionTime > 0f)
			this.detentionText.text = "You have detention! \n" + Mathf.CeilToInt(this.gc.remainingDetentionTime) + " seconds remain!";
    }

    public void RuleText(int rule)
    {
        if (PlayerPrefs.GetInt("NotifBoard") == 1)
            this.ruleGroup.SetActive(true);

        switch(rule)
        {
            case 1:
                this.ruleText.text = "You have broken a rule!\n\"No entering faculty!\"";
                break;
            case 2:
                this.ruleText.text = "You have broken a rule!\n\"No running!\"";
                break;
            case 3:
                this.ruleText.text = "You have broken a rule!\n\"No drinking drinks!\"";
                break;
            case 4:
                this.ruleText.text = "You have broken a rule!\n\"No escaping detention!\"";
                break;
            case 5:
                this.ruleText.text = "You have broken a rule!\n\"No Bullying!\"";
                break;
        }
    }

    public void DetentionBoardRoutine()
    {
        StartCoroutine(this.DeactivateDetentionBoard());
    }

    public IEnumerator DeactivateDetentionBoard()
    {
        this.ruleGroup.SetActive(false);
        this.detentionGroup.SetActive(true);

        while (this.gc.remainingDetentionTime > 0f)
            yield return null;

        this.detentionGroup.SetActive(false);
    }

    public void StartNotebooRoutine(string noteName)
    {
        switch(noteName)
        {
            case "Math Notebook":
                this.notebooColor = new Color(0.75f, 0.52f, 0.54f, 1f);
                break;
            case "Spelling Notebook":
                this.notebooColor = new Color(0.12f, 0.15f, 0.75f, 1f);
                break;
            case "English Notebook":
                this.notebooColor = new Color(0.11f, 0.75f, 0.12f, 1f);
                break;
            case "History Notebook":
                this.notebooColor = new Color(0.75f, 0.11f, 0.11f, 1f);
                break;
            case "Geography Notebook":
                this.notebooColor = new Color(0.75f, 0.75f, 0.12f, 1f);
                break;
            case "Geology Notebook":
                this.notebooColor = new Color(0.07f, 0.75f, 0.75f, 1f);
                break;
            case "Culinary Notebook":
                this.notebooColor = new Color(0.42f, 0.31f, 0.12f, 1f);
                break;
            case "Culture Notebook":
                this.notebooColor = new Color(1f, 0f, 1f, 1f);
                break;
            case "Art Notebook":
                this.notebooColor = new Color(0.42f, 0.12f, 0.12f, 1f);
                break;
            case "Chemistry Notebook":
                this.notebooColor = new Color(0.96f, 0.54f, 0f, 1f);
                break;
            case "Biology Notebook":
                this.notebooColor = new Color(0.59f, 0f, 1f, 1f);
                break;
            default:
                this.notebooColor = new Color(1f, 1f, 1f, 1f);
                break;
        }

        this.StartCoroutine(this.NotebooRoutine(noteName, notebooColor));
    }

    private IEnumerator NotebooRoutine(string notename, Color color)
    {
        if (PlayerPrefs.GetInt("NotifBoard") == 1)
            this.notebooGroup.SetActive(true);
        else
            yield break;

        this.notebooText.text = notename;
        this.notebooText.color = color;

        float time = 3f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        this.notebooGroup.SetActive(false);
    }

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private TMP_Text ruleText;
    [SerializeField] private TMP_Text detentionText;
    [SerializeField] private TMP_Text notebooText;
    [SerializeField] private GameObject notebooGroup;
    [SerializeField] private GameObject detentionGroup;
    [SerializeField] private GameObject ruleGroup;
    [SerializeField] private Color notebooColor;
}
