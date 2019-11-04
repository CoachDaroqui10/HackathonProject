using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public Image happinessFace;
    public Sprite happy;
    public Sprite neutral;
    public Sprite angry;
    public Text districtText;
    public Text priceText;

    public GameObject constructionMenu;
    public Button b_Pipes;
    public Button b_Building;
    public Button b_Joints;

    public Button b_CancelConstruction;
    public Button b_CancelInformation;

    public SelectionManagerMac select;
    public Barrios districts;

    public GameObject districtInfo;
    public Text water;

    public Image[] tutorial;

    public Image moneyBackground;
    public Sprite redMoney;
    public Sprite normalMoney;

    private int tutorialImagen = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        water.text = GameManager.instance.precioAgua.ToString();
        if (GameManager.instance.boolBarrio)
        {
            ShowBarrio();
        }
        else
        {
            districtInfo.SetActive(false);
        }

        if (GameManager.instance.dinero < 1000)
        {
            moneyBackground.sprite = redMoney;
        }
        else
        {
            moneyBackground.sprite = normalMoney;
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
        b_CancelConstruction.gameObject.SetActive(true);
        constructionMenu.SetActive(true);
    }

    public void CancelConstruction()
    {
        b_CancelConstruction.gameObject.SetActive(false);
        constructionMenu.SetActive(false);
    }

    public void CancelInformation()
    {
        GameManager.instance.showBarrio();
        districtInfo.SetActive(false);
        b_CancelInformation.gameObject.SetActive(false);
    }

    private void ShowBarrio()
    {
        districtInfo.SetActive(true);

        Barrios barrio = GameObject.Find(select.getBarrio()).GetComponent<Barrios>();

        b_CancelInformation.gameObject.SetActive(true);

        if (barrio.getFelicidad() < 33)
        {
            happinessFace.sprite = angry;
            priceText.text = "Los habitantes de este barrio creen que el precio del agua es demasiado alto";
        }
        else if (barrio.getFelicidad() > 66)
        {
            happinessFace.sprite = happy;
            priceText.text = "Los habitantes de este barrio creen que el precio del agua es perfecto";
        }
        else
        {
            happinessFace.sprite = neutral;
            priceText.text = "Los habitantes de este barrio creen que el precio del agua es aceptable";
        }

        districtText.text = select.getBarrio();
    }
}
