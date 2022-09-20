using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

    private float delay = 0.025f;

    string currentText = "";

    public GameObject MossaAnimation;
    GameObject partitaFinita;
    GameObject combactButtons;

    int x;
    GameObject ColpitoGiusto;

    public void Start()
    {
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        partitaFinita = GameObject.FindGameObjectWithTag("PartitaFinita").transform.GetChild(0).gameObject;
        
    }

    public void SalvaMossa(Mossa mossa) //Questa funzione viene attaccata ad ogni bottone, personalizzata a seconda della mossa eseguita. Qui salviamo la mossa che il Player dovr? eseguire.
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
            combactButtons = GameObject.FindGameObjectWithTag("CombactButtons");
            combactButtons.SetActive(false);
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");
            battleSystem.nemicoAttaccatoDalPlayer = battleSystem.enemyUnit;
            battleSystem.nemicoAttaccatoDalPlayerHUD = battleSystem.enemyHUD;
            battleSystem.mossaDaEseguire = mossa;      
            battleSystem.SceltaTurno();
        }
    }

    IEnumerator ColpitoLampeggiante(GameObject Colpito)
    {
        yield return new WaitForSeconds(6f);
        Colpito.GetComponent<Animator>().Play("ColpitoPg");
    }

    IEnumerator BoosterLampeggiante(GameObject Curato)
    {
        yield return new WaitForSeconds(6f);
        Curato.GetComponent<Animator>().Play("CuraPg");
    }

    IEnumerator MalusLampeggiante(GameObject Malus)
    {
        yield return new WaitForSeconds(6f);
        Malus.GetComponent<Animator>().Play("MalusPg");
    }

    IEnumerator Paralizzato(GameObject Paralizzato)
    {
        yield return new WaitForSeconds(6f);
        Paralizzato.GetComponent<Animator>().Play("ParalizzatoPg");
    }

    IEnumerator WaitAnimationMossa(Unit Colpito)
    {
        yield return new WaitForSeconds(3f);
        GameObject MossaInstanziata = Instantiate(MossaAnimation, Colpito.transform.position, transform.rotation);
        Destroy(MossaInstanziata, 1.5f);
    }

    IEnumerator WaitAnimationMossaDiagonale(Unit Attacante,Unit Colpito, Quaternion Inclinazione)
    {
        yield return new WaitForSeconds(3f);
        GameObject MossaInstanziata = Instantiate(MossaAnimation, Attacante.transform.position, Inclinazione);
        if (Attacante.transform.position.y > 0)
            MossaInstanziata.GetComponent<SpriteRenderer>().flipX = true;
        MossaInstanziata.transform.DOMove(Colpito.transform.position, 1.3f);
        Destroy(MossaInstanziata, 1.3f);
    }

    IEnumerator WaitMossaAttaccoFuoriPosto()
    {
        AttaccoNormale.Successo = true;
        yield return new WaitForSeconds(5f);
        AttaccoNormale.Successo = false;
    }

    IEnumerator WaitTakeDamageFusioneReattore(Unit Attacante,BattleHUD attacanteHUD,int danno)
    {
        AttaccoNormale.Successo = true;
        yield return new WaitForSeconds(6f);
        Attacante.TakeDamage(danno);
        attacanteHUD.SetHP(Attacante.currentHP);

    }

    public GameObject FindColpito(Unit colpitoUnit)
    {
        if (GameObject.Find("Battle System").GetComponent<BattleSystem>().playerPrefab.GetComponent<Unit>().unitID == colpitoUnit.GetComponent<Unit>().unitID)
            ColpitoGiusto = GameObject.Find("Battle System").GetComponent<BattleSystem>().playerPrefab;
        else if (GameObject.Find("Battle System").GetComponent<BattleSystem>().enemyPrefab.GetComponent<Unit>().unitID == colpitoUnit.GetComponent<Unit>().unitID)
            ColpitoGiusto = GameObject.Find("Battle System").GetComponent<BattleSystem>().enemyPrefab;
        else if (GameObject.Find("Battle System").GetComponent<BattleSystem>().enemy2Prefab.GetComponent<Unit>().unitID == colpitoUnit.GetComponent<Unit>().unitID)
            ColpitoGiusto = GameObject.Find("Battle System").GetComponent<BattleSystem>().enemy2Prefab;
        else if (GameObject.Find("Battle System").GetComponent<BattleSystem>().friendPrefab.GetComponent<Unit>().unitID == colpitoUnit.GetComponent<Unit>().unitID)
            ColpitoGiusto = GameObject.Find("Battle System").GetComponent<BattleSystem>().friendPrefab;
        Debug.Log(ColpitoGiusto);
        return (ColpitoGiusto);
    }

    public void Esegui(Mossa mossa, Unit attaccanteUnit, BattleHUD attacanteHUD, Unit colpitoUnit, BattleHUD colpitoHUD) //Quando viene eseguita questa funzione, la mossa viene realmente lanciata. 
    {
        Debug.Log(attaccanteUnit.unitName + " prova ad usare " + mossa.nomeMossa);

        if (mossa.tipologiaDiMosaa == "ATTACCO NORMALE")
        {
            //Debug.Log("QUAAA");
            StartCoroutine(mossa.GetComponent<AttaccoNormale>().Attacco(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));
            if(mossa.nomeMossa == "Lanciafiamme" && AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, new Quaternion(0, 0, 0.843391478f, 0.537299633f)));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
            }
            else if (mossa.nomeMossa == "Pioggia Di Meteoriti" && AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, new Quaternion(0, 0, 0.216439605f, 0.976296067f)));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
            }
            else if (mossa.nomeMossa == "Uragano" && AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, Quaternion.identity));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
            }
            else if (AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossa(colpitoUnit));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
                StartCoroutine(WaitMossaAttaccoFuoriPosto());
            }
        }
        else if (mossa.nomeMossa == "Fusione Del Reattore")
        {
            //Fusione Del Reattore ? una mossa normale, solo che in pi? l'attaccante perde 1/3 della propria vita:
            StartCoroutine(mossa.GetComponent<AttaccoNormale>(). Attacco(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));
            if (AttaccoNormale.Successo == true)
            {
                int danno = attaccanteUnit.currentHP / 3;
                //attaccanteUnit.TakeDamage(danno);
                //attacanteHUD.SetHP(attaccanteUnit.currentHP);
                StartCoroutine(WaitAnimationMossa(colpitoUnit));
                StartCoroutine(ColpitoLampeggiante(FindColpito(attaccanteUnit)));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
                StartCoroutine(WaitMossaAttaccoFuoriPosto());
                StartCoroutine(WaitTakeDamageFusioneReattore(attaccanteUnit, attacanteHUD,danno));
            }
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Difensore della Giustizia")
        {
            //Aumenta di due punti la propia difesa speciale
            DifensoreDellaGiustizia(mossa, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Scarica Di Coltelli")
        {
            //Scarica Di Coltelli ? una mossa normale, ma viene eseguita 1-5 volte:
            //string ScaricaColtelli = attaccanteUnit.unitName + " usa Scarica Di Coltelli.";
            ScaricaDiColtelli(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Scorpacciata Del Cacciatore")
        {
            //Cura la propria vita del 30%
            ScorpacciataDelCacciatore(mossa, attaccanteUnit, attacanteHUD);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Bacio Della Principessa")
        {
            BacioDellaPrincipessa(mossa, colpitoUnit, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Ordine Della Futura Regina")
        {
            OrdineDellaFuturaRegina(mossa, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Sguardo Del Drago")
        {
            SguardoDelDrago(mossa, colpitoUnit, attaccanteUnit);
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
            string DifensoreGiustizia = attaccanteUnit.unitName + " usa Difensore della Giustizia.";
            string AumentaDifesa = attaccanteUnit.unitName + " aumenta di 2 punti la sua difesa speciale.";
            StartCoroutine(WaitAnimationMossa(attaccanteUnit));
            StartCoroutine(BoosterLampeggiante(GameObject.Find(attaccanteUnit.unitName)));
            StartCoroutine(ShowTextDouble(DifensoreGiustizia, AumentaDifesa));
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            attaccanteUnit.difesa_speciale += 2;
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
            Debug.Log("ATTACCO FALLITO COLTELLI");
        }
    }


    public void ScaricaDiColtelli(Mossa mossa, Unit attaccanteUnit, BattleHUD attacanteHUD, Unit colpitoUnit, BattleHUD colpitoHUD)
    {
        battleSystem.bottoniMosse.SetActive(false);

        string tipo = mossa.tipo;
        int danno = mossa.danni;
        int precisione = mossa.precisione;
        //string elemento = mossa.elemento;

        bool attaccatoMorto = false;

        int mossaHaEffetto = battleSystem.calcolaDannoEffettivo(danno, attaccanteUnit, colpitoUnit, mossa); //Questo serve per verificare se la mossa ha effetto contro l'avversario

        if (mossaHaEffetto > 0)
        {

            if (AttaccoRiesce(precisione))
            {
                x = Random.Range(0, 5);
                Debug.Log("Scarica di coltelli viene usata " + x + " volte");
                for (int i = 0; i < x; i++)
                {
                    StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
                    StartCoroutine(WaitMossaAttaccoFuoriPosto());
                    StartCoroutine(Coltelli(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));

                    if (colpitoUnit.currentHP <= 0)
                    {
                        attaccatoMorto = true;
                    }

                    int z = 0;
                    foreach (Unit personaggio in battleSystem.amici)
                    {
                        if (personaggio.unitID == colpitoUnit.unitID)
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
                        if (personaggio.unitID == colpitoUnit.unitID)
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
                        partitaFinita.SetActive(true);

                    }

                    if (attaccatoMorto)
                    {
                        break;
                    }
                }
            }
            else
            {
                string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
                StartCoroutine(ShowText(AttaccoFallito));
                Debug.Log("ATTACCO FALLITO COLTELLI");
            }
        }
        else
        {
            Debug.Log(mossa.elemento + " non ha efficacia contro personaggi di tipo" + colpitoUnit.elemento);
        }
    }


    public void ScorpacciataDelCacciatore(Mossa mossa, Unit attaccanteUnit, BattleHUD attacanteHUD)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            string ScorpacciataCacciatore = attaccanteUnit.unitName + " usa Scorpacciata Del Cacciatore.";
            string Cura = attaccanteUnit.unitName + " si cura del 30% la propria vita.";
            StartCoroutine(WaitAnimationMossa(attaccanteUnit));
            StartCoroutine(BoosterLampeggiante(FindColpito(attaccanteUnit)));
            StartCoroutine(ShowTextDouble(ScorpacciataCacciatore, Cura));
            StartCoroutine(WaitMossaAttaccoFuoriPosto());

            int vita = attaccanteUnit.maxHP;
            int cura = vita / 3;
            attaccanteUnit.Heal(cura);
            attacanteHUD.SetHP(attaccanteUnit.currentHP);
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
        }
    }

    public void BacioDellaPrincipessa(Mossa mossa, Unit colpitoUnit, Unit attaccanteUnit)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            string BacioPrincipessa = attaccanteUnit.unitName + " usa Bacio Della Principessa.";
            string VieneParalalizzato = colpitoUnit.unitName + " viene paralizzato.";
            StartCoroutine(WaitAnimationMossa(colpitoUnit));
            StartCoroutine(Paralizzato(FindColpito(colpitoUnit)));
            StartCoroutine(ShowTextDouble(BacioPrincipessa, VieneParalalizzato));
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            colpitoUnit.paralizzato = true;
            Debug.Log(colpitoUnit.unitName + " viene paralizzato.");
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
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
                if (personaggio.unitID == attaccanteUnit.unitID)
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
                if (personaggio.unitID == attaccanteUnit.unitID)
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
            string OrdineRegina = attaccanteUnit.unitName + " usa Ordine Della Futura Regina.";
            string AmicoBoostato = "La difesa normale e speciale di " + amicoDaBoostare.unitName + " aumenta di 3 punti!";
            StartCoroutine(ShowTextDouble(OrdineRegina, AmicoBoostato));
            StartCoroutine(WaitAnimationMossa(amicoDaBoostare));
            StartCoroutine(BoosterLampeggiante(FindColpito(amicoDaBoostare)));
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            Debug.Log(attaccanteUnit.unitName + " aumenta la difesa e la difesa speciale di " + amicoDaBoostare.unitName + " 3 punti!");
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
            Debug.Log("ATTACCO FALLITO ORDINE");
        }
    }


    public void SguardoDelDrago(Mossa mossa, Unit colpitoUnit, Unit attaccanteUnit)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            string SguardoDrago = attaccanteUnit.unitName + " usa Sguardo Del Drago.";
            string RiduciAttacco = attaccanteUnit.unitName + " riduce di 1 l'attacco degli avversari ";
            //StartCoroutine(WaitAnimationMossa(colpitoUnit));
            //StartCoroutine(MalusLampeggiante(GameObject.Find(colpitoUnit.unitName)));
            StartCoroutine(ShowTextDouble(SguardoDrago, RiduciAttacco));
            StartCoroutine(WaitMossaAttaccoFuoriPosto());

            int z = 0;
            foreach (Unit personaggio in battleSystem.amici)
            {
                if (personaggio.unitID == colpitoUnit.unitID)
                {
                    battleSystem.amici[0].attacco -= 1;
                    battleSystem.amici[1].attacco -= 1;
                    StartCoroutine(WaitAnimationMossa(battleSystem.amici[0]));
                    StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.amici[0])));
                    StartCoroutine(WaitAnimationMossa(battleSystem.amici[1]));
                    StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.amici[1])));
                }
                z++;
            }

            z = 0;

            foreach (Unit personaggio in battleSystem.nemici)
            {
                if (personaggio.unitID == colpitoUnit.unitID)
                {
                    battleSystem.nemici[0].attacco -= 1;
                    battleSystem.nemici[1].attacco -= 1;
                    StartCoroutine(WaitAnimationMossa(battleSystem.nemici[0]));
                    StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.nemici[0])));
                    StartCoroutine(WaitAnimationMossa(battleSystem.nemici[1]));
                    StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.nemici[1])));
                }
                z++;
            }
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
            Debug.Log("ATTACCO FALLITO SGUARDO");
        }
    }


    bool AttaccoRiesce(int precisione)
    {
        int k = Random.Range(0, 100);
        if (k <= precisione)
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
        StartCoroutine(WaitAnimationMossaDiagonale(giocatoreCheAttacca, qualeNemicoAttacchi, new Quaternion(0, 0, 0.216439605f, 0.976296067f)));
        string UsaSuccesso = giocatoreCheAttacca.unitName + " usa " + mossa.nomeMossa + " contro " + qualeNemicoAttacchi.unitName + " e ha successo! ";
        StartCoroutine(ShowText(UsaSuccesso));
        yield return new WaitForSecondsRealtime(5);
        int dannoEffettivo = battleSystem.calcolaDannoEffettivo(mossa.danni, giocatoreCheAttacca, qualeNemicoAttacchi, mossa);
        string perdeXP = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo*x + "HP ";
        StartCoroutine(ShowText(perdeXP));
        yield return new WaitForSecondsRealtime(2);
        bool isDead = qualeNemicoAttacchi.TakeDamage(dannoEffettivo);
        qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP);
        Debug.Log(dannoEffettivo);
    }

    IEnumerator ShowTextDouble(string textDaScrivere, string textDaScrivere2)
    {
        for (int i = 0; i < textDaScrivere.Length; i++)
        {
            currentText = textDaScrivere.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            battleSystem.dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }

        currentText = "";
        yield return new WaitForSeconds(4);

        for (int i = 0; i < textDaScrivere2.Length+1; i++)
        {
            currentText = textDaScrivere2.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            battleSystem.dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator ShowText(string textDaScrivere)
    {
        for (int i = 0; i < textDaScrivere.Length +1; i++)
        {
            currentText = textDaScrivere.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            battleSystem.dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

}
