using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirectoryPath = "";
    private string _dataFileName = "";
    private bool _useEncryption = false;
    private readonly string _encyptionKey = "thisWasBasicallyASoloProject";

    public FileDataHandler(string dataDirectoryPath, string dataFileName, bool shouldEncrypt)
    {
        _dataDirectoryPath = dataDirectoryPath;
        _dataFileName = dataFileName;
        _useEncryption = shouldEncrypt;
    }

    public GameData Load(string profileId)
    {
        string fullPath = Path.Combine(_dataDirectoryPath, profileId, _dataFileName);
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

                Debug.LogError($"Error occured when trying to load data from file:  {fullPath} \n {ex}");
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        string fullPath = Path.Combine(_dataDirectoryPath, profileId, _dataFileName);

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
            Debug.LogError($"Error occured when trying to save data to file:  {fullPath} \n {ex}");
        }
    }

    public void Delete(string profileId)
    {
        string fullPath = Path.Combine(_dataDirectoryPath, profileId);
        if (Directory.Exists(fullPath))
        {
            try
            {
                Directory.Delete(fullPath, true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error occured when trying to load data from file:  {fullPath} \n {ex}");
            }
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> directoryInfo = new DirectoryInfo(_dataDirectoryPath).EnumerateDirectories();
        foreach (DirectoryInfo directory in directoryInfo)
        {
            string profileId = directory.Name;

            string fullpath = Path.Combine(_dataDirectoryPath, profileId, _dataFileName);
            if (!File.Exists(fullpath))
            {
                Debug.LogWarning($"Skipping directory when loading all profiles because it does not contain data: {profileId}");
                continue;
            }
            GameData profileData = Load(profileId);
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError($"Failed to load profile: {profileId}");
            }
        }

        return profileDictionary;
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
