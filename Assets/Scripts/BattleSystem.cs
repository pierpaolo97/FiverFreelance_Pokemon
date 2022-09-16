﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, FRIENDTURN, ENEMY2TURN, WON, LOST, FINISHED}

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

    public GameObject friendPrefab;
    public GameObject enemy2Prefab;

    public Transform playerBattleStation; //posizione del nostro giocatore
	public Transform enemyBattleStation; //posizione del nemico
    public Transform friendBattleStation; //posizione del nostro giocatore
    public Transform enemy2BattleStation; //posizione del nemico

    public Unit playerUnit;
    public Unit enemyUnit;
    public Unit friendUnit;
    public Unit enemy2Unit;

    public TextMeshProUGUI dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;
    public BattleHUD friendHUD;
    public BattleHUD enemy2HUD;

    public BattleState state;

    public GameObject chiVuoiAttaccare;
    public Text chiVuoiAttaccareText;
    public Button chiVuoiAttaccareButton1;
    public Button chiVuoiAttaccareButton2;

    public GameObject bottoniMosse;

    public List<Tuple<GameObject, int>> valoriVelocita = new List<Tuple<GameObject, int>>();
    public GameObject[] gameobjectInOrdine;
    public GameObject turnoDiGameobject;

    public Unit nemicoAttaccatoDalPlayer;
    public BattleHUD nemicoAttaccatoDalPlayerHUD;

    public Unit[] amici;
    public Unit[] nemici;

    public Mossa emptyMossa;

    public Mossa mossaDaEseguire;
    public Mossa friendMossaDaEseguire;
    public Mossa enemyMossaDaEseguire;
    public Mossa enemy2MossaDaEseguire;

    public Unit giocatoreDaAttaccareFRIEND;
    public Unit giocatoreDaAttaccareENEMY;
    public Unit giocatoreDaAttaccareENEMY2;

    public BattleHUD giocatoreDaAttaccareFRIEND_HUD;
    public BattleHUD giocatoreDaAttaccareENEMY_HUD;
    public BattleHUD giocatoreDaAttaccareENEMY2_HUD;

    public GameObject[] bot;

    private float delay = 0.025f;

    string currentText = "";

    // Start is called before the first frame update
    void Start()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
        OrdinaAttacchi();
    }

	IEnumerator SetupBattle()
	{
        /*GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

        GameObject friendGO = Instantiate(friendPrefab, friendBattleStation);
        friendUnit = friendGO.GetComponent<Unit>();

        GameObject enemy2GO = Instantiate(enemy2Prefab, enemy2BattleStation);
        enemy2Unit = enemy2GO.GetComponent<Unit>();*/

        playerUnit = playerPrefab.GetComponent<Unit>();
        friendUnit = friendPrefab.GetComponent<Unit>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();
        enemy2Unit = enemy2Prefab.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        friendHUD.SetHUD(friendUnit);
        enemy2HUD.SetHUD(enemy2Unit);

        string TestoBenvenuto = playerUnit.unitName + " e " + friendUnit.unitName + ", " + enemyUnit.unitName + " e " + enemy2Unit.unitName + " vi sfidano! ";

        StartCoroutine(ShowText(TestoBenvenuto));

        yield return new WaitForSeconds(5);


        for (int i=0; i<4; i++)
        {
            bottoniMosse.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerUnit.mosse[i].nomeMossa;
            Mossa mossa = playerUnit.mosse[i];
            bottoniMosse.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => mossa.SalvaMossa(mossa));
            //bottoniMosse.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => emptyMossa.EseguiMossa(playerUnit.mosse[i]));

        }

        amici[0] = playerPrefab.GetComponent<Unit>();
        amici[1] = friendPrefab.GetComponent<Unit>();
        nemici[0] = enemyPrefab.GetComponent<Unit>();
        nemici[1] = enemy2Prefab.GetComponent<Unit>();

        bot[0] = enemyPrefab;
        bot[1] = enemy2Prefab;
        bot[2] = friendPrefab;

        //yield return new WaitForSecondsRealtime(5);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}


    /*IEnumerator PlayerAttackMIO(Unit qualeNemicoAttacchi, BattleHUD qualeNemicoHUD)
    {
        bool isDead = qualeNemicoAttacchi.TakeDamage(playerUnit.damage);

        qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
        dialogueText.text = "The attack is successful!";

        Unit giocatoreNONAttaccato;

        if (qualeNemicoAttacchi.unitName == enemyUnit.unitName)
        {
            giocatoreNONAttaccato = enemy2Unit;
        }
        else
        {
            giocatoreNONAttaccato = enemyUnit;
        }

        if (isDead && giocatoreNONAttaccato.currentHP <= 0)
        {
            state = BattleState.WON;
            qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP = 0);
            EndBattle();
        }
        else
        {
            //state = BattleState.ENEMYTURN;
            qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
            dialogueText.text = "Hai tolto " + playerUnit.damage + " HP...";

            yield return new WaitForSeconds(2f);
            ProssimoCheAttacca();
            //StartCoroutine(EnemyTurn());
        }
    }*/


    void PlayerTurn() //Qui il giocatore sceglie la mossa, che non viene eseguita qui ma dopo quando sarà il suo turno di gioco. Se sei morto esce che sei esausto.
    {
        state = BattleState.PLAYERTURN;
        BotSalvaMossa();

        Debug.Log("IL GIOCATORE DEVE SCEGLIEREEEEEEEEEEEEEEEEEEEEEEEEE");
        
        if (playerUnit.currentHP > 0)
        {
            if (!playerUnit.paralizzato)
            {
                Debug.Log("TUTTO OK");
                string ScegliAzione = playerUnit.unitName + " scegli un'azione! ";
                StartCoroutine(ShowText(ScegliAzione));
                bottoniMosse.SetActive(true);
            }
            else
            {
                Debug.Log(playerUnit.unitName + " è paralizzatooooooooooooooooooooooooooooooooooo PASSA AL PROSSIMO!");
                //playerUnit.paralizzato = false;
                string Paralizzato = playerUnit.unitName + " è paralizzato! ";
                StartCoroutine(ShowText(Paralizzato));
                StartCoroutine(WaitSceltaTurno(5));

                //ProssimoCheAttacca();
            }
        }
        else
        {
            Debug.Log(playerUnit.unitName + " è esaustooooooooooooooooooooooooooooooooo PASSA AL PROSSIMO!");
            string Esausto = playerUnit.unitName + " è esausto! ";
            StartCoroutine(ShowText(Esausto));
            state = BattleState.ENEMYTURN;
            StartCoroutine(WaitProssimoCheAttacca(5));
            //StartCoroutine(EnemyTurn());
        }
    }

    public IEnumerator WaitProssimoCheAttacca(float delayInSecs)
    {
        yield return new WaitForSeconds(delayInSecs);
        Debug.Log("INIZIA IL GIRO DI ATTACCHI OOOOOOOOOOOOOOOOOO");
        ProssimoCheAttacca();
    }

    public IEnumerator WaitBottoniMosse()
    {
        yield return new WaitForSeconds(2);
        bottoniMosse.SetActive(true);
    }

    public IEnumerator WaitSceltaTurno(float delayInSecs)
    {
        yield return new WaitForSeconds(delayInSecs);
        Debug.Log("INIZIA IL GIRO DI ATTACCHI OOOOOOOOOOOOOOOOOO");
        SceltaTurno();
    }


    public void BotSalvaMossa()
    {
        Unit giocatoreAttaccato;
        BattleHUD giocatoreAttaccatoHUD;
        bool boolAttacco;
        

        for (int i=0; i<3; i++)
        {
            GameObject personaggioCheDeveScegliere = bot[i];
            //int x = UnityEngine.Random.Range(0, 4);
            //Mossa mossaSelezionata = personaggioCheDeveScegliere.GetComponent<Unit>().mosse[x];

            if (personaggioCheDeveScegliere.GetComponent<Unit>().unitName == friendUnit.unitName)
            {
                Debug.Log("Friend mossa selezionata");
                //friendMossaDaEseguire = mossaSelezionata;
                (giocatoreAttaccato, giocatoreAttaccatoHUD, friendMossaDaEseguire, boolAttacco) = BotSceglieMossa(friendUnit);
                giocatoreDaAttaccareFRIEND = giocatoreAttaccato;
                giocatoreDaAttaccareFRIEND_HUD = giocatoreAttaccatoHUD;
            }
            else if (personaggioCheDeveScegliere.GetComponent<Unit>().unitName == enemyUnit.unitName)
            {
                Debug.Log("ENEMY mossa selezionata");
                (giocatoreAttaccato, giocatoreAttaccatoHUD, enemyMossaDaEseguire, boolAttacco) = BotSceglieMossa(enemyUnit);
                giocatoreDaAttaccareENEMY = giocatoreAttaccato;
                giocatoreDaAttaccareENEMY_HUD = giocatoreAttaccatoHUD;
                //enemyMossaDaEseguire = mossaSelezionata;
            }
            else if (personaggioCheDeveScegliere.GetComponent<Unit>().unitName == enemy2Unit.unitName)
            {
                Debug.Log("ENEMY 2 mossa selezionata");
                (giocatoreAttaccato, giocatoreAttaccatoHUD, enemy2MossaDaEseguire, boolAttacco) = BotSceglieMossa(enemy2Unit);
                giocatoreDaAttaccareENEMY2 = giocatoreAttaccato;
                giocatoreDaAttaccareENEMY2_HUD = giocatoreAttaccatoHUD;
                //enemy2MossaDaEseguire = mossaSelezionata;
            }
        }
    }


    public (Unit, BattleHUD, Mossa, bool) BotSceglieMossa(Unit giocatoreCheDeveDecidereUnit)
    {
        Mossa mossa_casuale = new Mossa();

        if (giocatoreCheDeveDecidereUnit.unitName == friendUnit.unitName)
        {
            //il giocatore che deve scegliere è Friend, controllo se i due avversari hanno meno XP di quelli che tolgo e quindi posso ucciderli. Scelgo la mossa che li può uccidere;
            //Se enemy1 sta per morire lo uccido, altrimenti cerco di uccidere enemy2. 

            foreach (Mossa mossa in giocatoreCheDeveDecidereUnit.mosse)
            {
                if (enemyUnit.currentHP < calcolaDannoEffettivo(mossa.danni, giocatoreCheDeveDecidereUnit.attacco_speciale) && enemyUnit.currentHP > 0)
                {
                    return (enemyUnit, enemyHUD, mossa, true);
                }
                else if (enemy2Unit.currentHP < calcolaDannoEffettivo(mossa.danni, giocatoreCheDeveDecidereUnit.attacco_speciale) && enemy2Unit.currentHP > 0)
                {
                    return (enemy2Unit, enemy2HUD, mossa, true);
                }
            }

            //se nessuno dei due stava per morire, controllo se sono in fin di vita e voglio aumentare la mia vita
            foreach (Mossa mossa in giocatoreCheDeveDecidereUnit.mosse)
            {
                if(mossa.tipologiaDiMosaa == "CURA")
                {
                    if (giocatoreCheDeveDecidereUnit.currentHP < giocatoreCheDeveDecidereUnit.maxHP / 4)
                    {
                        return (giocatoreCheDeveDecidereUnit, friendHUD, mossa, false); //in questo caso non servono i primi 2, perché vengono ripresi dalla funzione dentro cui viene richiamata ChooseToAttackOrDefend
                    }
                }
            }

            // In questa parte ci entra solo se non è nei due casi particolari precedenti: se l'avversario non sta schiattando e se io non sto schiattando, scelgo una mossa a caso.
            // Qua si potrebbe far si che si seleziona una mossa a caso solo tra quelle di attacco o comunque non di cura. 

            //Se quando carichiamo le mosse, mettiamo le mosse di cura sempre per ultime nel vettore mosse, possiamo contare per ogni giocatore le mosse di CURA, 
            //e quindi poi scegliere solo tra le altre mosse che saranno le prime 4-numeroDiMossaCura del vettore giocatoreCheDeveDecidereUnit.mosse
    
            int numeroDiMossaCura = 0;
            foreach (Mossa mossa in giocatoreCheDeveDecidereUnit.mosse)
            {
                if(mossa.tipologiaDiMosaa == "CURA")
                {
                    numeroDiMossaCura++;
                }
            }

            int x = UnityEngine.Random.Range(0, 4-numeroDiMossaCura);
            Mossa mossaSelezionata = giocatoreCheDeveDecidereUnit.mosse[x];
            bool mossaDiAttacco;

            if (mossaSelezionata.tipologiaDiMosaa == "CURA")
            {
                mossaDiAttacco = false;
            }
            else
            {
                mossaDiAttacco = true;
            }


            int y = UnityEngine.Random.Range(0, 2); //se y=0, cerco di attaccare prima il primo se è vivo, altrimenti attacco il secondo.
                                                    //se y=1, cerco di attaccare prima il secondo se è vivo, altrimenti attacco il primo.
                                                    // In questo modo garantisco casualità di chi attacco. 
            if (y == 0 && enemyUnit.currentHP > 0)
            {
                return (enemyUnit, enemyHUD, mossaSelezionata, mossaDiAttacco);
            }
            else if (y == 0 && enemy2Unit.currentHP > 0)
            {
                return (enemy2Unit, enemy2HUD, mossaSelezionata, mossaDiAttacco);
            }
            else if (y == 1 && enemy2Unit.currentHP > 0)
            {
                return (enemy2Unit, enemy2HUD, mossaSelezionata, mossaDiAttacco);
            }
            else if (y == 1 && enemyUnit.currentHP > 0)
            {
                return (enemyUnit, enemyHUD, mossaSelezionata, mossaDiAttacco);
            }
            
        }
        else if (giocatoreCheDeveDecidereUnit.unitName == enemyUnit.unitName || giocatoreCheDeveDecidereUnit.unitName == enemy2Unit.unitName)
        {

            foreach (Mossa mossa in giocatoreCheDeveDecidereUnit.mosse)
            {
                if (playerUnit.currentHP < calcolaDannoEffettivo(mossa.danni, giocatoreCheDeveDecidereUnit.attacco_speciale) && playerUnit.currentHP > 0)
                {
                    return (playerUnit, playerHUD, mossa, true);
                }
                else if (friendUnit.currentHP < calcolaDannoEffettivo(mossa.danni, giocatoreCheDeveDecidereUnit.attacco_speciale) && friendUnit.currentHP > 0)
                {
                    return (enemy2Unit, enemy2HUD, mossa, true);
                }
            }

            //se nessuno dei due stava per morire, controllo se sono in fin di vita e voglio aumentare la mia vita
            foreach (Mossa mossa in giocatoreCheDeveDecidereUnit.mosse)
            {
                if (mossa.tipologiaDiMosaa == "CURA")
                {
                    if (giocatoreCheDeveDecidereUnit.currentHP < giocatoreCheDeveDecidereUnit.maxHP / 4)
                    {
                        return (giocatoreCheDeveDecidereUnit, friendHUD, mossa, false); //in questo caso non servono i primi 2, perché vengono ripresi dalla funzione dentro cui viene richiamata ChooseToAttackOrDefend
                    }
                }
            }

            int numeroDiMossaCura = 0;
            foreach (Mossa mossa in giocatoreCheDeveDecidereUnit.mosse)
            {
                if (mossa.tipologiaDiMosaa == "CURA")
                {
                    numeroDiMossaCura++;
                }
            }

            int x = UnityEngine.Random.Range(0, 4 - numeroDiMossaCura);
            Mossa mossaSelezionata = giocatoreCheDeveDecidereUnit.mosse[x];
            bool mossaDiAttacco;

            if (mossaSelezionata.tipologiaDiMosaa == "CURA")
            {
                mossaDiAttacco = false;
            }
            else
            {
                mossaDiAttacco = true;
            }

            int y = UnityEngine.Random.Range(0, 2);

            if (y == 0 && playerUnit.currentHP > 0)
            {
                return (playerUnit, playerHUD, mossaSelezionata, true);
            }
            else if (y == 0 && friendUnit.currentHP > 0)
            {
                return (friendUnit, friendHUD, mossaSelezionata, true);
            }
            else if (y == 1 && friendUnit.currentHP > 0)
            {
                return (friendUnit, friendHUD, mossaSelezionata, true);
            }
            else if (y == 1 && playerUnit.currentHP > 0)
            {
                return (playerUnit, playerHUD, mossaSelezionata, true);
            }
       
        }

        return (giocatoreCheDeveDecidereUnit, playerHUD, mossa_casuale, false); //qui non dovrebbe mai finirci
    }


    IEnumerator FriendTurn()
    {
        bottoniMosse.SetActive(false);
        if (friendUnit.currentHP > 0)
        {
            if (!friendUnit.paralizzato)
            {
                //yield return new WaitForSeconds(1);

                friendMossaDaEseguire.GetComponent<Mossa>().Esegui(friendMossaDaEseguire, friendUnit, friendHUD, giocatoreDaAttaccareFRIEND, giocatoreDaAttaccareFRIEND_HUD);

            }
            else
            {
                Debug.Log(friendUnit.unitName + " è paralizzato, non può attaccare! ");
                friendUnit.paralizzato = false;
                string ParalizzatoNoAttaccare = friendUnit.unitName + " è paralizzato, non può attaccare! ";
                StartCoroutine(ShowText(ParalizzatoNoAttaccare));
                friendUnit.gameObject.GetComponent<Animator>().Play("ParalizzatoPg");
                yield return new WaitForSeconds(5f);
                //ProssimoCheAttacca();
            }

        }
        else
        {
            //state = BattleState.ENEMY2TURN;
            string Esausto = friendUnit.unitName + " è esausto ";
            StartCoroutine(ShowText(Esausto));
            yield return new WaitForSeconds(5f);
            //StartCoroutine(Enemy2Turn());
            //ProssimoCheAttacca();
        }
        if (AttaccoNormale.Successo == true)
        {
            yield return new WaitForSeconds(10f);
            ProssimoCheAttacca();
        }
        else
        {
            yield return new WaitForSeconds(4f);
            ProssimoCheAttacca();
        }

    }

    IEnumerator EnemyTurn()
    {
        bottoniMosse.SetActive(false);
        if (enemyUnit.currentHP > 0)
        {
            if (!enemyUnit.paralizzato)
            {
                //yield return new WaitForSeconds(1);

                enemyMossaDaEseguire.GetComponent<Mossa>().Esegui(enemyMossaDaEseguire, enemyUnit, enemyHUD, giocatoreDaAttaccareENEMY, giocatoreDaAttaccareENEMY_HUD);
            }
            else
            {
                enemyUnit.paralizzato = false;
                string ParalizzatoNoAttaccare = enemyUnit.unitName + " è paralizzato, non può attaccare! ";
                StartCoroutine(ShowText(ParalizzatoNoAttaccare));
                enemyUnit.gameObject.GetComponent<Animator>().Play("ParalizzatoPg");
                yield return new WaitForSeconds(5f);
                //ProssimoCheAttacca();
            }
        }
        else
        {
            //state = BattleState.FRIENDTURN;
            string Esausto = enemyUnit.unitName + " è esausto ";
            StartCoroutine(ShowText(Esausto));
            yield return new WaitForSeconds(4f);
            //StartCoroutine(FriendTurn());
            //ProssimoCheAttacca();
        }

        if (AttaccoNormale.Successo == true)
        {
            yield return new WaitForSeconds(10f);
            ProssimoCheAttacca();
        }
        else
        {
            Debug.Log("HA FALLITO MA STA QUA?");
            yield return new WaitForSeconds(3f);
            ProssimoCheAttacca();
        }
    }

    IEnumerator Enemy2Turn()
    {
        bottoniMosse.SetActive(false);
        if (enemy2Unit.currentHP > 0)
        {
            if (!enemy2Unit.paralizzato)
            {
                //yield return new WaitForSeconds(1);

                enemy2MossaDaEseguire.GetComponent<Mossa>().Esegui(enemy2MossaDaEseguire, enemy2Unit, enemy2HUD, giocatoreDaAttaccareENEMY2, giocatoreDaAttaccareENEMY2_HUD);
            }
            else
            {
                enemy2Unit.paralizzato = false;
                string ParalizzatoNoAttaccare = enemy2Unit.unitName + " è paralizzato, non può attaccare! ";
                StartCoroutine(ShowText(ParalizzatoNoAttaccare));
                enemy2Unit.gameObject.GetComponent<Animator>().Play("ParalizzatoPg");
                yield return new WaitForSeconds(5f);
                //ProssimoCheAttacca();
            }
        }
        else
        {
            string Esausto = enemy2Unit.unitName + " è esausto ";
            StartCoroutine(ShowText(Esausto));
            //state = BattleState.PLAYERTURN;
            yield return new WaitForSeconds(4f);
            //PlayerTurn();
            //ProssimoCheAttacca();
        }
        if (AttaccoNormale.Successo == true)
        {
            yield return new WaitForSeconds(10f);
            ProssimoCheAttacca();
        }
        else
        {
            Debug.Log("HA FALLITO MA STA QUA?");
            yield return new WaitForSeconds(3f);
            ProssimoCheAttacca();
        }
    }


    public void OrdinaAttacchi() //Metto in ordine i giocatori per velocità per capire chi attacca per primo
    {
        
        valoriVelocita.Add(Tuple.Create(playerPrefab, playerUnit.velocita));
        valoriVelocita.Add(Tuple.Create(friendPrefab, friendUnit.velocita));
        valoriVelocita.Add(Tuple.Create(enemyPrefab, enemyUnit.velocita));
        valoriVelocita.Add(Tuple.Create(enemy2Prefab, enemy2Unit.velocita));

        valoriVelocita = valoriVelocita.OrderByDescending(t => t.Item2).ToList();

        int k = 0;

        foreach (var item in valoriVelocita)
        {
            gameobjectInOrdine[k] = item.Item1;
            k++;       
        }

        turnoDiGameobject = gameobjectInOrdine[0];

        //PlayerTurn();         //Ho messo in ordine i giocatori per velocità. A questo punto però deve comunque fare la prima mossa il Player. Quindi richiamo questa funzione. 
                              //In PlayerTurn() attivo i bottoni per fargli fare la scelta. 
                              //Nelle funzioni attaccate ai bottoni devo salvare la mossa da qualche parte, per poterla usare quando sarà il turno.
                              //Sarà lì che richiamo SceltaTurno(), 

        //SceltaTurno();
    }


    public void ProssimoCheAttacca() //Dopo aver ordinato i giocatori per velocità, vedo chi sarà il prossimo ad attaccare. Se sta attaccando l'ultimo (index=3) ricomincio il giro.
    {
        int index = Array.IndexOf(gameobjectInOrdine, turnoDiGameobject);
        Debug.Log(index);
        //Debug.Log(index);

        if (index == 3)
        {
            index = 0;
            turnoDiGameobject = gameobjectInOrdine[index];
            StartCoroutine(WaitPlayerTurn());
        }
        else
        {
            index++;
            turnoDiGameobject = gameobjectInOrdine[index];
            SceltaTurno();
        }
    }

    IEnumerator WaitPlayerTurn()
    {
            yield return new WaitForSeconds(3f);
            PlayerTurn();
    }

    IEnumerator WaitProssimoAttaccaIfSuccesso()
    {
        if (AttaccoNormale.Successo == true)
        {
            yield return new WaitForSeconds(10f);
            ProssimoCheAttacca();
        }
        else
        {
            yield return new WaitForSeconds(3f);
            ProssimoCheAttacca();
        }
    }


    public void SceltaTurno() //Qui è il momento vero in cui il giocatore esegue la mossa. Prima la salva e basta, poi a questo punto viene realmente eseguita.
    {

        if (turnoDiGameobject.name == playerPrefab.gameObject.name)
        {
            Debug.Log("SCELTA TURNO Player, turnoDiGameobject.name: " + turnoDiGameobject.name + ", playerPrefab.gameObject.name: " + playerPrefab.gameObject.name);

            //state = BattleState.PLAYERTURN;
            if (!playerUnit.paralizzato)
            {

                mossaDaEseguire.GetComponent<Mossa>().Esegui(mossaDaEseguire, playerUnit, playerHUD, nemicoAttaccatoDalPlayer, nemicoAttaccatoDalPlayerHUD);
            }
            else
            {
                playerUnit.paralizzato = false;
                Debug.Log(playerUnit.unitName + " è paralizzato, non può attaccare");
                string PlayerParalizzato = playerUnit.unitName + " è paralizzato, non può attaccare";
                StartCoroutine(ShowTextParalizzato(PlayerParalizzato));
                playerPrefab.gameObject.GetComponent<Animator>().Play("ParalizzatoPg");

            }

            //ProssimoCheAttacca();

            StartCoroutine(WaitProssimoAttaccaIfSuccesso());

            //StartCoroutine(PlayerAttackMIO(nemicoAttaccatoDalPlayer, nemicoAttaccatoDalPlayerHUD));
            //PlayerTurn();

        }
        else if (turnoDiGameobject.name == friendPrefab.gameObject.name)
        {
            Debug.Log("SCELTA TURNO Friend, turnoDiGameobject.name: " + turnoDiGameobject.name + ", friendPrefab.gameObject.name: " + friendPrefab.gameObject.name);
            state = BattleState.FRIENDTURN;
            StartCoroutine(FriendTurn());
            
        }
        else if (turnoDiGameobject.name == enemy2Prefab.gameObject.name)
        {
            Debug.Log("SCELTA TURNO Enemy2, turnoDiGameobject.name: " + turnoDiGameobject.name + ", enemy2Prefab.gameObject.name: " + enemy2Prefab.gameObject.name);
            state = BattleState.ENEMY2TURN;
            StartCoroutine(Enemy2Turn());           
        }
        else
        {
            Debug.Log("SCELTA TURNO ENEMY, turnoDiGameobject.name: " + turnoDiGameobject.name + ", enemyPrefab.gameObject.name: " + enemyPrefab.gameObject.name);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());      
        }
    }


    /*public (Unit, BattleHUD, bool) ChooseToAttackOrDefend(Unit giocatoreCheDeveDecidereUnit)
    {
        if (giocatoreCheDeveDecidereUnit.unitName == friendUnit.unitName) 
        {
            //il giocatore che deve scegliere è Friend, controllo se i due avversari hanno meno XP di quelli che tolgo e quindi posso ucciderli.
            //Se enemy1 sta per morire lo uccido, altrimenti cerco di uccidere enemy2. 

            if (enemyUnit.currentHP < giocatoreCheDeveDecidereUnit.damage && enemyUnit.currentHP>0)
            {
                return (enemyUnit, enemyHUD, true);
            }
            else if (enemy2Unit.currentHP < giocatoreCheDeveDecidereUnit.damage && enemy2Unit.currentHP > 0)
            {
                return (enemy2Unit, enemy2HUD, true);
            }

            //se nessuno dei due stava per morire, controllo se sono in fin di vita e voglio aumentare la mia vita

            if (giocatoreCheDeveDecidereUnit.currentHP < giocatoreCheDeveDecidereUnit.maxHP/4)
            {
                return (giocatoreCheDeveDecidereUnit, friendHUD, false); //in questo caso non servono i primi 2, perché vengono ripresi dalla funzione dentro cui viene richiamata ChooseToAttackOrDefend
            }
            else //altrimenti sono in una zona neutra e posso attaccare
            {
                int y = UnityEngine.Random.Range(0, 2); //se y=0, cerco di attaccare prima il primo se è vivo, altrimenti attacco il secondo.
                                            //se y=1, cerco di attaccare prima il secondo se è vivo, altrimenti attacco il primo.
                                            // In questo modo garantisco casualità di chi attacco. 
                if (y == 0 && enemyUnit.currentHP > 0)
                {
                    return (enemyUnit, enemyHUD, true);
                }
                else if (y == 0 && enemy2Unit.currentHP > 0)
                {
                    return (enemy2Unit, enemy2HUD, true);
                }
                else if (y==1 && enemy2Unit.currentHP > 0)
                {
                    return (enemy2Unit, enemy2HUD, true);
                }   
                else if (y == 1 && enemyUnit.currentHP > 0)
                {
                    return (enemyUnit, enemyHUD, true);
                }
            }
        }
        else if (giocatoreCheDeveDecidereUnit.unitName == enemyUnit.unitName || giocatoreCheDeveDecidereUnit.unitName == enemy2Unit.unitName)
        {
            if (playerUnit.currentHP < giocatoreCheDeveDecidereUnit.damage && playerUnit.currentHP > 0)
            {
                return (playerUnit, playerHUD, true);
            }
            else if (friendUnit.currentHP < giocatoreCheDeveDecidereUnit.damage && friendUnit.currentHP > 0)
            {
                return (friendUnit, friendHUD, true);
            }

            if (giocatoreCheDeveDecidereUnit.currentHP < giocatoreCheDeveDecidereUnit.maxHP / 4)
            {
                return (giocatoreCheDeveDecidereUnit, enemyHUD, false); //controllare qua per sicurezza ????
            }
            else
            {
                int y = UnityEngine.Random.Range(0, 2);

                if (y == 0 && playerUnit.currentHP > 0)
                {
                    return (playerUnit, playerHUD, true);
                }
                else if (y == 0 && friendUnit.currentHP > 0)
                {
                    return (friendUnit, friendHUD, true);
                }
                else if (y == 1 && friendUnit.currentHP > 0)
                {
                    return (friendUnit, friendHUD, true);
                }
                else if (y == 1 && playerUnit.currentHP > 0)
                {
                    return (playerUnit, playerHUD, true);
                }

            }
        }
        
        return (giocatoreCheDeveDecidereUnit, playerHUD, false); //qui non dovrebbe mai finirci
    }*/


    public void ChiAttacchi(Button button) //Il player sceglie chi vuole attaccare, schiacciando sul bottone dei giocatori
    {
        if(button.name == "Enemy1")
        {
            //Debug.Log("Hai scelto di attaccare enemy1");
            nemicoAttaccatoDalPlayer = enemyUnit;
            nemicoAttaccatoDalPlayerHUD = enemyHUD;
            //StartCoroutine(PlayerAttackMIO(enemyUnit, enemyHUD));

        }
        else if (button.name == "Enemy2")
        {
            nemicoAttaccatoDalPlayer = enemy2Unit;
            nemicoAttaccatoDalPlayerHUD = enemy2HUD;
            //Debug.Log("Hai scelto di attaccare enemy2");
            //StartCoroutine(PlayerAttackMIO(enemy2Unit, enemy2HUD));
        }

        chiVuoiAttaccare.SetActive(false);

        Debug.Log("INIZIA IL GIRO DI ATTACCHI OOOOOOOOOOOOOOOOOO");
        SceltaTurno();
    }

    public void ScegliChiAttaccare() //Dopo aver selezionato la mossa, si attivano i bottoni dei due nemici. Se uno dei due è morto è reso interactable=false.
    {
        chiVuoiAttaccare.SetActive(true);
        chiVuoiAttaccareText.text = "Scegli chi attaccare!";

        chiVuoiAttaccareButton1.transform.GetChild(0).GetComponent<Text>().text = enemyUnit.unitName;
        chiVuoiAttaccareButton2.transform.GetChild(0).GetComponent<Text>().text = enemy2Unit.unitName;

        if (enemyUnit.currentHP < 0)
        {
            chiVuoiAttaccareButton1.interactable = false;
        }
        if (enemy2Unit.currentHP < 0)
        {
            chiVuoiAttaccareButton2.interactable = false;
        }
    }


    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        ScegliChiAttaccare();
        //StartCoroutine(PlayerAttack());
    }


    public void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		}
        else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}


    /*IEnumerator PlayerHeal()
	{
		playerUnit.Heal(5);

        state = BattleState.ENEMYTURN;

        playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);
		StartCoroutine(EnemyTurn());
	}*/


    /*IEnumerator EnemyHeal(Unit qualeNemicoSiDifende, BattleHUD qualeNemicoSiDifendeHUD)
    {
        qualeNemicoSiDifende.Heal(5);

        state = BattleState.ENEMYTURN;

        qualeNemicoSiDifendeHUD.SetHP(qualeNemicoSiDifende.currentHP);
        dialogueText.text = qualeNemicoSiDifende.unitName + " ricarica PE!";

        yield return new WaitForSeconds(2f);
        //StartCoroutine(EnemyTurn());
        PlayerTurn();
    }*/


    /*public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal());
	}*/


    public int calcolaDannoEffettivo(int danni, int attaccoSpeciale)
    {
        int valoreDanno = (danni + 10) * 2 / 3;
        return valoreDanno;
    }

    IEnumerator ShowText(string textDaScrivere)
    {    
        for (int i = 0; i < textDaScrivere.Length; i++)
        {
            currentText = textDaScrivere.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }

        //yield return new WaitForSeconds(5);
    }

    IEnumerator ShowTextParalizzato(string textDaScrivere)
    {
        for (int i = 0; i < textDaScrivere.Length; i++)
        {
            currentText = textDaScrivere.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(3);
        //ProssimoCheAttacca();
    }
}