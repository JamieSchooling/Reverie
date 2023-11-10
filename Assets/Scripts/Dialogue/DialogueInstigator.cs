using UnityEngine;

public class DialogueInstigator : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private DialogueEventChannel _dialogueChannel;

    private DialogueSequencer _dialogueSequencer;


    private void Awake()
    {
        _dialogueSequencer = new DialogueSequencer();

        _dialogueSequencer.OnDialogueStart += OnDialogueStart;
        _dialogueSequencer.OnDialogueEnd += OnDialogueEnd;
        _dialogueSequencer.OnDialogueNodeStart += _dialogueChannel.RaiseStartDialogueNode;
        _dialogueSequencer.OnDialogueNodeEnd += _dialogueChannel.RaiseEndDialogueNode;

        _dialogueChannel.OnDialogueRequested += _dialogueSequencer.StartDialogue;
        _dialogueChannel.OnDialogueNodeRequested += _dialogueSequencer.StartDialogueNode;
    }

    private void OnDestroy()
    {
        _dialogueChannel.OnDialogueNodeRequested -= _dialogueSequencer.StartDialogueNode;
        _dialogueChannel.OnDialogueRequested -= _dialogueSequencer.StartDialogue;

        _dialogueSequencer.OnDialogueNodeEnd -= _dialogueChannel.RaiseEndDialogueNode;
        _dialogueSequencer.OnDialogueNodeStart -= _dialogueChannel.RaiseStartDialogueNode;
        _dialogueSequencer.OnDialogueEnd -= OnDialogueEnd;
        _dialogueSequencer.OnDialogueStart -= OnDialogueStart;

        _dialogueSequencer = null;
    }

    private void OnDialogueStart(Dialogue dialogue)
    {
        _dialogueChannel.RaiseStartDialogue(dialogue);
        _inputReader.EnableDialogueInput();
    }

    private void OnDialogueEnd(Dialogue dialogue)
    {
        _inputReader.EnableGameplayInput();
        _dialogueChannel.RaiseEndDialogue(dialogue);
    }
}
