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
    [SerializeField] private TextMeshProUGUI _deathCountText;

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

            _deathCountText.text = $"Death Count: {data.deathCount}";
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
