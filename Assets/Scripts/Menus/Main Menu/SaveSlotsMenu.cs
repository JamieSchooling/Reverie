using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
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

        SceneManager.LoadSceneAsync("Chapter 1");
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
