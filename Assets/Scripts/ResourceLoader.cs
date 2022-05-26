using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ResourceLoader
{
    private static readonly List<InventoryItemData> InventoryItems = new();
    private static readonly List<PlantData> Plants = new();
    public static void Initialize()
    {
        InventoryItems.AddRange(Resources.LoadAll<InventoryItemData>("InventoryItems"));
        Plants.AddRange(Resources.LoadAll<PlantData>("Plants"));
    }

    public static InventoryItem GetInventoryItem(InventoryItemType type, int quantity)
    {
        return new InventoryItem(InventoryItems.First(i => i.Type == type))
        {
            Quantity = quantity
        };
    }
    public static Plant GetPlant(InventoryItemType type, int stage, int lastStageTransitionTick)
    {
        return new Plant(Plants.First(p => p.HarvestableType == type), stage, lastStageTransitionTick);
    }
}

