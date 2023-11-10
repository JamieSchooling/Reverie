using System;

public class DialogueSequencer
{
    public event Action<Dialogue> OnDialogueStart;
    public event Action<Dialogue> OnDialogueEnd;
    public event Action<DialogueNode> OnDialogueNodeStart;
    public event Action<DialogueNode> OnDialogueNodeEnd;

    private Dialogue _currentDialogue;
    private DialogueNode _currentNode;

    public void StartDialogue(Dialogue dialogue)
    {
        if (_currentDialogue == null)
        {
            _currentDialogue = dialogue;
            OnDialogueStart?.Invoke(dialogue);
            StartDialogueNode(dialogue.FirstNode);
        }
        else
        {
            throw new DialogueException("Can't start dialogue when another is already running.");
        }
    }

    public void EndDialogue(Dialogue dialogue)
    {
        if (_currentDialogue == dialogue)
        {
            StopDialogueNode(_currentNode);
            OnDialogueEnd?.Invoke(dialogue);
            _currentDialogue = null;
        }
        else
        {
            throw new DialogueException("Can't stop dialogue if no dialogue is running.");
        }
    }

    private bool CanStartNode(DialogueNode node)
    {
        return (_currentNode == null || node == null || _currentNode.CanBeFollowedByNode(node));
    }

    public void StartDialogueNode(DialogueNode node)
    {
        if (CanStartNode(node))
        {
            StopDialogueNode(_currentNode);
            _currentNode = node;
            if (_currentNode != null) OnDialogueNodeStart?.Invoke(_currentNode);
            else EndDialogue(_currentDialogue);
        }
        else
        {
            throw new DialogueException("Failed to start dialogue node");
        }
    }

    public void StopDialogueNode(DialogueNode node)
    {
        if (_currentNode == node)
        {
            OnDialogueNodeEnd?.Invoke(_currentNode);
            _currentNode = null; 
        }
        else
        {
            throw new DialogueException("Failed to stop dialogue node.");
        }
    }
}

public class DialogueException : Exception
{
    public DialogueException(string message) : base(message) { }
}

