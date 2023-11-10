using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string _profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject _noDataContent;
    [SerializeField] private GameObject _hasDataContent;
    [SerializeField] private TextMeshProUGUI _chapterTitleText;
    [SerializeField] private TextMeshProUGUI _deathCountText;
    [SerializeField] private TextMeshProUGUI _collectibleCountText;

    private Button _saveSlotButton;

    private void Awake()
    {
        _saveSlotButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if (data == null)
        {
            _noDataContent.SetActive(true);
            _hasDataContent.SetActive(false);
        }
        else
        {
            _hasDataContent.SetActive(true);
            _noDataContent.SetActive(false);

            _chapterTitleText.text = data.isPrologueComplete ? "Chapter 1" : "Prologue";
            _deathCountText.text = $"Death Count: {data.deathCount}";
            _collectibleCountText.text = data.collectibles.Count.ToString();
        }
    }

    public string GetProfileId()
    {
        return _profileId;
    }

    public void SetInteractable(bool interactable)
    {
        _saveSlotButton.interactable = interactable;
    }
}
