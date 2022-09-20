using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOrganizer : MonoBehaviour
{
    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void GoToMenuStart()
    {
        SceneManager.LoadScene("MenuStart");
    }

    public void GoToGameMode()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToPersonaggiMode()
    {
        SceneManager.LoadScene("PersonaggiScene");
    }

}
