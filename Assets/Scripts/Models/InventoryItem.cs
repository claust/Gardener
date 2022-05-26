using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public int Quantity { get; set; }
    public string Name { get { return _data.Name; } }
    public string Description { get { return _data.Description; } }
    public Sprite Icon { get { return _data.Icon; } }
    public InventoryItemType Type { get { return _data.Type; } }
    private InventoryItemData _data;

    public InventoryItem(InventoryItemData data)
    {
        _data = data;
    }
}

public enum InventoryItemType
{
    Tomato, TomatoSeeds, Cucumber, CucumberSeeds
}
