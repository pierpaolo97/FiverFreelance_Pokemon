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
    //public string elemento;
    public string tipologiaDiMosaa;

    BattleSystem battleSystem;
    public Unit amicoDaBoostare;
    public Unit giocatoreNONAttaccato;
    public Button bottoneDefaultPerMosseCura;

    private float delay = 0.05f;

    string currentText = "";

    public GameObject MossaAnimation;

    public void Start()
    {
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
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
        GameObject MossaInstanziata = Instantiate(MossaAnimation, Colpito.transform.position, Quaternion.identity);
        Destroy(MossaInstanziata, 1.3f);
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

    IEnumerator WaitTakeDamageFusioneReattore(Unit Attacante,int danno)
    {
        AttaccoNormale.Successo = true;
        yield return new WaitForSeconds(6f);
        Attacante.TakeDamage(danno);
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
                StartCoroutine(ColpitoLampeggiante(GameObject.Find(colpitoUnit.unitName)));             
            }
            else if (mossa.nomeMossa == "Pioggia Di Meteoriti" && AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, new Quaternion(0, 0, 0.216439605f, 0.976296067f)));
                StartCoroutine(ColpitoLampeggiante(GameObject.Find(colpitoUnit.unitName)));
            }
            else if (AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossa(colpitoUnit));
                StartCoroutine(ColpitoLampeggiante(GameObject.Find(colpitoUnit.unitName)));
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
                attacanteHUD.SetHP(attaccanteUnit.currentHP);
                StartCoroutine(WaitAnimationMossa(colpitoUnit));
                StartCoroutine(ColpitoLampeggiante(GameObject.Find(attaccanteUnit.unitName)));
                StartCoroutine(ColpitoLampeggiante(GameObject.Find(colpitoUnit.unitName)));
                StartCoroutine(WaitMossaAttaccoFuoriPosto());
                StartCoroutine(WaitTakeDamageFusioneReattore(attaccanteUnit, danno));
            }
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Difensore della Giustizia")
        {
            //Aumenta di due punti la propia difesa speciale
            string DifensoreGiustizia = attaccanteUnit.unitName + " usa Difensore della Giustizia.";
            string AumentaDifesa = attaccanteUnit.unitName + " aumenta di 2 punti la sua difesa speciale.";
            StartCoroutine(WaitAnimationMossa(attaccanteUnit));
            StartCoroutine(BoosterLampeggiante(GameObject.Find(attaccanteUnit.unitName)));
            StartCoroutine(ShowTextDouble(DifensoreGiustizia, AumentaDifesa));
            DifensoreDellaGiustizia(mossa, attaccanteUnit);
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Scarica Di Coltelli")
        {
            //Scarica Di Coltelli ? una mossa normale, ma viene eseguita 1-5 volte:
            //string ScaricaColtelli = attaccanteUnit.unitName + " usa Scarica Di Coltelli.";
            StartCoroutine(ColpitoLampeggiante(GameObject.Find(colpitoUnit.unitName)));
            ScaricaDiColtelli(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD);
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Scorpacciata Del Cacciatore")
        {
            //Cura la propria vita del 30%
            string ScorpacciataCacciatore = attaccanteUnit.unitName + " usa Scorpacciata Del Cacciatore.";
            string Cura = attaccanteUnit.unitName + " si cura del 30% la propria vita.";
            StartCoroutine(WaitAnimationMossa(attaccanteUnit));
            StartCoroutine(BoosterLampeggiante(GameObject.Find(attaccanteUnit.unitName)));
            StartCoroutine(ShowTextDouble(ScorpacciataCacciatore, Cura));
            ScorpacciataDelCacciatore(mossa, attaccanteUnit, attacanteHUD);
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Bacio Della Principessa")
        {
            string BacioPrincipessa = attaccanteUnit.unitName + " usa Bacio Della Principessa.";
            string VieneParalalizzato = colpitoUnit.unitName + " viene paralizzato.";
            StartCoroutine(WaitAnimationMossa(colpitoUnit));
            StartCoroutine(Paralizzato(GameObject.Find(colpitoUnit.unitName)));
            StartCoroutine(ShowTextDouble(BacioPrincipessa, VieneParalalizzato));
            BacioDellaPrincipessa(mossa, colpitoUnit);
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Ordine Della Futura Regina")
        {
            //string OrdineRegina = attaccanteUnit.unitName + " usa Ordine Della Futura Regina.";
            //string AmicoBoostato = attaccanteUnit.unitName + " aumenta la difesa e la difesa speciale di " + amicoDaBoostare.unitName + " 3 punti!";
            //StartCoroutine(WaitAnimationMossa(amicoDaBoostare));
            //StartCoroutine(BoosterLampeggiante(GameObject.Find(amicoDaBoostare.unitName)));
            //StartCoroutine(ShowText(OrdineRegina));
            OrdineDellaFuturaRegina(mossa, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Sguardo Del Drago")
        {
            string SguardoDrago = attaccanteUnit.unitName + " usa Sguardo Del Drago.";
            string RiduciAttacco = attaccanteUnit.unitName + " riduce di 1 l'attacco degli avversari ";
            //StartCoroutine(WaitAnimationMossa(colpitoUnit));
            //StartCoroutine(MalusLampeggiante(GameObject.Find(colpitoUnit.unitName)));
            StartCoroutine(ShowTextDouble(SguardoDrago, RiduciAttacco));
            SguardoDelDrago(mossa, colpitoUnit);
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
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
        //string elemento = mossa.elemento;

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
            string OrdineRegina = attaccanteUnit.unitName + " usa Ordine Della Futura Regina.";
            string AmicoBoostato = "La difesa normale e speciale di " + amicoDaBoostare.unitName + " aumenta di 3 punti!";
            StartCoroutine(ShowTextDouble(OrdineRegina, AmicoBoostato));
            StartCoroutine(WaitAnimationMossa(amicoDaBoostare));
            StartCoroutine(BoosterLampeggiante(GameObject.Find(amicoDaBoostare.unitName)));
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
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
                    StartCoroutine(WaitAnimationMossa(battleSystem.amici[0]));
                    StartCoroutine(MalusLampeggiante(GameObject.Find(battleSystem.amici[0].unitName)));
                    StartCoroutine(WaitAnimationMossa(battleSystem.amici[1]));
                    StartCoroutine(MalusLampeggiante(GameObject.Find(battleSystem.amici[1].unitName)));
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
                    StartCoroutine(WaitAnimationMossa(battleSystem.nemici[0]));
                    StartCoroutine(MalusLampeggiante(GameObject.Find(battleSystem.nemici[0].unitName)));
                    StartCoroutine(WaitAnimationMossa(battleSystem.nemici[1]));
                    StartCoroutine(MalusLampeggiante(GameObject.Find(battleSystem.nemici[1].unitName)));
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

        StartCoroutine(WaitAnimationMossaDiagonale(giocatoreCheAttacca, qualeNemicoAttacchi, new Quaternion(0, 0, 0.216439605f, 0.976296067f)));
        string UsaSuccesso = giocatoreCheAttacca.unitName + " usa " + mossa.nomeMossa + " contro " + qualeNemicoAttacchi.unitName + " e ha successo! ";
        StartCoroutine(ShowText(UsaSuccesso));
        yield return new WaitForSecondsRealtime(5);
        string perdeXP = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo + "HP ";
        StartCoroutine(ShowText(perdeXP));
        yield return new WaitForSecondsRealtime(2);
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

        for (int i = 0; i < textDaScrivere2.Length; i++)
        {
            currentText = textDaScrivere2.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            battleSystem.dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator ShowText(string textDaScrivere)
    {
        for (int i = 0; i < textDaScrivere.Length; i++)
        {
            currentText = textDaScrivere.Substring(0, i);
            //Debug.Log(Bird.transform.GetChild(0).transform.GetChild(1).name);
            battleSystem.dialogueText.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

}
