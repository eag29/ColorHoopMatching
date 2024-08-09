using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    GameObject seciliobje;
    GameObject secilistand;
    [SerializeField] GameObject winfxpztsn;

    [Header("VALUABLES")]
    Cember cember;
    public bool hareket;
    bool play;
    bool soundplay;
    bool vibration;
    [SerializeField] int tamamlananStandSayisi;
    [SerializeField] int hedefStandSayisi;
    [SerializeField] ParticleSystem winfx;

    [Header("SOUNDS")]
    [SerializeField] AudioSource[] sounds; //bg,soketgeriSound, cembergirmeSound, vibrationSound;

    [Header("CANVAS SETTINGS")]
    [SerializeField] TextMeshProUGUI leveltxt;
    [SerializeField] GameObject settingspnl;
    [SerializeField] GameObject winpnl;
    [SerializeField] Button[] buttons;
    [SerializeField] Sprite[] sprites;

    private void Awake()
    {
        SahneIlkIslemler();
    }
    void SahneIlkIslemler()
    {
        if (PlayerPrefs.GetInt("Sound") == 1 && PlayerPrefs.GetInt("Music") == 1)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].mute = false;
            }
            buttons[0].image.sprite = sprites[0];
            buttons[1].image.sprite = sprites[2];
        }
        else if (PlayerPrefs.GetInt("Sound") == 1 && PlayerPrefs.GetInt("Music") == 0)
        {
            sounds[0].mute = true;
            sounds[1].mute = false;
            sounds[2].mute = false;

            buttons[0].image.sprite = sprites[0];
            buttons[1].image.sprite = sprites[3];
        }
        else if (PlayerPrefs.GetInt("Sound") == 0 && PlayerPrefs.GetInt("Music") == 1)
        {
            sounds[0].mute = false;
            sounds[1].mute = true;
            sounds[2].mute = true;

            buttons[0].image.sprite = sprites[1];
            buttons[1].image.sprite = sprites[2];
        }
        else if (PlayerPrefs.GetInt("Sound") == 0 && PlayerPrefs.GetInt("Music") == 0)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].mute = true;
            }
            buttons[0].image.sprite = sprites[1];
            buttons[1].image.sprite = sprites[3];
        }

        if (PlayerPrefs.GetInt("Vibration") == 0)
        {
            sounds[3].mute = true;
            buttons[2].image.sprite = sprites[5];
        }
        else if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            sounds[3].mute = false;
            buttons[2].image.sprite = sprites[4];
        }
    }
    private void Start()
    {
        play = false;
        soundplay = false;
        vibration = false;
        leveltxt.text = SceneManager.GetActiveScene().name;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!play)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 300f))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("stand"))
                        {
                            Invoke("VibrationSoundPlay", 0.01f);

                            if (seciliobje != null & secilistand != hit.collider.gameObject)
                            {
                                Stand stand = hit.collider.GetComponent<Stand>();

                                if (stand.cemberler.Count != 4 & stand.cemberler.Count != 0)
                                {
                                    if (cember.color == stand.cemberler[^1].GetComponent<Cember>().color)
                                    {
                                        secilistand.GetComponent<Stand>().SoketDegistirmeIslemleri(seciliobje);

                                        cember.HareketEt("changePozs", hit.collider.gameObject, stand.MusaitSoketiVer(), stand.hareketpztsn);
                                        sounds[1].Play();

                                        stand.bos_SoketSayisi++;
                                        stand.cemberler.Add(seciliobje);
                                        stand.CemberleriKontrolEt();

                                        seciliobje = null;
                                        secilistand = null;
                                    }
                                    else
                                    {
                                        cember.HareketEt("SoketeGeriGit");
                                        sounds[2].Play();

                                        seciliobje = null;
                                        secilistand = null;
                                    }
                                }
                                else if (stand.cemberler.Count == 0)
                                {
                                    secilistand.GetComponent<Stand>().SoketDegistirmeIslemleri(seciliobje);

                                    cember.HareketEt("changePozs", hit.collider.gameObject, stand.MusaitSoketiVer(), stand.hareketpztsn);
                                    sounds[1].Play();


                                    stand.bos_SoketSayisi++;
                                    stand.cemberler.Add(seciliobje);
                                    stand.CemberleriKontrolEt();

                                    seciliobje = null;
                                    secilistand = null;
                                }
                                else
                                {
                                    cember.HareketEt("SoketeGeriGit");
                                    sounds[2].Play();

                                    seciliobje = null;
                                    secilistand = null;
                                }
                            }
                            else if (secilistand == hit.collider.gameObject)
                            {
                                cember.HareketEt("SoketeGeriGit");
                                sounds[2].Play();

                                seciliobje = null;
                                secilistand = null;
                            }
                            else
                            {
                                Stand stand = hit.collider.GetComponent<Stand>();
                                seciliobje = stand.EnUsttekiCemberiAl();
                                cember = seciliobje.GetComponent<Cember>();
                                hareket = true;

                                if (cember.hareketEt)
                                {
                                    cember.HareketEt("sec", null, null, cember.Stand.GetComponent<Stand>().hareketpztsn);

                                    secilistand = cember.Stand;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void StandTamamlandi()
    {
        tamamlananStandSayisi++;
        if (tamamlananStandSayisi == hedefStandSayisi)
        {
            Win();
        }
    }
    public void ButtonIslemleri(string islem)
    {
        switch (islem)
        {
            case "settings":
                play = true;
                settingspnl.SetActive(true);
                break;
            case "replay":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "exit":
                play = false;
                settingspnl.SetActive(false);
                break;
            case "music":
                if (PlayerPrefs.GetInt("Music") == 1)
                {
                    PlayerPrefs.SetInt("Music", 0);
                    sounds[0].mute = true;
                    buttons[1].image.sprite = sprites[3];
                }

                else if (PlayerPrefs.GetInt("Music") == 0)
                {
                    PlayerPrefs.SetInt("Music", 1);
                    sounds[0].mute = false;
                    buttons[1].image.sprite = sprites[2];
                }
                break;
            case "vibration":
                if (PlayerPrefs.GetInt("Vibration") == 1)
                {
                    PlayerPrefs.SetInt("Vibration", 0);
                    sounds[3].mute = true;
                    buttons[2].image.sprite = sprites[5];
                }

                else if (PlayerPrefs.GetInt("Vibration") == 0)
                {
                    PlayerPrefs.SetInt("Vibration", 1);
                    sounds[3].mute = false;
                    buttons[2].image.sprite = sprites[4];
                }
                break;

            case "sound":
                if (PlayerPrefs.GetInt("Sound") == 1)
                {
                    PlayerPrefs.SetInt("Sound", 0);

                    sounds[1].mute = true;
                    sounds[2].mute = true;

                    buttons[0].image.sprite = sprites[1];
                }

                else if (PlayerPrefs.GetInt("Sound") == 0)
                {
                    PlayerPrefs.SetInt("Sound", 1);

                    sounds[1].mute = false;
                    sounds[2].mute = false;

                    buttons[0].image.sprite = sprites[0];
                }
                break;
            case "nextlevel":
                SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
                break;
        }
    }
    void VibrationSoundPlay()
    {
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            if (!sounds[3].isPlaying)
            {
                sounds[3].Play();
            }
        }
    }
    void Win()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        winfx.transform.position = winfxpztsn.transform.position;
        winfx.gameObject.SetActive(true);
        winfx.Play();
        StartCoroutine(WinPnlCikar());
    }
    IEnumerator WinPnlCikar()
    {
        yield return new WaitForSeconds(1f);
        winpnl.SetActive(true);
    }
}
