using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectableTextColourChanger : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] private Color _defaultColour = Color.white;
    [SerializeField] private Color _selectedColour = Color.gray;
    [SerializeField] private AudioEventChannel _audioEventChannel;
    [SerializeField] private AudioClip _selectSFX;

    TextMeshProUGUI[] textObjects;

    private void OnEnable()
    {
        textObjects = gameObject.GetComponentsInChildren<TextMeshProUGUI>(); 
        foreach (var textObject in textObjects)
        {
            textObject.color = _defaultColour;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        textObjects = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textObject in textObjects)
        {
            textObject.color = _selectedColour;
        }
        _audioEventChannel.RequestPlayAudio(_selectSFX);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        textObjects = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textObject in textObjects)
        {
            textObject.color = _defaultColour;
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        textObjects = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textObject in textObjects)
        {
            textObject.color = _selectedColour;
        }
    }
}
