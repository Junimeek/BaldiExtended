using System.Collections;
using UnityEngine;

public class Baldicator : MonoBehaviour
{
    private void Start()
    {
        allowSightTrigger = true;
        ChangeAnimState(none);
    }

    private void ChangeAnimState(string newState)
    {
        if (curState == newState)
        {
            if (curState == "Pursuit" || curState == "Ignore") return;
        }

        animator.Play(newState);
        curState = newState;
    }

    public void ChangeBaldicatorState(string state)
    {
        if (state == "Pursuit" || state == "Ignore")
        {
            attentionRem = -1f;
            decideRem = -1f;
            sightRem = -1f;
            StopAllCoroutines();

            StartCoroutine(Attention(state));
        }
        else if ((state == "Sight" || state == "Next" || state == "End") && allowSightTrigger)
        {
            StopAllCoroutines();
            ChangeAnimState(none);

            StartCoroutine(Sight(state));
        }
    }

    private IEnumerator Attention(string state)
    {
        ChangeAnimState(none);
        allowSightTrigger = false;
        
        float quickPause = 0.1f;

        while (quickPause > 0f)
        {
            quickPause -= Time.deltaTime;
            yield return null;
        }

        ChangeAnimState(attention);

        attentionRem = 0.9f;

        while (attentionRem > 0f)
        {
            attentionRem -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Decide(state));
        StopCoroutine(Attention(state));
    }

    private IEnumerator Decide(string state)
    {
        decideRem = 1.2f;
        
        if (state == "Pursuit") ChangeAnimState(pursuit);
        else if (state == "Ignore") ChangeAnimState(ignore);

        while (decideRem > 0f)
        {
            decideRem -= Time.deltaTime;
            yield return null;
        }

        ChangeAnimState(none);
        allowSightTrigger = true;
        StopCoroutine(Decide(state));
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
        StopCoroutine(Sight(state));
    }

    [SerializeField] private Animator animator;
    [SerializeField] private string curState;
    [SerializeField] private bool allowSightTrigger;
    [SerializeField] private float attentionRem;
    [SerializeField] private float decideRem;
    [SerializeField] private float sightRem;

    // Animation States
    const string none = "Baldicator_none";
    const string attention = "Baldicator_Attention";
    const string pursuit = "Baldicator_Pursuit";
    const string ignore = "Baldicator_Ignore";
    const string sight = "Baldicator_Sight";
    const string next = "Baldicator_Next";
    const string end = "Baldicator_End";
}
