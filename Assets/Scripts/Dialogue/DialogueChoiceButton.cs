using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DialogueChoiceButton : MonoBehaviour
{
    [SerializeField] private DialogueEventChannel _dialogueChannel;
    [SerializeField] private TextMeshProUGUI _choice;

    private DialogueNode _choiceNextNode;

    public DialogueChoice Choice
    {
        set
        {
            _choice.text = value.ChoicePreview;
            _choiceNextNode = value.ChoiceNode;
            gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _dialogueChannel.RequestDialogueNode(_choiceNextNode);
    }
}