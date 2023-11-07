using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Utwor
{
    public string autor;
    public string tytul;

    public string ZwrocNazwe()
    {
        string nazwa = autor + " - " + tytul;
        return nazwa;
    }

    public Utwor()
    {
        autor = "brak autora";
        tytul = "brak tytulu";
    }
}
[System.Serializable]
public class ListaNazwDzwiekowXML 
{
    [SerializeField]
    public Utwor[] nazwydzwiekow;

    public ListaNazwDzwiekowXML()
    {
        nazwydzwiekow = new Utwor[6];
        for(int i = 0; i < 6; i++)
        {
            //Debug.Log("i = " + i);
            Utwor nowy = new Utwor();
            nazwydzwiekow[i] = nowy;
        }
    }
    public ListaNazwDzwiekowXML(int x)
    {
        nazwydzwiekow = new Utwor[x];
        Debug_me.ins.Log("x = " + x);
        for (int i = 0; i < x; i++)
        {
            Debug_me.ins.Log("i = " + i);
            Utwor nowy = new Utwor();
            nazwydzwiekow[i] = nowy;
        }
    }
}
