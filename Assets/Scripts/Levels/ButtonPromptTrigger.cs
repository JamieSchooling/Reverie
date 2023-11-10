using UnityEngine;

public class ButtonPromptTrigger : MonoBehaviour
{
    [SerializeField] private ButtonPrompt _buttonPrompt;
    [SerializeField] private bool _disableOnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            _buttonPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            _buttonPrompt.gameObject.SetActive(false);
            if (_disableOnExit) 
            { 
                gameObject.SetActive(false); 
            }
        }
    }
}
