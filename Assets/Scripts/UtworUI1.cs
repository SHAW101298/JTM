using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtworUI1 : MonoBehaviour
{
    public int id;
    public int runda;
    public Text tekst;
    public string autor;
    public string tytul;
    public Image backGround;

    public void BTN_WybranieUtworu()
    {
        switch (runda)
        {
            case 0:
                Initialization.ins.runda1.WybranoUtwor(id);
                break;
            case 1:
                Initialization.ins.runda2.WybranoUtwor(id);
                break;
            case 2:
                Initialization.ins.runda3.WybranoUtwor(id);
                break;
            case 3:
                Initialization.ins.runda4.WybranoUtwor(id);
                break;
        }
    }

    public void UstawPodstawowaNazwe()
    {
        tekst.text = id + ". " + autor;
    }
    public void UstawUkrytanazwe()
    {
        tekst.text = id + ". ???";
    }

    public void OdkryjNazwe()
    {
        tekst.text = id + ". " + autor + " - " + tytul;
        backGround.color = Color.green;
    }
}
