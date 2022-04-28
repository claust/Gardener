using Assets.Scripts.Models;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    int Coins = 100;

    ToolType _selectedTool = ToolType.GrassRemover;

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
        CreateTiles();
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

    private void CreateTiles()
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
            var pos = tileScript.gameObject.transform.position;
            Debug.Log($"Planting seed at {pos.x},{pos.z}");
            var newPos = new Vector3(pos.x - 0.2f, pos.y + 0.2f, pos.z);
            tile.Plant = Plant.Tomato(new GameObject[] { TomatoSeedPrefab, TomatoSproutPrefab, TomatoPlantPrefab, TomatoPlantWithTomatoesPrefab }, _ticks);
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
            var newTile = ts.CloneAsType(DirtPrefab, _tiles[ts.x, ts.z]);
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
        if (tile.Plant != null)
        {
            if (tile.Plant.Harvest(_ticks))
            {
                Coins += 1;
                UI.GetComponent<MainView>().SetCoins(Coins);
            };
        }
    }
}
