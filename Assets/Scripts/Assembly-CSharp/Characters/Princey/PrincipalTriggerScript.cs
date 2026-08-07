using System.Collections;
using UnityEngine;

public class PrincipalTriggerScript : MonoBehaviour
{
    private void Start()
    {
        this.isTriggerShared = false;
        this.gc = FindObjectOfType<GameControllerScript>();
    }

    private void FixedUpdate()
    {
        if (this.isPlayer && this.isPrincey)
            this.isTriggerShared = true;
        else
            this.isTriggerShared = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.isPlayer = true;
            if (this.isPrincey == true)
                StartCoroutine(TriggerStay());

            if (this.isProtected)
                StartCoroutine(IgnoreTriggerStay());
            
        }
        if (other.gameObject.name == "Principal of the Thing")
        {
            this.isPrincey = true;
            if (this.isPlayer == true)
                StartCoroutine(TriggerStay());

            if (this.isProtected)
                StartCoroutine(IgnoreTriggerStay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.isPlayer = false;
            if (this.isProtected)
                gc.isPrinceyIgnore = false;
        }
            
        if (other.gameObject.name == "Principal of the Thing")
            this.isPrincey = false;
    }

    private IEnumerator TriggerStay()
    {
        gc.UpdatePrinceyTrigger(1, true);
        
        while (this.isPlayer && this.isPrincey)
        {
            gc.UpdatePrinceyTrigger(1, true);
            yield return null;
        }

        gc.UpdatePrinceyTrigger(1, false);
    }

    private IEnumerator IgnoreTriggerStay()
    {
        gc.UpdatePrinceyTrigger(2, true);

        while (this.isPlayer || this.isPrincey)
        {
            gc.UpdatePrinceyTrigger(2, true);
            yield return null;
        }

        gc.UpdatePrinceyTrigger(2, false);
    }

    public bool isProtected;
    public bool isTriggerShared;
    [SerializeField] bool isPlayer;
    [SerializeField] bool isPrincey;
    [SerializeField] GameControllerScript gc;
}
