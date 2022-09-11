using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mossa : MonoBehaviour
{

    public string nomeMossa;
    public string tipo;
    public int danni;
    public int precisione;
    public string elemento;
    public string tipologiaDiMosaa;

    BattleSystem battleSystem;
    public Unit amicoDaBoostare;
    public Unit giocatoreNONAttaccato;
    public Button bottoneDefaultPerMosseCura;


    public void Start()
    {
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
    }


    public void SalvaMossa(Mossa mossa) //Questa funzione viene attaccata ad ogni bottone, personalizzata a seconda della mossa eseguita. Qui salviamo la mossa che il Player dovrà eseguire.
    {
        if (mossa.tipologiaDiMosaa != "CURA" && mossa.tipologiaDiMosaa != "SENZA_TARGET")
        {
            battleSystem.ScegliChiAttaccare();
            battleSystem.mossaDaEseguire = mossa;
            Debug.Log("OOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        }
        else
        {
            //battleSystem.ChiAttacchi(bottoneDefaultPerMosseCura);
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");
            battleSystem.nemicoAttaccatoDalPlayer = battleSystem.enemyUnit;
            battleSystem.nemicoAttaccatoDalPlayerHUD = battleSystem.enemyHUD;
            battleSystem.mossaDaEseguire = mossa;      
            battleSystem.SceltaTurno();
        }
    }


    public void Esegui(Mossa mossa, Unit attaccanteUnit, BattleHUD attacanteHUD, Unit colpitoUnit, BattleHUD colpitoHUD) //Quando viene eseguita questa funzione, la mossa viene realmente lanciata. 
    {
        Debug.Log(attaccanteUnit.unitName + " prova ad usare " + mossa.nomeMossa);

        if (mossa.tipologiaDiMosaa == "ATTACCO NORMALE")
        {
            //Debug.Log("QUAAA");
            StartCoroutine(mossa.GetComponent<AttaccoNormale>().Attacco(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));
        }
        else if (mossa.nomeMossa == "Fusione Del Reattore")
        {
            //Fusione Del Reattore è una mossa normale, solo che in più l'attaccante perde 1/3 della propria vita:
            StartCoroutine(mossa.GetComponent<AttaccoNormale>().Attacco(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));
            int danno = attaccanteUnit.currentHP/3;
            attaccanteUnit.TakeDamage(danno);
            attacanteHUD.SetHP(attaccanteUnit.currentHP);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Difensore della Giustizia")
        {
            //Aumenta di due punti la propia difesa speciale
            battleSystem.dialogueText.text = attaccanteUnit.unitName + " usa Difensore della Giustizia.";
            DifensoreDellaGiustizia(mossa, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Scarica Di Coltelli")
        {
            //Scarica Di Coltelli è una mossa normale, ma viene eseguita 1-5 volte:
            battleSystem.dialogueText.text = attaccanteUnit.unitName + " usa Scarica Di Coltelli.";
            
            ScaricaDiColtelli(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD);
            
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Scorpacciata Del Cacciatore")
        {
            //Cura la propria vita del 30%
            battleSystem.dialogueText.text = attaccanteUnit.unitName + " Scorpacciata Del Cacciatore.";
            ScorpacciataDelCacciatore(mossa, attaccanteUnit, attacanteHUD);
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Bacio Della Principessa")
        {
            battleSystem.dialogueText.text = attaccanteUnit.unitName + " Bacio Della Principessa.";
            BacioDellaPrincipessa(mossa, colpitoUnit);
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Ordine Della Futura Regina")
        {
            battleSystem.dialogueText.text = attaccanteUnit.unitName + " Ordine Della Futura Regina.";
            OrdineDellaFuturaRegina(mossa, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Sguardo Del Drago")
        {
            battleSystem.dialogueText.text = attaccanteUnit.unitName + " Sguardo Del Drago.";
            SguardoDelDrago(mossa, colpitoUnit);
            //battleSystem.ProssimoCheAttacca();

        }
        else
        {
            Debug.Log("Mossa ancora non programmata: " + mossa.nomeMossa);
        }
    }


    public void DifensoreDellaGiustizia(Mossa mossa, Unit attaccanteUnit)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            attaccanteUnit.difesa_speciale += 2;
        }
    }


    public void ScaricaDiColtelli(Mossa mossa, Unit attaccanteUnit, BattleHUD attacanteHUD, Unit colpitoUnit, BattleHUD colpitoHUD)
    {
        battleSystem.bottoniMosse.SetActive(false);

        string tipo = mossa.tipo;
        int danno = mossa.danni;
        int precisione = mossa.precisione;
        string elemento = mossa.elemento;

        bool attaccatoMorto = false;

        if (AttaccoRiesce(precisione))
        {
            int x = Random.Range(0, 5);
            Debug.Log("Scarica di coltelli viene usata " + x + " volte");
            for (int i = 0; i < x; i++)
            {
                StartCoroutine(Coltelli(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));
      
                if(colpitoUnit.currentHP <= 0)
                {
                    attaccatoMorto = true;
                }

                int z = 0;
                foreach (Unit personaggio in battleSystem.amici)
                {
                    if (personaggio.unitName == colpitoUnit.unitName)
                    {
                        if (z == 0)
                        {
                            giocatoreNONAttaccato = battleSystem.amici[1];
                        }
                        else
                        {
                            giocatoreNONAttaccato = battleSystem.amici[0];
                        }
                    }
                    z++;
                }

                z = 0;

                foreach (Unit personaggio in battleSystem.nemici)
                {
                    if (personaggio.unitName == colpitoUnit.unitName)
                    {
                        if (z == 0)
                        {
                            giocatoreNONAttaccato = battleSystem.nemici[1];
                        }
                        else
                        {
                            giocatoreNONAttaccato = battleSystem.nemici[0];
                        }
                    }
                    z++;
                }

                if (attaccatoMorto && giocatoreNONAttaccato.currentHP <= 0)
                {
                    battleSystem.state = BattleState.FINISHED;
                    //qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP = 0);
                    battleSystem.EndBattle();
                }

                if (attaccatoMorto)
                {
                    break;
                }


            }
        }
    }


    public void ScorpacciataDelCacciatore(Mossa mossa, Unit attaccanteUnit, BattleHUD attacanteHUD)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            int vita = attaccanteUnit.maxHP;
            int cura = vita / 3;
            attaccanteUnit.Heal(cura);
            attacanteHUD.SetHP(attaccanteUnit.currentHP);
        }
    }


    public void BacioDellaPrincipessa(Mossa mossa, Unit colpitoUnit)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            colpitoUnit.paralizzato = true;
            Debug.Log(colpitoUnit.unitName + " viene paralizzato.");
        }
    }


    public void OrdineDellaFuturaRegina(Mossa mossa, Unit attaccanteUnit)
    {
        Debug.Log("OrdineDellaFuturaRegina");

        if (AttaccoRiesce(mossa.precisione))
        {

            int z = 0;

            foreach (Unit personaggio in battleSystem.amici)
            {
                if (personaggio.unitName == attaccanteUnit.unitName)
                {
                    if (z == 0)
                    {
                        amicoDaBoostare = battleSystem.amici[1];
                    }
                    else
                    {
                        amicoDaBoostare = battleSystem.amici[0];
                    }
                }
                z++;
            }

            z = 0;

            foreach (Unit personaggio in battleSystem.nemici)
            {
                if (personaggio.unitName == attaccanteUnit.unitName)
                {
                    if (z == 0)
                    {
                        amicoDaBoostare = battleSystem.nemici[1];
                    }
                    else
                    {
                        amicoDaBoostare = battleSystem.nemici[0];
                    }
                }
                z++;
            }

            amicoDaBoostare.difesa += 3;
            amicoDaBoostare.difesa_speciale += 3;
            battleSystem.dialogueText.text = attaccanteUnit.unitName + " aumenta la difesa e la difesa speciale di " + amicoDaBoostare.unitName + " 3 punti!";
            Debug.Log(attaccanteUnit.unitName + " aumenta la difesa e la difesa speciale di " + amicoDaBoostare.unitName + " 3 punti!");
        }
    }


    public void SguardoDelDrago(Mossa mossa, Unit colpitoUnit)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            int z = 0;
            foreach (Unit personaggio in battleSystem.amici)
            {
                if (personaggio.unitName == colpitoUnit.unitName)
                {
                    battleSystem.amici[0].attacco -= 1;
                    battleSystem.amici[1].attacco -= 1;
                }
                z++;
            }

            z = 0;

            foreach (Unit personaggio in battleSystem.nemici)
            {
                if (personaggio.unitName == colpitoUnit.unitName)
                {
                    battleSystem.nemici[0].attacco -= 1;
                    battleSystem.nemici[1].attacco -= 1;
                }
                z++;
            }
        }


    }


    bool AttaccoRiesce(int precisione)
    {
        int x = Random.Range(0, 100);
        if (x <= precisione)
        {
            Debug.Log("Attacco riuscito");
            return true;
        }
        else
        {
            Debug.Log("Attacco fallito");
            return false;
        }
    }


    public IEnumerator Coltelli(Mossa mossa, Unit giocatoreCheAttacca, BattleHUD giocatoreCheAttaccaoHUD, Unit qualeNemicoAttacchi, BattleHUD qualeNemicoHUD)
    {

        Debug.Log(giocatoreCheAttacca.unitName + " usa " + mossa.nomeMossa + " contro " + qualeNemicoAttacchi.unitName);
        int dannoEffettivo = battleSystem.calcolaDannoEffettivo(mossa.danni, giocatoreCheAttacca.attacco_speciale);

        bool isDead = qualeNemicoAttacchi.TakeDamage(dannoEffettivo);

        qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
        battleSystem.dialogueText.text = giocatoreCheAttacca.unitName + " usa " + mossa.nomeMossa + " contro " + qualeNemicoAttacchi.unitName + " e ha successo!";


        yield return new WaitForSecondsRealtime(2);
        battleSystem.dialogueText.text = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo + "XP";

    }





}
