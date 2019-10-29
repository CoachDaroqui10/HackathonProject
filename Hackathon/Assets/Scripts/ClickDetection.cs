using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickDetectlion : MonoBehaviour
{

    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        //You can serialize this and assign it in the editor instead
        tilemap = GameObject.Find("Suelo").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Pulsado");

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);

            if (tilemap.HasTile(gridPos))
                Debug.Log("Hello World from " + gridPos);
        }
    }
}
