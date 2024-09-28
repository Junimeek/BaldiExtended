using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldicatorOld : MonoBehaviour
{
    /*
    void Start()
    {
        animator.SetTrigger("None");
        AllowSightTrigger = true;
    }

    public void BaldicatorStateSwitch(string state)
    {
        StopAllCoroutines();
        ResetAllTriggers();

        if (state == "Sight") StartCoroutine(PlayerSight());
        else StartCoroutine(Attention(state));
    }

    private void ResetAllTriggers()
    {
        animator.ResetTrigger("None");
        animator.ResetTrigger("Attention");
        animator.ResetTrigger("Pursuit");
        animator.ResetTrigger("Ignore");
        animator.ResetTrigger("Sight");
    }

    private IEnumerator Attention(string state)
    {
        animator.SetTrigger("None");
        AllowSightTrigger = false;

        attentionRem = 1f;
        animator.SetTrigger("Attention");

        while (attentionRem > 0f)
        {
            attentionRem -= 1f * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Direction(state));
        StopCoroutine(Attention(state));
    }

    private IEnumerator Direction(string state)
    {
        faceRem = 1.5f;
        animator.SetTrigger(state);

        while (faceRem > 0f)
        {
            faceRem -= 1f * Time.deltaTime;
            yield return null;
        }

        ResetAllTriggers();
        animator.SetTrigger("None");
        AllowSightTrigger = true;

        StopCoroutine(Direction(state));
    }

    private IEnumerator PlayerSight()
    {
        if (!AllowSightTrigger)
        {
            StopCoroutine(PlayerSight());
        }

        animator.SetTrigger("Sight");
        float sightRem = 1.5f;

        while (sightRem > 0f)
        {
            sightRem -= Time.deltaTime;
            yield return null;
        }

        ResetAllTriggers();
        animator.SetTrigger("None");
        StopCoroutine(PlayerSight());
    }

    public Animator animator;
    public BaldiScript baldi;
    private float attentionRem;
    private float faceRem;
    [SerializeField] private bool AllowSightTrigger;
    */
}
