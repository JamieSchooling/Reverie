using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ButtonPrompt : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [TextArea(2,3)]
    [SerializeField] private string _promptText = "Press BUTTONPROMPT0 to interact.";

    [SerializeField] private ListOfTmpSpriteAssets _tmpSpriteAssets;
    [SerializeField] private DeviceType _deviceType;
    [SerializeField] private string[] _actionNames;
    [Range(1f, 4f)]
    [SerializeField] private int _compositeBinding;

    private GameInput _gameInput;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _gameInput = new GameInput();
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ChangeDeviceType();
        SetText();
    }
    private void OnDisable()
    {
        ChangeDeviceType();
        SetText();
    }

    [ContextMenu("Set Text")]
    private void SetText()
    {
        if ((int)_deviceType > _tmpSpriteAssets.SpriteAssets.Count - 1)
        {
            Debug.LogWarning($"Missing Sprite Asset for {_deviceType}.");
            return;
        }

        InputBinding[] bindings = new InputBinding[_actionNames.Length];
        for (int i = 0; i < bindings.Length; i++)
        {
            bindings[i] = _gameInput.asset.FindAction(_actionNames[i]).bindings[(int)_deviceType];
            
            if (bindings[i].isComposite)
            {
                bindings[i] = _gameInput.asset.FindAction(_actionNames[i]).bindings[(int)_deviceType + _compositeBinding];
            }
            else if ((int)_deviceType - 1 >= 0 && _gameInput.asset.FindAction(_actionNames[i]).bindings[(int)_deviceType - 1].isComposite)
            {
                bindings[i] = _gameInput.asset.FindAction(_actionNames[i]).bindings[(int)_deviceType + 4];
            }
        }

        _text.text = ReplaceButtonPromptSprite.ReadAndReplaceBinding(_promptText, bindings, _tmpSpriteAssets.SpriteAssets[(int)_deviceType]);
    }

    private void ChangeDeviceType()
    {
        _deviceType = (DeviceType)(_gameInput.controlSchemes[0].name == _playerInput.currentControlScheme ? 0 : 1);
    }

    public void OnDeviceChange(PlayerInput input)
    {
        ChangeDeviceType();

        SetText();
    }



    private enum DeviceType
    {
        Keyboard,
        Gamepad
    }
}
