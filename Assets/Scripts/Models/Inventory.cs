using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    public int SlotsFilled
    {
        get
        {
            return List.Count;
        }
    }

    private int _maxSlots = 6;
    public int MaxSlots
    {
        get
        {
            return _maxSlots;
        }
        set
        {
            _maxSlots = value;
        }
    }

    public List<InventoryItem> List = new();

    public bool Add(InventoryItem item)
    {
        InventoryItem existingItem = List.Where(i => i.Type == item.Type).FirstOrDefault();
        if (existingItem == null)
        {
            if (List.Count < MaxSlots)
            {
                List.Add(item);
                return true;
            }
        }
        else
        {
            existingItem.Quantity += 1;
            return true;
        }
        return false;
    }

    public bool HasRoomFor(InventoryItemType type)
    {
        return SlotsFilled < MaxSlots || List.Exists(i => i.Type == type);
    }
}
