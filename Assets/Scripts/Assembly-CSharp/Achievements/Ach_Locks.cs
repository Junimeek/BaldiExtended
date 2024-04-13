using System;
using UnityEngine;
using UnityEngine.UI;

public class Ach_Locks : MonoBehaviour
{
    [SerializeField] private Image[] locks;
    [SerializeField] private int[] index;
    [SerializeField] private AchievementManager achievementManager;

    private void Start()
    {
        achievementManager = FindObjectOfType<AchievementManager>();
        locks = GetComponentsInChildren<Image>();

        Array.Resize<int>(ref this.index, this.locks.Length+1);

        for(int i = 0; i < this.index.Length; i++) index[i] = i;
    }

    private void OnEnable()
    {
        GetAchievements();
    }

    public void GetAchievements()
    {
        int foundLocks;
        foundLocks = 0;
        
        for(int i = 0; i < this.locks.Length; i++)
        {
            if (!Array.Exists<int>(achievementManager.ach_Maps, helpme => achievementManager.ach_Maps[foundLocks] == index[i+1]))
                continue;
            else
            {
                try
                {
                    this.locks[i].color = new Color(1f, 1f, 1f, 0f);
                    foundLocks++;
                }
                catch
                {
                    Debug.LogError("Failed to get " + (i+1));
                }
            }
        }
    }
}
