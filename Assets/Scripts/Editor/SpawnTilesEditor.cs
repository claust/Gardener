using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpawnTilesEditor : EditorWindow
{
    [MenuItem("Tools/Tiles Spawner")]
    public static void ShowWindow()
    {
        GetWindow<SpawnTilesEditor>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn all tiles", EditorStyles.boldLabel);
        if (GUILayout.Button("Spawn"))
        {
            FindObjectOfType<GameManager>().CreateTiles();
        }
        if (GUILayout.Button("Remove all"))
        {
            foreach(Transform child in FindObjectsOfType<GameObject>().Where(go => go.name == "Tiles").First().transform)
            {
                Destroy(child.gameObject);
            }
        }

    }
}
