using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class AttaccoNormale : MonoBehaviour
{
    
    Unit giocatoreNONAttaccato;


    public IEnumerator Attacco(Unit giocatoreCheAttacca, BattleHUD giocatoreCheAttaccaoHUD, Unit qualeNemicoAttacchi, BattleHUD qualeNemicoHUD)
    {

        Debug.Log(giocatoreCheAttacca.unitName + " usa " + this.gameObject.GetComponent<Mossa>().name + " contro " + qualeNemicoAttacchi.unitName);


        BattleSystem battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        battleSystem.bottoniMosse.SetActive(false);

        string tipo = this.gameObject.GetComponent<Mossa>().tipo;
        int danno = this.gameObject.GetComponent<Mossa>().danni;
        int precisione = this.gameObject.GetComponent<Mossa>().precisione;
        string elemento = this.gameObject.GetComponent<Mossa>().elemento;

        
        int x = UnityEngine.Random.Range(0, 100);

        if (x <= precisione)
        {
            int dannoEffettivo = battleSystem.calcolaDannoEffettivo(danno, giocatoreCheAttacca.attacco_speciale);

            bool isDead = qualeNemicoAttacchi.TakeDamage(dannoEffettivo);

            qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
            battleSystem.dialogueText.text = giocatoreCheAttacca.unitName + " usa " + this.gameObject.GetComponent<Mossa>().name + " contro " + qualeNemicoAttacchi.unitName + " e ha successo!";
            Debug.Log("ooooooooooooooooooo");
            yield return new WaitForSecondsRealtime(2);
            

            battleSystem.dialogueText.text = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo + "XP";
            

            //Unit giocatoreNONAttaccato;

            int z = 0;

            foreach (Unit personaggio in battleSystem.amici)
            {
                if(personaggio.unitName == qualeNemicoAttacchi.unitName)
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
                if (personaggio.unitName == qualeNemicoAttacchi.unitName)
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

            if (isDead && giocatoreNONAttaccato.currentHP <= 0)
            {
                battleSystem.state = BattleState.FINISHED;
                //qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP = 0);
                battleSystem.EndBattle();
            }
            else
            {
                //state = BattleState.ENEMYTURN;
                qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
                //dialogueText.text = "Hai tolto " + playerUnit.damage + " HP...";

                yield return new WaitForSeconds(2f);
                
                battleSystem.ProssimoCheAttacca();
                //StartCoroutine(EnemyTurn());
            }

        }
        else
        {
            battleSystem.dialogueText.text = giocatoreCheAttacca.unitName + " fallisce l'attacco!";
            yield return new WaitForSeconds(2f);
            battleSystem.ProssimoCheAttacca();
        }
     
    }







}
