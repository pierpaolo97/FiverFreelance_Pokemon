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

    private float delay = 0.05f;

    public bool Successo;


    public IEnumerator Attacco(Mossa mossa, Unit giocatoreCheAttacca, BattleHUD giocatoreCheAttaccaoHUD, Unit qualeNemicoAttacchi, BattleHUD qualeNemicoHUD)
    {
       
        BattleSystem battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        battleSystem.bottoniMosse.SetActive(false);

        string tipo = mossa.tipo;
        int danno = mossa.danni;
        int precisione = mossa.precisione;
        string elemento = mossa.elemento;

        Successo = false;

        int x = UnityEngine.Random.Range(0, 100);

        if (x <= precisione)
        {
            Successo = true;

            yield return new WaitForSecondsRealtime(1);

            Debug.Log(giocatoreCheAttacca.unitName + " usa " + this.gameObject.GetComponent<Mossa>().nomeMossa + " contro " + qualeNemicoAttacchi.unitName);
            int dannoEffettivo = battleSystem.calcolaDannoEffettivo(danno, giocatoreCheAttacca.attacco_speciale);

            bool isDead = qualeNemicoAttacchi.TakeDamage(dannoEffettivo);

            //qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
            string UsaConSuccesso = giocatoreCheAttacca.unitName + " usa " + this.gameObject.GetComponent<Mossa>().nomeMossa + " contro " + qualeNemicoAttacchi.unitName + " e ha successo! ";

            StartCoroutine(ShowText(UsaConSuccesso));

            yield return new WaitForSecondsRealtime(5);

            

            string NemicoPerdeDanni = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo + "XP";

            StartCoroutine(ShowText(NemicoPerdeDanni));

            yield return new WaitForSecondsRealtime(5);

            qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);

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

                yield return new WaitForSeconds(5f);
                
                //battleSystem.ProssimoCheAttacca();
                //StartCoroutine(EnemyTurn());
            }

        }
        else
        {
            Debug.Log(giocatoreCheAttacca.unitName + " fallisce l'attacco! ");
            String AttaccoFallito = giocatoreCheAttacca.unitName + " fallisce l'attacco! ";
            StartCoroutine(ShowText(AttaccoFallito));
            //yield return new WaitForSeconds(5);
            //battleSystem.ProssimoCheAttacca();
            Successo = false;

        }

        Successo = false;

    }

    IEnumerator ShowText(string textDaScrivere)
    {
        BattleSystem battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();

        string currentText = "";

        for (int i = 0; i < textDaScrivere.Length; i++)
        {
            currentText = textDaScrivere.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            battleSystem.dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }

        //yield return new WaitForSeconds(5);
    }
}
