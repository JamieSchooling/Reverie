using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Slider))]
public class TextSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private bool _makePercentage;
    [SerializeField] private bool _flipMinMax;
    [SerializeField] private bool _makeInt;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(UpdateText);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(UpdateText);
    }

    private void OnEnable()
    {
        UpdateText(_slider.value);
    }

    private void UpdateText(float value)
    {
        if (_makePercentage)
        {
            if (_flipMinMax)
                value /= _slider.minValue;
            else
                value /= _slider.maxValue;
            value = Mathf.Abs(value) * 100f;
            if (_flipMinMax) value = 100f - value;
        }

        if (_makeInt)
        {
            int displayValue = Mathf.RoundToInt(value);
            _valueText.text = displayValue.ToString();
        }
        else
        {
            _valueText.text = value.ToString();
        }
    }
}
