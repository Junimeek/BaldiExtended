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
        detentionTextScript = FindObjectOfType<DetentionTextScript>();
        doorScript = FindObjectOfType<DoorScript>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.board.SetActive(false);
        this.text.text = string.Empty;
    }

    // Update is called once per frame
    private void Update()
    {
        if (detentionTextScript.door.lockTime > 0f)
		{
			this.text.text = "You have detention! \n" + Mathf.CeilToInt(detentionTextScript.door.lockTime) + " seconds remain!";
		}
        else if (isBoardActive == false)
        {
            this.board.SetActive(false);
            this.text.text = string.Empty;
        }
        else if (isBoardActive == true)
        this.board.SetActive(true);
    }

    public void RuleText(int rule)
    {
        isBoardActive = true;

        if (rule == 1)
        {
            this.text.text = "You have broken a rule! \n \"No entering faculty!\"";
        }
        else if (rule == 2)
        {
            this.text.text = "You have broken a rule! \n \"No running!\"";
        }
        else if (rule == 3)
        {
            this.text.text = "You have broken a rule! \n \"No drinking drinks!\"";
        }
        else if (rule == 4)
        {
            this.text.text = "You have broken a rule! \n \"No escaping detention!\"";
        }
    }

    public void startBoardRoutine()
    {
        StartCoroutine(deactivateBoard());
    }

    public IEnumerator deactivateBoard()
    {
        while (doorScript.lockTime > 0f)
        {
            yield return null;
        }

        this.isBoardActive = false;
    }

    private PrincipalScript princey;
    private PlayerScript player;
    private GameControllerScript gc;
    private DetentionTextScript detentionTextScript;
    private DoorScript doorScript;
    [SerializeField] private GameObject board;
    [SerializeField] private TMP_Text text;
    [SerializeField] private bool isBoardActive;
}
