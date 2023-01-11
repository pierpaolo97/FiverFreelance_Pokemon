using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioScelta : MonoBehaviour
{
    public List<AudioClip> listaAudio;
    //public Button immaginePersonaggioScelto;
    public TextMeshProUGUI txt;

    public int indexPlayer = 0;
    public AudioSource cameraAudio;

    private void Start()
    {
        txt.text = (indexPlayer + 1).ToString();
    }

    public void RightArrow(TextMeshProUGUI txt)
    {
        if (indexPlayer == listaAudio.Count - 1)
        {
            indexPlayer = 0;
        }
        else
        {
            indexPlayer++;
        }

        //immaginePersonaggioScelto.GetComponent<Image>().sprite = personaggiDisponibili[indexPlayer].GetComponent<Unit>().spriteUnit;
        txt.text = (indexPlayer + 1).ToString();
    }

    public void LeftArrow(TextMeshProUGUI txt)
    {
        if (indexPlayer == 0)
        {
            indexPlayer = listaAudio.Count - 1;
        }
        else
        {
            indexPlayer--;
        }

        //immaginePersonaggioScelto.GetComponent<Image>().sprite = personaggiDisponibili[indexPlayer].GetComponent<Unit>().spriteUnit;
        txt.text = (indexPlayer + 1).ToString();
    }

    public void Riproduci()
    {
        cameraAudio.PlayOneShot(listaAudio[indexPlayer]);
    }
}
