using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool tutorial = true;

    //Construir
    public Bloque[] bloques;
    public Tuberias tuberias;

    private bool construccionHabilitada = false;
    private bool comprarJunta = false;
    private bool desbloquearEdificio = false;

    public Tile tilaX;
    public Tile tilaY; 
    public Tile tilaExtremo;
    public Tile curva1;
    public Tile curva2;
    public Tile curva3;
    public Tile curva4;

    private Tilemap tilemap;
    private Tilemap provisionalTilemap;

    private bool primerClick = true;
    private bool segundoClick = false;
    private Vector3Int gridPosPrimer;
    private Vector3Int gridPosSegundo;
    private Vector3Int finalPos;

    private bool esExtremo = false;

    private const int limite = 12;

    //Simulacion
    public int precioAgua;
    public int dinero = 100;
    public int nivelDelAgua = 1;

    private int dia;

    //UI
    public Text textInformacion;
    public Image imagenInformacion;

    public Text textDinero;

    public bool boolBarrio = false;

    public HUDManager hud;

    //Crear instancia patrón Singleton
    void Awake()
    {
        if (instance == null) instance = this;
        else
            if (instance != this) Destroy(gameObject);


        tilemap = GameObject.Find("Tuberias").GetComponent<Tilemap>();
        provisionalTilemap = GameObject.Find("provisionalTuberias").GetComponent<Tilemap>();

    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while(true)
        {
            yield return new WaitForSeconds(2.5f);
            changeDay();
        }
    }

    // Update is called once per frame
    void Update()
    {

        clickNormal();

        if (construccionHabilitada)
        {
            mostrarOpciones();

            tilesClick();
        }

        elegirJuntaCompra();

        updateUI();
    }

    public int getDia()
    {
        return dia;
    }

    void changeDay()
    {
        if (!tutorial)
        {
            dia++;

            updateDinero();
        }
    }

    void updateDinero()
    {
        int ingresos = 3;
        for (int i = 0; i < bloques.Length; i++)
        {
            if (bloques[i].getFelicidad() > 33)
            {
                ingresos += precioAgua;
            }
            else
            {
                ingresos -= 2;
            }
        }

        dinero += ingresos;
    }

    void clickNormal()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!tutorial)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int pos = tilemap.WorldToCell(mousePos);

                print(pos);

                desbloquearEdificioClick(pos);
            }
            else
            {
                hud.updateTutorial();
            }
        }
    }

    void desbloquearEdificioClick(Vector3Int pos)
    {
        if (desbloquearEdificio)
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (hit)
            {
                print("Hay hit");
                Tilemap hitTile = hit.transform.GetComponent<Tilemap>();

                if (hitTile.transform.parent.name == "Construibles")
                {
                    if (hitTile.transform.name == "Deposito1" || hitTile.transform.name == "Deposito2" || hitTile.transform.name == "Deposito3" ||
                        hitTile.transform.name == "Deposito4" || hitTile.transform.name == "Deposito5")
                    {
                        print("Pulsado contruible " + true);
                        if (dinero > 15)
                        {
                            hitTile.color = Color.white;
                            dinero -= 15;
                            tuberias.getExtremosComprados().Add(pos);
                            tuberias.comprobarExtremos();
                            desbloquearEdificio = false;
                        }
                    }
                    if (hitTile.transform.name == "Depuradora1" || hitTile.transform.name == "Depuradora2" || hitTile.transform.name == "Depuradora3")
                    {
                        hitTile.color = Color.white;
                        dinero -= 50;
                        nivelDelAgua += 2;
                        desbloquearEdificio = false;
                    }
                }
            }
            //Poner ray en hijo del array construibles. El que detecte se cambia el color y se añade a extremos comprados
        }
    }

    //Tuberias
    public void añadirTuberia(Vector3Int inicio, Vector3Int fin)
    {
        //print("Añadiendo tuberia");
        tuberias.añadirTuberia(inicio, fin);

        for (int i = 0; i < bloques.Length; i++)
        {
            bool comprobado = comprobarBloque(bloques[i].bloqueInicio, bloques[i].bloqueFin, inicio, fin);
            //print(comprobado);

            if (comprobado)
            {
                bloques[i].setSuministrado(true);
            }
        }

        for (int i = 0; i < tuberias.getExtremos().Count; i++)
        {
            tilemap.SetTile(tuberias.getExtremos()[i], tilaExtremo);
        }

        tutorial = false;
    }

    private void elegirJuntaCompra()
    {
        if (comprarJunta)
        {
            if (dinero > 10)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int pos = tilemap.WorldToCell(mousePos);

                    if (tuberias.getTuberias().Contains(pos))
                    {
                        print(true + " " + pos);
                        tuberias.getExtremosComprados().Add(pos);
                        foreach (var extremo in tuberias.getExtremosComprados())
                        {
                            print("Extremos Comprados: " + extremo);
                        }
                        tuberias.comprobarExtremos();
                        tilemap.SetTile(pos, tilaExtremo);

                        comprarJunta = false;
                        dinero -= 10;
                    }
                }
            }            
        }
    }


    private bool comprobarBloque(Vector3Int bInicio, Vector3Int bFin,
        Vector3Int tuberiaInicio, Vector3Int tuberiaFin)
    {
        //Crear matriz del bloque
        Vector3Int[,] matriz = new Vector3Int[Mathf.Abs(bFin.x - bInicio.x) + 1,
            Mathf.Abs(bFin.y - bInicio.y) + 1];
        for (int i = 0; i < matriz.GetLength(0); i++)
        {
            for (int j = 0; j < matriz.GetLength(1); j++)
            {
                matriz[i, j] = new Vector3Int(bInicio.x + i, bInicio.y + j, 0);
                //print(matriz[i, j]);

                //print("NUEVA COMPROBACION");

                if (tuberiaInicio.x == tuberiaFin.x)
                {
                    if (tuberiaFin.y < tuberiaInicio.y)
                    {
                        Vector3Int aux = tuberiaFin;
                        tuberiaFin = tuberiaInicio;
                        tuberiaInicio = aux;
                    }

                    for (int k = 0; k < tuberiaFin.y - tuberiaInicio.y + 1; k++)
                    {
                        Vector3Int tuberiaPos = new Vector3Int(tuberiaInicio.x, tuberiaInicio.y + k, 0);
                        //print("Comprobando si " + tuberiaPos + " es igual a " + matriz[i, j]);

                        if (tuberiaPos.x == matriz[i,j].x && tuberiaPos.y == matriz[i,j].y)
                        {
                            return true;
                        }
                    }
                }
                if (tuberiaInicio.y == tuberiaFin.y)
                {
                    if (tuberiaFin.x < tuberiaInicio.x)
                    {
                        Vector3Int aux = tuberiaFin;
                        tuberiaFin = tuberiaInicio;
                        tuberiaInicio = aux;
                    }

                    for (int k = 0; k < tuberiaFin.x - tuberiaInicio.x + 1; k++)
                    {
                        Vector3Int tuberiaPos = new Vector3Int(tuberiaInicio.x + k, tuberiaInicio.y, 0);
                        //print("Comprobando si " + tuberiaPos + " es igual a " + matriz[i, j]);

                        if (tuberiaPos.x == matriz[i, j].x && tuberiaPos.y == matriz[i, j].y)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    void mostrarOpciones()
    {
        if (!primerClick)
        {
            provisionalTilemap.ClearAllTiles();

            for (int i = 0; i < limite; i++)
            {
                Vector3Int pos1 = new Vector3Int(gridPosPrimer.x, gridPosPrimer.y + i, 0);
                Vector3Int pos2 = new Vector3Int(gridPosPrimer.x, gridPosPrimer.y - i, 0);
                Vector3Int pos3 = new Vector3Int(gridPosPrimer.x + i, gridPosPrimer.y, 0);
                Vector3Int pos4 = new Vector3Int(gridPosPrimer.x - i, gridPosPrimer.y, 0);

                provisionalTilemap.SetTile(pos1, tilaX);
                provisionalTilemap.SetTile(pos2, tilaX);
                provisionalTilemap.SetTile(pos3, tilaY);
                provisionalTilemap.SetTile(pos4, tilaY);
            }
        }
    }

    void comprobarSiEsExtremo(Vector3Int pulsacion)
    {
        if (GameManager.instance.tuberias.getExtremos().Contains(pulsacion))
        {
            esExtremo = true;
        }

        //print(esExtremo);
    }

    void tilesClick()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (primerClick)
            {
                print("Primer click");
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gridPosPrimer = tilemap.WorldToCell(mousePos);

                comprobarSiEsExtremo(gridPosPrimer);

                if (esExtremo)
                {
                    primerClick = false;
                    segundoClick = true;
                    esExtremo = false;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
                Vector3Int position = tilemap.WorldToCell(worldPoint);

                //print(gridPosPrimer);
            }

            else if (segundoClick)
            {
                print("Segundo click");
                primerClick = true;
                segundoClick = false;

                provisionalTilemap.ClearAllTiles();

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gridPosSegundo = tilemap.WorldToCell(mousePos);

                //print(gridPosSegundo);

                int contador = 0;

                if (gridPosPrimer.x == gridPosSegundo.x)
                {
                    if (gridPosPrimer.y <= gridPosSegundo.y)
                    {
                        for (int i = gridPosPrimer.y; i < gridPosSegundo.y + 1; i++)
                        {
                            if (contador < limite)
                            {
                                contador++;

                                finalPos = new Vector3Int(gridPosPrimer.x, i, 0);
                                tilemap.SetTile(gridPosPrimer, tilaExtremo);
                                tilemap.SetTile(finalPos, tilaX);
                            }
                            else
                            {
                                finalPos = new Vector3Int(gridPosPrimer.x, i, 0);
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = gridPosPrimer.y; i > gridPosSegundo.y - 1; i--)
                        {
                            if (contador < limite)
                            {
                                contador++;

                                finalPos = new Vector3Int(gridPosPrimer.x, i, 0);
                                tilemap.SetTile(gridPosPrimer, tilaExtremo);
                                tilemap.SetTile(finalPos, tilaX);
                            }
                            else
                            {
                                finalPos = new Vector3Int(gridPosPrimer.x, i, 0);
                                break;
                            }
                        }
                    }
                    GameManager.instance.añadirTuberia(gridPosPrimer, finalPos);
                }

                else if (gridPosPrimer.y == gridPosSegundo.y)
                {
                    if (gridPosPrimer.x <= gridPosSegundo.x)
                    {
                        for (int i = gridPosPrimer.x; i < gridPosSegundo.x + 1; i++)
                        {
                            if (contador < limite)
                            {
                                contador++;

                                finalPos = new Vector3Int(i, gridPosPrimer.y, 0);
                                tilemap.SetTile(gridPosPrimer, tilaExtremo);
                                tilemap.SetTile(finalPos, tilaY);
                            }
                            else
                            {
                                finalPos = new Vector3Int(i, gridPosPrimer.y, 0);
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = gridPosPrimer.x; i > gridPosSegundo.x - 1; i--)
                        {
                            if (contador < limite)
                            {
                                //print("CONTADOR " + contador);
                                contador++;

                                finalPos = new Vector3Int(i, gridPosPrimer.y, 0);
                                tilemap.SetTile(gridPosPrimer, tilaExtremo);
                                tilemap.SetTile(finalPos, tilaY);
                            }
                            else
                            {
                                //print("Final");
                                finalPos = new Vector3Int(i, gridPosPrimer.y, 0);
                                break;
                            }
                        }
                    }

                    GameManager.instance.añadirTuberia(gridPosPrimer, finalPos);
                }
            }
        }
    }

    public bool getConstruccionHabilitada()
    {
        return construccionHabilitada;
    }

    private void updateUI()
    {
        //Texto información
        textInformacion.text = "";
        for (int i = 0; i < bloques.Length; i++)
        {
            textInformacion.text += "Bloque " + i + ": " + bloques[i].getFelicidad() + "\n";
        }

        textInformacion.text += "\n";

        textInformacion.text += "Nivel del agua: " + nivelDelAgua;

        //Texto dinero
        textDinero.text = dinero.ToString();
    }

    public void habilitarConstruccionDeTuberias()
    {
        if (!construccionHabilitada)
        {
            construccionHabilitada = true;
        }
        else
        {
            construccionHabilitada = false;
        }
    }

    public void mostrarInfoPulsado()
    {
        if (imagenInformacion.gameObject.active)
        {
            imagenInformacion.gameObject.SetActive(false);
        }
        else
        {
            imagenInformacion.gameObject.SetActive(true);
        }
    }

    public void mejorarNivelDelAgua()
    {
        nivelDelAgua++;
    }

    public void comprarJuntaPulsado()
    {
        if (comprarJunta)
        {
            comprarJunta = false;
        }
        else
        {
            comprarJunta = true;
        }
    }

    public void desbloquearEdificioPulsado()
    {
        if (!desbloquearEdificio)
        {
            desbloquearEdificio = true;
        }
        construccionHabilitada = false;
    }

    public void subirPrecioAgua()
    {
        precioAgua++;
    }

    public void bajarPrecioAgua()
    {
        precioAgua--;
    }

    public void showBarrio()
    {
        if (boolBarrio)
        {
            boolBarrio = false;
        }
        else
        {
            boolBarrio = true;
        }

    }

    public bool getInfoBarrio()
    {
        return boolBarrio;
    }

}
