using UnityEngine;
using UnityEngine.SceneManagement;

public class BadSumTeleporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        sceneLoader.LoadTheScene("BAD SUM", 0);
    }
    
    [SerializeField] private DebugSceneLoader sceneLoader;
}
