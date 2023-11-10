using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentDataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName;
    [SerializeField] private bool _shouldEncrypt;

    private GameData _gameData;

    private List<IPersistentData> _dataPersistenceObjects;
    private FileDataHandler _fileDataHandler;

    private string _selectedProfileId = "";

    public static PersistentDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying newest one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _shouldEncrypt);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IPersistentData> FindAllDataPersistenceObjects()
    {
        IEnumerable<IPersistentData> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPersistentData>();

        return new List<IPersistentData>(dataPersistenceObjects);
    }

    public void ChangeSelectedProfileid(string newProfileId)
    {
        _selectedProfileId = newProfileId;
        LoadGame();
    }

    public GameData GetGameDataForSelectedProfile()
    {
        return _gameData;
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load(_selectedProfileId);

        if (_gameData == null)
        {
            Debug.Log("No data was found. Initialising data to defaults.");
            NewGame();
        }

        foreach (IPersistentData dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IPersistentData dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref _gameData);
        }

        _fileDataHandler.Save(_gameData, _selectedProfileId);
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return _fileDataHandler.LoadAllProfiles();
    }

    public SettingsData GetPlayerPrefs()
    {
        SettingsData settingsData = new SettingsData();

        settingsData.isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        settingsData.masterVolume = PlayerPrefs.GetFloat("MasterVolume", settingsData.masterVolume);

        return settingsData;
    }

    public void SavePlayerPrefs(SettingsData settingsData)
    {
        PlayerPrefs.SetInt("IsFullscreen", settingsData.isFullscreen ? 1 : 0);
        PlayerPrefs.SetFloat("MasterVolume", settingsData.masterVolume);
        PlayerPrefs.Save();
    }
}
