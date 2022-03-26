using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject Cursor;
    public GameManager GameManager;

    public int x;
    public int z;

    public GameObject CloneAsType(GameObject tile)
    {
        var newTile = Instantiate(tile, transform.position, Quaternion.identity);
        newTile.transform.position = newTile.transform.position + Vector3.up * 0.01f;
        var tileScript = tile.GetComponent<Tile>();
        tileScript.Cursor = Cursor;
        tileScript.GameManager = GameManager;
        tileScript.x = x;
        tileScript.z = z;
        return newTile;
    }

    private void OnMouseDown()
    {
        GameManager.OnTileClicked(this);
    }

    private void OnMouseEnter()
    {
        Cursor.transform.position = transform.position + Vector3.up * 0.2f;
        Cursor.SetActive(true);
    }

    private void OnMouseExit()
    {
        Cursor.SetActive(false);
    }
}
