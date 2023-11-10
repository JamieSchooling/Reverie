using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public static class ReplaceButtonPromptSprite
{
    public static string ReadAndReplaceBinding(string textToDisplay, InputBinding[] actionsNeeded, TMP_SpriteAsset spriteAsset)
    {
        string[] stringButtonNames = new string[actionsNeeded.Length];
        for (int i = 0; i < stringButtonNames.Length; i++)
        {
            stringButtonNames[i] = actionsNeeded[i].ToString();
            stringButtonNames[i] = RenameInput(stringButtonNames[i]);
        }

        for (int i = 0; i < stringButtonNames.Length; i++)
        {
            textToDisplay = textToDisplay.Replace($"BUTTONPROMPT{i}", $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonNames[i]}\">");
        }


        return textToDisplay;
    }

    private static string RenameInput(string stringButtonName)
    {
        stringButtonName =  stringButtonName.Split(':')[1];
        stringButtonName = stringButtonName.Replace("Interact:", string.Empty);
        stringButtonName = stringButtonName.Replace("<Keyboard>/", "Keyboard_");
        stringButtonName = stringButtonName.Replace("[Keyboard]", string.Empty);
        stringButtonName = stringButtonName.Replace("<Gamepad>/", "Gamepad_");
        stringButtonName = stringButtonName.Replace("[Gamepad]", string.Empty);

        return stringButtonName;
    }
}
