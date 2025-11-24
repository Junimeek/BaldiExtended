namespace SaveDataEncryption
{
    public static class SaveEncryption
    {
        static byte[] EncryptData(string data)
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(data);
            byte key = 39;
            // char xorKey = (char)5903;

            for (int i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] ^= key;
            }
            return inputBytes;
        }

        static string DecryptData(byte[] data)
        {
            byte[] inputBytes = data;
            byte key = 39;

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
