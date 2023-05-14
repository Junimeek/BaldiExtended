using UnityEngine;
using UnityEngine.SceneManagement;

public class BadSumTeleporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadSceneAsync("BAD SUM");
    }
    
    [SerializeField] private Collider badsumCollider;
}
