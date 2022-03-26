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

    [SerializeField]
    int SizeX = 10;
    int SizeZ = 10;

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
        var px = _prefabX * 1.1f;
        var pz = _prefabZ * 1.1f;
        for (int x = 0; x < SizeX; x++)
        {
            for (int z = 0; z < SizeZ; z++)
            {
                // Why does adding the tiles to a parent make them fall?
                // Instantiate(_tilePrefab, new Vector3(px * x, 1, pz * z), Quaternion.identity, parent.transform);
                Instantiate(_tilePrefab, new Vector3(px * x, -0.5f, pz * z), Quaternion.identity);
            }
        }
    }
}
