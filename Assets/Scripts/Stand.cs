using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    [Header("OBJECTS")]
    public List<GameObject> cemberler = new List<GameObject>();
    public GameObject[] soketler;
    public GameObject hareketpztsn;

    [Header("VALUABLES")]
    [SerializeField] GameManager gm;
    public int bos_SoketSayisi;
    [SerializeField] int tamamlanmaSayisi;
    [SerializeField] ParticleSystem kesmefx;

    public GameObject EnUsttekiCemberiAl()
    {
        return cemberler[^1];
    }
    public GameObject MusaitSoketiVer()
    {
        return soketler[bos_SoketSayisi];
    }
    public void SoketDegistirmeIslemleri(GameObject delete_object)
    {
        cemberler.Remove(delete_object);

        if (cemberler.Count != 0)
        {
            bos_SoketSayisi--;
            cemberler[^1].GetComponent<Cember>().hareketEt = true;
        }
        else
        {
            bos_SoketSayisi = 0;
        }
    }
    public void CemberleriKontrolEt()
    {
        if (cemberler.Count == 4)
        {
            string color = cemberler[0].GetComponent<Cember>().color;

            foreach (var item in cemberler)
            {
                if (color == item.GetComponent<Cember>().color)
                {
                    tamamlanmaSayisi++;
                }
            }
            if (tamamlanmaSayisi == 4)
            {
                gm.StandTamamlandi();
                TamamlanmisIslemler();
            }
            else
            {
                tamamlanmaSayisi = 0;
            }
        }
    }
    void TamamlanmisIslemler()
    {
        foreach (var item in cemberler)
        {
            item.GetComponent<Cember>().hareketEt = false;

            Color32 clr = item.GetComponent<MeshRenderer>().material.GetColor("_Color");
            clr.a = 150;
            item.GetComponent<MeshRenderer>().material.SetColor("_Color", clr);
            gameObject.tag = "tamamlanmisstand";

            kesmefx.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            kesmefx.gameObject.SetActive(true);
            kesmefx.Play();
        }
    }
}
