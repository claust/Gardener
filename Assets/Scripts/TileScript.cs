using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField]
    private Color ColorFrom;
    [SerializeField]
    private Color ColorTo;
    public GameObject Cursor { get; set; }
    public GameManager GameManager { get; set; }
    public Tile Tile { get; private set; }

    private Color _colorOriginal;

    public int X { get; set; }
    public int Z { get; set; }

    bool _tileClickedAtLeastOnce = false;
    static readonly int WaterLevel = Shader.PropertyToID("_WaterLevel");

    public void Update()
    {
        if (Tile != null)
        {
            if (Tile.Type == TileType.Dirt && Tile.WaterLevel > 0.01)
            {
                // GetComponent<Renderer>().material.color = Color.Lerp(Color.black, _colorOriginal, Tile.Dryness);
                GetComponent<Renderer>().sharedMaterial.SetFloat(WaterLevel, Tile.WaterLevel);
                Tile.WaterLevel = 0.9992f * Tile.WaterLevel;
            }
        }
        else
        {
            Debug.Log($"No tile at {X},{Z}?");
        }
    }

    public void SetTile(Tile tile)
    {
        Tile = tile;
        _colorOriginal = Color.Lerp(ColorFrom, ColorTo, Random.value);
        GetComponent<Renderer>().material.color = _colorOriginal;
    }

    public GameObject CloneAsType(GameObject tilePrefab, Tile tile, Transform parent)
    {
        var newTile = Instantiate(tilePrefab, transform.position, Quaternion.identity);
        newTile.transform.parent = parent;
        newTile.transform.position = newTile.transform.position; //  - Vector3.up * 0.05f;
        var tileScript = newTile.GetComponent<TileScript>();
        tileScript.Cursor = Cursor;
        tileScript.GameManager = GameManager;
        tileScript.X = X;
        tileScript.Z = Z;
        tileScript.SetTile(tile);
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
