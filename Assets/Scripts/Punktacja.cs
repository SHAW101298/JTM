using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ENUM_StanSlidera
{
    neutralne,
    wejscie,
    wyjscie
}

public class Punktacja : MonoBehaviour
{
    public int[] punkty;
    public Text[] punkty_pola;
    public RectTransform okno;

    public ENUM_StanSlidera stanSlidera;
    public bool animacja;
    public float modyfikatorPredkosci;

    public void BTN_DodajPunktyDlaGracza100(int x)
    {
        punkty[x] += 100;
        AktualizujTabele();
    }
    public void BTN_OdejmijPunktyDlaGracza100(int x)
    {
        punkty[x] -= 100;
        AktualizujTabele();
    }
    void AktualizujTabele()
    {
        for (int i = 0; i < punkty.Length; i++)
        {
            punkty_pola[i].text = punkty[i].ToString();
        }
    }

    public void Wejscie()
    {
        Debug.Log("Wejscie");
        stanSlidera = ENUM_StanSlidera.wejscie;
        animacja = true;
    }
    public void Wyjscie()
    {
        Debug.Log("Wyjscie");
        stanSlidera = ENUM_StanSlidera.wyjscie;
        animacja = true;
    }

    private void Update()
    {
        if (animacja == false)
            return;

        /*
        if(stanSlidera == ENUM_StanSlidera.wejscie)
        {
            Vector3 vec = okno.localScale;
            vec.x += Time.deltaTime;
            okno.localScale = vec;
            if(okno.localScale.x >= 1)
            {
                vec.x = 1;
                okno.localScale = vec;
                stanSlidera = ENUM_StanSlidera.neutralne;
                animacja = false;
                Debug.Log("Powinno przestac");
            }
        }

        
        if (stanSlidera == ENUM_StanSlidera.wyjscie)
        {
            Vector3 vec = okno.localScale;
            vec.x -= Time.deltaTime;
            okno.localScale = vec;
            if (okno.localScale.x <= 0.5f)
            {
                vec.x = 0.5f;
                okno.localScale = vec;
                stanSlidera = ENUM_StanSlidera.neutralne;
                animacja = false;
            }
        }
        */
        if(stanSlidera == ENUM_StanSlidera.wejscie)
        {
            Vector3 vec = okno.localPosition;
            vec.x -= Time.deltaTime * modyfikatorPredkosci;
            okno.localPosition = vec;
            
            if(okno.localPosition.x <= 800 )
            {
                vec.x = 800;
                okno.localPosition = vec;
                stanSlidera = ENUM_StanSlidera.neutralne;
                animacja = false;
            }
        }
        
        if (stanSlidera == ENUM_StanSlidera.wyjscie)
        {
            Vector3 vec = okno.localPosition;
            vec.x += Time.deltaTime * modyfikatorPredkosci;
            okno.localPosition = vec;

            if (okno.localPosition.x >= 1400)
            {
                vec.x = 1400;
                okno.localPosition = vec;
                stanSlidera = ENUM_StanSlidera.neutralne;
                animacja = false;
            }
        }
    }
}
