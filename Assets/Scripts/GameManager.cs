using Assets.Scripts.Models;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject _container;
    [SerializeField]
    GameObject UI;
    [SerializeField]
    GameObject DirtPrefab;
    [SerializeField]
    GameObject GrassPrefab;
    [SerializeField]
    GameObject TomatoSeedPrefab;
    [SerializeField]
    GameObject TomatoSproutPrefab;
    [SerializeField]
    GameObject TomatoPlantPrefab;
    [SerializeField]
    GameObject TomatoPlantWithTomatoesPrefab;

    [SerializeField]
    public GameObject Cursor;

    public Func<bool> IsMouseOverHUD;

    [SerializeField]
    public int Coins = 100;
    public Inventory Inventory = new();

    ToolType _selectedTool = ToolType.GrassRemover;
    List<PlantData> _plants = new List<PlantData>();
    List<InventoryItemData> _inventoryItems = new List<InventoryItemData>();
    Tile[,] _tiles;

    int _ticks = 0;
    float _tileMargin = 0f;
    float _tileWidth = 0;
    float _tileHeight = 0;
    float _prefabX
    {
        get
        {
            if (_tileWidth == 0)
            {
                _tileWidth = DirtPrefab.GetComponent<Renderer>().bounds.size.x;
            }
            return _tileWidth;
        }
    }
    float _prefabZ
    {
        get
        {
            if (_tileHeight == 0)
            {
                _tileHeight = DirtPrefab.GetComponent<Renderer>().bounds.size.z;
            }
            return _tileHeight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RemoveEditorTiles();
        CreateTiles();
        InitializePlants();
        InitializeInventoryItems();
        BuySeeds();
    }

    private void InitializePlants()
    {
        _plants.AddRange(Resources.LoadAll<PlantData>("Plants"));
    }

    private void InitializeInventoryItems()
    {
        _inventoryItems.AddRange(Resources.LoadAll<InventoryItemData>("InventoryItems"));
    }

    private void FixedUpdate()
    {
        _ticks += 1;
        foreach (Tile t in _tiles)
        {
            if (t.Plant != null)
            {
                t.Plant.TransitionIfNeeded(_ticks);
            }
        };
    }

    public void CreateTiles()
    {
        _tiles = new Tile[Globals.WorldSize, Globals.WorldSize];
        // var parent = GameObject.FindGameObjectWithTag("tiles");
        var px = _prefabX * (1 + _tileMargin);
        var pz = _prefabZ * (1 + _tileMargin);
        for (int x = 0; x < Globals.WorldSize; x++)
        {
            for (int z = 0; z < Globals.WorldSize; z++)
            {
                // Why does adding the tiles to a parent make them fall?
                // Instantiate(_tilePrefab, new Vector3(px * x, 1, pz * z), Quaternion.identity, parent.transform);
                var tile = Instantiate(GrassPrefab, new Vector3(px * x, -0.5f, pz * z), Quaternion.identity);
                tile.transform.parent = _container.transform;
                var tileScript = tile.GetComponent<TileScript>();
                tileScript.Cursor = Cursor;
                tileScript.GameManager = this;
                tileScript.x = x;
                tileScript.z = z;
                _tiles[x, z] = new Tile(TileType.Grass);
                tileScript.SetTile(_tiles[x, z]);
            }
        }
    }

    public void RemoveEditorTiles()
    {
        var parent = FindObjectsOfType<GameObject>().Where(go => go.name == "Tiles").First().transform;
        var list = new List<GameObject>();
        foreach (Transform child in parent)
        {
            list.Add(child.gameObject);
        }
        for (int i = 0; i < list.Count; i++)
        {
            GameObject child = list[i];
            DestroyImmediate(child);
        }
    }

    public void OnToolClicked(ToolType toolClicked)
    {
        Debug.Log($"OnToolClicked: {toolClicked}");
        _selectedTool = toolClicked;
    }

    public void OnTileClicked(TileScript tile)
    {
        if (!IsMouseOverHUD())
        {
            Debug.Log("OnTileClicked");
            switch (_selectedTool)
            {
                case ToolType.None:
                    Debug.Log("No tool selected");
                    break;
                case ToolType.GrassRemover:
                    RemoveGrass(tile);
                    break;
                case ToolType.WateringCan:
                    Water(tile);
                    break;
                case ToolType.Seeder:
                    PlantSeed(tile);
                    break;
                case ToolType.Scissors:
                    Harvest(tile);
                    break;
                default:
                    Debug.Log($"Unknown selected tool: {_selectedTool}");
                    break;
            }
        }
    }

    private void PlantSeed(TileScript tileScript)
    {
        Debug.Log("Plant seed");
        var tile = _tiles[tileScript.x, tileScript.z];
        if (tile.Type == TileType.Dirt && tile.Plant == null)
        {
            var plantData = _plants[UnityEngine.Random.Range(0, _plants.Count)];
            InventoryItemType? seedType;
            switch (plantData.HarvestableType)
            {
                case InventoryItemType.Tomato:
                    seedType = InventoryItemType.TomatoSeeds;
                    break;
                case InventoryItemType.Cucumber:
                    seedType = InventoryItemType.CucumberSeeds;
                    break;
                default:
                    Debug.Log($"Cannot plant {plantData.Name}. Unknown seed");
                    return;
            }
            if (!Inventory.Remove((InventoryItemType)seedType))
            {
                Debug.Log($"Cannot plant {plantData.Name}. No {seedType} in inventory");
                return;
            };

            var pos = tileScript.gameObject.transform.position;
            Debug.Log($"Planting seed at {pos.x},{pos.z}");
            var newPos = new Vector3(pos.x - 0.2f, pos.y + 0.2f, pos.z);
            tile.Plant = new Plant(plantData, _ticks);


            Debug.Log($"Planted {tile.Plant.Name}");
            var seed = tile.Plant.GameObject;
            seed.transform.position = newPos;
        }
        else if (tile.Plant != null)
        {
            Debug.Log($"Cannot plant on top of {tile.Plant.Name}");
        }
        else
        {
            Debug.Log($"Can only plant seed on dirt, not on {tile.Type}");
        }
    }

    private void Water(TileScript ts)
    {
        Debug.Log("Water");
        if (ts.Tile.Type == TileType.Dirt)
        {
            Debug.Log("Watering dirt");
            ts.Tile.Water();
        }
        else
        {
            Debug.Log("No dirt to water on this tile");
        }
    }

    void RemoveGrass(TileScript ts)
    {
        Debug.Log("RemoveGrass");
        if (_tiles[ts.x, ts.z].Type == TileType.Grass)
        {
            Debug.Log("Removing grass");
            _tiles[ts.x, ts.z] = new Tile(TileType.Dirt);
            var newTile = ts.CloneAsType(DirtPrefab, _tiles[ts.x, ts.z], _container.transform);
            Destroy(ts.gameObject);
        }
        else
        {
            Debug.Log("No grass to remove on this tile");
        }
    }

    private void Harvest(TileScript ts)
    {
        Debug.Log("Harvest");
        var tile = _tiles[ts.x, ts.z];
        var plant = tile.Plant;
        if (plant != null && Inventory.HasRoomFor(plant.HarvestableType))
        {
            if (tile.Plant.Harvest(_ticks))
            {
                Coins += 1;
                var harvested = new InventoryItem(_inventoryItems.First(i => i.Type == plant.HarvestableType))
                {
                    Quantity = 1
                };
                Inventory.Add(harvested);
                UI.GetComponent<MainView>().SetCoins(Coins);
            };
        }
    }

    private void BuySeeds()
    {
        Inventory.Add(new InventoryItem(_inventoryItems[1])
        {
            Quantity = 2,
        });
    }
}
