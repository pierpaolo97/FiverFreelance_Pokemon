using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    GameObject enemy1;
    GameObject enemy2;
    GameObject player;
    GameObject friend;


    public void Avanza()
    {
        playercompagno.SetActive(false);
        player = Instantiate(scegliPlayer.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliPlayer.GetComponent<ScegliPersonaggi>().indexPlayer]);
        player.name = player.GetComponent<Unit>().unitName;
        player.GetComponent<Unit>().unitID = 0;
        friend = Instantiate(scegliCompagno.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliCompagno.GetComponent<ScegliPersonaggi>().indexPlayer]);
        friend.name = friend.GetComponent<Unit>().unitName;
        friend.GetComponent<Unit>().unitID = 1;
        battleSystem.playerPrefab = player;
        battleSystem.friendPrefab = friend;
        ememy1e2.SetActive(true);

        GameObject parentMossePlayer = new GameObject();
        parentMossePlayer.name = "Mosse_" + player.GetComponent<Unit>().unitName;

        for (int i=0; i<4; i++)
        {
            Mossa mossa_da_istanziare = Instantiate(player.GetComponent<Unit>().mosse[i], parentMossePlayer.transform);
            mossa_da_istanziare.name = player.GetComponent<Unit>().mosse[i].GetComponent<Mossa>().nomeMossa;
            player.GetComponent<Unit>().mosse[i] = mossa_da_istanziare;
        }

        GameObject parentMosseFriend = new GameObject();
        parentMosseFriend.name = "Mosse_" + friend.GetComponent<Unit>().unitName;

        for (int i = 0; i < 4; i++)
        {
            Mossa mossa_da_istanziare = Instantiate(friend.GetComponent<Unit>().mosse[i], parentMosseFriend.transform);
            mossa_da_istanziare.name = friend.GetComponent<Unit>().mosse[i].GetComponent<Mossa>().nomeMossa;
            friend.GetComponent<Unit>().mosse[i] = mossa_da_istanziare;
        }
        back.SetActive(false);
    }

    public void StartGame()
    {
        ememy1e2.SetActive(false);
        enemy1 = Instantiate(scegliEnemy1.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliEnemy1.GetComponent<ScegliPersonaggi>().indexPlayer]);
        enemy1.name = enemy1.GetComponent<Unit>().unitName;
        enemy1.GetComponent<Unit>().unitID = 2;
        enemy2 = Instantiate(scegliEnemy2.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliEnemy2.GetComponent<ScegliPersonaggi>().indexPlayer]);
        enemy2.name = enemy2.GetComponent<Unit>().unitName;
        enemy2.GetComponent<Unit>().unitID = 3;
        battleSystem.enemyPrefab = enemy1;
        battleSystem.enemy2Prefab = enemy2;

        GameObject parentMosseEnemy1 = new GameObject();
        parentMosseEnemy1.name = "Mosse_" + enemy1.GetComponent<Unit>().unitName;

        for (int i = 0; i < 4; i++)
        {
            Mossa mossa_da_istanziare = Instantiate(enemy1.GetComponent<Unit>().mosse[i], parentMosseEnemy1.transform);
            mossa_da_istanziare.name = enemy1.GetComponent<Unit>().mosse[i].GetComponent<Mossa>().nomeMossa;
            enemy1.GetComponent<Unit>().mosse[i] = mossa_da_istanziare;
        }

        GameObject parentMosseEnemy2 = new GameObject();
        parentMosseEnemy2.name = "Mosse_" + enemy2.GetComponent<Unit>().unitName;

        for (int i = 0; i < 4; i++)
        {
            Mossa mossa_da_istanziare = Instantiate(enemy2.GetComponent<Unit>().mosse[i], parentMosseEnemy2.transform);
            mossa_da_istanziare.name = enemy2.GetComponent<Unit>().mosse[i].GetComponent<Mossa>().nomeMossa;
            enemy2.GetComponent<Unit>().mosse[i] = mossa_da_istanziare;
        }


        interoMenu.SetActive(false);
        battleSystem.enabled = true;
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
