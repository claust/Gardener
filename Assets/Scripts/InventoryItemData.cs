using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryItemData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public InventoryItemType Type;
}
