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


    public void GoToFirtScene()
    {
        StartCoroutine(LoadSceneTrans("FirstScene"));
    }

    public void GoToStoryScene()
    {
        StartCoroutine(LoadSceneTrans("StoriaScene"));
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

    public void OpenBloodyRoar()
    {
        Application.OpenURL("https://www.amazon.it/Bloody-Roar-Nicola-Racca/dp/B0DKP47TT3/ref=sr_1_2?__mk_it_IT=ÅMÅŽÕÑ&crid=2LITGIXCUB7XO&dib=eyJ2IjoiMSJ9.IvuIq10VTmK-LQfkSNJhR_J-xQZzxO4JxQPlHwLskgAUBVd2Mb02X85FbfuJ0UoDa5uHEkU2Uy2cu5pjkTdh5A.h-tTuFGe_LTjYfATiYS-t8g3bG00ZqXTAkjPzeJenis&dib_tag=se&keywords=bloody+roar+fumetto&qid=1750110760&sprefix=bloody+roar+fmetto%2Caps%2C67&sr=8-2");
        Debug.Log("is this working?");
    }

    public void OpenComicOfLove()
    {
        Application.OpenURL("https://www.amazon.it/Comic-love-Nicola-Racca/dp/B0F9HWTMMZ/ref=sr_1_1?__mk_it_IT=ÅMÅŽÕÑ&crid=1F4H68524BZ1V&dib=eyJ2IjoiMSJ9.ZVeos-K9oBXAys91qg2z2hmZrVNbt-tc6diJp6k2QNM6lBUyFUnhwyiVw0xXcmmWUXu8ZjlwZ2is0YJNFCZyFJcxlYtJKwWdMtECIpkf8XibdMQDbG23cxxfGMR7hiSnX-8H_WEl5JBWPSrvGqZQk5V0GY_eTvnRRv1ODMPCUPJ3akVvxpm8GoQzT9Ime0D-aRs8hjq3oSwB9Z2XLfiyqVZ3UAuAB8MNj2fF4uXq_o_THVhdJuVfWOcNc3TFKLQl4zRNrUJWbVCKv2eKriJpNnR5xAcz-EPCL1vIt2Lb9D4.U3GozbXFZTAYANU4u1pc63iDbGc8zBnZCzLQiFxLfV8&dib_tag=se&keywords=Comic+of+Love&qid=1750110909&sprefix=comic+of+love%2Caps%2C141&sr=8-1");
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
