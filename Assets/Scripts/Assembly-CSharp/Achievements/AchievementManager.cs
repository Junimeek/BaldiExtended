using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Runtime.CompilerServices;

public class AchievementManager : MonoBehaviour
{
    
    public int[] ach_Maps;
    [SerializeField] private string dataPath;

    private void Awake()
    {
        this.dataPath = Application.persistentDataPath + "/achievements.sav";
    }

    public void SaveNewAchievement(int map)
    {
        
        Array.Resize<int>(ref this.ach_Maps, this.ach_Maps.Length+1);
        this.ach_Maps[this.ach_Maps.Length-1] = map;
        this.SaveAchievementData();
    }

    public void GetAchievements()
    {
        //Array.Find<int>(ref this.ach_Maps, )
    }

    public void SaveAchievementData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(this.dataPath);
        AchievementData data = new AchievementData();
        data.ach_Maps = this.ach_Maps;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("saved");
    }

    public void LoadAchievementData()
    {
        if (File.Exists(this.dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(this.dataPath, FileMode.Open);
            AchievementData data = (AchievementData)bf.Deserialize(file);
            file.Close();
            this.ach_Maps = data.ach_Maps;
            Debug.Log("LAOD");
        }
        else Debug.LogError("No Achievements Found");
    }
}
