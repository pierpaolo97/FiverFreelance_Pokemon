using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    [SerializeField] Slider volumeSlider;
    public AudioSource audioBackgroud;
    // Start is called before the first frame update
    void Start()
    {

        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        audioBackgroud = GameObject.Find("BackgroundSound").GetComponent<AudioSource>();

        if (!PlayerPrefs.HasKey("musicvolume"))
        {
            PlayerPrefs.SetFloat("musicvolume", 0.25f);
            volumeSlider.value = 1f;
        }
        else
        {
            Load();
        }
        //Debug.Log(PlayerPrefs.GetFloat("musicvolume"));
    }

    public void ChangeVolume()
    {
        audioBackgroud.volume = volumeSlider.value / 4;
        //AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicvolume", 0.25f) * 4;
        AudioListener.volume = PlayerPrefs.GetFloat("musicvolume", 0.25f);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicvolume", volumeSlider.value / 4);
        AudioListener.volume = PlayerPrefs.GetFloat("musicvolume", 0.25f);
    }
}
