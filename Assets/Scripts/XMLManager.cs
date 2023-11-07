
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public static class XMLManager
{
    static public ListaNazwDzwiekowXML WczytajListeNazwDzwiekow(int numerFolderu)
    {
        Debug.Log("XML MANAGER wczytywanie nazw dzwiekow");
        SprawdzCzyIstniejeSciezka();
        if (File.Exists(Application.dataPath + "/Dzwieki/" + numerFolderu + "/ListaNazwDzwiekow.xml"))
        {
            
            Debug_me.ins.Log("Znaleziono Plik Liste Nazw Dzwiekow XML");
            ListaNazwDzwiekowXML lista_dzwiekow;

            XmlSerializer serializer = new XmlSerializer(typeof(ListaNazwDzwiekowXML));
            FileStream stream = new FileStream(Application.dataPath + "/Dzwieki/" + numerFolderu + "/ListaNazwDzwiekow.xml", FileMode.OpenOrCreate);

            lista_dzwiekow = serializer.Deserialize(stream) as ListaNazwDzwiekowXML;
            stream.Close();

            return lista_dzwiekow;
        }
        else
        {
            Debug_me.ins.Log("Nie Znaleziono Plik Liste Nazw Dzwiekow XML");
            ListaNazwDzwiekowXML lista_dzwiekow;
            lista_dzwiekow = new ListaNazwDzwiekowXML();
            // Przypisanie jakiejkolwiek nazwy

            XmlSerializer serializer = new XmlSerializer(typeof(ListaNazwDzwiekowXML));
            FileStream stream = new FileStream(Application.dataPath + "/Dzwieki/" + numerFolderu + "/ListaNazwDzwiekow.xml", FileMode.Create);
            serializer.Serialize(stream, lista_dzwiekow);
            stream.Close();

            return lista_dzwiekow;
        }
    }
    static public ListaNazwDzwiekowXML WczytajListeNazwDzwiekow(int numerFolderu, int x)
    {
        Debug.Log("XML MANAGER wczytywanie nazw dzwiekow");
        SprawdzCzyIstniejeSciezka();
        if (File.Exists(Application.dataPath + "/Dzwieki/" + numerFolderu + "/ListaNazwDzwiekow.xml"))
        {

            Debug_me.ins.Log("Znaleziono Plik Liste Nazw Dzwiekow XML");
            ListaNazwDzwiekowXML lista_dzwiekow;

            XmlSerializer serializer = new XmlSerializer(typeof(ListaNazwDzwiekowXML));
            FileStream stream = new FileStream(Application.dataPath + "/Dzwieki/" + numerFolderu + "/ListaNazwDzwiekow.xml", FileMode.OpenOrCreate);

            lista_dzwiekow = serializer.Deserialize(stream) as ListaNazwDzwiekowXML;
            stream.Close();

            return lista_dzwiekow;
        }
        else
        {
            Debug_me.ins.Log("Nie Znaleziono Plik Liste Nazw Dzwiekow XML");
            ListaNazwDzwiekowXML lista_dzwiekow;
            lista_dzwiekow = new ListaNazwDzwiekowXML(x);
            // Przypisanie jakiejkolwiek nazwy

            XmlSerializer serializer = new XmlSerializer(typeof(ListaNazwDzwiekowXML));
            FileStream stream = new FileStream(Application.dataPath + "/Dzwieki/" + numerFolderu + "/ListaNazwDzwiekow.xml", FileMode.Create);
            serializer.Serialize(stream, lista_dzwiekow);
            stream.Close();

            return lista_dzwiekow;
        }
    }

    // Sprawdzenie czy istnieja wszystkie wymagane foldery
    public static void SprawdzCzyIstniejeSciezka()
    {
        if(!System.IO.Directory.Exists(Application.dataPath + "/Dzwieki"))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki");
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki/0");
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki/1");
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki/2");
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki/3");
        }
        else
        {
            if (!System.IO.Directory.Exists(Application.dataPath + "/Dzwieki/0"))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki/0");
            }
            if (!System.IO.Directory.Exists(Application.dataPath + "/Dzwieki/1"))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki/1");
            }
            if (!System.IO.Directory.Exists(Application.dataPath + "/Dzwieki/2"))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki/2");
            }
            if (!System.IO.Directory.Exists(Application.dataPath + "/Dzwieki/3"))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/Dzwieki/3");
            }
        }
    }
}
