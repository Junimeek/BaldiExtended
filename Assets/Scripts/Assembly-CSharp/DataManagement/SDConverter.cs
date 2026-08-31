using System.IO;
using UnityEngine;
using TMPro;
using OldSaveData;
using Tsuruno;

public class SDConverter : MonoBehaviour
{
    void Awake()
    {
        PlayerPrefs.SetInt("V3PromptSeen", 0);
        if (PlayerPrefs.GetInt("V3PromptSeen", 0) == 0)
        {
            this.CheckOldDirectory();
        }
        else
            this.StartBoot();
    }

    void CheckOldDirectory()
    {
        string currentDataDirectory = Application.persistentDataPath;
        string[] fragmentedDirectory = currentDataDirectory.Split("/");
        fragmentedDirectory[^2] = "JuniDev";
        this.oldDataDirectory = string.Join("/", fragmentedDirectory);
        oldDataDirectory += "/BaldiData";

        if (Directory.Exists(oldDataDirectory))
        {
            Debug.Log("Old JuniDev directory found. Checking for old save files...");
            this.existingOldSaveFiles = new bool[4];
            int foundFiles = 0;
            for (int i = 0; i < existingOldSaveFiles.Length; i++)
            {
                existingOldSaveFiles[i] = this.CheckOldFiles(i, oldDataDirectory);
                if (existingOldSaveFiles[i])
                    foundFiles++;
            }
            Debug.Log(foundFiles + " old save files found.");
            if (foundFiles > 0)
                this.ShowPrompt(1);
            else
                this.StartBoot();
        }
        else
            this.StartBoot();
    }

    bool CheckOldFiles(int fileCheck, string directory)
    {
        switch(fileCheck)
        {
            case 0:
                if (File.Exists(directory + "/story.sav"))
                    return true;
                else
                    return false;
            case 1:
                if (File.Exists(directory + "/endless.sav"))
                    return true;
                else
                    return false;
            case 2:
                if (File.Exists(directory + "/challenge.sav"))
                    return true;
                else
                    return false;
            case 3:
                if (File.Exists(directory + "/progression.sav"))
                    return true;
                else
                    return false;
            default:
                Debug.LogError("Unknown number inputted");
                return false;
        }
    }

    public void ShowPrompt(int promptNum)
    {
        this.choicePrompt.SetActive(false);
        this.preConvertPrompt.SetActive(false);
        this.namePrompt.SetActive(false);
        this.postConvertPrompt.SetActive(false);
        switch(promptNum)
        {
            case 1:
                this.choicePrompt.SetActive(true);
                this.audioDevice.PlayOneShot(this.errorSound);
                break;
            case 2:
                this.preConvertPrompt.SetActive(true);
                break;
            case 3:
                this.namePrompt.SetActive(true);
                this.nameInputField.Select();
                break;
            case 4:
                this.postConvertPrompt.SetActive(true);
                break;
            case 10:
                this.ReadV2DataAndSetV3DataInMemory();
                break;
            default:
                break;
        }
    }

    void ReadV2DataAndSetV3DataInMemory()
    {
        if (this.existingOldSaveFiles[0])
        {
            SaveData_Story storyDataV2 = OldSaveDataLoader.LoadOldStoryData(this.oldDataDirectory);
            this.itemsUsedStoryClassicV2 = storyDataV2.itemsUsed_Classic;
            this.itemsUsedStoryClassicExtendedV2 = storyDataV2.itemsUsed_ClassicExtended;
            this.itemsUsedStoryJuniperHillsV2 = storyDataV2.itemsUsed_JuniperHills;
            this.bestTimesStoryV2 = storyDataV2.bestTime;
            this.totalDetentionsStoryV2 = storyDataV2.totalDetentions;
            this.SetV3StoryData(true);
        }
        else
            this.SetV3StoryData(false);
        
        if (this.existingOldSaveFiles[1])
        {
            SaveData_Endless endlessDataV2 = OldSaveDataLoader.LoadOldEndlessData(this.oldDataDirectory);
            this.itemsUsedEndlessClassicV2 = endlessDataV2.itemsUsed_Classic;
            this.itemsUsedEndlessClassicExtendedV2 = endlessDataV2.itemsUsed_ClassicExtended;
            this.itemsUsedEndlessJuniperHillsV2 = endlessDataV2.itemsUsed_JuniperHills;
            this.endlessNotebooksV2 = endlessDataV2.notebooks;
            this.totalDetentionsEndlessV2 = endlessDataV2.totalDetentions;
            this.SetV3EndlessData(true);
        }
        else
            this.SetV3EndlessData(false);
        
        if (this.existingOldSaveFiles[2])
        {
            SaveData_Challenge challengeDataV2 = OldSaveDataLoader.LoadOldChallengeData(this.oldDataDirectory);
            this.itemsUsedChallengeDarkModeV2 = challengeDataV2.itemsUsed_NullStyle;
            this.bestTimesChallengeV2 = challengeDataV2.bestTime;
            this.totalDetentionsChallengeV2 = challengeDataV2.totalDetentions;
            this.SetV3ChallengeData(true);
        }
        else
            this.SetV3ChallengeData(false);
        
        if (this.existingOldSaveFiles[3])
        {
            ProgressionData progressionDataV2 = OldSaveDataLoader.LoadOldProgressionData(this.oldDataDirectory);
            this.challengeUnlocksV2 = progressionDataV2.mapUnlocks;
            this.SetV3ProgressionData();
        }

        this.playerDataV3.fileVersion = 3;
        this.playerDataV3.playerIcon = 1;
        this.playerDataV3.totalPlaySeconds = 0;
        this.playerDataV3.gameVersion = this.currentVersion;
        this.ShowPrompt(3);
    }

    public void SetPlayerNameAndSaveNewFile()
    {
        if (this.nameInputField.text == string.Empty)
            this.playerDataV3.playerName = "Player";
        else
            this.playerDataV3.playerName = this.nameInputField.text;
        
        if (!Directory.Exists(Application.persistentDataPath + "/ExtendedData"))
            Directory.CreateDirectory(Application.persistentDataPath + "/ExtendedData");
        
        Yui yui = new Yui(this.playerDataV3, this.storyDatasV3, this.endlessDatasV3, this.challengeDatasV3, this.progressionDataV3);
        DataSaver.SaveAllV3Data(yui);

        PlayerPrefs.SetInt("currentSaveFileNumber", 1);
        
        this.ShowPrompt(4);
    }

    void SetV3StoryData(bool dataExists)
    {
        this.storyDatasV3 = new SD3StoryData[3];

        this.storyDatasV3[0] = new SD3StoryData {
            mapID = 1,
            mapName = "Classic",
            lifetimeItemsUsed = new int[1],
            bestTime = 0,
            lifetimeDetentions = 0
        };
        this.storyDatasV3[1] = new SD3StoryData {
            mapID = 2,
            mapName = "ClassicExtended",
            lifetimeItemsUsed = new int[1],
            bestTime = 0,
            lifetimeDetentions = 0
        };
        this.storyDatasV3[2] = new SD3StoryData {
            mapID = 3,
            mapName = "JuniperHills",
            lifetimeItemsUsed = new int[1],
            bestTime = 0,
            lifetimeDetentions = 0
        };
        
        if (dataExists)
        {
            this.storyDatasV3[0].lifetimeItemsUsed = this.itemsUsedStoryClassicV2;
            this.storyDatasV3[0].bestTime = Mathf.RoundToInt(this.bestTimesStoryV2[0] * 1000f);
            this.storyDatasV3[0].lifetimeDetentions = this.totalDetentionsStoryV2[0];
            this.storyDatasV3[1].lifetimeItemsUsed = this.itemsUsedStoryClassicExtendedV2;
            this.storyDatasV3[1].bestTime = Mathf.RoundToInt(this.bestTimesStoryV2[1] * 1000f);
            this.storyDatasV3[1].lifetimeDetentions = this.totalDetentionsStoryV2[1];
            this.storyDatasV3[2].lifetimeItemsUsed = this.itemsUsedStoryJuniperHillsV2;
            this.storyDatasV3[2].bestTime = Mathf.RoundToInt(this.bestTimesStoryV2[2] * 1000f);
            this.storyDatasV3[2].lifetimeDetentions = this.totalDetentionsStoryV2[2];
        }
    }

    void SetV3EndlessData(bool dataExists)
    {
        this.endlessDatasV3 = new SD3EndlessData[3];

        this.endlessDatasV3[0] = new SD3EndlessData {
            mapID = 1,
            mapName = "Classic",
            lifetimeItemsUsed = new int[1],
            bestNotebooks = 0,
            lifetimeDetentions = 0
        };
        this.endlessDatasV3[1] = new SD3EndlessData {
            mapID = 2,
            mapName = "ClassicExtended",
            lifetimeItemsUsed = new int[1],
            bestNotebooks = 0,
            lifetimeDetentions = 0
        };
        this.endlessDatasV3[2] = new SD3EndlessData {
            mapID = 3,
            mapName = "JuniperHills",
            lifetimeItemsUsed = new int[1],
            bestNotebooks = 0,
            lifetimeDetentions = 0
        };

        if (dataExists)
        {
            this.endlessDatasV3[0].lifetimeItemsUsed = this.itemsUsedEndlessClassicV2;
            this.endlessDatasV3[0].bestNotebooks = this.endlessNotebooksV2[0];
            this.endlessDatasV3[0].lifetimeDetentions = this.totalDetentionsEndlessV2[0];
            this.endlessDatasV3[1].lifetimeItemsUsed = this.itemsUsedEndlessClassicV2;
            this.endlessDatasV3[1].bestNotebooks = this.endlessNotebooksV2[1];
            this.endlessDatasV3[1].lifetimeDetentions = this.totalDetentionsEndlessV2[1];
            this.endlessDatasV3[2].lifetimeItemsUsed = this.itemsUsedEndlessClassicV2;
            this.endlessDatasV3[2].bestNotebooks = this.endlessNotebooksV2[2];
            this.endlessDatasV3[2].lifetimeDetentions = this.totalDetentionsEndlessV2[2];
        }
    }

    void SetV3ChallengeData(bool dataExists)
    {
        this.challengeDatasV3 = new SD3ChallengeData[1];

        this.challengeDatasV3[0] = new SD3ChallengeData {
            mapID = 1,
            mapName = "ClassicDark",
            lifetimeItemsUsed = new int[1],
            bestTime = 0,
            lifetimeDetentions = 0
        };

        if (dataExists)
        {
            this.challengeDatasV3[0].lifetimeItemsUsed = this.itemsUsedChallengeDarkModeV2;
            this.challengeDatasV3[0].bestTime = Mathf.RoundToInt(this.bestTimesChallengeV2[0] * 1000f);
            this.challengeDatasV3[0].lifetimeDetentions = this.totalDetentionsChallengeV2[0];
        }
    }

    void SetV3ProgressionData()
    {
        this.progressionDataV3 = new SD3ProgressionData
        {
            challengeUnlocks = this.challengeUnlocksV2
        };
    }

    public void StartBoot()
    {
        //PlayerPrefs.SetInt("V3PromptSeen", 1);
        launcher.BeginBoot();
        this.ShowPrompt(0);
    }

    [SerializeField] int[] currentVersion;
    [SerializeField] bool[] existingOldSaveFiles;
    [SerializeField] Launcher launcher;
    [SerializeField] string oldDataDirectory;

    [Header("UI")]
    [SerializeField] GameObject choicePrompt;
    [SerializeField] GameObject preConvertPrompt;
    [SerializeField] GameObject namePrompt;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] GameObject postConvertPrompt;
    [SerializeField] AudioSource audioDevice;
    [SerializeField] AudioClip errorSound;

    [Header("Old Save Data")]
    [SerializeField] int[] itemsUsedStoryClassicV2;
    [SerializeField] int[] itemsUsedStoryClassicExtendedV2;
    [SerializeField] int[] itemsUsedStoryJuniperHillsV2;
    [SerializeField] float[] bestTimesStoryV2;
    [SerializeField] int[] totalDetentionsStoryV2;
    [SerializeField] int[] itemsUsedEndlessClassicV2;
    [SerializeField] int[] itemsUsedEndlessClassicExtendedV2;
    [SerializeField] int[] itemsUsedEndlessJuniperHillsV2;
    [SerializeField] int[] endlessNotebooksV2;
    [SerializeField] int[] totalDetentionsEndlessV2;
    [SerializeField] int[] itemsUsedChallengeDarkModeV2;
    [SerializeField] float[] bestTimesChallengeV2;
    [SerializeField] int[] totalDetentionsChallengeV2;
    [SerializeField] bool[] challengeUnlocksV2;

    [Header("New Save Data")]
    [SerializeField] SD3PlayerData playerDataV3;
    [SerializeField] SD3StoryData[] storyDatasV3;
    [SerializeField] SD3EndlessData[] endlessDatasV3;
    [SerializeField] SD3ChallengeData[] challengeDatasV3;
    [SerializeField] SD3ProgressionData progressionDataV3;
}
