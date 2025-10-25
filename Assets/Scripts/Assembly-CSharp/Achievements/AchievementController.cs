using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementController : MonoBehaviour
{
    private void Start()
    {
        this.border.color = new Color(1f, 1f, 1f, 0f);
        this.iconRenderer.color = new Color(1f, 1f, 1f, 0f);
        this.titleText.text = string.Empty;
        this.descriptionText.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            this.CollectAchievement(1, 0);
        }
    }

    public void CheckAchievement(byte type, ushort id)
    {
        this.CollectAchievement(type, id);
    }

    public void CollectAchievement(byte type, ushort id)
    {
        this.QueueAchievement(type, id);

        if (!this.isQueuePlaying)
            StartCoroutine(this.PlayAchievementQueue());
    }

    private void QueueAchievement(byte type, ushort id)
    {
        Array.Resize(ref this.queue, this.queue.Length + 1);
        this.queue[^1] = new Vector2(type, id);
    }

    private IEnumerator PlayAchievementQueue()
    {
        this.isQueuePlaying = true;
        string state = "start";
        float delay;

        switch(state)
        {
            case "start":
                this.initialDelay = this.GetAchievementDelay(this.queue[0].x);
                delay = 0.2f;
                while (delay > 0f)
                {
                    delay -= Time.unscaledDeltaTime;
                    yield return null;
                }
                goto case "checkInfo";
            
            case "checkQueue":
                if (this.queue.Length > 0)
                {
                    for (int i = 0; i < this.queue.Length; i++)
                    {
                        try {
                            this.queue[i] = this.queue[i + 1];
                        }
                        catch {
                            Array.Resize(ref this.queue, this.queue.Length - 1);
                            goto case "checkInfo";
                        }
                    }
                }
                break;
            
            case "checkInfo":
                if (this.queue.Length == 0)
                    goto case "hide";
                
                this.achievementColor = this.GetAchievementColor(this.queue[0].x);
                this.initialDelay = this.GetAchievementDelay(this.queue[0].x);

                this.DisplayAchievement(this.queue[0]);
                goto case "delay";

            case "delay":
                delay = this.initialDelay;
                while (delay > 0f)
                {
                    delay -= Time.unscaledDeltaTime;
                    yield return null;
                }
                goto case "checkQueue";
            
            case "hide":
                this.isQueuePlaying = false;
                StartCoroutine(this.Flash());
                this.border.color = new Color(1f, 1f, 1f, 0f);
                this.iconRenderer.color = new Color(1f, 1f, 1f, 0f);
                this.iconRenderer.sprite = this.placeholderSprite;
                this.titleText.text = string.Empty;
                this.descriptionText.text = string.Empty;
                yield break;
        }
    }

    private Color GetAchievementColor(float type)
    {
        switch(type)
        {
            case 1:
                return new Color(0.77f, 0.42f, 0.14f, 1f);
            case 2:
                return new Color(0.79f, 0f, 0f, 1f);
            case 3:
                return new Color(0f, 1f, 0f, 1f);
            case 4:
                return new Color(1f, 0.55f, 0.79f, 1f);
            default:
                return new Color(1f, 1f, 1f, 1f);
        }
    }

    private float GetAchievementDelay(float id)
    {
        switch(id)
        {
            case 1f:
                return 4f;
            default:
                return 4f;
        }
    }

    private void DisplayAchievement(Vector2 unlock)
    {
        int type = Mathf.FloorToInt(unlock.x);
        int id = Mathf.FloorToInt(unlock.y);
        string title = "";
        string description = "";
        Sprite iconToShow = this.placeholderSprite;

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
                    title = "Classic+";
                    description = "Beat the Classic Extended map.";
                    break;
                case 2:
                    title = "Where were the Hills, anyway?";
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
                    description = "Play with Playtime 10 times in a single game.";
                    break;
                case 2:
                    title = "Doork";
                    description = "Alert Baldi with a door 99 times in a single game.";
                    break;
                case 3:
                    title = "Suspension for you!";
                    description = "Get 99 seconds detention.";
                    break;
            }
        }

        StartCoroutine(this.Flash());

        this.iconRenderer.sprite = iconToShow;
        this.iconRenderer.color = new Color(1f, 1f, 1f, 1f);
        this.border.color = this.achievementColor;
        this.titleText.text = title;
        this.descriptionText.text = description;
        this.audioDevice.PlayOneShot(this.fanfares[Mathf.RoundToInt(unlock.x) - 1]);
    }

    private IEnumerator Flash()
    {
        this.flasher.color = new Color(1f, 1f, 1f, 1f);
        float alpha = 1f;

        while (alpha > 0f)
        {
            this.flasher.color = new Color(1f, 1f, 1f, alpha);
            alpha -= Time.unscaledDeltaTime * 3f;
            yield return null;
        }

        this.flasher.color = new Color(1f, 1f, 1f, 0f);
    }

    private static AchievementController instance;

    [Header("Achievement State")]
    [SerializeField] private Vector2[] queue;
    [SerializeField] private Color achievementColor;
    [SerializeField] private float initialDelay;
    [SerializeField] private bool isQueuePlaying;
 
    [Header("UI")]
    [SerializeField] private Image border;
    [SerializeField] private Image iconRenderer;
    [SerializeField] private Image flasher;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private AudioClip[] fanfares;
    [SerializeField] private Sprite placeholderSprite;
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
