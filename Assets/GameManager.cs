using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject DirtPrefab;
    [SerializeField]
    GameObject GrassPrefab;

    [SerializeField]
    public GameObject Cursor;

    [SerializeField]
    GameObject Parent;

    ToolType _selectedTool = ToolType.GrassRemover;

    Tile[,] _tiles;

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
            }
        }
    }

    public void OnToolClicked(ToolType toolClicked)
    {
        Debug.Log($"OnToolClicked: {toolClicked}");
    }

    public void OnTileClicked(TileScript tile)
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
                break;
        }

    }

    void RemoveGrass(TileScript tile)
    {
        Debug.Log("RemoveGrass");
        if (_tiles[tile.x, tile.z].Type == TileType.Grass)
        {
            Debug.Log("Removing grass");
            _tiles[tile.x, tile.z] = new Tile(TileType.Dirt);
            var newTile = tile.CloneAsType(DirtPrefab);
            Destroy(tile);
        } else
        {
            Debug.Log("No grass to remove on this tile");
        }
    }
}

public enum ToolType
{
    None, GrassRemover, WateringCan
}
