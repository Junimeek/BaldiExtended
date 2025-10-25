using System.Collections;
using UnityEngine;

public class AchievementMonitor : MonoBehaviour
{
    private void Start()
    {
        this.ac = Instantiate(this.achievementInstance).transform.Find("Bar").GetComponent<AchievementController>();
    }

    public void CollectAchievement(byte type, ushort id)
    {
        this.ac.CheckAchievement(type, id);
    }

    [SerializeField] private AchievementController ac;
    [SerializeField] private GameObject achievementInstance;
}
