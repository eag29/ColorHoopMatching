using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cember : MonoBehaviour
{
    [Header("OBJECTS")]
    [SerializeField] GameObject Soket;
    public GameObject Stand;
    GameObject hareketpztsn;
    GameObject gidilecekstand;

    [Header("VALUABLES")]
    [SerializeField] GameManager gm;
    public bool hareketEt;
    public string color;
    bool sec, changePozs, soketeOtur, soketeGeriGit;


    public void HareketEt(string islem, GameObject stand = null, GameObject soket = null, GameObject gidilecekobje = null)
    {
        switch (islem)
        {
            case "sec":
                hareketpztsn = gidilecekobje;
                sec = true;
                break;
            case "changePozs":
                gidilecekstand = stand;
                Soket = soket;
                changePozs = true;
                break;
            case "SoketeGeriGit":
                soketeGeriGit = true;
                break;
        }
    }
    void Update()
    {
        if (sec)
        {
            transform.position = Vector3.Lerp(transform.position, hareketpztsn.transform.position, 0.4f);
            if (Vector3.Distance(transform.position, hareketpztsn.transform.position) < 0.1f)
            {
                sec = false;
            }
        }
        if (changePozs)
        {
            transform.position = Vector3.Lerp(transform.position, hareketpztsn.transform.position, 0.45f);
            if (Vector3.Distance(transform.position, hareketpztsn.transform.position) < 0.1f)
            {
                changePozs = false;
                soketeOtur = true;
            }
        }
        if (soketeOtur)
        {
            transform.position = Vector3.Lerp(transform.position, Soket.transform.position, 0.4f);
            if (Vector3.Distance(transform.position, Soket.transform.position) < 0.1f)
            {
                transform.position = Soket.transform.position;
                soketeOtur = false;

                Stand = gidilecekstand;

                if (Stand.GetComponent<Stand>().cemberler.Count > 1)
                {
                    Stand.GetComponent<Stand>().cemberler[^2].GetComponent<Cember>().hareketEt = true;
                }
                gm.hareket = false;
            }
        }
        if (soketeGeriGit)
        {
            transform.position = Vector3.Lerp(transform.position, Soket.transform.position, 0.4f);
            if (Vector3.Distance(transform.position, Soket.transform.position) < 0.1f)
            {
                transform.position = Soket.transform.position;
                soketeGeriGit = false;
                gm.hareket = false;
            }
        }
    }
}
