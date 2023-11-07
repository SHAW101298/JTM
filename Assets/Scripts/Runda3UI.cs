using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Runda3UI : MonoBehaviour
{
    public Button[] przyciskiUtworow;
    public int wybranyUtwor = -1;
    public AudioSource source;

    public ListaNazwDzwiekowXML wczytaneNazwyUtworow;
    public AudioClip[] dzwieki;

    public float timer;
    public float timerLimit;
    public bool liczCzas = false;
    public Text oknoCzasu;

    [Header("Kolory")]
    public Color zaliczone;

    public IEnumerator InicjalizujProces()
    {
        Debug_me.ins.Log("Rozpoczecie Coroutine 2");
        wczytaneNazwyUtworow = XMLManager.WczytajListeNazwDzwiekow(2,6);
        dzwieki = new AudioClip[6];

        for (int i = 0; i < 6; i++)
        {
            Debug_me.ins.Log("Dzwiek " + i + " nazwa = " + wczytaneNazwyUtworow.nazwydzwiekow[i].tytul);
            UnityWebRequest AudioFile = UnityWebRequestMultimedia.GetAudioClip(Application.dataPath + "/Dzwieki/2/" + i + ".wav", AudioType.WAV);
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
                catch(InvalidOperationException excpetion)
                {

                }
                

            }
        }
        Debug_me.ins.Log("Zakonczenie Coroutine 2");
        UstawNazwy();
    }
    public IEnumerator InicjalizujProcesMP3()
    {
        Debug_me.ins.Log("Rozpoczecie Coroutine 2");
        wczytaneNazwyUtworow = XMLManager.WczytajListeNazwDzwiekow(2, 6);
        dzwieki = new AudioClip[6];

        for (int i = 0; i < 6; i++)
        {
            Debug_me.ins.Log("Dzwiek " + i + " nazwa = " + wczytaneNazwyUtworow.nazwydzwiekow[i].tytul);
            UnityWebRequest AudioFile = UnityWebRequestMultimedia.GetAudioClip(Application.dataPath + "/Dzwieki/2/" + i + ".mp3", AudioType.MPEG);
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
                    TagLib.File tagFile = TagLib.File.Create(Application.dataPath + "/Dzwieki/2/" + i + ".mp3");

                    wczytaneNazwyUtworow.nazwydzwiekow[i].autor = tagFile.Tag.FirstPerformer;
                    wczytaneNazwyUtworow.nazwydzwiekow[i].tytul = tagFile.Tag.Title;

                    clip.name = wczytaneNazwyUtworow.nazwydzwiekow[i].tytul;

                    dzwieki[i] = clip;
                }
                catch (InvalidOperationException excpetion)
                {

                }


            }
        }
        Debug_me.ins.Log("Zakonczenie Coroutine 2");
        UstawNazwy();
    }

    public void BTN_WybranoUtwor(int i)
    {
        wybranyUtwor = i;
        source.clip = dzwieki[wybranyUtwor];
        przyciskiUtworow[wybranyUtwor].GetComponentInChildren<Text>().text = wczytaneNazwyUtworow.nazwydzwiekow[wybranyUtwor].autor;
    }
    public void BTN_Odtworz()
    {
        source.Play();
        liczCzas = true;
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
            przyciskiUtworow[i].GetComponentInChildren<Text>().text = i + ". ???";
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BTN_Stop();
        }

        if(liczCzas == true)
        {
            timer += Time.deltaTime;
            oknoCzasu.text = timer.ToString("n2");
            if (timer >= timerLimit)
            {
                timer = 0;
                liczCzas = false;
                source.Stop();
                oknoCzasu.text = timer.ToString("n2");
            }
        }
    }

    #region
    public void BTN_05()
    {
        timerLimit = 0.5f;
    }
    public void BTN_1()
    {
        timerLimit = 1f;
    }
    public void BTN_15()
    {
        timerLimit = 1.5f;
    }
    public void BTN_2()
    {
        timerLimit = 2f;
    }
    public void BTN_25()
    {
        timerLimit = 2.5f;
    }
    public void BTN_3()
    {
        timerLimit = 3f;
    }
    public void BTN_35()
    {
        timerLimit = 3.5f;
    }

    public void BTN_WybranieCzasu(int czas)
    {
        timerLimit = ((float)czas) / 10;
    }

    #endregion
}
