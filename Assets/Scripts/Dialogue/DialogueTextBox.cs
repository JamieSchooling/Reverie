using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTextBox : MonoBehaviour, DialogueNodeVisitor
{
    [Header("Input")]
    [SerializeField] private InputReader _inputReader;

    [Header("Event Channels")]
    [SerializeField] private AudioEventChannel _audioEventChannel;
    [SerializeField] private DialogueEventChannel _dialogueEventChannel;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _speakerText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private float _textSpeed = 0.2f;

    [Header("Choices")]
    [SerializeField] private RectTransform _choicesBoxTransform;
    [SerializeField] private DialogueChoiceButton[] _choiceButtons;

    private DialogueNode _nextNode = null;

    private string _currentLine;
    private bool _isCurrentlyChoiceNode;

    private void Awake()
    {
        _dialogueEventChannel.OnDialogueNodeStart += OnDialogueNodeStart;
        _dialogueEventChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

        gameObject.SetActive(false);
        _choicesBoxTransform.gameObject.SetActive(false);
        for (int i = 0; i < _choiceButtons.Length; i++)
        {
            _choiceButtons[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _inputReader.OnDialogueSkipPressed += OnSkipPressed;
    }

    private void OnDisable()
    {
        _inputReader.OnDialogueSkipPressed -= OnSkipPressed;
    }

    private void OnDestroy()
    {
        _dialogueEventChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
        _dialogueEventChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
    }

    private void OnSkipPressed()
    {
        if (!_isCurrentlyChoiceNode && _dialogueText.text == _currentLine) _dialogueEventChannel.RequestDialogueNode(_nextNode);
        else
        {
            StopAllCoroutines();
            _dialogueText.text = _currentLine;
            _audioEventChannel.RequestStopAudio();
            _choicesBoxTransform.gameObject.SetActive(true);
        }
    }

    private void OnDialogueNodeStart(DialogueNode node)
    {
        gameObject.SetActive(true);

        _speakerText.text = node.DialogueLine.Speaker.CharacterName + ":";
        _currentLine = node.DialogueLine.Text;
        if (node.DialogueLine.Audio != null)
        {
            _audioEventChannel.RequestPlayAudio(node.DialogueLine.Audio);
            Debug.Log("Playing Audio");
        }
        StartCoroutine(TypeText(_currentLine));
        node.Accept(this);
    }

    private void OnDialogueNodeEnd(DialogueNode node)
    {
        _nextNode = null;
        _dialogueText.text = "";
        _speakerText.text = "";

        for (int i = 0; i < _choiceButtons.Length; i++)
        {
            _choiceButtons[i].gameObject.SetActive(false);
        }
        _choicesBoxTransform.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    IEnumerator TypeText(string text)
    {
        foreach (char c in text.ToCharArray())
        {
            _dialogueText.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }

        _choicesBoxTransform.gameObject.SetActive(true);
    }

    public void Visit(BasicDialogueNode node)
    {
        _isCurrentlyChoiceNode = false;
        _nextNode = node.NextNode;
    }

    public void Visit(ChoiceDialogueNode node)
    {
        _isCurrentlyChoiceNode = true;

        for (int i = 0; i < node.Choices.Length; i++)
        {
            _choiceButtons[i].Choice = node.Choices[i];
        }
    }
}