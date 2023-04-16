using System.Collections;
using System.Collections.Generic;
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
        else if (state == "Sight" && allowSightTrigger)
        {
            StopAllCoroutines();
            ChangeAnimState(none);

            StartCoroutine(Sight());
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

        attentionRem = 1f;

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
        if (state == "Pursuit") ChangeAnimState(pursuit);
        else if (state == "Ignore") ChangeAnimState(ignore);

        decideRem = 1.5f;

        while (decideRem > 0f)
        {
            decideRem -= Time.deltaTime;
            yield return null;
        }

        ChangeAnimState(none);
        allowSightTrigger = true;
        StopCoroutine(Decide(state));
    }

    private IEnumerator Sight()
    {
        ChangeAnimState(sight);

        sightRem = 1.5f;

        while (sightRem > 0f)
        {
            sightRem -= Time.deltaTime;
            yield return null;
        }

        ChangeAnimState(none);
        StopCoroutine(Sight());
    }




    [SerializeField] private Animator animator;
    [SerializeField] private string curState;
    [SerializeField] private BaldiScript baldi;
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
}
