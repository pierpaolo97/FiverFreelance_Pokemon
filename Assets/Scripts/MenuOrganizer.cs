using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOrganizer : MonoBehaviour
{
    public GameObject transitionBlog;
    public GameObject transitionIcona;

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

    public void OpenDomaArtBook()
    {
        Application.OpenURL("https://www.amazon.it/dp/8894924416/ref=as_sl_pc_tf_til?tag=nik1995-21&linkCode=w00&linkId=e953367834677b4139842398595535ff&creativeASIN=88949244166");
        Debug.Log("is this working?");
    }

    public void OpenTraversa()
    {
        Application.OpenURL("https://www.amazon.it/dp/8827848193/ref=as_sl_pc_tf_til?tag=nik1995-21&linkCode=w00&linkId=10843d7683468348fba1663368ccac12&creativeASIN=8827848193");
        Debug.Log("is this working?");
    }

    public void OpenSamElaRagazzaStelle()
    {
        Application.OpenURL("https://www.amazon.it/dp/B092TZF1GM/ref=as_sl_pc_tf_til?tag=nik1995-21&linkCode=w00&linkId=e5650035c3a34ae3d45a034c75f50bd4&creativeASIN=B092TZF1GM");
        Debug.Log("is this working?");
    }

    IEnumerator LoadSceneTrans(string nameScene)
    {
        if (Random.value < 0.5f)
        {
            GameObject transitionGB = Instantiate(transitionIcona);
            DontDestroyOnLoad(transitionGB);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(nameScene);
        }
        else
        {
            GameObject transitionGB = Instantiate(transitionBlog);
            DontDestroyOnLoad(transitionGB);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(nameScene);
        }
    }
}
