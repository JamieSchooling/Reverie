using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Toggle))]
public class TextToggle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _onText;
    [SerializeField] private TextMeshProUGUI _offText;

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(UpdateText);
    }

    private void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(UpdateText);
    }

    private void OnEnable()
    {
        UpdateText(_toggle.isOn);
    }

    private void UpdateText(bool value)
    {
        _onText.gameObject.SetActive(value);
        _offText.gameObject.SetActive(!value);
    }

}
