using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveDataController
{
    public static void UpgradeSaves(string type, int fileVersion)
    {
        PlayerPrefs.SetInt("highbooks_Classic", 4);
        PlayerPrefs.SetInt("highbooks_ClassicExtended", 3);
        PlayerPrefs.SetInt("highbooks_JuniperHills", 2);
        
        if (File.Exists(Application.persistentDataPath + "/BaldiData/story.sav"))
            File.Copy(Application.persistentDataPath + "/BaldiData/story.sav", Application.persistentDataPath + "/BaldiData_Backup/story.sav");
        if (File.Exists(Application.persistentDataPath + "/BaldiData/endless.sav"))
            File.Copy(Application.persistentDataPath + "/BaldiData/endless.sav", Application.persistentDataPath + "/BaldiData_Backup/endless.sav");
        if (File.Exists(Application.persistentDataPath + "/BaldiData/challenge.sav"))
            File.Copy(Application.persistentDataPath + "/BaldiData/challenge.sav", Application.persistentDataPath + "/BaldiData_Backup/challenge.sav");
    }

    public static void SaveStoryData(StatisticsController stats)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/BaldiData/story.sav";
        FileStream file = File.Create(path);
        SaveData_Story data = new SaveData_Story(stats);
        data.fileVersion = 1;
        bf.Serialize(file, data);
        file.Close();
    }

    public static SaveData_Story LoadStoryData()
    {
        string path = Application.persistentDataPath + "/BaldiData/story.sav";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        SaveData_Story data = bf.Deserialize(file) as SaveData_Story;
        file.Close();

        return data;
    }

    public static void SaveEndlessData(StatisticsController stats)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/BaldiData/endless.sav";
        FileStream file = File.Create(path);
        SaveData_Endless data = new SaveData_Endless(stats);
        data.fileVersion = 1;
        bf.Serialize(file, data);
        file.Close();
    }

    public static SaveData_Endless LoadEndlessData()
    {
        string path = Application.persistentDataPath + "/BaldiData/endless.sav";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        SaveData_Endless data = bf.Deserialize(file) as SaveData_Endless;
        file.Close();

        return data;
    }

    public static void SaveChallengeData(StatisticsController stats)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/BaldiData/challenge.sav";
        FileStream file = File.Create(path);
        SaveData_Challenge data = new SaveData_Challenge(stats);
        data.fileVersion = 1;
        bf.Serialize(file, data);
        file.Close();
    }

    public static SaveData_Challenge LoadChallengeData()
    {
        string path = Application.persistentDataPath + "/BaldiData/challenge.sav";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        SaveData_Challenge data = bf.Deserialize(file) as SaveData_Challenge;
        file.Close();

        return data;
    }
}
