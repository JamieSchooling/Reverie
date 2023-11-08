using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelected;

    private void OnEnable()
    {
        StartCoroutine(SetFirstSelected());
    }

    private IEnumerator SetFirstSelected()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }
}
