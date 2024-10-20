using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementController : MonoBehaviour
{
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void CollectAchievement(byte type, ushort id)
    {
        if (this.queue.Length == 0)
            StartCoroutine(this.PlayAchievementQueue());
        
        this.QueueAchievement(type, id);
    }

    private void QueueAchievement(byte type, ushort id)
    {
        Array.Resize(ref this.queue, this.queue.Length + 1);
        this.queue[^1] = new Vector2(type, id);
    }

    private IEnumerator PlayAchievementQueue()
    {
        string state = "start";
        float delay = 2f;

        switch(state)
        {
            case "start":
                this.DisplayAchievement(this.queue[0]);
                goto case "delay";
            
            case "checkQueue":
                if (this.queue.Length > 0)
                {
                    for (int i = 0; i < this.queue.Length; i++)
                    {
                        try {
                            this.queue[i] = this.queue[i + 1];
                        }
                        catch {
                            this.DisplayAchievement(this.queue[0]);
                            goto case "delay";
                        }
                    }
                }
                break;

            case "delay":
                delay = 2f;
                while (delay > 0f)
                {
                    delay -= Time.unscaledDeltaTime;
                    yield return null;
                }
                goto case "checkQueue";
        }
    }

    private void DisplayAchievement(Vector2 unlock)
    {
        int type = Mathf.FloorToInt(unlock.x);
        int id = Mathf.FloorToInt(unlock.y);
        string title = "";
        string description = "";
        Sprite iconToShow = null;

        if (type == 1) // Story Mode Completion
        {
            iconToShow = this.storyIcons[id];
            this.data_storyCompletion[id] = true;

            switch(id)
            {
                case 0:
                    title = "What a Classic!";
                    description = "Beat the Classic map.";
                    break;
                case 1:
                    title = "What an Extended Classic!";
                    description = "Beat the Classic Extended map.";
                    break;
                case 2:
                    title = "But where are the Hills, anyway?";
                    description = "Beat the Juniper Hills map.";
                    break;
            }
        }
        else if (type == 2) // Endless Mode Milestones
        {
            iconToShow = this.endlessIcons[id];

            switch(id)
            {
                case 0:
                    title = "Classic Notebook Master!";
                    description = "Collect 20 notebooks in the Classic map.";
                    break;
                case 1:
                    title = "Extended Notebook Master!";
                    description = "Collect 25 notebooks in the Classic Extended map.";
                    break;
                case 2:
                    title = "Notebook Master of the Hills!";
                    description = "Collect 20 notebooks in the Juniper Hills map.";
                    break;
            }
        }
        else if (type == 3) // Challenge Mode Completion
        {
            iconToShow = this.challengeIcons[id];

            switch(id)
            {
                case 0:
                    title = "The Darkest Day";
                    description = "Beat the Dark Mode challenge.";
                    break;
            }
        }
        else if (type == 4) // Mini-Challenges
        {
            iconToShow = this.miniChallengeIcons[id];

            switch(id)
            {
                case 0:
                    title = "Looks like it's Sweeping Time!";
                    description = "Get swept by Gotta Sweep for 30 seconds straight.";
                    break;
                case 1:
                    title = "Let's Play!";
                    description = "Play with Playtime 5 times in a single game.";
                    break;
                case 2:
                    title = "Doork";
                    description = "Alert Baldi with a door 99 times in a single game.";
                    break;
            }
        }

        this.iconRenderer.sprite = iconToShow;
        this.titleText.text = title;
        this.descriptionText.text = description;
    }

    private static AchievementController instance;

    [Header("Achievement Statse")]
    [SerializeField] private Vector2[] queue;

    [Header("UI")]
    [SerializeField] private Image iconRenderer;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Sprite[] storyIcons;
    [SerializeField] private Sprite[] endlessIcons;
    [SerializeField] private Sprite[] challengeIcons;
    [SerializeField] private Sprite[] miniChallengeIcons;
    [SerializeField] private Sprite[] secretIcons;

    [Header("Memory")]
    public ushort data_fileVersion;
    public bool[] data_storyCompletion;
    public bool[] data_endlessMilestones;
    public bool[] data_challengeCompletion;
    public bool[] data_miniChallenges;
    public bool[] data_secretUnlocks;
}
