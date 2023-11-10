using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Basic Dialogue Node")]
public class BasicDialogueNode : DialogueNode
{
    [SerializeField] private DialogueNode _nextNode;
    public DialogueNode NextNode => _nextNode;


    public override bool CanBeFollowedByNode(DialogueNode node)
    {
        return _nextNode == node;
    }

    public override void Accept(DialogueNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}
