using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuberias : MonoBehaviour
{
    private List<Vector3Int> tuberias = new List<Vector3Int>();

    private List<Vector3Int> extremos = new List<Vector3Int>();
    private List<Vector3Int> extremosComprados = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        //extremosComprados.Add(new Vector3Int(7, 15, 0));
        comprobarExtremos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3Int> getTuberias()
    {
        return tuberias;
    }

    public List<Vector3Int> getExtremos()
    {
        return extremos;
    }

    public List<Vector3Int> getExtremosComprados()
    {
        return extremosComprados;
    }

    public void añadirTuberia(Vector3Int inicio, Vector3Int fin)
    {
        if (inicio.x == fin.x)
        {
            if (fin.y < inicio.y)
            {
                Vector3Int aux = fin;
                fin = inicio;
                inicio = aux;
            }

            for (int i = 0; i < fin.y - inicio.y + 1; i++)
            {
                tuberias.Add(new Vector3Int(inicio.x, inicio.y + i, 0));
            }
        }

        if (inicio.y == fin.y)
        {
            if (fin.x < inicio.x)
            {
                Vector3Int aux = fin;
                fin = inicio;
                inicio = aux;
            }

            for (int i = 0; i < fin.x - inicio.x + 1; i++)
            {
                Vector3Int tuberia = new Vector3Int(inicio.x + i, inicio.y, 0);
                if (!tuberias.Contains(tuberia))
                {
                    tuberias.Add(new Vector3Int(inicio.x + i, inicio.y, 0));
                }
            }
        }

        comprobarExtremos();
    }

    public void comprobarExtremos()
    {
        //extremos.Clear();

        for (int i = 0; i < tuberias.Count; i++)
        {
            //Comprobar X
            Vector3Int ladoIzquierdo = new Vector3Int(tuberias[i].x - 1, tuberias[i].y, 0);
            Vector3Int ladoDerecho = new Vector3Int(tuberias[i].x + 1, tuberias[i].y, 0);
            Vector3Int ladoArriba = new Vector3Int(tuberias[i].x, tuberias[i].y + 1, 0);
            Vector3Int ladoAbajo= new Vector3Int(tuberias[i].x, tuberias[i].y - 1, 0);

            int contador = 0;

            if (!tuberias.Contains(ladoIzquierdo) )
            {
                contador++;
            }
            if (!tuberias.Contains(ladoDerecho))
            {
                contador++;
            }
            if (!tuberias.Contains(ladoArriba))
            {
                contador++;
            }
            if (!tuberias.Contains(ladoAbajo))
            {
                contador++;
            }

            if (contador >= 3)
            {
                if (!extremos.Contains(tuberias[i]))
                {
                    extremos.Add(tuberias[i]);
                }
            }
        }

        for (int i = 0; i < extremosComprados.Count; i++)
        {
            if (!extremos.Contains(extremosComprados[i]))
            {
                extremos.Add(extremosComprados[i]);
            }
            else
            {
                print("Extremo existente");
            }
        }

        foreach (var extremo in extremos)
        {
            print("Extremo: " + extremo);
        }
    }
}
