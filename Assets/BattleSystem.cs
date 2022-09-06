using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, FRIENDTURN, ENEMY2TURN, WON, LOST }

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

    Unit playerUnit;
	Unit enemyUnit;
    Unit friendUnit;
    Unit enemy2Unit;

    public Text dialogueText;


	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;
    public BattleHUD friendHUD;
    public BattleHUD enemy2HUD;

    public BattleState state;

    public GameObject chiVuoiAttaccare;
    public Text chiVuoiAttaccareText;
    public Button chiVuoiAttaccareButton1;
    public Button chiVuoiAttaccareButton2;

    // Start is called before the first frame update
    void Start()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

        GameObject friendGO = Instantiate(friendPrefab, friendBattleStation);
        friendUnit = friendGO.GetComponent<Unit>();

        GameObject enemy2GO = Instantiate(enemy2Prefab, enemy2BattleStation);
        enemy2Unit = enemy2GO.GetComponent<Unit>();

        dialogueText.text = enemyUnit.unitName + " e " + enemy2Unit.unitName + " ti sfidano!";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);
        friendHUD.SetHUD(friendUnit);
        enemy2HUD.SetHUD(enemy2Unit);

        yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}


    IEnumerator PlayerAttackMIO(Unit qualeNemicoAttacchi, BattleHUD qualeNemicoHUD)
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

        if (isDead && giocatoreNONAttaccato.currentHP==0)
        {
            state = BattleState.WON;
            qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP = 0);
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
            dialogueText.text = "Hai tolto " + playerUnit.damage + " XP...";

            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator FriendTurn()
    {

        if (friendUnit.currentHP != 0)
        {


            Unit giocatoreAttaccato;
            BattleHUD giocatoreAttaccatoHUD;
            bool boolAttacco;

            (giocatoreAttaccato, giocatoreAttaccatoHUD, boolAttacco) = ChooseToAttackOrDefend(friendUnit);


            if (boolAttacco) //se è vero vuol dire che devo attaccare
            {

                /*dialogueText.text = friendUnit.unitName + " attacca!";
                int y = Random.Range(0, 2); //uso la y per dire quale nemico attacco
                if (y == 0)
                {
                    Debug.Log("Friend attacca enemy1");
                    giocatoreAttaccato = enemyUnit;
                    giocatoreAttaccatoHUD = enemyHUD;
                }
                else
                {
                    Debug.Log("Friend attacca enemy2");
                    giocatoreAttaccato = enemy2Unit;
                    giocatoreAttaccatoHUD = enemy2HUD;
                }*/

                dialogueText.text = friendUnit.unitName + " attacca " + giocatoreAttaccato.unitName;
                bool isDead = giocatoreAttaccato.TakeDamage(friendUnit.damage);
                giocatoreAttaccatoHUD.SetHP(giocatoreAttaccato.currentHP);

                yield return new WaitForSeconds(2f);

                Unit giocatoreNONAttaccato;

                if (giocatoreAttaccato.unitName == enemyUnit.unitName)
                {
                    giocatoreNONAttaccato = enemy2Unit;
                }
                else
                {
                    giocatoreNONAttaccato = enemyUnit;
                }

                if (isDead && giocatoreNONAttaccato.currentHP==0) // se è morto anche l'altro finisce altrimenti no!!!
                {
                    state = BattleState.WON;
                    EndBattle();
                }
                else
                {
                    state = BattleState.ENEMY2TURN;
                    //PlayerTurn();
                    StartCoroutine(Enemy2Turn());
                }

            }
            else
            {
                Debug.Log(friendUnit.unitName + " si difende!");
                StartCoroutine(EnemyHeal(friendUnit, friendHUD));
                dialogueText.text = friendUnit.unitName + " si difende!";
                state = BattleState.ENEMY2TURN;
                yield return new WaitForSeconds(2f);
                //PlayerTurn();
                StartCoroutine(Enemy2Turn());
            }
        }
        else
        {
            state = BattleState.ENEMY2TURN;
            dialogueText.text = friendUnit.unitName + " è esausto";
            yield return new WaitForSeconds(1f);
            StartCoroutine(Enemy2Turn());
        }

        yield return new WaitForSeconds(1f);
        

    }


    IEnumerator EnemyTurn()
    {

        if (enemyUnit.currentHP != 0)
        {

            Unit giocatoreAttaccato;
            BattleHUD giocatoreAttaccatoHUD;
            bool boolAttacco;

            (giocatoreAttaccato, giocatoreAttaccatoHUD, boolAttacco) = ChooseToAttackOrDefend(enemyUnit);

            if (boolAttacco) //se è vero vuol dire che devo attaccare
            {
                /*dialogueText.text = enemyUnit.unitName + " attacca!";
                int y = Random.Range(0, 2); //uso la y per dire quale nemico attacco


                if (y == 0)
                {
                    Debug.Log("Il nemico attacca player");
                    giocatoreAttaccato = playerUnit;
                    giocatoreAttaccatoHUD = playerHUD;
                }
                else
                {
                    Debug.Log("Il nemico attacca friend");
                    giocatoreAttaccato = friendUnit;
                    giocatoreAttaccatoHUD = friendHUD;
                }*/

                dialogueText.text = enemyUnit.unitName + " attacca " + giocatoreAttaccato.unitName;
                bool isDead = giocatoreAttaccato.TakeDamage(enemyUnit.damage);
                giocatoreAttaccatoHUD.SetHP(giocatoreAttaccato.currentHP);

                yield return new WaitForSeconds(2f);


                Unit giocatoreNONAttaccato;

                if (giocatoreAttaccato.unitName == playerUnit.unitName)
                {
                    giocatoreNONAttaccato = friendUnit;
                }
                else
                {
                    giocatoreNONAttaccato = playerUnit;
                }

                if (isDead && giocatoreNONAttaccato.currentHP==0)
                {
                    state = BattleState.LOST;
                    EndBattle();
                }
                else
                {
                    state = BattleState.FRIENDTURN;
                    //PlayerTurn();
                    StartCoroutine(FriendTurn());
                }

            }
            else
            {
                Debug.Log(enemyUnit.unitName + " si difende!");
                StartCoroutine(EnemyHeal(enemyUnit, enemyHUD));
                dialogueText.text = enemyUnit.unitName + " si difende!";
                state = BattleState.FRIENDTURN;
                yield return new WaitForSeconds(2f);
                //PlayerTurn();
                StartCoroutine(FriendTurn());
            }
        }
        else
        {
            state = BattleState.FRIENDTURN;
            dialogueText.text = enemyUnit.unitName + " è esausto";
            yield return new WaitForSeconds(1f);
            StartCoroutine(FriendTurn());
        }

        yield return new WaitForSeconds(1f);
        

    }


    IEnumerator Enemy2Turn()
    {
        if (enemy2Unit.currentHP != 0)
        {
            Unit giocatoreAttaccato;
            BattleHUD giocatoreAttaccatoHUD;
            bool boolAttacco;

            (giocatoreAttaccato, giocatoreAttaccatoHUD, boolAttacco) = ChooseToAttackOrDefend(enemyUnit);

            if (boolAttacco) //se è vero vuol dire che devo attaccare
            {
                /*dialogueText.text = enemy2Unit.unitName + " attacca!";
                int y = Random.Range(0, 2); //uso la y per dire quale nemico attacco

                if (y == 0)
                {
                    Debug.Log("Enemy2 attacca player");
                    giocatoreAttaccato = playerUnit;
                    giocatoreAttaccatoHUD = playerHUD;
                }
                else
                {
                    Debug.Log("Enemy2 attacca firend");
                    giocatoreAttaccato = friendUnit;
                    giocatoreAttaccatoHUD = friendHUD;
                }*/

                dialogueText.text = enemy2Unit.unitName + " attacca " + giocatoreAttaccato.unitName;
                bool isDead = giocatoreAttaccato.TakeDamage(enemy2Unit.damage);
                giocatoreAttaccatoHUD.SetHP(giocatoreAttaccato.currentHP);

                yield return new WaitForSeconds(2f);

                Unit giocatoreNONAttaccato;
                if (giocatoreAttaccato.unitName == playerUnit.unitName)
                {
                    giocatoreNONAttaccato = friendUnit;
                }
                else
                {
                    giocatoreNONAttaccato = playerUnit;
                }

                if (isDead && giocatoreNONAttaccato.currentHP == 0)
                {
                    state = BattleState.LOST;
                    EndBattle();
                }
                else
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }

            }
            else
            {
                Debug.Log(enemy2Unit.unitName + " si difende!");
                StartCoroutine(EnemyHeal(enemy2Unit, enemy2HUD));
                dialogueText.text = enemy2Unit.unitName + " si difende!";
                state = BattleState.PLAYERTURN;
                yield return new WaitForSeconds(2f);
                PlayerTurn();
            }
        }
        else
        {
            dialogueText.text = enemy2Unit.unitName + " è esausto";
            state = BattleState.PLAYERTURN;
            yield return new WaitForSeconds(1f);
            PlayerTurn();
        }


        yield return new WaitForSeconds(1f);

    }


    public (Unit, BattleHUD, bool) ChooseToAttackOrDefend(Unit giocatoreCheDeveDecidereUnit)
    {
        if (giocatoreCheDeveDecidereUnit.unitName == friendUnit.unitName) 
        {
            //il giocatore che deve scegliere è Friend, controllo se i due avversari hanno meno XP di quelli che tolgo e quindi posso ucciderli.
            //Se enemy1 sta per morire lo uccido, altrimenti cerco di uccidere enemy2. 

            if (enemyUnit.currentHP < giocatoreCheDeveDecidereUnit.damage && enemyUnit.currentHP!=0)
            {
                return (enemyUnit, enemyHUD, true);
            }
            else if (enemy2Unit.currentHP < giocatoreCheDeveDecidereUnit.damage && enemy2Unit.currentHP != 0)
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
                int y = Random.Range(0, 2); //se y=0, cerco di attaccare prima il primo se è vivo, altrimenti attacco il secondo.
                                            //se y=1, cerco di attaccare prima il secondo se è vivo, altrimenti attacco il primo.
                                            // In questo modo garantisco casualità di chi attacco. 
                if (y == 0 && enemyUnit.currentHP != 0)
                {
                    return (enemyUnit, enemyHUD, true);
                }
                else if (y == 0 && enemy2Unit.currentHP != 0)
                {
                    return (enemy2Unit, enemy2HUD, true);
                }
                else if (y==1 && enemy2Unit.currentHP != 0)
                {
                    return (enemy2Unit, enemy2HUD, true);
                }   
                else if (y == 1 && enemyUnit.currentHP != 0)
                {
                    return (enemyUnit, enemyHUD, true);
                }
            }
        }
        else if (giocatoreCheDeveDecidereUnit.unitName == enemyUnit.unitName || giocatoreCheDeveDecidereUnit.unitName == enemy2Unit.unitName)
        {
            if (playerUnit.currentHP < giocatoreCheDeveDecidereUnit.damage && playerUnit.currentHP != 0)
            {
                return (playerUnit, playerHUD, true);
            }
            else if (friendUnit.currentHP < giocatoreCheDeveDecidereUnit.damage && friendUnit.currentHP != 0)
            {
                return (friendUnit, friendHUD, true);
            }

            if (giocatoreCheDeveDecidereUnit.currentHP < giocatoreCheDeveDecidereUnit.maxHP / 4)
            {
                return (giocatoreCheDeveDecidereUnit, enemyHUD, false); //controllare qua per sicurezza ????
            }
            else
            {
                int y = Random.Range(0, 2);

                if (y == 0 && playerUnit.currentHP != 0)
                {
                    return (playerUnit, playerHUD, true);
                }
                else if (y == 0 && friendUnit.currentHP != 0)
                {
                    return (friendUnit, friendHUD, true);
                }
                else if (y == 1 && friendUnit.currentHP != 0)
                {
                    return (friendUnit, friendHUD, true);
                }
                else if (y == 1 && playerUnit.currentHP != 0)
                {
                    return (playerUnit, playerHUD, true);
                }

            }
        }
        
        return (giocatoreCheDeveDecidereUnit, playerHUD, false); //qui non dovrebbe mai finirci
    }

    public void ChiAttacchi(Button button)
    {
        if(button.name== "Enemy1")
        {
            Debug.Log("Hai scelto di attaccare enemy1");
            StartCoroutine(PlayerAttackMIO(enemyUnit, enemyHUD));
            chiVuoiAttaccare.SetActive(false);
        }
        else if (button.name == "Enemy2")
        {
            Debug.Log("Hai scelto di attaccare enemy2");
            StartCoroutine(PlayerAttackMIO(enemy2Unit, enemy2HUD));
            chiVuoiAttaccare.SetActive(false);
        }
    }

    public void ScegliChiAttaccare()
    {
        chiVuoiAttaccare.SetActive(true);
        chiVuoiAttaccareText.text = "Scegli chi attaccare!";
        chiVuoiAttaccareButton1.transform.GetChild(0).GetComponent<Text>().text = enemyUnit.unitName;
        chiVuoiAttaccareButton2.transform.GetChild(0).GetComponent<Text>().text = enemy2Unit.unitName;

        if (enemyUnit.currentHP == 0)
        {
            chiVuoiAttaccareButton1.interactable = false;
        }
        if (enemy2Unit.currentHP == 0)
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


    void EndBattle()
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

	void PlayerTurn()
	{
        if (playerUnit.currentHP != 0)
        {
            dialogueText.text = playerUnit.unitName + " scegli un'azione!";
        }
        else
        {
            dialogueText.text = playerUnit.unitName + " è esausto!";
            state = BattleState.ENEMYTURN;
            StartCoroutine(Wait(2));
            StartCoroutine(EnemyTurn());
        }
	}

    public IEnumerator Wait(float delayInSecs)
    {
        yield return new WaitForSeconds(delayInSecs);
    }

    IEnumerator PlayerHeal()
	{
		playerUnit.Heal(5);

        state = BattleState.ENEMYTURN;

        playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);
		StartCoroutine(EnemyTurn());
	}


    IEnumerator EnemyHeal(Unit qualeNemicoSiDifende, BattleHUD qualeNemicoSiDifendeHUD)
    {
        qualeNemicoSiDifende.Heal(5);

        state = BattleState.ENEMYTURN;

        qualeNemicoSiDifendeHUD.SetHP(qualeNemicoSiDifende.currentHP);
        dialogueText.text = qualeNemicoSiDifende.unitName + " ricarica PE!";

        yield return new WaitForSeconds(2f);
        //StartCoroutine(EnemyTurn());
        PlayerTurn();
    }


    public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal());
	}

}
