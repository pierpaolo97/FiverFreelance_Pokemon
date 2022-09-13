using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        yield return new WaitForSeconds(1.5f);
        Colpito.GetComponent<Animator>().Play("ColpitoPg");
    }

    IEnumerator BoosterLampeggiante(GameObject Curato)
    {
        yield return new WaitForSeconds(1.5f);
        Curato.GetComponent<Animator>().Play("CuraPg");
    }

    IEnumerator MalusLampeggiante(GameObject Curato)
    {
        yield return new WaitForSeconds(1.5f);
        Curato.GetComponent<Animator>().Play("MalusPg");
    }

    IEnumerator Paralizzato(GameObject Curato)
    {
        yield return new WaitForSeconds(1.5f);
        Curato.GetComponent<Animator>().Play("ParalizzatoPg");
    }

    public void Esegui(Mossa mossa, Unit attaccanteUnit, BattleHUD attacanteHUD, Unit colpitoUnit, BattleHUD colpitoHUD) //Quando viene eseguita questa funzione, la mossa viene realmente lanciata. 
    {
        Debug.Log(attaccanteUnit.unitName + " prova ad usare " + mossa.nomeMossa);

        if (mossa.tipologiaDiMosaa == "ATTACCO NORMALE")
        {
            //Debug.Log("QUAAA");
            StartCoroutine(mossa.GetComponent<AttaccoNormale>().Attacco(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));
            if (mossa.GetComponent<AttaccoNormale>().Successo == true)
            {
                GameObject MossaInstanziata = Instantiate(MossaAnimation, colpitoUnit.transform.position, Quaternion.identity);
                Destroy(MossaInstanziata, 1.3f);
                StartCoroutine(ColpitoLampeggiante(GameObject.Find(colpitoUnit.unitName)));
            }
        }
        else if (mossa.nomeMossa == "Fusione Del Reattore")
        {
            //Fusione Del Reattore ? una mossa normale, solo che in pi? l'attaccante perde 1/3 della propria vita:
            StartCoroutine(mossa.GetComponent<AttaccoNormale>(). Attacco(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));
            int danno = attaccanteUnit.currentHP/3;
            attaccanteUnit.TakeDamage(danno);
            attacanteHUD.SetHP(attaccanteUnit.currentHP);

            GameObject MossaInstanziata = Instantiate(MossaAnimation, colpitoUnit.transform.position, Quaternion.identity);
            Destroy(MossaInstanziata, 1.3f);
            StartCoroutine(ColpitoLampeggiante(GameObject.Find(attaccanteUnit.unitName)));
            StartCoroutine(ColpitoLampeggiante(GameObject.Find(colpitoUnit.unitName)));

            //ANIMAZIONE MALUS PERDE VITA E ANCHE IL TESTO MANCA (IF mossa.name fusione del reattore in attacco normale cambia il testo string)

            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Difensore della Giustizia")
        {
            //Aumenta di due punti la propia difesa speciale
            string DifensoreGiustizia = attaccanteUnit.unitName + " usa Difensore della Giustizia.";
            string AumentaDifesa = attaccanteUnit.unitName + " aumenta di 2 punti la sua difesa speciale.";

            StartCoroutine(ShowTextDouble(DifensoreGiustizia, AumentaDifesa));
            DifensoreDellaGiustizia(mossa, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Scarica Di Coltelli")
        {
            //Scarica Di Coltelli ? una mossa normale, ma viene eseguita 1-5 volte:
            string ScaricaColtelli = attaccanteUnit.unitName + " usa Scarica Di Coltelli.";

            StartCoroutine(ShowText(ScaricaColtelli));

            ScaricaDiColtelli(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD);
            
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Scorpacciata Del Cacciatore")
        {
            //Cura la propria vita del 30%
            string ScorpacciataCacciatore = attaccanteUnit.unitName + " usa Scorpacciata Del Cacciatore.";
            string Cura = attaccanteUnit.unitName + " si cura del 30% la propria vita.";

            GameObject MossaInstanziata = Instantiate(MossaAnimation, new Vector3 (attaccanteUnit.transform.position.x, attaccanteUnit.transform.position.y, attaccanteUnit.transform.position.z), Quaternion.identity);
            Destroy(MossaInstanziata, 1.5f);
            StartCoroutine(BoosterLampeggiante(GameObject.Find(attaccanteUnit.unitName)));

            StartCoroutine(ShowTextDouble(ScorpacciataCacciatore, Cura));

            ScorpacciataDelCacciatore(mossa, attaccanteUnit, attacanteHUD);
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Bacio Della Principessa")
        {
            string BacioPrincipessa = attaccanteUnit.unitName + " usa Bacio Della Principessa.";
            string VieneParalalizzato = colpitoUnit.unitName + " viene paralizzato.";

            GameObject MossaInstanziata = Instantiate(MossaAnimation, new Vector3(colpitoUnit.transform.position.x, colpitoUnit.transform.position.y, colpitoUnit.transform.position.z), Quaternion.identity);
            Destroy(MossaInstanziata, 1.5f);
            StartCoroutine(Paralizzato(GameObject.Find(colpitoUnit.unitName)));

            StartCoroutine(ShowTextDouble(BacioPrincipessa, VieneParalalizzato));

            BacioDellaPrincipessa(mossa, colpitoUnit);
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Ordine Della Futura Regina")
        {
            string OrdineRegina = attaccanteUnit.unitName + " usa Ordine Della Futura Regina.";
            string AmicoBoostato = attaccanteUnit.unitName + " aumenta la difesa e la difesa speciale di " + amicoDaBoostare.unitName + " 3 punti!";

            GameObject MossaInstanziata = Instantiate(MossaAnimation, new Vector3(amicoDaBoostare.transform.position.x, amicoDaBoostare.transform.position.y, amicoDaBoostare.transform.position.z), Quaternion.identity);
            Destroy(MossaInstanziata, 1.5f);
            StartCoroutine(BoosterLampeggiante(GameObject.Find(amicoDaBoostare.unitName)));

            StartCoroutine(ShowTextDouble(OrdineRegina, AmicoBoostato));

            OrdineDellaFuturaRegina(mossa, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();

        }
        else if (mossa.nomeMossa == "Sguardo Del Drago")
        {
            string SguardoDrago = attaccanteUnit.unitName + " usa Sguardo Del Drago.";
            string RiduciAttacco = attaccanteUnit.unitName + " riduce di 1 l'attacco degli avversari";

            GameObject MossaInstanziata = Instantiate(MossaAnimation, new Vector3(colpitoUnit.transform.position.x, colpitoUnit.transform.position.y , colpitoUnit.transform.position.z), Quaternion.identity);
            Destroy(MossaInstanziata, 1.5f);
            StartCoroutine(MalusLampeggiante(GameObject.Find(colpitoUnit.unitName)));

            StartCoroutine(ShowTextDouble(SguardoDrago, RiduciAttacco));

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
            //string AmicoBoostato = attaccanteUnit.unitName + " aumenta la difesa e la difesa speciale di " + amicoDaBoostare.unitName + " 3 punti!";
            //StartCoroutine(ShowText(AmicoBoostato));
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

            //string RiduciAttacco = "COLPISCE GLI AVVERSASRI MA BOO";
            //StartCoroutine(ShowText(RiduciAttacco));
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
        string UsaSuccesso = giocatoreCheAttacca.unitName + " usa " + mossa.nomeMossa + " contro " + qualeNemicoAttacchi.unitName + " e ha successo!";
        StartCoroutine(ShowText(UsaSuccesso));

        yield return new WaitForSecondsRealtime(5);
        string perdeXP = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo + "XP";
        StartCoroutine(ShowText(perdeXP));

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
        yield return new WaitForSeconds(3);

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
