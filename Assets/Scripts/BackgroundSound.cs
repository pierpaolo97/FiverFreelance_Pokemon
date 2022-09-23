using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    private static BackgroundSound instance = null;
    public AudioSource audioback;
    public GameObject backgroundSound;

    private static BackgroundSound Instance
    {
        get { return instance; }
    }


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        //audioback.volume = PlayerPrefs.GetFloat("musicvolume", 0.25f);
        //
        DontDestroyOnLoad(backgroundSound.gameObject);

        AudioListener.volume = PlayerPrefs.GetFloat("musicvolume", 0.25f);


        //Debug.Log(AudioListener.volume);
        ////Debug.Log(PlayerPrefs.GetFloat("musicvolume"));
    }
}