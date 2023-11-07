using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

using TagLib;

public class BeginLoading : MonoBehaviour
{
    //public Runda1UI runda1;
    //public Runda2UI runda2;
    //public Runda3UI runda3;
    //public Runda4UI runda4;

    public Runda1 runda1;
    public Runda2 runda2;
    public Runda3 runda3;
    public Runda4 runda4;

    private void Start()
    {
        Debug_me.ins.Log("Begin Loadin | Rozpcozecie ladowania dzwiekow");
        StartCoroutine(WczytajDzwieki());
    }
    IEnumerator WczytajDzwieki()
    {
        yield return StartCoroutine(runda1.InicjalizujProces());
        yield return StartCoroutine(runda2.InicjalizujProces());
        yield return StartCoroutine(runda3.InicjalizujProces());
        yield return StartCoroutine(runda4.InicjalizujProces());

        Destroy(gameObject);
    }

}
