using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject _tilePrefab;

    [SerializeField]
    GameObject Parent;

    float _tileMargin = 0.02f;
    float _tileWidth = 0;
    float _tileHeight = 0;
    float _prefabX
    {
        get
        {
            if (_tileWidth == 0)
            {
                _tileWidth = _tilePrefab.GetComponent<Renderer>().bounds.size.x;
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
                _tileHeight = _tilePrefab.GetComponent<Renderer>().bounds.size.z;
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
        // var parent = GameObject.FindGameObjectWithTag("tiles");
        var px = _prefabX * (1 + _tileMargin);
        var pz = _prefabZ * (1 + _tileMargin);
        for (int x = 0; x < Globals.WorldSize; x++)
        {
            for (int z = 0; z < Globals.WorldSize; z++)
            {
                // Why does adding the tiles to a parent make them fall?
                // Instantiate(_tilePrefab, new Vector3(px * x, 1, pz * z), Quaternion.identity, parent.transform);
                Instantiate(_tilePrefab, new Vector3(px * x, -0.5f, pz * z), Quaternion.identity);
            }
        }
    }
}
