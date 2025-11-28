using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace OldSaveData
{
    public static class OldSaveDataLoader
    {
        public static SaveData_Story LoadOldStoryData()
        {
            string path = Application.persistentDataPath + "/BaldiData/story.sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            SaveData_Story data = bf.Deserialize(stream) as SaveData_Story;
            stream.Close();
            return data;
        }

        public static SaveData_Endless LoadOldEndlessData()
        {
            string path = Application.persistentDataPath + "/BaldiData/endless.sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            SaveData_Endless data = bf.Deserialize(stream) as SaveData_Endless;
            stream.Close();
            return data;
        }

        public static SaveData_Challenge LoadOldChallengeData()
        {
            string path = Application.persistentDataPath + "/BaldiData/challenge.sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            SaveData_Challenge data = bf.Deserialize(stream) as SaveData_Challenge;
            stream.Close();
            return data;
        }

        public static ProgressionData LoadOldProgressionData()
        {
            string path = Application.persistentDataPath + "/BaldiData/progression.sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            ProgressionData data = bf.Deserialize(stream) as ProgressionData;
            stream.Close();
            return data;
        }

        public static void SaveOldStoryData(StatisticsController stats)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/BaldiData/story.sav";
            FileStream stream = File.Create(path);
            SaveData_Story data = new SaveData_Story(stats);
            data.fileVersion = 1;
            bf.Serialize(stream, data);
            stream.Close();
        }

        public static void SaveOldEndlessData(StatisticsController stats)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/BaldiData/endless.sav";
            FileStream stream = File.Create(path);
            SaveData_Endless data = new SaveData_Endless(stats);
            data.fileVersion = 1;
            bf.Serialize(stream, data);
            stream.Close();
        }

        public static void SaveOldChallengeData(StatisticsController stats)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/BaldiData/challenge.sav";
            FileStream stream = File.Create(path);
            SaveData_Challenge data = new SaveData_Challenge(stats);
            data.fileVersion = 1;
            bf.Serialize(stream, data);
            stream.Close();
        }

        public static void SaveOldProgressionData(ProgressionController progression)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/BaldiData/progression.sav";
            FileStream stream = File.Create(path);
            ProgressionData data = new ProgressionData(progression);
            data.fileVersion = 1;
            bf.Serialize(stream, data);
            stream.Close();
        }
    }
}