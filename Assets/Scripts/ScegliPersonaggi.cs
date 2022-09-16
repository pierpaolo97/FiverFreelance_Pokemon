using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScegliPersonaggi : MonoBehaviour
{
    public GameObject[] personaggiDisponibili;
    public Button immaginePersonaggioScelto;

    public int indexPlayer=0;

    private void Start()
    {
        immaginePersonaggioScelto.GetComponent<Image>().sprite = personaggiDisponibili[indexPlayer].GetComponent<Unit>().spriteUnit;
    }

    public void RightArrow()
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
    }

    public void LeftArrow()
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
    }


}
