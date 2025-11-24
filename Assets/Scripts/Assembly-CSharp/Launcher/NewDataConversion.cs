using System;
using System.Collections;
using System.IO;
using UnityEngine;
using TMPro;
using OldSaveData;
using SaveDataEncryption;

public class NewDataConversion : MonoBehaviour
{
    [SerializeField] bool isInitialized;
    [SerializeField] bool[] completedChecks;
    [SerializeField] GameObject finishPrompt;
    [SerializeField] GameObject[] checkMarks;
    [SerializeField] string[] fragmentedJunidevDirectory;
    [SerializeField] GameObject errorPrompt;
    [SerializeField] TMP_Text errorText;
    [SerializeField] SaveDataContainer saveDataContainer;
    [SerializeField] ExportedSettings settingsContainer;
    [Header("Debug")]
    [SerializeField] bool isEncrypted;

    void OnEnable()
    {
        if (!this.isInitialized)
        {
            completedChecks = new bool[2];
            completedChecks[0] = false;
            completedChecks[1] = false;
            finishPrompt.SetActive(false);
            checkMarks[0].SetActive(false);
            checkMarks[1].SetActive(false);
            errorPrompt.SetActive(false);
            errorText.text = string.Empty;
            this.isInitialized = true;
            this.saveDataContainer = FindObjectOfType<SaveDataContainer>();
        }
    }

    public void UpdateGameData(UnityEngine.UI.Button settingsButton)
    {
        string junidevDataDirectory = Application.persistentDataPath;
        string kyokuraDataDirectory = GetKyokuraDirectory(junidevDataDirectory);
        if (kyokuraDataDirectory == null)
        {
            ThrowError("Unable to locate JuniDev AppData directory.", string.Empty);
            return;
        }

        if (!completedChecks[0])
        {
            if (!Directory.Exists(kyokuraDataDirectory))
            {
                try
                {
                    Directory.CreateDirectory(kyokuraDataDirectory);
                }
                catch(Exception e)
                {
                    ThrowError("Unable to create Kyokura directory.", e.ToString());
                }
            }
            try
            {
                ConvertGameData();
                SaveGameDataInNewDirectory(kyokuraDataDirectory);
                checkMarks[0].SetActive(true);
                completedChecks[0] = true;
                EnableSettingsButton(settingsButton);
                // LoadSave(kyokuraDataDirectory);
            }
            catch(Exception e)
            {
                ThrowError("Failed to copy and save data to new directory.", e.ToString());
            }
        }
    }

    void EnableSettingsButton(UnityEngine.UI.Button settingsButton)
    {
        settingsButton.interactable = true;
    }

    void ConvertGameData()
    {
        UnityEngine.Debug.Log("Save files will now be converted to the Version 2 format.");

        SaveData_Story oldStoryData = OldSaveDataLoader.LoadOldStoryData();
        SaveData_Endless oldEndlessData = OldSaveDataLoader.LoadOldEndlessData();
        SaveData_Challenge oldChallengeData = OldSaveDataLoader.LoadOldChallengeData();
        ProgressionData oldProgressionData = OldSaveDataLoader.LoadOldProgressionData();

        int itemCount = 18;
        int mapCount = 5;

        int saveFileVersion = 2;
        int[] gameVersion = new int[] {
            0, 6, 0
        };
        float[] bestTimes = oldStoryData.bestTime;

        Array.Resize(ref bestTimes, mapCount);

        bestTimes[3] = oldChallengeData.bestTime[0];
        int[] bestTimesInMS = new int[mapCount];
        float[] timeInSeconds = new float[mapCount];

        for (int i = 0; i < mapCount; i++)
        {
            timeInSeconds[i] = bestTimes[i] * 1000f;
            bestTimesInMS[i] = Mathf.RoundToInt(timeInSeconds[i]);
        }

        saveDataContainer.saveFileVersion = saveFileVersion;
        saveDataContainer.gameVersion = gameVersion;
        saveDataContainer.bestTimes = bestTimesInMS;
        saveDataContainer.sItems_classic = oldStoryData.itemsUsed_Classic;
        saveDataContainer.sItems_classicExtended = oldStoryData.itemsUsed_ClassicExtended;
        saveDataContainer.sItems_juniperHills = oldStoryData.itemsUsed_JuniperHills;
        saveDataContainer.sItems_mitakihara = new int[itemCount];
        saveDataContainer.sDetentions = oldStoryData.totalDetentions;
        saveDataContainer.eItems_classic = oldEndlessData.itemsUsed_Classic;
        saveDataContainer.eItems_classicExtended = oldEndlessData.itemsUsed_ClassicExtended;
        saveDataContainer.eItems_juniperHills = oldEndlessData.itemsUsed_JuniperHills;
        saveDataContainer.eItems_mitakihara = new int[itemCount];
        saveDataContainer.eDetentions = oldEndlessData.totalDetentions;
        saveDataContainer.eNotebooks = oldEndlessData.notebooks;
        saveDataContainer.challengeUnlocks = oldProgressionData.mapUnlocks;
        saveDataContainer.cItemsClassicDark = oldChallengeData.itemsUsed_NullStyle;
    }

    void SaveGameDataInNewDirectory(string kyoDir)
    {
        string jsonData = JsonUtility.ToJson(saveDataContainer, false);
        if (!Directory.Exists(kyoDir + "/BaldiData"))
            Directory.CreateDirectory(kyoDir + "/BaldiData");
        string path = kyoDir;

        using (StreamWriter sw = File.CreateText(this.GetFileName(kyoDir, 1)))
        {
            if (isEncrypted)
            {
                sw.WriteLine(SaveEncryption.EncryptSaveFile(jsonData));
            }
            else
            {
                sw.WriteLine(jsonData);
            }
            sw.Close();
        }
        UnityEngine.Debug.Log("Data saved in " + path);
    }

    string GetFileName(string directory, int fileID)
    {
        string fileName = directory + "/BaldiData/EXT" + fileID;

        if (isEncrypted)
            fileName += ".sav";
        else
            fileName += ".json";
        
        return fileName;
    }

    void LoadSave(string kyoDir)
    {
        if (!isEncrypted)
            return;
        string path = kyoDir + "/BaldiData/EXT1.sav";
        string base64Data = "";
        
        using (StreamReader sr = File.OpenText(path))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                base64Data += s;
            }
            sr.Close();
        }

        string jsonData = SaveEncryption.DecryptSaveFile(base64Data);
        UnityEngine.Debug.Log(jsonData);
    }

    public void ExportSettingsData()
    {
        if (completedChecks[1])
            return;
        
        settingsContainer.turnSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        settingsContainer.volumeVoice = PlayerPrefs.GetFloat("VolumeVoice");
        settingsContainer.volumeBGM = PlayerPrefs.GetFloat("VolumeBGM");
        settingsContainer.volumeSFX = PlayerPrefs.GetFloat("VolumeSFX");
        settingsContainer.framerate = PlayerPrefs.GetInt("Framerate");
        settingsContainer.instantReset = PlayerPrefs.GetInt("InstantReset");
        settingsContainer.additionalMusic = PlayerPrefs.GetInt("AdditionalMusic");
        settingsContainer.notifBoard = PlayerPrefs.GetInt("NotifBoard");
        settingsContainer.familyFriendly = PlayerPrefs.GetInt("gps_familyFriendly");
        settingsContainer.doorFix = PlayerPrefs.GetInt("pat_doorFix");
        settingsContainer.safeMode = PlayerPrefs.GetInt("gps_safemode");
        settingsContainer.difficultMath = PlayerPrefs.GetInt("gps_difficultmath");

        string jsonData = JsonUtility.ToJson(settingsContainer, true);
        string path = GetKyokuraDirectory(Application.persistentDataPath) + "/exportedsettings.json";

        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine(jsonData);
            sw.Close();
        }

        checkMarks[1].SetActive(true);
        completedChecks[1] = true;
        finishPrompt.SetActive(true);

        UnityEngine.Debug.Log("Settings exported.");
    }

    void ThrowError(string theError, string exceptionThing)
    {
        this.errorPrompt.SetActive(true);
        this.errorText.text = theError + "\n\n" + exceptionThing;
        UnityEngine.Debug.LogError(theError);
        UnityEngine.Debug.LogError(exceptionThing);
    }

    string GetKyokuraDirectory(string juniDir)
    {
        fragmentedJunidevDirectory = juniDir.Split("/");
        if (fragmentedJunidevDirectory[^2] == "JuniDev")
        {
            fragmentedJunidevDirectory[^2] = "Kyokura";
            string kyoDir = string.Join("/", fragmentedJunidevDirectory);
            return kyoDir;
        }
        else
            return null;
    }
}
