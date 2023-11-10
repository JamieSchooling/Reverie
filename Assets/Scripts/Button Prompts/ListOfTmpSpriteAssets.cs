using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Button Prompts/List of TMP Sprite Assets")]
public class ListOfTmpSpriteAssets : ScriptableObject
{
    public List<TMP_SpriteAsset> SpriteAssets;
}
