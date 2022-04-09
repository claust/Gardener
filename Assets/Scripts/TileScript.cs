using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public GameObject Cursor;
    public GameManager GameManager;

    public int x;
    public int z;

    bool _tileClickedAtLeastOnce = false;

    public GameObject CloneAsType(GameObject tilePrefab)
    {
        var newTile = Instantiate(tilePrefab, transform.position, Quaternion.identity);
        newTile.transform.position = newTile.transform.position + Vector3.up * 0.01f;
        var tileScript = tilePrefab.GetComponent<TileScript>();
        tileScript.Cursor = Cursor;
        tileScript.GameManager = GameManager;
        tileScript.x = x;
        tileScript.z = z;
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
        // Debug.Log($"Mouse Down {mouseDown}");
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
