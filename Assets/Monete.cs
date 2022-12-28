using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Monete : MonoBehaviour
{
    public TextMeshProUGUI txtMonete;
    void Start()
    {
        txtMonete.text = PlayerPrefs.GetInt("MONETE", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
