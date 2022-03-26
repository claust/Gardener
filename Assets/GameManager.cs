using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject _tilePrefab;

    [SerializeField]
    GameObject parent;

    float prefabX { get => _tilePrefab.GetComponent<Renderer>().bounds.size.x; }
    float prefabZ { get => _tilePrefab.GetComponent<Renderer>().bounds.size.z; }

    [SerializeField]
    int sizeX = 10;
    int sizeZ = 10;

    // Start is called before the first frame update
    void Start()
    {
        CreateTiles();

    }

    private void CreateTiles()
    {
        // var parent = GameObject.FindGameObjectWithTag("tiles");
        var px = prefabX * 1.1f;
        var pz = prefabZ * 1.1f;
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                // Why does adding the tiles to a parent make them fall?
                // Instantiate(_tilePrefab, new Vector3(px * x, 1, pz * z), Quaternion.identity, parent.transform);
                Instantiate(_tilePrefab, new Vector3(px * x, 1, pz * z), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
