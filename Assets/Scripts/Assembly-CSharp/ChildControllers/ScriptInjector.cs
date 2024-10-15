using System.Collections;
using UnityEngine;

public class ScriptInjector : MonoBehaviour
{

    private void Awake()
    {
        if (this.isOverridesEnabled)
        {
            this.player.OverrideValues(this);
            StartCoroutine(this.DelayValueSetting());
        }
    }

    private IEnumerator DelayValueSetting()
    {
        float delay = 1f;
        while (delay > 0f)
        {
            delay -= Time.unscaledDeltaTime;
            yield return null;
        }

        this.gc.isSafeMode = this.safeMode;
        this.gc.mode = this.mode;

        if (this.overrideNotebooks)
            this.gc.notebooks = 2;
    }

    [SerializeField] private GameControllerScript gc;
    [SerializeField] private PlayerScript player;
    [SerializeField] private GameObject AchievementController;

    [Header("Gameplay Overrides")]
    [SerializeField] bool isOverridesEnabled;
    [Space(20)]
    [SerializeField] private bool overrideNotebooks;
    [SerializeField] private bool safeMode;
    [SerializeField] private string mode;
    public Vector3 inj_playerSpeed;
}
