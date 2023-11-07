using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.IO;

public class Runda4 : MonoBehaviour
{
    AudioClip[] utwory;
    ListaNazwDzwiekowXML listaNazw;
    public GameObject content;
    public GameObject prefab;
    public AudioSource source;
    UtworUI1[] ikonyUtworow;

    int wybranyUtwor = -1;
    float timer = 20;
    [Space(20)]
    public float maxTimer;
    public Text czasField;
    [SerializeField] bool liczCzas;


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
        string sciezka = Application.dataPath + "/Dzwieki/Runda 4";
        bool kontrolka = SprawdzIstnienieSciezki(sciezka);

        if(kontrolka == false)
        {
            UtworzFolder(sciezka);
        }

        int iloscPlikow = Directory.GetFiles(sciezka, "*.mp3", SearchOption.AllDirectories).Length;
        Initialization.ins.iloscUtworowMaks += iloscPlikow;
        Debug_me.ins.Log("ilsoc plikow = " + iloscPlikow);
        string[] pliki = Directory.GetFiles(sciezka, "*.mp3");

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


                    //TagLib.File tagFile = TagLib.File.Create(pliki[i]);

                    //listaNazw.nazwydzwiekow[i].autor = tagFile.Tag.FirstPerformer;
                    //listaNazw.nazwydzwiekow[i].tytul = tagFile.Tag.Title;

                    bool kontrolkaNazwy = ProbaWpisaniaAutoraiTytulu(pliki[i], i);

                    utwory[i] = clip;
                    if(kontrolkaNazwy == true)
                    {
                        Initialization.ins.WczytanoNastepnyUtwor("Runda 4");
                    }
                    else
                    {
                        Initialization.ins.WczytanoNastepnyUtwor("Runda 4", i);
                    }
                }
                catch (InvalidOperationException exception)
                {

                }



            }
        }
        //source.clip = utwory[0];
        //source.Play();
        Initialization.ins.UkryjOknoLadowania();
        UstawNazwy();

    }
    bool ProbaWpisaniaAutoraiTytulu(string plik, int x)
    {
        TagLib.File tagFile = TagLib.File.Create(plik);
        if(tagFile.Tag.FirstPerformer != null && tagFile.Tag.Title != null)
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
            string sciezka_do_folderu = Application.dataPath + "/Dzwieki/Runda 4/";
            plik = plik.Substring(sciezka_do_folderu.Length);
            Debug_me.ins.Log("Plik = " + plik);
            int index = -1;
            for(int i = 0; i < plik.Length; i++)
            {
                if(plik[i] == '-')
                {
                    index = i;
                    break;
                }
            }

            if(index != -1)
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
            utwor_temp.UstawUkrytanazwe();
            ikonyUtworow[i] = utwor_temp;
            utwor_temp.runda = 3;
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
        if (timer <= 0)
            return;

        liczCzas = true;
        source.clip = utwory[wybranyUtwor];
        source.Play();
        ikonyUtworow[wybranyUtwor].GetComponent<Button>().Select();
    }
    public void BTN_WyswietlPelnaNazwe()
    {
        ikonyUtworow[wybranyUtwor].OdkryjNazwe();
        wybranyUtwor = -1;
        ZatrzymajUtwor();
    }
    void ZatrzymajUtwor()
    {
        liczCzas = false;
        source.Stop();
    }
    void AktualizujTimer()
    {
        czasField.text = timer.ToString("n1");
    }


    
    private void Update()
    {
        if (liczCzas == true)
        {
            timer -= Time.deltaTime;
            AktualizujTimer();
            if (timer <= 0)
            {
                ZatrzymajUtwor();
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ZatrzymajUtwor();
        }
    }


    
}
