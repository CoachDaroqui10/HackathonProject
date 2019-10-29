using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloque : MonoBehaviour
{
    public Vector3Int bloqueInicio;
    public Vector3Int bloqueFin;

    private bool suministrado;

    private int precioAceptable;

    private int happinessLevel;

    private int previousDay = 0;

    // Start is called before the first frame update
    void Start()
    {
        happinessLevel = 50 + Random.Range(-10, 10);

        precioAceptable = transform.GetComponentInParent<Barrios>().precioAceptable;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.getDia() != previousDay)
        {
            previousDay = GameManager.instance.getDia();

            ajustarFelicidad();

            //CheckHappiness();
        }
    }

    private void ajustarFelicidad()
    {
        //Segun suministro
        if (suministrado)
        {
            if (GameManager.instance.precioAgua < precioAceptable + 3)
            {
                if (happinessLevel < 100)
                {
                    happinessLevel += 2 + GameManager.instance.nivelDelAgua;
                }
                else
                {
                    happinessLevel = 100;
                }
            }
            else
            {
                if (happinessLevel > 0)
                {
                    happinessLevel -= 1 - GameManager.instance.nivelDelAgua;
                }
                else
                {
                    happinessLevel = 0;
                }
            }
        }
        else
        {
            if (GameManager.instance.precioAgua < precioAceptable)
            {
                if (happinessLevel < 100)
                {
                    happinessLevel += 1 + GameManager.instance.nivelDelAgua;
                }
                else
                {
                    happinessLevel = 100;
                }
            }
            else
            {
                if(happinessLevel > 0)
                {
                    happinessLevel -= 2 - GameManager.instance.nivelDelAgua;
                }
                else
                {
                    happinessLevel = 0;
                }
            }
        }
    }

    private void CheckHappiness()
    {
        if (happinessLevel < 33)
            print("ENFADADOS " + transform.name + " : " + happinessLevel + " : " + suministrado);
        else if (happinessLevel > 66)
            print("FELICES " + transform.name + " : " + happinessLevel + " : " + suministrado);
        else
            print("NEUTRO " + transform.name + " : " + happinessLevel + " : " + suministrado);
    }

    public void setSuministrado(bool suministro)
    {
        suministrado = suministro;
    }

    public bool getSuministrado()
    {
        return suministrado;
    }

    public int getFelicidad()
    {
        return happinessLevel;
    }
}
