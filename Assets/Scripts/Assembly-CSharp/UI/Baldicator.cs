using System;
using System.Collections;
using UnityEngine;

public class Baldicator : MonoBehaviour
{
    private void Start()
    {
        ChangeAnimState(none);
    }

    private void ChangeAnimState(string newState)
    {
        if (this.curState == newState && (this.curState == "Pursuit" || this.curState == "Ignore"))
            return;

        this.animator.Play(newState);
        this.curState = newState;
    }

    public void ChangeBaldicatorState(string state)
    {
        if (state == "Pursuit" || state == "Ignore")
        {
            this.attentionRem = -1f;
            this.decideRem = -1f;
            this.sightRem = -1f;
            StopAllCoroutines();

            StartCoroutine(this.Attention(state));
        }
        else if (state == "Sight" || state == "Next" || state == "End")
        {
            try {
                if (this.queue[this.queue.Length - 1] == state)
                    return;
                else
                    this.AddToQueue(state);
            }
            catch {
                this.AddToQueue(state);
            }
        }
    }

    private void AddToQueue(string state)
    {
        Debug.Log(state + " added to queue");
        Array.Resize(ref this.queue, this.queue.Length + 1);
        this.queue[this.queue.Length - 1] = state;
    }

    private void LateUpdate()
    {
        if (this.queue.Length > 0 && this.curState == none && !this.interruptQueue)
        {
            StartCoroutine(this.Sight(this.queue[0]));

            for (int i = 0; i < this.queue.Length; i++)
            {
                try {
                    this.queue[i] = this.queue[i + 1];
                }
                catch {
                    this.queue[i] = string.Empty;
                }
            }
            Array.Resize(ref this.queue, this.queue.Length - 1);
        }
    }

    private void ClearQueue()
    {
        for (int i = 0; i < this.queue.Length; i++)
            this.queue[i] = string.Empty;
        
        Array.Resize(ref this.queue, 0);
    }

    private IEnumerator Attention(string state)
    {
        this.interruptQueue = true;
        this.ClearQueue();
        this.ChangeAnimState(none);
        
        float quickPause = 0.1f;

        while (quickPause > 0f)
        {
            quickPause -= Time.deltaTime;
            yield return null;
        }

        this.ChangeAnimState(attention);
        this.interruptQueue = false;

        attentionRem = 0.9f;

        while (attentionRem > 0f)
        {
            attentionRem -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(this.Decide(state));
    }

    private IEnumerator Decide(string state)
    {
        decideRem = 1.2f;
        
        if (state == "Pursuit")
            ChangeAnimState(pursuit);
        else if (state == "Ignore")
            ChangeAnimState(ignore);

        while (decideRem > 0f)
        {
            decideRem -= Time.deltaTime;
            yield return null;
        }

        ChangeAnimState(none);
    }

    private IEnumerator Sight(string state)
    {
        sightRem = 1.3f;

        switch(state)
        {
            case "Sight":
                ChangeAnimState(sight);
                break;
            case "Next":
                ChangeAnimState(next);
                break;
            case "End":
                ChangeAnimState(end);
                break;
        }

        while (sightRem > 0f)
        {
            sightRem -= Time.deltaTime;
            yield return null;
        }

        ChangeAnimState(none);
    }

    [SerializeField] private Animator animator;
    [SerializeField] private string curState;
    [SerializeField] private float attentionRem;
    [SerializeField] private float decideRem;
    [SerializeField] private float sightRem;
    [SerializeField] private string[] queue;
    [SerializeField] private bool interruptQueue;

    // Animation States
    const string none = "Baldicator_none";
    const string attention = "Baldicator_Attention";
    const string pursuit = "Baldicator_Pursuit";
    const string ignore = "Baldicator_Ignore";
    const string sight = "Baldicator_Sight";
    const string next = "Baldicator_Next";
    const string end = "Baldicator_End";
}
