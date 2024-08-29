using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveDataController
{
    /*
    private readonly string directory = Application.persistentDataPath + "/BaldiData";
    private readonly string versionDataPath = Application.persistentDataPath + "/BaldiData/fileVersion.dat";
    private readonly string storyDataPath = Application.persistentDataPath + "/BaldiData/story.sav";
    public string endlessDataPath = Application.persistentDataPath + "/BaldiData/endless.sav";
    private readonly string challengeDataPath = Application.persistentDataPath + "/BaldiData/challenge.sav";
    private SaveData_FileVersion versionData;
    public SaveData_Story storyData;
    public SaveData_Challenge challengeData;
    private readonly int currentVersion = 1;

    public void InitializeFiles()
    {
        versionData = new SaveData_FileVersion();
        storyData = new SaveData_Story();
        challengeData = new SaveData_Challenge();

        if (!Directory.Exists(this.directory))
            Directory.CreateDirectory(this.directory);

        if (!File.Exists(this.versionDataPath))
            this.SetScoreDefaults("version");

        if (!File.Exists(this.storyDataPath))
            this.SetScoreDefaults("story");
        
        if (!File.Exists(this.challengeDataPath))
            this.SetScoreDefaults("challenge");
        
        PlayerPrefs.SetInt("highbooks_Classic", 3);
        PlayerPrefs.SetInt("highbooks_ClassicExtended", 5);
        PlayerPrefs.SetInt("highbooks_JuniperHills", 6);
    }
    */

    public static bool CheckFileExist(string type)
    {
        switch(type)
        {
            case "story":
                if (!File.Exists(Application.persistentDataPath + "/BaldiData/story.sav"))
                {
                    SaveStoryData("defaults", 0);
                    return false;
                }
                else
                    return true;
            case "endless":
                if (!File.Exists(Application.persistentDataPath + "/BaldiData/endless.sav"))
                {
                    SaveEndlessData("defaults", 0);
                    return false;
                }
                else
                    return true;
            default:
                return false;
        }
        
    }

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

        SaveEndlessData("Classic", PlayerPrefs.GetInt("highbooks_Classic", 0));
        SaveEndlessData("ClassicExtended", PlayerPrefs.GetInt("highbooks_ClassicExtended", 0));
        SaveEndlessData("JuniperHills", PlayerPrefs.GetInt("highbooks_JuniperHills", 0));
        PlayerPrefs.DeleteKey("highbooks_Classic");
        PlayerPrefs.DeleteKey("highbooks_ClassicExtended");
        PlayerPrefs.DeleteKey("highbooks_JuniperHills");
    }

    public static void SaveStoryData(string map, int score)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/BaldiData/story.sav";
        FileStream file = File.Create(path);
        SaveData_Story data = new SaveData_Story(map, score);
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

    public static void SaveEndlessData(string map, int score)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/BaldiData/endless.sav";
        FileStream file = File.Create(path);
        SaveData_Endless data = new SaveData_Endless(map, score);
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

    public static void SaveChallengeData(string map, int score)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/BaldiData/challenge.sav";
        FileStream file = File.Create(path);
        SaveData_Challenge data = new SaveData_Challenge(map, score);
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
