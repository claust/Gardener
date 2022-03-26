using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject Cursor;
    public GameManager GameManager;

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
