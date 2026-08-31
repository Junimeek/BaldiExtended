using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace OldSaveData
{
    public static class OldSaveDataLoader
    {
        public static SaveData_Story LoadOldStoryData(string oldDirectory)
        {
            string path = oldDirectory + "/story.sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            SaveData_Story data = bf.Deserialize(stream) as SaveData_Story;
            stream.Close();
            return data;
        }

        public static SaveData_Endless LoadOldEndlessData(string oldDirectory)
        {
            string path = oldDirectory + "/endless.sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            SaveData_Endless data = bf.Deserialize(stream) as SaveData_Endless;
            stream.Close();
            return data;
        }

        public static SaveData_Challenge LoadOldChallengeData(string oldDirectory)
        {
            string path = oldDirectory + "/challenge.sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            SaveData_Challenge data = bf.Deserialize(stream) as SaveData_Challenge;
            stream.Close();
            return data;
        }

        public static ProgressionData LoadOldProgressionData(string oldDirectory)
        {
            string path = oldDirectory + "/progression.sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            ProgressionData data = bf.Deserialize(stream) as ProgressionData;
            stream.Close();
            return data;
        }
    }
}