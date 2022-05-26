using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlantData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public int ticksPerTransition = 200;
    public List<GameObject> StagePrefabs;
    public InventoryItemType HarvestableType;
    public int HarvestableQuantity;
}
