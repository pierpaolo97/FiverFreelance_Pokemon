using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOrganizer : MonoBehaviour
{
    public void GoToGameMode()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToPersonaggiMode()
    {
        SceneManager.LoadScene("PersonaggiMode");
    }

}
