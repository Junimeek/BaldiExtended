using System.Collections;
using UnityEngine;

public class NotebookCountScript : MonoBehaviour
{
    void Start()
    {
        animator.Play(idle);
    }

    public void FlipNotebooks()
    {
        StartCoroutine(NotebookFlip());
    }

    IEnumerator NotebookFlip()
    {
        float delay = 0.5f;
        animator.Play(flip);

        while (delay >= 0f)
        {
            delay -= Time.deltaTime;
            yield return null;
        }

        animator.Play(idle);
    }

    [SerializeField] Animator animator;
    const string flip = "NotebookFlip";
    const string idle = "NotebookIdle";
}
