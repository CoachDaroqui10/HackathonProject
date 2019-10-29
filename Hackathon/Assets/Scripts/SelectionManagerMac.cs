using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionManagerMac : MonoBehaviour
{
    private Color tileColor;
    private bool bloqueSeleccionado;
    private Tilemap hitTileMap;
    private RaycastHit hit;

    private Vector3 origen;
    private Vector3 fin;

    // Start is called before the first frame update
    void Start()
    {
        origen = new Vector3(-9, -4, 0);
        fin = new Vector3(-4, -4, 0);

        
        if (Physics.Raycast(origen, origen - fin, out hit))
        {
            print("choca");
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.getConstruccionHabilitada() && !GameManager.instance.tutorial)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 auxPos = Input.mousePosition;
                auxPos.z = Mathf.Infinity;

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

                if (hit)
                {
                    print("Hay hit en el otro");

                    if (bloqueSeleccionado)
                    {
                        hitTileMap.color = Color.white;
                        GameManager.instance.showBarrio();
                        bloqueSeleccionado = false;
                    }
                    else
                    {
                        bloqueSeleccionado = true;

                        hitTileMap = hit.transform.gameObject.GetComponent<Tilemap>();

                        hitTileMap.color = Color.red;
                        GameManager.instance.showBarrio();

                        Vector3Int gridPos = hitTileMap.WorldToCell(mousePos);
                        print(gridPos);
                    }

                }
                else
                {
                    if (bloqueSeleccionado)
                    {
                        hitTileMap.color = Color.red;
                        GameManager.instance.showBarrio();
                        bloqueSeleccionado = false;
                    }
                }
            }
        }    
    }

    public string getBarrio()
    {
        return hitTileMap.transform.parent.name;
    }
}
