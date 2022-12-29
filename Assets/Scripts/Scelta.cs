using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scelta : MonoBehaviour
{
    public GameObject scegliPlayer;
    public GameObject scegliCompagno;
    public BattleSystem battleSystem;

    public GameObject scegliEnemy1;
    public GameObject scegliEnemy2;

    public GameObject playercompagno;
    public GameObject ememy1e2;

    public GameObject interoMenu;

    public GameObject back;
    public GameObject avanti;
    public GameObject startGame;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject player;
    public GameObject friend;

    public AudioSource cameraAudio;
    public List<GameObject> gameobjectDaDisattivare_uno;
    public List<GameObject> gameobjectDaDisattivare_due;
    public List<GameObject> gameobjectDaDisattivare_tre;
    public List<GameObject> gameobjectDaDisattivare_quattro;

    public void Avanza()
    {
        //scegliCompagno.SetActive(true);
        //playercompagno.SetActive(false);
        player = Instantiate(scegliPlayer.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliPlayer.GetComponent<ScegliPersonaggi>().indexPlayer]);
        player.name = player.GetComponent<Unit>().unitName;
        player.GetComponent<Unit>().unitID = 0;      
        battleSystem.playerPrefab = player;
       
        GameObject parentMossePlayer = new GameObject();
        parentMossePlayer.name = "Mosse_" + player.GetComponent<Unit>().unitName;

        for (int i=0; i<4; i++)
        {
            Mossa mossa_da_istanziare = Instantiate(player.GetComponent<Unit>().mosse[i], parentMossePlayer.transform);
            mossa_da_istanziare.name = player.GetComponent<Unit>().mosse[i].GetComponent<Mossa>().nomeMossa;
            player.GetComponent<Unit>().mosse[i] = mossa_da_istanziare;
        }

        StartCoroutine(example(scegliPlayer, gameobjectDaDisattivare_uno, scegliCompagno, scegliPlayer, 0));
    }

    IEnumerator example(GameObject scegli, List<GameObject> lista, GameObject daAttivare, GameObject daDisattivare, int ultimo)
    {
        int x = UnityEngine.Random.RandomRange(0, 2);
        cameraAudio.PlayOneShot(scegli.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegli.GetComponent<ScegliPersonaggi>().indexPlayer].GetComponent<Unit>().audioSelezioni[x]);
        foreach(GameObject g in lista)
        {
            g.GetComponent<Button>().interactable = false;
        }
        yield return new WaitWhile(() => cameraAudio.isPlaying);

        daDisattivare.SetActive(false);
        daAttivare.SetActive(true);

        foreach (GameObject g in lista)
        {
            g.GetComponent<Button>().interactable = true;
        }
        if (ultimo == 1)
        {
            disattivaUltimo();
        }
    }

    public void Back1()
    {
        scegliPlayer.SetActive(true);
        scegliCompagno.SetActive(false);
        //Destroy(friend);
        Destroy(player);
    }

    public void Avanza_2()
    {
        friend = Instantiate(scegliCompagno.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliCompagno.GetComponent<ScegliPersonaggi>().indexPlayer]);
        friend.name = friend.GetComponent<Unit>().unitName;
        friend.GetComponent<Unit>().unitID = 1;
        battleSystem.friendPrefab = friend;
        ememy1e2.SetActive(true);
        GameObject parentMosseFriend = new GameObject();
        parentMosseFriend.name = "Mosse_" + friend.GetComponent<Unit>().unitName;

        for (int i = 0; i < 4; i++)
        {
            Mossa mossa_da_istanziare = Instantiate(friend.GetComponent<Unit>().mosse[i], parentMosseFriend.transform);
            mossa_da_istanziare.name = friend.GetComponent<Unit>().mosse[i].GetComponent<Mossa>().nomeMossa;
            friend.GetComponent<Unit>().mosse[i] = mossa_da_istanziare;
        }
        back.SetActive(false);
        StartCoroutine(example(scegliCompagno, gameobjectDaDisattivare_due, scegliEnemy1, scegliCompagno, 0));
    }

    public void Back2()
    {
        scegliEnemy1.SetActive(false);
        scegliCompagno.SetActive(true);
        //Destroy(friend);
        Destroy(friend);
    }

    public void Avanza_3()
    {
        enemy1 = Instantiate(scegliEnemy1.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliEnemy1.GetComponent<ScegliPersonaggi>().indexPlayer]);
        enemy1.name = enemy1.GetComponent<Unit>().unitName;
        enemy1.GetComponent<Unit>().unitID = 2;
        battleSystem.enemyPrefab = enemy1;
        GameObject parentMosseEnemy1 = new GameObject();
        parentMosseEnemy1.name = "Mosse_" + enemy1.GetComponent<Unit>().unitName;

        for (int i = 0; i < 4; i++)
        {
            Mossa mossa_da_istanziare = Instantiate(enemy1.GetComponent<Unit>().mosse[i], parentMosseEnemy1.transform);
            mossa_da_istanziare.name = enemy1.GetComponent<Unit>().mosse[i].GetComponent<Mossa>().nomeMossa;
            enemy1.GetComponent<Unit>().mosse[i] = mossa_da_istanziare;
        }
        StartCoroutine(example(scegliEnemy1, gameobjectDaDisattivare_tre, scegliEnemy2, scegliEnemy1, 0));
    }

    public void Back3()
    {
        scegliEnemy1.SetActive(true);
        scegliEnemy2.SetActive(false);
        //Destroy(friend);
        Destroy(enemy1);
    }

    public void StartGame()
    {
        
        enemy2 = Instantiate(scegliEnemy2.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliEnemy2.GetComponent<ScegliPersonaggi>().indexPlayer]);
        enemy2.name = enemy2.GetComponent<Unit>().unitName;
        enemy2.GetComponent<Unit>().unitID = 3;
        battleSystem.enemyPrefab = enemy1;
        battleSystem.enemy2Prefab = enemy2;

        GameObject parentMosseEnemy2 = new GameObject();
        parentMosseEnemy2.name = "Mosse_" + enemy2.GetComponent<Unit>().unitName;

        for (int i = 0; i < 4; i++)
        {
            Mossa mossa_da_istanziare = Instantiate(enemy2.GetComponent<Unit>().mosse[i], parentMosseEnemy2.transform);
            mossa_da_istanziare.name = enemy2.GetComponent<Unit>().mosse[i].GetComponent<Mossa>().nomeMossa;
            enemy2.GetComponent<Unit>().mosse[i] = mossa_da_istanziare;
        }

        StartCoroutine(example(scegliEnemy2, gameobjectDaDisattivare_quattro, scegliEnemy2, scegliEnemy2, 1));
       
    }

    public void disattivaUltimo()
    {
        interoMenu.SetActive(false);
        battleSystem.enabled = true;
        ememy1e2.SetActive(false);
    }

    public void ScegliDiNuovo()
    {
        ememy1e2.SetActive(false);
        Destroy(friend);
        Destroy(player);
        back.SetActive(true);
        playercompagno.SetActive(true);
        avanti.SetActive(true);
        startGame.SetActive(false);
    }

    
}
