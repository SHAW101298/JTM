﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.IO;

public class Runda1 : MonoBehaviour
{
    AudioClip[] utwory;
    ListaNazwDzwiekowXML listaNazw;
    public GameObject content;
    public GameObject prefab;
    public AudioSource source;
    public Image[] ikonyGraczy;
    UtworUI1[] ikonyUtworow;

    int wybranyUtwor = -1;
    [SerializeField] bool oczekujePrzycisku;
    float timer;
    [Space(20)]
    public float maxTimer;
    public Text czasField;
    [SerializeField] bool liczCzas;

    [Header("Debug")]
    public string[] pliki;


    /*
    1. Dowiedziec sie czy zrobic mozlwiosc dynamicznej ilosci utworow
    2. Spawnowanie Przyciskow w content

    // searches the current directory and sub directory
    int fCount = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;
    // searches the current directory
    int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;


     */


    public IEnumerator InicjalizujProces()
    {
        string sciezka = Application.dataPath + "/Dzwieki/Runda 1";
        bool kontrolka = SprawdzIstnienieSciezki(sciezka);

        if(kontrolka == false)
        {
            UtworzFolder(sciezka);
        }

        int iloscPlikow = Directory.GetFiles(sciezka, "*.mp3", SearchOption.AllDirectories).Length;
        Initialization.ins.iloscUtworowMaks += iloscPlikow;
        Debug_me.ins.Log("ilsoc plikow = " + iloscPlikow);
        pliki = new string[iloscPlikow];
        pliki = Directory.GetFiles(sciezka, "*.mp3");
        int iloscZnakow = sciezka.Length - 1;

        utwory = new AudioClip[iloscPlikow];
        listaNazw = new ListaNazwDzwiekowXML(iloscPlikow);

        iloscPlikow -= 1;



        for(int i = 0; i <= iloscPlikow; i++)
        {
            Debug_me.ins.Log("Utwor = " + pliki[i]);
            UnityWebRequest AudioFile = UnityWebRequestMultimedia.GetAudioClip(pliki[i], AudioType.MPEG);
            yield return AudioFile.SendWebRequest();

            if (AudioFile.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug_me.ins.Log("ERROR || Blad z plikiem audio = " + AudioFile.error);
            }
            else
            {
                try
                {
                    Debug_me.ins.Log("Udalo sie wczytac plik");
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(AudioFile);
                    bool kontrolkaNazwy = ProbaWpisaniaAutoraiTytulu(pliki[i], i);

                    utwory[i] = clip;
                    if (kontrolkaNazwy == true)
                    {
                        Initialization.ins.WczytanoNastepnyUtwor("Runda 1");
                    }
                    else
                    {
                        Initialization.ins.WczytanoNastepnyUtwor("Runda 1", i);
                    }
                }
                catch (InvalidOperationException exception)
                {

                }



            }
        }
        //source.clip = utwory[0];
        //source.Play();
        UstawNazwy();

    }

    bool ProbaWpisaniaAutoraiTytulu(string plik, int x)
    {
        TagLib.File tagFile = TagLib.File.Create(plik);
        if (tagFile.Tag.FirstPerformer != null && tagFile.Tag.Title != null)
        {
            Debug_me.ins.Log("Plik ma wlasciwe dane");
            Debug_me.ins.Log("Autor = " + tagFile.Tag.FirstPerformer);
            Debug_me.ins.Log("Tytul = " + tagFile.Tag.Title);

            listaNazw.nazwydzwiekow[x].autor = tagFile.Tag.FirstPerformer;
            listaNazw.nazwydzwiekow[x].tytul = tagFile.Tag.Title;
            return true;
        }
        else
        {
            Debug_me.ins.Log("Ustawianie danych za pomoca nazwy pliku");
            string sciezka_do_folderu = Application.dataPath + "/Dzwieki/Runda 1/";
            plik = plik.Substring(sciezka_do_folderu.Length);
            Debug_me.ins.Log("Plik = " + plik);
            int index = -1;
            for (int i = 0; i < plik.Length; i++)
            {
                if (plik[i] == '-')
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                string autor = plik.Substring(0, index);
                string tytul = plik.Substring(index + 2);
                tytul = tytul.Remove(tytul.Length - 4);

                listaNazw.nazwydzwiekow[x].autor = autor;
                listaNazw.nazwydzwiekow[x].tytul = tytul;
                Debug_me.ins.Log("autor = " + autor);
                Debug_me.ins.Log("Tytul = " + tytul);

                return true;
            }
            else
            {
                return false;
            }

        }
    }

    void UstawNazwy()
    {
        ikonyUtworow = new UtworUI1[utwory.Length];
        for(int i = 0; i <= utwory.Length -1; i++)
        {
            GameObject temp = Instantiate(prefab);
            temp.transform.SetParent(content.transform);

            UtworUI1 utwor_temp = temp.GetComponent<UtworUI1>();
            utwor_temp.id = i;
            utwor_temp.autor = listaNazw.nazwydzwiekow[i].autor;
            utwor_temp.tytul = listaNazw.nazwydzwiekow[i].tytul;
            utwor_temp.UstawPodstawowaNazwe();
            ikonyUtworow[i] = utwor_temp;
            utwor_temp.runda = 0;
        }
    }

    bool SprawdzIstnienieSciezki(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void UtworzFolder(string path)
    {
        string sciezka = Application.dataPath + "/Dzwieki";

        if (!Directory.Exists(sciezka))
        {
            Directory.CreateDirectory(sciezka);
        }

        Directory.CreateDirectory(path);
    }

    // Kontrola UI

    public void WybranoUtwor(int id)
    {
        wybranyUtwor = id;
        ZatrzymajUtwor();
    }

    public void BTN_Odtworz()
    {
        if (wybranyUtwor == -1)
        {
            return;
        }
        timer = 0;
        oczekujePrzycisku = true;
        liczCzas = true;
        source.clip = utwory[wybranyUtwor];
        source.Play();
        ResetujKolory();
        ikonyUtworow[wybranyUtwor].GetComponent<Button>().Select();
    }
    public void BTN_WyswietlPelnaNazwe()
    {
        ResetujKolory();
        ikonyUtworow[wybranyUtwor].OdkryjNazwe();
        wybranyUtwor = -1;
    }
    void ZatrzymajUtwor()
    {
        liczCzas = false;
        source.Stop();
        oczekujePrzycisku = false;
    }
    void AktualizujTimer()
    {
        czasField.text = timer.ToString("n1");
    }
    void ResetujKolory()
    {
        foreach (Image gracz in ikonyGraczy)
        {
            gracz.color = Color.white;
        }
    }

    void ZgloszenieGracza(int i)
    {
        ResetujKolory();
        ikonyGraczy[i].color = Color.green;
        ZatrzymajUtwor();
        oczekujePrzycisku = false;
    }
    
    private void Update()
    {
        if (liczCzas == true)
        {
            timer += Time.deltaTime;
            AktualizujTimer();
            if (timer >= maxTimer)
            {
                ZatrzymajUtwor();
                oczekujePrzycisku = true;
            }
        }     
        if(oczekujePrzycisku == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ZgloszenieGracza(0);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ZgloszenieGracza(1);
            }
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                ZgloszenieGracza(2);
            }
        }
    }


    
}