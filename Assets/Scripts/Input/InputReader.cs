using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Input/Input Reader")]
public class InputReader : ScriptableObject
{
    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
        }
    }
}
