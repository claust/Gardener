using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public GameObject Cursor;
    public GameManager GameManager;
    public Tile Tile { get; set; }

    public int x;
    public int z;

    Color OriginalColor { get; set; }
    bool _tileClickedAtLeastOnce = false;

    public void OnEnable()
    {
        OriginalColor = GetComponent<Renderer>().material.color;
    }
    public void Update()
    {
        if (Tile != null)
        {
            if (Tile.Type == TileType.Dirt)
            {
                var color = Color.white;
                color.r = Tile.Dryness * OriginalColor.r;
                color.g = Tile.Dryness * OriginalColor.g;
                color.b = Tile.Dryness * OriginalColor.b;
                GetComponent<Renderer>().material.color = color;
                Tile.WaterLevel = 0.9999f * Tile.WaterLevel;
            }
        }
        else
        {
            Debug.Log("No tile?");
        }
    }

    public GameObject CloneAsType(GameObject tilePrefab, Tile tile)
    {
        var newTile = Instantiate(tilePrefab, transform.position, Quaternion.identity);
        newTile.transform.position = newTile.transform.position + Vector3.up * 0.01f;
        var tileScript = newTile.GetComponent<TileScript>();
        tileScript.Cursor = Cursor;
        tileScript.GameManager = GameManager;
        tileScript.x = x;
        tileScript.z = z;
        tileScript.Tile = tile;
        return newTile;
    }

    private void OnMouseDown()
    {
        GameManager.OnTileClicked(this);
        _tileClickedAtLeastOnce = true;
    }

    private void OnMouseEnter()
    {
        Cursor.transform.position = transform.position + Vector3.up * 0.2f;
        Cursor.SetActive(true);
        var mouseDown = Input.GetMouseButton(0);
        if (mouseDown && !_tileClickedAtLeastOnce)
        {
            OnMouseDown();
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetActive(false);
    }
}
