using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Tsuruno
{
    public static class DataLoader
    {
        public static Yui LoadAllV3Data(int fileNumber)
        {
            string path = Application.persistentDataPath + "/ExtendedData/EXT" + fileNumber.ToString("00") + ".sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            Yui data = bf.Deserialize(stream) as Yui;
            stream.Close();
            return data;
        }
    }

    public static class DataSaver
    {
        public static void SaveAllV3Data(Yui yui)
        {
            int fileNumber = PlayerPrefs.GetInt("YuiFileNumber", 1);
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/ExtendedData/EXT" + fileNumber.ToString("00") + ".sav";
            FileStream stream = File.Create(path);
            bf.Serialize(stream, yui);
            stream.Close();
        }
    }

    public static class SummaryWriter
    {
        public static void WriteFileSummary(Yui yui)
        {
            
        }
    }

    public static class Encryption
    {
        static byte[] EncryptData(string data)
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(data);
            byte key = 53;

            for (int i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] ^= key;
            }
            return inputBytes;
        }

        static string DecryptData(byte[] data)
        {
            byte[] inputBytes = data;
            byte key = 53;

            for (int i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] ^= key;
            }

            string decryptedString = System.Text.Encoding.UTF8.GetString(inputBytes);
            return decryptedString;
        }

        public static string EncryptSaveFile(string data)
        {
            byte[] encryptedData = EncryptData(data);
            string dataAsBase64 = System.Convert.ToBase64String(encryptedData);
            return dataAsBase64;
        }

        public static string DecryptSaveFile(string data)
        {
            byte[] encryptedData = System.Convert.FromBase64String(data);
            string decryptedString = DecryptData(encryptedData);
            return decryptedString;
        }
    }
}