using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private DialogueEventChannel _dialogueEventChannel;
    [SerializeField] private Dialogue _dialogue;

    private bool _isPlayerInRange = false;

    private void Awake()
    {
        _dialogueEventChannel.OnDialogueEnd += OnDialogueEnd;
    }

    private void OnDestroy()
    {
        _dialogueEventChannel.OnDialogueEnd -= OnDialogueEnd;
    }

    private void OnEnable()
    {
        _inputReader.OnInteractPressed += OnTalkPressed;
    }

    private void OnDisable()
    {
        _inputReader.OnInteractPressed -= OnTalkPressed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            _isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            _isPlayerInRange = false;
        }
    }

    private void OnTalkPressed()
    {
        if (!_isPlayerInRange) return;

        _dialogueEventChannel.RequestDialogue(_dialogue);
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnDialogueEnd(Dialogue dialogue)
    {
        gameObject.SetActive(false);
    }
}
