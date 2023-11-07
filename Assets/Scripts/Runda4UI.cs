using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


public class Runda4UI : MonoBehaviour
{
    public Button[] przyciskiUtworow;
    public int wybranyUtwor = -1;
    public AudioSource source;

    public ListaNazwDzwiekowXML wczytaneNazwyUtworow;
    public AudioClip[] dzwieki;

    [Header("Czas")]
    public float timer = 30f;
    public float timerLimit;
    public bool liczCzas = false;
    public Text oknoCzasu;

    [Header("Kolory")]
    public Color zaliczone;


    public IEnumerator InicjalizujProces()
    {
        Debug_me.ins.Log("Rozpoczecie Coroutine 3");
        wczytaneNazwyUtworow = XMLManager.WczytajListeNazwDzwiekow(3,7);
        dzwieki = new AudioClip[7];
        for (int i = 0; i < 7; i++)
        {
            Debug_me.ins.Log("Dzwiek " + i + " nazwa = " + wczytaneNazwyUtworow.nazwydzwiekow[i].tytul);
            UnityWebRequest AudioFile = UnityWebRequestMultimedia.GetAudioClip(Application.dataPath + "/Dzwieki/3/" + i + ".wav", AudioType.WAV);
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
                catch(InvalidOperationException exception)
                {

                }
                

            }
        }
        Debug_me.ins.Log("Zakonczenie Coroutine 3");
        UstawNazwy();
    }
    public IEnumerator InicjalizujProcesMP3()
    {
        Debug_me.ins.Log("Rozpoczecie Coroutine 3");
        wczytaneNazwyUtworow = XMLManager.WczytajListeNazwDzwiekow(3, 7);
        dzwieki = new AudioClip[7];
        for (int i = 0; i < 7; i++)
        {
            Debug_me.ins.Log("Dzwiek " + i + " nazwa = " + wczytaneNazwyUtworow.nazwydzwiekow[i].tytul);
            UnityWebRequest AudioFile = UnityWebRequestMultimedia.GetAudioClip(Application.dataPath + "/Dzwieki/3/" + i + ".mp3", AudioType.MPEG);
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
                    TagLib.File tagFile = TagLib.File.Create(Application.dataPath + "/Dzwieki/3/" + i + ".mp3");

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
        Debug_me.ins.Log("Zakonczenie Coroutine 3");
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

        source.Play();
        liczCzas = true;
    }
    public void BTN_Stop()
    {
        source.Stop();
        liczCzas = false;
    }
    public void BTN_Wyswietl()
    {
        if (wybranyUtwor == -1)
            return;

        Button przycisk = przyciskiUtworow[wybranyUtwor];
        przycisk.GetComponentInChildren<Text>().text = wybranyUtwor + ". " +  wczytaneNazwyUtworow.nazwydzwiekow[wybranyUtwor].ZwrocNazwe();
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
            timer -= Time.deltaTime;
            oknoCzasu.text = timer.ToString("n2");

            if (timer <= 0)
            {
                liczCzas = false;
                timer = 0;
                oknoCzasu.text = timer.ToString("n2");
                BTN_Stop();
            }
        }
    }
}
