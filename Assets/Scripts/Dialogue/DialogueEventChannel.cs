using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Event Channel")]
public class DialogueEventChannel : ScriptableObject
{
    public event Action<Dialogue> OnDialogueRequested;
    public event Action<Dialogue> OnDialogueStart;
    public event Action<Dialogue> OnDialogueEnd;

    public event Action<DialogueNode> OnDialogueNodeRequested;
    public event Action<DialogueNode> OnDialogueNodeStart;
    public event Action<DialogueNode> OnDialogueNodeEnd;

    public void RaiseRequestDialogue(Dialogue dialogue) => OnDialogueRequested?.Invoke(dialogue);
    public void RaiseStartDialogue(Dialogue dialogue) => OnDialogueStart?.Invoke(dialogue);
    public void RaiseEndDialogue(Dialogue dialogue) => OnDialogueEnd?.Invoke(dialogue);

    public void RaiseRequestDialogueNode(DialogueNode node) => OnDialogueNodeRequested?.Invoke(node);
    public void RaiseStartDialogueNode(DialogueNode node) => OnDialogueNodeStart?.Invoke(node);
    public void RaiseEndDialogueNode(DialogueNode node) => OnDialogueNodeEnd?.Invoke(node);
}
