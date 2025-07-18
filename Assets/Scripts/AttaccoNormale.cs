using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using DG.Tweening;

public class AttaccoNormale : MonoBehaviour
{
    
    Unit giocatoreNONAttaccato;
    private float delay = 0.025f;
    public static bool Successo;
    public GameObject partitaFinita;

    public void Start()
    {
        partitaFinita = GameObject.Find("Fine").transform.GetChild(0).gameObject;
    }

    public IEnumerator Attacco(Mossa mossa, Unit giocatoreCheAttacca, BattleHUD giocatoreCheAttaccaoHUD, Unit qualeNemicoAttacchi, BattleHUD qualeNemicoHUD)
    {
       
        BattleSystem battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        battleSystem.bottoniMosse.SetActive(false);

        string tipo = mossa.tipo;
        int danno = mossa.danni;
        int precisione = mossa.precisione;
        //string elemento = mossa.elemento;

        Successo = false;

        int x = UnityEngine.Random.Range(0, 100);
        int mossaHaEffetto = battleSystem.calcolaDannoEffettivo(danno, giocatoreCheAttacca, qualeNemicoAttacchi, mossa);
        //Debug.Log(mossaHaEffetto);

        string superEfficace;

        if(battleSystem.Modificatore(mossa, qualeNemicoAttacchi) == 2)
        {
            superEfficace = " È superefficace!";
        }
        else
        {
            superEfficace = "";
        }


        if (mossaHaEffetto > 0)
        {
            if (x <= precisione)
            {
                if (qualeNemicoAttacchi.subisceDanno)
                {
                    Successo = true;

                    int dannoEffettivo = battleSystem.calcolaDannoEffettivo(danno, giocatoreCheAttacca, qualeNemicoAttacchi, mossa);

                    bool isDead = qualeNemicoAttacchi.TakeDamage(dannoEffettivo);

                    //qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
                    string UsaConSuccesso = giocatoreCheAttacca.unitName + " usa " + this.gameObject.GetComponent<Mossa>().nomeMossa + " contro " + qualeNemicoAttacchi.unitName + "! ";

                    StartCoroutine(ShowText(UsaConSuccesso));

                    yield return new WaitForSecondsRealtime(5);

                    if (this.gameObject.GetComponent<Mossa>().nomeMossa == "Fusione Del Reattore")
                    {
                        string NemicoPerdeDanni = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo + "HP, ma anche Atomo perde 1/3 dei propri HP." + superEfficace;
                        StartCoroutine(ShowText(NemicoPerdeDanni));
                    }
                    else
                    {
                        string NemicoPerdeDanni = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo + "HP." + superEfficace;
                        StartCoroutine(ShowText(NemicoPerdeDanni));
                    }

                    Successo = false;

                    yield return new WaitForSecondsRealtime(2);

                    qualeNemicoHUD.SetHP(qualeNemicoAttacchi);

                    //Unit giocatoreNONAttaccato;

                    int z = 0;

                    foreach (Unit personaggio in battleSystem.amici)
                    {
                        if (personaggio.unitID == qualeNemicoAttacchi.unitID)
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
                        if (personaggio.unitID == qualeNemicoAttacchi.unitID)
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
                        Debug.Log("FINE ATTACCO");
                        battleSystem.state = BattleState.FINISHED;
                        //qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP = 0);
                        battleSystem.EndBattle();
                        if (giocatoreCheAttacca.unitID == 0 || giocatoreCheAttacca.unitID == 1)
                            StartCoroutine(gameObject.GetComponent<Mossa>().FinePartita(battleSystem.playerPrefab, battleSystem.friendPrefab));
                        else
                            StartCoroutine(gameObject.GetComponent<Mossa>().FinePartita(battleSystem.enemyPrefab, battleSystem.enemy2Prefab));
                    }
                    else
                    {
                        //state = BattleState.ENEMYTURN;
                        qualeNemicoHUD.SetHP(qualeNemicoAttacchi);
                        //dialogueText.text = "Hai tolto " + playerUnit.damage + " HP...";

                        //yield return new WaitForSeconds(5f);

                        //battleSystem.StartCoroutine(ProssimoCheAttacca());
                        //StartCoroutine(EnemyTurn());
                    }
                }
                else
                {
                    string arthurSiSostituisce = "";
                    if (qualeNemicoAttacchi.unitName == "Mary")
                    {
                        arthurSiSostituisce = " Arthur subisce i danni al posto di Mary!";
                    }
                    String attaccoSenzaDanno = giocatoreCheAttacca.unitName + " attacca " + qualeNemicoAttacchi.unitName + ", che però non subisce danno." + arthurSiSostituisce;
                    StartCoroutine(ShowText(attaccoSenzaDanno));
                }
            }
            else
            {
                //Debug.Log(giocatoreCheAttacca.unitName + " fallisce l'attacco! ");
                String AttaccoFallito = giocatoreCheAttacca.unitName + " prova ad attaccare ma fallisce!";
                StartCoroutine(ShowText(AttaccoFallito));
                //yield return new WaitForSeconds(5);
                //battleSystem.StartCoroutine(ProssimoCheAttacca());
            }
        }
        else
        {
            //Debug.Log(mossa.elemento + " non ha efficacia contro personaggi di tipo" + qualeNemicoAttacchi.elemento);
            String AttaccoFallito = mossa.nomeMossa + " non è efficace contro " + qualeNemicoAttacchi.unitName;
            StartCoroutine(ShowText(AttaccoFallito));
            //yield return new WaitForSeconds(5);
            //battleSystem.StartCoroutine(ProssimoCheAttacca());
            Successo = false;
        }
    }

    IEnumerator ShowText(string textDaScrivere)
    {
        BattleSystem battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();

        string currentText = "";

        for (int i = 0; i < textDaScrivere.Length+1; i++)
        {
            currentText = textDaScrivere.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            battleSystem.dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}
