using System.Collections;
using UnityEngine;

public class SaveHead : MonoBehaviour
{
    private void Start()
    {
        this.chalkboard.SetActive(false);
        this.saveHead.Play("none");
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    public void ActivateSaveHead(float length)
    {
        StartCoroutine(this.SaveRoutine(length));
    }

    private IEnumerator SaveRoutine(float length)
    {
        this.chalkboard.SetActive(true);
        this.saveHead.Play("nod");

        while (length > 0f)
        {
            length -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.chalkboard.SetActive(false);
        this.saveHead.Play("none");
    }

    [SerializeField] private Animator saveHead;
    [SerializeField] private GameObject chalkboard;
    [SerializeField] private static SaveHead instance;
}
