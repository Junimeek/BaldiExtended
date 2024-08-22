using System.Collections;
using UnityEngine;

public class EmergencyLightScript : MonoBehaviour
{
    private void Start()
    {
        this.gc = FindObjectOfType<GameControllerScript>();
        this.StartCoroutine(this.WaitForBattle());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Baldi")
        {
            for (int i = 0; i < this.lights.Length; i++)
                this.lights[i].color = Color.black;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Baldi")
        {
            for (int i = 0; i < this.lights.Length; i++)
                this.lights[i].color = Color.white;
        }
    }

    private IEnumerator WaitForBattle()
    {
        float delay = 1f;
        while (delay > 0f)
        {
            delay -= Time.deltaTime;
            yield return null;
        }

        while (this.gc.isDynamicColor)
            yield return null;
        
        for (int i = 0; i < this.lights.Length; i++)
            this.lights[i].intensity = 0f;
    }

    [SerializeField] private Light[] lights;
    private GameControllerScript gc;
}
