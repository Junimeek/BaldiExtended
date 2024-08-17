using UnityEngine;

public class EmergencyLightScript : MonoBehaviour
{
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

    [SerializeField] private Light[] lights;
}
