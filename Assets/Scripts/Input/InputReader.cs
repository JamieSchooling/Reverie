using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scriptable Objects/Input/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IPauseActions, GameInput.IChapterSelectActions
{
    public event Action<float> OnMoveInput;
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;
    public event Action OnDashPressed;
    public event Action<Vector2> OnDashAimed;
    public event Action OnInteractPressed;

    public event Action OnPausePressed;

    public event Action<int> OnChapterSelectSwitch;

    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.Pause.SetCallbacks(this);
            _gameInput.ChapterSelect.SetCallbacks(this);
        }
        _gameInput.Enable();
    }

    public void EnableAllInput()
    {
        _gameInput.Enable();
    }

    public void DisableAllInput()
    {
        _gameInput.Disable();
    }

    public void EnableGameplayInput()
    {
        _gameInput.Gameplay.Enable();
    }

    public void DisableGameplayInput()
    {
        _gameInput.Gameplay.Disable();
    }

    public void EnablePauseInput()
    {
        _gameInput.Pause.Enable();
    }

    public void DisablePauseInput()
    {
        _gameInput.Pause.Disable();
    }

    public void EnableChapterSelectInput()
    {
        _gameInput.ChapterSelect.Enable();
    }

    public void DisableChapterSelectInput()
    {
        _gameInput.ChapterSelect.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(Mathf.Round(context.ReadValue<float>()));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OnJumpPressed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            OnJumpReleased?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OnDashPressed?.Invoke();
    }

    public void OnDashAim(InputAction.CallbackContext context)
    {
        Vector2 aimValue = context.ReadValue<Vector2>();
        aimValue.x = Mathf.Round(aimValue.x);
        aimValue.y = Mathf.Round(aimValue.y);
        aimValue.Normalize();
        OnDashAimed?.Invoke(aimValue);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OnInteractPressed?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OnPausePressed?.Invoke();
    }

    public void OnSide(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            int contextValue = Mathf.RoundToInt(context.ReadValue<float>());
            OnChapterSelectSwitch?.Invoke(contextValue);
        }
    }
}
