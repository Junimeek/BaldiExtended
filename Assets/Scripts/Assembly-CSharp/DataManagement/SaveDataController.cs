namespace SaveDataEncryption
{
    public static class SaveEncryption
    {
        static string EncryptionAlgorithm(string data)
        {
            char xorKey = (char)5903;
            string outputString = "";
            int length = data.Length;

            for (int i = 0; i < length; i++)
            {
                outputString += char.ToString((char)(data[i] ^ xorKey));
            }

            return outputString;
        }

        public static string EncryptSaveFile(string data)
        {
            string encryptedString = EncryptionAlgorithm(data);
            return encryptedString;
        }

        public static string DecryptSaveFile(string data)
        {
            string decryptedString = EncryptionAlgorithm(data);
            return decryptedString;
        }
    }
}
