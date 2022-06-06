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
    GameObject StonePrefab;
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
    [SerializeField]
    GameObject ShopObject;
    
    public Func<bool> IsMouseOverHUD;

    [SerializeField]
    public int Coins = 100;
    public Inventory Inventory;
    public Inventory ShopInventory;
    private World World = new();
    private MainView HUD;
    private ShopScript Shop;

    public ToolType SelectedTool = ToolType.GrassRemover;

    Tile[,] _tiles;

    int LastClickTick;
    int Ticks = 0;
    readonly float _tileMargin = 0f;
    float _tileWidth = 0;
    float _tileHeight = 0;
    float PrefabX
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
    float PrefabZ
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
        HUD = UI.GetComponent<MainView>();
        Shop = ShopObject.GetComponent<ShopScript>();
        Shop.GameManager = this;
        
        ResourceLoader.Initialize();
        RemoveEditorTiles();
        LoadWorld();
        CreateTiles();
    }

    void OnApplicationQuit()
    {
        SaveWorld();
    }

    private void LoadWorld()
    {
        World = SavedGameUtility.LoadWorld();
        if (World == null)
        {
            _tiles = new Tile[Globals.WorldSize, Globals.WorldSize];
            for (int x = 0; x < Globals.WorldSize; x++)
            {
                for (int z = 0; z < Globals.WorldSize; z++)
                {
                    _tiles[x, z] = new Tile(TileType.Grass);
                }
            }
            Coins = 100;
            Inventory = new();
            ShopInventory = new();
            BuySeeds();
        }
        else
        {
            _tiles = new Tile[Globals.WorldSize, Globals.WorldSize];
            for (int x = 0; x < Globals.WorldSize; x++)
            {
                for (int z = 0; z < Globals.WorldSize; z++)
                {
                    _tiles[x, z] = Tile.FromSaved(World.Tiles[x, z]);
                }
            }
            Coins = World.Coins;
            Inventory = Inventory.FromSaved(World.Inventory);
            ShopInventory = Inventory.FromSaved(World.ShopInventory);
            SelectedTool = World.SelectedTool;
            Debug.Log($"Previously selected toll: {SelectedTool}");
        }
        UpdateHUD();
    }

    private void SaveWorld()
    {
        World = new()
        {
            Tiles = new TileSaved[Globals.WorldSize, Globals.WorldSize],
            Coins = Coins,
            Inventory = Inventory.ToSaved(),
            ShopInventory = ShopInventory.ToSaved(),
            SelectedTool = SelectedTool,
        };
        for (int x = 0; x < Globals.WorldSize; x++)
        {
            for (int z = 0; z < Globals.WorldSize; z++)
            {
                World.Tiles[x, z] = _tiles[x, z].ToSaved();
            }
        }
        SavedGameUtility.SaveWorld(World);
    }

    private void FixedUpdate()
    {
        Ticks += 1;
        foreach (Tile t in _tiles)
        {
            if (t.Plant != null)
            {
                t.Plant.TransitionIfNeeded(Ticks);
            }
        };
    }

    public void CreateTiles()
    {
        var px = PrefabX * (1 + _tileMargin);
        var pz = PrefabZ * (1 + _tileMargin);
        for (int x = 0; x < Globals.WorldSize; x++)
        {
            for (int z = 0; z < Globals.WorldSize; z++)
            {
                var tile = _tiles[x, z];
                GameObject prefab;
                switch (tile.Type)
                {
                    case TileType.Dirt:
                        prefab = DirtPrefab;
                        break;
                    case TileType.Stone:
                        prefab = StonePrefab;
                        break;
                    default:
                    case TileType.Grass:
                        prefab = GrassPrefab;
                        break;
                }
                var tileGO = Instantiate(prefab, new Vector3(px * x, -0.5f, pz * z), Quaternion.identity);
                tileGO.transform.parent = _container.transform;
                var tileScript = tileGO.GetComponent<TileScript>();
                tileScript.Cursor = Cursor;
                tileScript.GameManager = this;
                tileScript.X = x;
                tileScript.Z = z;
                tileScript.SetTile(tile);
                if (tile.Plant != null)
                {
                    var pos = tileScript.transform.position;
                    var newPos = new Vector3(pos.x - 0.2f, pos.y + 0.2f, pos.z);
                    var seed = tile.Plant.GameObject;
                    seed.transform.position = newPos;
                }
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
    public void OnShopClicked()
    {
        Debug.Log("OnShopClicked");
        HUD.ShowShop();
    }

    public void OnToolClicked(ToolType toolClicked)
    {
        Debug.Log($"OnToolClicked: {toolClicked}");
        SelectedTool = toolClicked;
    }

    public void OnTileClicked(TileScript tile)
    {
        if (!IsMouseOverHUD())
        {
            Debug.Log("OnTileClicked");
            switch (SelectedTool)
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
                case ToolType.Stone:
                    LayStoneTile(tile);
                    break;
                default:
                    Debug.Log($"Unknown selected tool: {SelectedTool}");
                    break;
            }
        }
    }

    private void PlantSeed(TileScript tileScript)
    {
        Debug.Log("Plant seed");
        var tile = _tiles[tileScript.X, tileScript.Z];
        if (tile.Type == TileType.Dirt && tile.Plant == null)
        {
            var plant = ResourceLoader.GetPlant(InventoryItemType.Tomato, 0, Ticks);
            InventoryItemType? seedType;
            switch (plant.HarvestableType)
            {
                case InventoryItemType.Tomato:
                    seedType = InventoryItemType.TomatoSeeds;
                    break;
                case InventoryItemType.Cucumber:
                    seedType = InventoryItemType.CucumberSeeds;
                    break;
                default:
                    Debug.Log($"Cannot plant {plant.Name}. Unknown seed");
                    return;
            }
            if (!Inventory.Remove((InventoryItemType)seedType))
            {
                Debug.Log($"Cannot plant {plant.Name}. No {seedType} in inventory");
                return;
            };

            var pos = tileScript.gameObject.transform.position;
            Debug.Log($"Planting seed at {pos.x},{pos.z}");
            var newPos = new Vector3(pos.x - 0.2f, pos.y + 0.2f, pos.z);
            tile.Plant = plant;

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
        if (_tiles[ts.X, ts.Z].Type == TileType.Grass)
        {
            Debug.Log("Removing grass");
            _tiles[ts.X, ts.Z] = new Tile(TileType.Dirt);
            ts.CloneAsType(DirtPrefab, _tiles[ts.X, ts.Z], _container.transform);
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
        var tile = _tiles[ts.X, ts.Z];
        var plant = tile.Plant;
        if (plant != null && Inventory.HasRoomFor(plant.HarvestableType))
        {
            if (tile.Plant.Harvest(Ticks))
            {
                Coins += 1;
                var harvested = ResourceLoader.GetInventoryItem(plant.HarvestableType, plant.HarvestableQuantity);
                Inventory.Add(harvested);
                UpdateHUD();
            };
        }
    }

    private void LayStoneTile(TileScript ts)
    {
        Debug.Log($"LayStoneTile {Ticks - LastClickTick}");
        if (Ticks - LastClickTick < 20)
        {
            Debug.Log("Not yet");
            return;
        }
        LastClickTick = Ticks;
        var tile = _tiles[ts.X, ts.Z];
        if (tile.Type == TileType.Dirt && tile.Plant == null)
        {
            Debug.Log("Laying stone tile");
            _tiles[ts.X, ts.Z] = new Tile(TileType.Stone);
            ts.CloneAsType(StonePrefab, _tiles[ts.X, ts.Z], _container.transform);
            Destroy(ts.gameObject);
        }
        else if (tile.Type == TileType.Stone)
        {
            Debug.Log("Removing stone tile");
            _tiles[ts.X, ts.Z] = new Tile(TileType.Dirt);
            ts.CloneAsType(DirtPrefab, _tiles[ts.X, ts.Z], _container.transform);
            Destroy(ts.gameObject);
        }
    }

    private void BuySeeds()
    {
        var seeds = ResourceLoader.GetInventoryItem(InventoryItemType.TomatoSeeds, 2);
        Inventory.Add(seeds);
    }

    private void UpdateHUD()
    {
        HUD.Bind();
    }
}
