using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    [Header("Body")]
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _message;
    [Header("Buttons")]
    [SerializeField] private Button _okButton;
    [SerializeField] private TextMeshProUGUI _okButtonText;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TextMeshProUGUI _cancelButtonText;

    public static ModalWindow Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(string title, string message, string ok = "Ok", string cancel = "Cancel", Action onSubmitAction = null, Action onCancelAction = null)
    {
        _title.text = title;
        _message.text = message;
        _okButtonText.text = ok;
        _cancelButtonText.text = cancel;
        _okButton.onClick.AddListener(() => onSubmitAction?.Invoke());
        _cancelButton.onClick.AddListener(() => onCancelAction?.Invoke());

        _okButton.onClick.AddListener(() => gameObject.SetActive(false));
        _cancelButton.onClick.AddListener(() => gameObject.SetActive(false));

        gameObject.SetActive(_okButton);
    }
}
