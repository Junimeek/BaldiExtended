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

    /*
    private void SetScoreDefaults(string type)
    {
        switch(type)
        {
            case "version":
                versionData.versionNumber = this.currentVersion;
                this.SaveToFile("version");
                break;
            case "story":
                storyData.bestTime = new float[3];
                for (int i = 0; i < 3; i++)
                    storyData.bestTime[i] = 0f;
                this.SaveToFile("story");
                break;
            case "challenge":
                challengeData.bestTime = new float[1];
                this.SaveToFile("challenge");
                break;
        }
    }

    public void SaveToFile(string type)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        string saveType;

        switch(type)
        {
            case "version":
                file = File.Create(this.versionDataPath);
                bf.Serialize(file, versionData);
                saveType = "Current Version";
                break;
            case "story":
                file = File.Create(this.storyDataPath);
                bf.Serialize(file, storyData);
                saveType = "Story Data";
                break;
            case "challenge":
                file = File.Create(this.challengeDataPath);
                bf.Serialize(file, challengeData);
                saveType = "Challenge Data";
                break;
            default:
                Debug.LogError("failed somehow");
                return;
        }

        file.Close();
        Debug.Log("Saved file: " + saveType);
    }

    public void LoadAllData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        file = File.Open(this.versionDataPath, FileMode.Open);
        SaveData_FileVersion fileData_version = (SaveData_FileVersion)bf.Deserialize(file);
        file.Close();
        versionData.versionNumber = fileData_version.versionNumber;
        Debug.Log("Loaded version number " + versionData.versionNumber);

        file = File.Open(this.storyDataPath, FileMode.Open);
        SaveData_Story fileData_story = (SaveData_Story)bf.Deserialize(file);
        file.Close();
        storyData.bestTime = fileData_story.bestTime;
        Debug.Log("Loaded data: Story");

        file = File.Open(this.challengeDataPath, FileMode.Open);
        SaveData_Challenge fileData_challenge = (SaveData_Challenge)bf.Deserialize(file);
        file.Close();
        challengeData.bestTime = fileData_challenge.bestTime;
        Debug.Log("Loaded data: Challenge");

        this.UpgradeSaves();
    }
    */

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
}
