using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Initialization : MonoBehaviour
{
    #region
    public static Initialization ins;
    public void Referencja()
    {
        ins = this;
    }
    #endregion
    public Debug_me debug;

    public GameObject[] rundy;
    //[SerializeField] GameObject runda1;
    //[SerializeField] GameObject runda2;
    //[SerializeField] GameObject runda3;
    //[SerializeField] GameObject runda4;

    public GameObject okno_wyjscia;

    //[Header("UI Scripts")]
    //public Runda1UI runda1UI;
    //public Runda2UI runda2UI;
    //public Runda3UI runda3UI;
    //public Runda4UI runda4UI;

    [Header("Rundy")]
    public Runda1 runda1;
    public Runda2 runda2;
    public Runda3 runda3;
    public Runda4 runda4;

    public GameObject oknoLadowania;
    public Text postep;
    public int iloscUtworowMaks;
    public int aktualnieWczytany;

    [Header("Bledy")]
    public GameObject content;
    public GameObject prefabBleduUtworu;
    public GameObject przyciskPowrotu;


    private void Awake()
    {
        Referencja();
        debug.REFERENCJA();
        //XMLManager.WczytajListeNazwDzwiekow(0);

    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            okno_wyjscia.SetActive(!okno_wyjscia.activeSelf);
        }
    }
    public void BTN_Wyjdz()
    {
        Application.Quit();
    }
    public void BTN_Wroc()
    {
        okno_wyjscia.SetActive(false);
    }

    public void PoprzedniaRunda(int x)
    {
        foreach (GameObject okno in rundy)
        {
            okno.SetActive(false);
        }

        if (x == 0)
        {
            x = 3;
        }
        else
        {
            x--;
        }
        rundy[x].SetActive(true);
    }
    public void NastepnaRunda(int x)
    {
        foreach (GameObject okno in rundy)
        {
            okno.SetActive(false);
        }

        if (x == 3)
        {
            x = 0;
        }
        else
        {
            x++;
        }
        rundy[x].SetActive(true);
    }

    public void WczytanoNastepnyUtwor()
    {
        aktualnieWczytany++;
        postep.text = aktualnieWczytany.ToString() + " / " + iloscUtworowMaks.ToString();
    }
    public void WczytanoNastepnyUtwor(string runda)
    {
        aktualnieWczytany++;
        postep.text = runda + "\n" + aktualnieWczytany.ToString() + " / " + iloscUtworowMaks.ToString();
    }
    public void WczytanoNastepnyUtwor(string runda, int x)
    {
        aktualnieWczytany++;
        postep.text = runda + "\n" + aktualnieWczytany.ToString() + " / " + iloscUtworowMaks.ToString();

        GameObject temp = Instantiate(prefabBleduUtworu);
        temp.GetComponentInChildren<Text>().text = runda + " | nr = " + (x + 1) + " | Nie udalo sie przydzielic nazwy";
        temp.transform.SetParent(content.transform);
    }

    public void UkryjOknoLadowania()
    {
        if (content.transform.childCount == 0)
        {
            oknoLadowania.SetActive(false);
        }
        else
        {
            przyciskPowrotu.SetActive(true);
        }
    }
    public void BTN_PowrotZLadowania()
    {
        oknoLadowania.SetActive(false);
    }
}
