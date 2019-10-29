using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public Image fondo;

    public Sprite inicio;
    public Sprite construccion;
    public Sprite informacion;

    public Image caraFelicidad;
    public Sprite feliz;
    public Sprite neutro;
    public Sprite enfadado;
    public Text barrioText;
    public Text precioText;

    public Button b_Construir;
    public Button b_Tuberias;
    public Button b_Edificio;
    public Button b_Juntas;

    public Button b_CancelConstruir;
    public Button b_CancelInformation;

    public SelectionManagerMac select;
    public Barrios barrios;

    public Text agua;

    public Image[] tutorial;
    public GameObject barrioInfo;

    private int tutorialImagen = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agua.text = GameManager.instance.precioAgua.ToString();
        if (GameManager.instance.boolBarrio)
        {
            ShowBarrio();
        }
        else
        {
            barrioInfo.SetActive(false);
        }
    }

    public void updateTutorial()
    {
        tutorial[tutorialImagen].gameObject.SetActive(false);
        if (tutorialImagen < 4)
        {
            tutorial[tutorialImagen + 1].gameObject.SetActive(true);
            tutorialImagen += 1;
        }
        else
        {
            GameManager.instance.tutorial = false;
        }
    }

    public void ShowConstruction()
    {
        fondo.sprite = construccion;
        b_CancelConstruir.gameObject.SetActive(true);
        b_Tuberias.gameObject.SetActive(true);
        b_Edificio.gameObject.SetActive(true);
        b_Juntas.gameObject.SetActive(true);
    }

    public void CancelConstruction()
    {
        fondo.sprite = inicio;
        b_CancelConstruir.gameObject.SetActive(false);
        b_Tuberias.gameObject.SetActive(false);
        b_Edificio.gameObject.SetActive(false);
        b_Juntas.gameObject.SetActive(false);
    }

    public void CancelInformation()
    {
        barrioInfo.gameObject.SetActive(false);
        b_CancelInformation.gameObject.SetActive(false);
    }

    private void ShowBarrio()
    {
        barrioInfo.SetActive(true);

        fondo.sprite = informacion;
        Barrios barrio = GameObject.Find(select.getBarrio()).GetComponent<Barrios>();

        b_CancelInformation.gameObject.SetActive(true);

        if (barrio.getFelicidad() < 33)
        {
            caraFelicidad.sprite = enfadado;
            precioText.text = "Los habitantes de este barrio creen que el precio del agua es demasiado alto";
        }
        else if (barrio.getFelicidad() > 66)
        {
            caraFelicidad.sprite = feliz;
            precioText.text = "Los habitantes de este barrio creen que el precio del agua es perfecto";
        }
        else
        {
            caraFelicidad.sprite = neutro;
            precioText.text = "Los habitantes de este barrio creen que el precio del agua es aceptable";
        }

        barrioText.text = select.getBarrio();
    }
}
