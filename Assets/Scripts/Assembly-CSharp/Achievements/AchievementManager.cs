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
    public int[] ach_Misc;
    [SerializeField] private string dataPath;
    [SerializeField] private static AchievementManager instance;
    [SerializeField] private AudioClip wow;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        this.dataPath = Application.persistentDataPath + "/achievements.sav";
        this.audioSource = GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        
    }

    public void GiveAchievement(string type, int id)
    {
        switch(type) // some function i dont understand
        {
            case "misc":
                if (!Array.Exists<int>(this.ach_Misc, h => h == id))
                {
                    CollectAchievement(type, id);
                }
            break;
        }
    }

    public void CollectAchievement(string type, int id)
    {
        switch(type)
        {
            case "misc":
                Array.Resize<int>(ref this.ach_Misc, this.ach_Misc.Length+1);
                this.ach_Misc[this.ach_Misc.Length-1] = id;
                Array.Sort(this.ach_Misc);
            break;
            case "map":
                Array.Resize<int>(ref this.ach_Maps, this.ach_Maps.Length+1);
                this.ach_Maps[this.ach_Maps.Length-1] = id;
                Array.Sort(this.ach_Maps);
            break;
        }

        this.SaveAchievementData();
        this.audioSource.PlayOneShot(this.wow);
    }

    public void AddNewAchievement()
    {
        
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
            Array.Sort(this.ach_Maps);
            Debug.Log("LAOD");
        }
        else Debug.LogError("No Achievements Found");
    }
}
