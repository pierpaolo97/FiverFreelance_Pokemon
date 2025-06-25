using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAppereance : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro textComponent; 
    public float delay = 0.05f;
    public string fullText;

    private void Start()
    {
        StartCoroutine(ShowTextLetterByLetter());
    }

    IEnumerator ShowTextLetterByLetter()
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
