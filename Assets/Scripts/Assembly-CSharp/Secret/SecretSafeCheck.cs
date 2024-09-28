using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretSafeCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            int thebool = PlayerPrefs.GetInt("gps_safemode");
            if (thebool == 1) Switcharoo();
            else
            {
                Destroy(this.balChar);
                Destroy(this.oink);
                Destroy(this.gameObject);
            }
        }
    }

    private void Switcharoo()
    {
        Debug.Log("Safe mode is enabled, switching to Baldi.");
        Destroy(this.nullChar);
        Destroy(this.balStretched);
        Destroy(this.gameObject);
    }

    [SerializeField] private GameObject nullChar;
    [SerializeField] private GameObject balChar;
    [SerializeField] private GameObject balStretched;
    [SerializeField] private GameObject oink;
}
