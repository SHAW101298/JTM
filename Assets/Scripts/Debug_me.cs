using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_me : MonoBehaviour
{
    #region
    public static Debug_me ins;
    public void REFERENCJA()
    {
        ins = this;
    }
    #endregion
    public int step;
    public bool pokazywacWiadomosci;


    private void Start()
    {
    }

    /*
    public void LogT(int krok, string tekst)
    {
        log = log +(krok + " ]" + tekst + "\n");
        LOG_FIELD.text = log;
    }
    */
    public void LogT(int krok, string tekst)
    {

        Debug.Log(krok + " | " + tekst);
    }
    public void Log(string tekst)
    {
        if(pokazywacWiadomosci == true)
        {
            Debug.Log(tekst);
        }
    }
}
