using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using TMPro;


public class Aggiornamento : MonoBehaviour
{
    public GameObject menuAggiornamento;
    //public GameObject menuPersonalizzate;
    public GameObject menuPrivacyPolicy;
    string url;
    string urlAndroid = "https://pastebin.com/raw/CQJknyxT";
    string urlIOS = "https://pastebin.com/raw/CQJknyxT";
    string textInternet;
    string linkStore="";

    void Start()
    {
#if UNITY_IOS
        url = urlIOS;
#else
        url = urlAndroid;
#endif

#if UNITY_IOS //DA ERRORE
        /*var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

        if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
        {
            Debug.Log("AUTORIZED PUBBLICITA");

            PlayerPrefs.SetString("PERSONALIZZATE", "SI");
        }
        else if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED)
        {
            Debug.Log("DENIED PUBBLICITA");
            PlayerPrefs.SetString("PERSONALIZZATE", "NO");
        }*/
#endif

        if (!PlayerPrefs.HasKey("PRIVACYPOLICY"))
        {
            menuPrivacyPolicy.SetActive(true);
        }

        GameObject agg = GameObject.Find("CanvasAggiornamento");

        if (agg == null)
        {
            StartCoroutine(checkInternet((isConnected) =>
            {
                if (isConnected)
                {
                    StartCoroutine(LoadTxtData(url));
                }
                else
                {

                }
            }));
        }
    }


    public void privacySI()
    {
        PlayerPrefs.SetString("PRIVACYPOLICY", "SI");
        menuPrivacyPolicy.SetActive(false);
    }


    IEnumerator checkInternet(Action<bool> action)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/");
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    IEnumerator LoadTxtData(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        textInternet = www.downloadHandler.text;

        string[] myStringSplit = textInternet.Split('\n');
        string version = myStringSplit[0];
        //Debug.Log("VERSIONE"+version);
        string text = myStringSplit[1];



        string lan = PlayerPrefs.GetString("LANGUAGE", "ITA");

        switch (lan)
        {
            case "ITA":
                break;
        }


        Debug.Log(version);
        Debug.Log(text);

        float intVersion = float.Parse(version);
        float currentVersion = float.Parse(Application.version);

        if (currentVersion != intVersion)
        {
            //menuAggiornamento.SetActive(true);
            /*GameObject aggiornamento = Instantiate(menuAggiornamento);
            aggiornamento.name = "CanvasAggiornamento";
            aggiornamento.gameObject.transform.Find("Carta").GetChild(0).GetComponent<TextMeshProUGUI>().text = text;*/
            menuAggiornamento.SetActive(true);
            //menuAggiornamento.gameObject.transform.Find("Carta").GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
            menuAggiornamento.gameObject.transform.Find("VaiAlloStore").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Vai allo store";
        }
        else
        {
            menuAggiornamento.gameObject.transform.Find("VaiAlloStore").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Hai gi? l'ultima versione";
        }
    }

    public void apriMenuAgg()
    {
        menuAggiornamento.SetActive(true);
        StartCoroutine(apriMenuAggiornamento());
    }

    IEnumerator apriMenuAggiornamento()
    {
        bool okinternet = false;
        StartCoroutine(checkInternet((isConnected) =>
        {
            if (isConnected)
            {
                okinternet = true;
                Debug.Log("ooooo");
                StartCoroutine(premiBottoneAggiornamento(url));
            }
            else
            {
               
            }
        }));

        /*if (okinternet)
        {
            Debug.Log("ok internet");
            premiBottoneAggiornamento(url);
        }
        else
        {

        }*/

        yield return new WaitForSeconds(0.1f);
    }

    public void Vaiallaggiornamento()
    {
        string link = "https://pastebin.com/raw/Z9LQWjUB";
        StartCoroutine(LoadPaginaAggiornamento(link));
        Application.OpenURL(linkStore);
    }

    IEnumerator LoadPaginaAggiornamento(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        textInternet = www.downloadHandler.text;

        string[] myStringSplit = textInternet.Split('\n');
        string linkAndroid = myStringSplit[0];
        string linkIOS = myStringSplit[1];

        // Debug.Log(linkAndroid);
        //Debug.Log(linkIOS);

#if UNITY_ANDROID
        linkStore = linkAndroid;
#endif
#if UNITY_IOS
        linkStore = linkIOS;
#endif

    }

    IEnumerator premiBottoneAggiornamento(string url)
    {
        Debug.Log("aaaaaaaaaaa");
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        textInternet = www.downloadHandler.text;

        string[] myStringSplit = textInternet.Split('\n');
        string version = myStringSplit[0];
        //Debug.Log("VERSIONE"+version);
        string text = myStringSplit[1];
        string lan = PlayerPrefs.GetString("LANGUAGE", "ITA");

        switch (lan)
        {
            case "ITA":
                break;
        }

        Debug.Log(version);
        Debug.Log(text);

        float intVersion = float.Parse(version);
        float currentVersion = float.Parse(Application.version);
        //menuAggiornamento.gameObject.transform.Find("Carta").GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

        if (currentVersion != intVersion)
        {
            menuAggiornamento.gameObject.transform.Find("VaiAlloStore").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Vai allo store";
        }
        else
        {
            menuAggiornamento.gameObject.transform.Find("VaiAlloStore").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Hai gia' l'ultima versione";
        }
    }



}
