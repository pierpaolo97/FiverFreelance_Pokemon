using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScegliPersonaggi : MonoBehaviour
{
    public GameObject[] personaggiDisponibili;
    public Button immaginePersonaggioScelto;
    public TextMeshProUGUI txt;

    public int indexPlayer=0;
    public AudioSource cameraAudio;

    private void Start()
    {
        //Debug.Log("qua");
        immaginePersonaggioScelto.GetComponent<Image>().sprite = personaggiDisponibili[indexPlayer].GetComponent<Unit>().spriteUnit;
        txt.text = personaggiDisponibili[indexPlayer].GetComponent<Unit>().unitName;
    }

    public void RightArrow(TextMeshProUGUI txt)
    {
        if (indexPlayer == personaggiDisponibili.Length-1)
        {
            indexPlayer = 0;
        }
        else
        {
            indexPlayer++;
        }

        immaginePersonaggioScelto.GetComponent<Image>().sprite = personaggiDisponibili[indexPlayer].GetComponent<Unit>().spriteUnit;
        txt.text = personaggiDisponibili[indexPlayer].GetComponent<Unit>().unitName;
    }

    public void LeftArrow(TextMeshProUGUI txt)
    {
        if (indexPlayer == 0)
        {
            indexPlayer = personaggiDisponibili.Length-1;
        }
        else
        {
            indexPlayer--;
        }

        immaginePersonaggioScelto.GetComponent<Image>().sprite = personaggiDisponibili[indexPlayer].GetComponent<Unit>().spriteUnit;
        txt.text = personaggiDisponibili[indexPlayer].GetComponent<Unit>().unitName;
    }


}
