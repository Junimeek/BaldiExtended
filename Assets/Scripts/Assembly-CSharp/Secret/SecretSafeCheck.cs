using UnityEngine;
using UnityEngine.UI;

public class SecretSafeCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int thebool = PlayerPrefs.GetInt("gps_safemode");
            if (thebool == 1)
                Switcharoo();
            else
            {
                Destroy(this.balChar);
                Destroy(this.oink);
            }
            Destroy(base.gameObject);
        }
    }

    void Switcharoo()
    {
        Destroy(this.nullChar);
        Destroy(this.balStretched);
        
        for (int i = 0; i < this.signRenderers.Length; i++)
        {
            this.signRenderers[i].sprite = this.signImages[i];
        }
    }

    [SerializeField] GameObject nullChar;
    [SerializeField] GameObject balChar;
    [SerializeField] GameObject balStretched;
    [SerializeField] GameObject oink;
    [SerializeField] SpriteRenderer[] signRenderers;
    [SerializeField] Sprite[] signImages;
}
