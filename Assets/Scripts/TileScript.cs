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
    public Tile Tile { get; set; }

    public int x { get; set; }
    public int z { get; set; }

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
                Tile.WaterLevel = 0.999f * Tile.WaterLevel;
            }
        }
        else
        {
            Debug.Log("No tile?");
        }
    }

    public void SetTile(Tile tile)
    {
        Tile = tile;
        if (Tile.Type == TileType.Grass)
        {
            GetComponent<Renderer>().material.color = Color.Lerp(ColorFrom, ColorTo, Random.value);
        }
    }

    public GameObject CloneAsType(GameObject tilePrefab, Tile tile)
    {
        var newTile = Instantiate(tilePrefab, transform.position, Quaternion.identity);
        newTile.transform.position = newTile.transform.position - Vector3.up * 0.05f;
        var tileScript = newTile.GetComponent<TileScript>();
        tileScript.Cursor = Cursor;
        tileScript.GameManager = GameManager;
        tileScript.x = x;
        tileScript.z = z;
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
