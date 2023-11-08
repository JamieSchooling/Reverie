using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirectoryPath = "";
    private string _dataFileName = "";
    private bool _useEncryption = false;
    private readonly string _encyptionKey = "thisWasBasicallyASoloProjectLOL";

    public FileDataHandler(string dataDirectoryPath, string dataFileName, bool shouldEncrypt)
    {
        _dataDirectoryPath = dataDirectoryPath;
        _dataFileName = dataFileName;
        _useEncryption = shouldEncrypt;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirectoryPath, _dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to load data from file:  " + fullPath + "\n" + ex);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_dataDirectoryPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (_useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occured when trying to save data to file:  " + fullPath + "\n" + ex);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encyptionKey[i % _encyptionKey.Length]);
        }
        return modifiedData;
    }
}
