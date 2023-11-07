using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Runda2UI : MonoBehaviour
{
    public Button[] przyciskiUtworow;
    public int wybranyUtwor = -1;
    public AudioSource source;

    public ListaNazwDzwiekowXML wczytaneNazwyUtworow;
    public AudioClip[] dzwieki;

    [Header("Kolory")]
    public Color zaliczone;

    [Header("Czas")]
    public float timer;
    public float timerLimit = 15;
    public bool liczCzas = false;
    public Text oknoCzasu;

    public Image[] gracze;
    public bool przyjmujInput = true;

    public IEnumerator InicjalizujProces()
    {
        Debug_me.ins.Log("Rozpoczecie Coroutine 1");
        wczytaneNazwyUtworow = XMLManager.WczytajListeNazwDzwiekow(1, 3);
        dzwieki = new AudioClip[3];

        for (int i = 0; i < 3; i++)
        {
            Debug_me.ins.Log("Dzwiek " + i + " nazwa = " + wczytaneNazwyUtworow.nazwydzwiekow[i].tytul);
            UnityWebRequest AudioFile = UnityWebRequestMultimedia.GetAudioClip(Application.dataPath + "/Dzwieki/1/" + i + ".wav", AudioType.WAV);
            yield return AudioFile.SendWebRequest();
            if (AudioFile.isNetworkError)
            {
                Debug_me.ins.Log("ERROR || Blad z plikiem audio = " + AudioFile.error);
            }
            else
            {
                try
                {
                    Debug_me.ins.Log("Udalo sie wczytac plik");
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(AudioFile);
                    clip.name = wczytaneNazwyUtworow.nazwydzwiekow[i].tytul;

                    dzwieki[i] = clip;
                }
                catch (InvalidOperationException exception)
                {

                }


            }
        }
        Debug_me.ins.Log("Zakonczenie Coroutine 1");
        UstawNazwy();
    }
    public IEnumerator InicjalizujProcesMP3()
    {
        Debug_me.ins.Log("Rozpoczecie Coroutine 1");
        wczytaneNazwyUtworow = XMLManager.WczytajListeNazwDzwiekow(1, 3);
        dzwieki = new AudioClip[3];

        for (int i = 0; i < 3; i++)
        {
            Debug_me.ins.Log("Dzwiek " + i + " nazwa = " + wczytaneNazwyUtworow.nazwydzwiekow[i].tytul);
            UnityWebRequest AudioFile = UnityWebRequestMultimedia.GetAudioClip(Application.dataPath + "/Dzwieki/1/" + i + ".mp3", AudioType.MPEG);
            yield return AudioFile.SendWebRequest();
            if (AudioFile.isNetworkError)
            {
                Debug_me.ins.Log("ERROR || Blad z plikiem audio = " + AudioFile.error);
            }
            else
            {
                try
                {
                    Debug_me.ins.Log("Udalo sie wczytac plik");
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(AudioFile);
                    TagLib.File tagFile = TagLib.File.Create(Application.dataPath + "/Dzwieki/1/" + i + ".mp3");

                    wczytaneNazwyUtworow.nazwydzwiekow[i].autor = tagFile.Tag.FirstPerformer;
                    wczytaneNazwyUtworow.nazwydzwiekow[i].tytul = tagFile.Tag.Title;

                    clip.name = wczytaneNazwyUtworow.nazwydzwiekow[i].tytul;
                    dzwieki[i] = clip;
                }
                catch (InvalidOperationException exception)
                {

                }


            }
        }
        Debug_me.ins.Log("Zakonczenie Coroutine 1");
        UstawNazwy();
    }

    public void BTN_WybranoUtwor(int i)
    {
        wybranyUtwor = i;
        source.clip = dzwieki[wybranyUtwor];
    }
    public void BTN_Odtworz()
    {
        if (wybranyUtwor == -1)
            return;

        ResetujLicznikCzasu();
        source.Play();
        liczCzas = true;
        przyjmujInput = true;
        foreach (Image zdjecie in gracze)
        {
            zdjecie.color = Color.white;
        }
    }
    public void BTN_Stop()
    {
        source.Stop();
        liczCzas = false;
        timer = 0;
    }
    public void BTN_Wyswietl()
    {
        if (wybranyUtwor == -1)
            return;

        Button przycisk = przyciskiUtworow[wybranyUtwor];
        przycisk.GetComponentInChildren<Text>().text = wybranyUtwor + ". " + wczytaneNazwyUtworow.nazwydzwiekow[wybranyUtwor].ZwrocNazwe();
        przycisk.GetComponent<Image>().color = zaliczone;
    }

    public void UstawNazwy()
    {
        for (int i = 0; i < przyciskiUtworow.Length; i++)
        {
            przyciskiUtworow[i].GetComponentInChildren<Text>().text = i + ". " + wczytaneNazwyUtworow.nazwydzwiekow[i].autor;
        }
    }

    void Update()
    {
        if (liczCzas == false)
            return;

        if (przyjmujInput == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ZatrzymajUtwor();
                przyjmujInput = false;
                gracze[0].color = Color.green;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ZatrzymajUtwor();
                przyjmujInput = false;
                gracze[1].color = Color.green;
            }
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                ZatrzymajUtwor();
                przyjmujInput = false;
                gracze[2].color = Color.green;
            }
        }

        timer += Time.deltaTime;
        oknoCzasu.text = timer.ToString("n1");
        if (timer >= timerLimit)
        {
            timer = timerLimit;
            oknoCzasu.text = timer.ToString("n2");
            ZatrzymajUtwor();

        }

    }

    void ZatrzymajUtwor()
    {
        source.Stop();
        liczCzas = false;
    }
    void ResetujLicznikCzasu()
    {
        timer = 0;
        oknoCzasu.text = 0.ToString();
    }
}
