using UnityEngine;

public class Collectible : MonoBehaviour, IPersistentData
{
    [SerializeField] private string _id;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private DialogueEventChannel _dialogueEventChannel;
    [SerializeField] private Dialogue _dialogue;

    private bool _isPlayerInRange = false;
    private bool _isCollected = false;

    [ContextMenu("Generate GUID for id")]
    private void GenerateGuid()
    {
        _id = System.Guid.NewGuid().ToString();
    }

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
        _isCollected = true;
        gameObject.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        data.collectibles.TryGetValue(_id, out _isCollected);
        if (_isCollected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.collectibles.ContainsKey(_id))
        {
            data.collectibles.Remove(_id);
        }
        data.collectibles.Add(_id, _isCollected);
    }
}
