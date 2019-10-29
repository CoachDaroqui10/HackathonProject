using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrios : MonoBehaviour
{
    public int precioAceptable;

    private float felicidadBarrio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mediaFelicidad();
    }

    void mediaFelicidad()
    {
        float sumatorio = 0;

        foreach (var bloque in transform.GetComponentsInChildren<Bloque>())
        {
            sumatorio += bloque.getFelicidad();
        }

        felicidadBarrio = sumatorio / transform.GetComponentsInChildren<Bloque>().Length;
    }

    public float getFelicidad()
    {
        return felicidadBarrio;
    }
}
