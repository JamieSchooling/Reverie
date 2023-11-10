using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectableTextColourChanger : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Color _defaultColour = Color.white;
    [SerializeField] private Color _selectedColour = Color.gray;

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
    }

    public void OnDeselect(BaseEventData eventData)
    {
        textObjects = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textObject in textObjects)
        {
            textObject.color = _defaultColour;
        }
    }
}
