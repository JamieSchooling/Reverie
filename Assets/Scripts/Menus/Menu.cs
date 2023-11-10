using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelected;
    [SerializeField] private bool _reselectOnDisable;

    private GameObject _lastSelected;
    private bool _isInitialDisable = true;

    private void OnEnable()
    {
        if (_reselectOnDisable) _lastSelected = EventSystem.current.currentSelectedGameObject;
        StartCoroutine(SetSelected(_firstSelected));
    }

    private void OnDisable()
    {
        if (!_isInitialDisable && _reselectOnDisable && _lastSelected != null)
        {
            // So coroutine runs when window is inactive
            Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(SetSelected(_lastSelected));
        }
        _isInitialDisable = false;
    }

    private IEnumerator SetSelected(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(selected);
    }
}
