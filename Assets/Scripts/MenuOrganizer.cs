using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOrganizer : MonoBehaviour
{
    public GameObject transition;

    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void GoToMenuStart()
    {
        StartCoroutine(LoadSceneTrans("MenuStart"));
    }

    public void GoToGameMode()
    {
        StartCoroutine(LoadSceneTrans("GameScene"));
    }

    public void GoToPersonaggiMode()
    {
        StartCoroutine(LoadSceneTrans("PersonaggiScene"));
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void OpenBlog()
    {
        Application.OpenURL("http://nicolaraccasceneggiature.altervista.org/");
        Debug.Log("is this working?");
    }

    IEnumerator LoadSceneTrans(string nameScene)
    {
        GameObject transitionGB = Instantiate(transition);
        DontDestroyOnLoad(transitionGB);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(nameScene);
    }
}
