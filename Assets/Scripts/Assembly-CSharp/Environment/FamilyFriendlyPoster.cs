using UnityEngine;

public class FamilyFriendlyPoster : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("gps_familyFriendly") == 1)
            this.meshRenderer.material = this.texture;
        
        Destroy(this);
    }

    [SerializeField] private Material texture;
    [SerializeField] private MeshRenderer meshRenderer;
}
