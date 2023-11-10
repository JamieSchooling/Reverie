using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private SceneObject _chapterSelectScene;
    [SerializeField] private SceneObject _prologueScene;

    [Header("Back Button")]
    [SerializeField] private Button _backButton;

    private SaveSlot[] _saveSlots;

    private void Awake()
    {
        _saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    private void OnEnable()
    {
        ActivateMenu();
    }

    public void ActivateMenu()
    {
        Dictionary<string, GameData> profilesGameData = PersistentDataManager.Instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in _saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        PersistentDataManager.Instance.ChangeSelectedProfileid(saveSlot.GetProfileId());

        if (PersistentDataManager.Instance.GetGameDataForSelectedProfile().isPrologueComplete)
        {
            SceneManager.LoadSceneAsync(_chapterSelectScene);
        }
        else
        {
            SceneManager.LoadSceneAsync(_prologueScene);
        }
    }
    
    public void OnDeleteClicked(SaveSlot saveSlotToDelete)
    {
        PersistentDataManager.Instance.DeleteSave(saveSlotToDelete.GetProfileId());
        ActivateMenu();
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in _saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        _backButton.interactable = false;
    }
}
