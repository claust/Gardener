using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public int Quantity;
    public string Name;
    public string Description;
    public InventoryItemType Type;
}

public enum InventoryItemType
{
    Tomato
}
