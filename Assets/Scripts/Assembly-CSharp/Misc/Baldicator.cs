using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baldicator : MonoBehaviour
{
    void Start()
    {
        animator.SetTrigger("None");
    }

    public void BaldicatorStateSwitch(string state)
    {
        StartCoroutine(Attention(state));
    }

    private IEnumerator Attention(string state)
    {
        animator.ResetTrigger(state);
        animator.SetTrigger("Attention");
        animator.ResetTrigger("None");

        attentionRem = 1f;

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
        animator.ResetTrigger("Attention");
        animator.SetTrigger(state);

        faceRem = 1.5f;

        while (faceRem > 0f)
        {
            faceRem -= 1f * Time.deltaTime;
            yield return null;
        }

        animator.ResetTrigger(state);
        animator.SetTrigger("None");

        StopCoroutine(Direction(state));
    }

    public Animator animator;
    [SerializeField] private float attentionRem;
    [SerializeField] private float faceRem;
}
