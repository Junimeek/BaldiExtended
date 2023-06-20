using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationBoard : MonoBehaviour
{
    private void Awake()
    {
        princey = FindObjectOfType<PrincipalScript>();
        gc = FindObjectOfType<GameControllerScript>();
        doorScript = FindObjectOfType<DoorScript>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.ruleText.text = string.Empty;
        this.detentionText.text = string.Empty;
        notebooText.text = string.Empty;

        this.notebooGroup.SetActive(false);
        this.detentionGroup.SetActive(false);
        this.ruleGroup.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.doorScript != null)
        {
            if ((this.doorScript.lockTime > 0f))
		    {
			    this.detentionText.text = "You have detention! \n" + Mathf.CeilToInt(doorScript.lockTime) + " seconds remain!";
		    }
        }
    }

    public void RuleText(int rule)
    {
        if (PlayerPrefs.GetInt("NotifBoard") == 1) this.ruleGroup.SetActive(true);

        if (rule == 1)
        {
            this.ruleText.text = "You have broken a rule! \n \"No entering faculty!\"";
        }
        else if (rule == 2)
        {
            this.ruleText.text = "You have broken a rule! \n \"No running!\"";
        }
        else if (rule == 3)
        {
            this.ruleText.text = "You have broken a rule! \n \"No drinking drinks!\"";
        }
        else if (rule == 4)
        {
            this.ruleText.text = "You have broken a rule! \n \"No escaping detention!\"";
        }
    }

    public void DetentionBoardRoutine()
    {
        StartCoroutine(DeactivateDetentionBoard());
    }

    public IEnumerator DeactivateDetentionBoard()
    {
        this.ruleGroup.SetActive(false);

        this.detentionGroup.SetActive(true);

        while (doorScript.lockTime > 0f)
        {
            yield return null;
        }

        this.detentionGroup.SetActive(false);
    }

    public void StartNotebooRoutine(string notename)
    {
        if (notename == "Math Notebook")
            notebooColor = new Color(0.75f, 0.52f, 0.54f, 1f);
        else if (notename == "Spelling Notebook")
            notebooColor = new Color(0.12f, 0.15f, 0.75f, 1f);
        else if (notename == "English Notebook")
            notebooColor = new Color(0.11f, 0.75f, 0.12f, 1f);
        else if (notename == "History Notebook")
            notebooColor = new Color(0.75f, 0.11f, 0.11f, 1f);
        else if (notename == "Geography Notebook")
            notebooColor = new Color(0.75f, 0.75f, 0.12f, 1f);
        else if (notename == "Geology Notebook")
            notebooColor = new Color(0.07f, 0.75f, 0.75f, 1f);
        else if (notename == "Culinary Notebook")
            notebooColor = new Color(0.42f, 0.31f, 0.12f, 1f);
        else if (notename == "Culture Notebook")
            notebooColor = new Color(1f, 0f, 1f, 1f);
        else if (notename == "Art Notebook")
            notebooColor = new Color(0.42f, 0.12f, 0.12f, 1f);
        else if (notename == "Chemistry Notebook")
            notebooColor = new Color(0.96f, 0.54f, 0f, 1f);
        else if (notename == "Biology Notebook")
            notebooColor = new Color(0.59f, 0f, 1f, 1f);
        else notebooColor = new Color(1f, 1f, 1f, 1f);

        StartCoroutine(NotebooRoutine(notename, notebooColor));
    }

    private IEnumerator NotebooRoutine(string notename, Color color)
    {
        if (PlayerPrefs.GetInt("NotifBoard") == 1) this.notebooGroup.SetActive(true);
        else StopCoroutine(NotebooRoutine("Science Notebook", new Color(1f, 1f, 1f, 1f)));

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

    private PrincipalScript princey;
    private GameControllerScript gc;
    [SerializeField] private DoorScript doorScript;
    [SerializeField] private TMP_Text ruleText;
    [SerializeField] private TMP_Text detentionText;
    [SerializeField] private TMP_Text notebooText;
    [SerializeField] private GameObject notebooGroup;
    [SerializeField] private GameObject detentionGroup;
    [SerializeField] private GameObject ruleGroup;
    [SerializeField] private Color notebooColor;
}
