using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable class that describes the basic data of items
/// </summary>
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject model;
    [TextArea] public string description;
}
