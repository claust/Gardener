using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameManager GameManager { get; set; }

    public Color color = Color.white;

    public void Start()
    {
        Debug.Log("Shop Start");
    }

    private void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter");
    }

    private void OnMouseExit()
    {
        Debug.Log("OnMouseExit");
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMousedown");
        GameManager.OnShopClicked();
    }
}
