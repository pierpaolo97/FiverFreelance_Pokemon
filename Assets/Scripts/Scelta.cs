using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Avanza()
    {
        playercompagno.SetActive(false);
        battleSystem.playerPrefab = scegliPlayer.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliPlayer.GetComponent<ScegliPersonaggi>().indexPlayer];
        battleSystem.friendPrefab = scegliCompagno.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliCompagno.GetComponent<ScegliPersonaggi>().indexPlayer];
        ememy1e2.SetActive(true);
    }

    public void StartGame()
    {
        ememy1e2.SetActive(false);
        battleSystem.enemyPrefab = scegliEnemy1.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliEnemy1.GetComponent<ScegliPersonaggi>().indexPlayer];
        battleSystem.enemy2Prefab = scegliEnemy2.GetComponent<ScegliPersonaggi>().personaggiDisponibili[scegliEnemy2.GetComponent<ScegliPersonaggi>().indexPlayer];
        interoMenu.SetActive(false);
        battleSystem.enabled = true;
    }

    
}
