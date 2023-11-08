using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName;
    [SerializeField] private bool _shouldEncrypt;

    private GameData _gameData;

    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _fileDataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }

        Instance = this;
    }

    private void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _shouldEncrypt);
        _dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();

        if (_gameData == null)
        {
            Debug.Log("No data was found. Initialising data to defaults.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref _gameData);
        }

        _fileDataHandler.Save(_gameData);
    }
}
