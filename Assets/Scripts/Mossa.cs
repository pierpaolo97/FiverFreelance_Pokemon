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
    public bool puoiUsarla = true;
   

    BattleSystem battleSystem;
    public Unit amicoDaBoostare;
    public Unit giocatoreNONAttaccato;
    public Button bottoneDefaultPerMosseCura;

    private float delay = 0.025f;

    string currentText = "";

    public GameObject MossaAnimation;
    public GameObject MossaAnimationUp;

    GameObject partitaFinita;
    GameObject combactButtons;

    int x;
    GameObject ColpitoGiusto;
    Unit giocatoreAmicoONemico;

    public AudioSource BoosterSource;
    public AudioSource MalusSource;
    public AudioSource ColpitoSource;

    public AudioSource CameraAudio;

    public void Start()
    {    
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        partitaFinita = GameObject.FindGameObjectWithTag("PartitaFinita").transform.GetChild(0).gameObject;
        BoosterSource = GameObject.Find("PowerUp").GetComponent<AudioSource>();
        MalusSource = GameObject.Find("PowerDown").GetComponent<AudioSource>();
        ColpitoSource = GameObject.Find("Colpito").GetComponent<AudioSource>();
        CameraAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }

    public void SalvaMossa(Mossa mossa) //Questa funzione viene attaccata ad ogni bottone, personalizzata a seconda della mossa eseguita. Qui salviamo la mossa che il Player dovr? eseguire.
    {
        if (mossa.tipologiaDiMosaa != "CURA" && mossa.tipologiaDiMosaa != "SENZA_TARGET")
        {
            battleSystem.ScegliChiAttaccare();
            battleSystem.mossaDaEseguire = mossa;
            //Debug.Log("OOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        }
        else
        {
            //battleSystem.ChiAttacchi(bottoneDefaultPerMosseCura);
            combactButtons = GameObject.FindGameObjectWithTag("CombactButtons");
            combactButtons.SetActive(false);
            //Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");
            battleSystem.nemicoAttaccatoDalPlayer = battleSystem.enemyUnit;
            battleSystem.nemicoAttaccatoDalPlayerHUD = battleSystem.enemyHUD;
            battleSystem.mossaDaEseguire = mossa;      
            battleSystem.SceltaTurno();
        }
    }

    IEnumerator ColpitoLampeggiante(GameObject Colpito)
    {
        yield return new WaitForSeconds(5f);
        Colpito.GetComponent<Animator>().Play("ColpitoPg");
        ColpitoSource.Play();
        yield return new WaitForSeconds(0.3f);
        ColpitoSource.Play();
    }

    IEnumerator BoosterLampeggiante(GameObject Curato)
    {
        yield return new WaitForSeconds(5f);
        Curato.GetComponent<Animator>().Play("CuraPg");
        BoosterSource.Play();
    }

    IEnumerator MalusLampeggiante(GameObject Malus)
    {
        yield return new WaitForSeconds(5f);
        Malus.GetComponent<Animator>().Play("MalusPg");
        MalusSource.Play();
    }

    IEnumerator Paralizzato(GameObject Paralizzato)
    {
        yield return new WaitForSeconds(5f);
        Paralizzato.GetComponent<Animator>().Play("ParalizzatoPg");
    }

    IEnumerator WaitAnimationMossa(Unit Colpito)
    {
        yield return new WaitForSeconds(2f);
        GameObject MossaInstanziata = Instantiate(MossaAnimation, Colpito.transform.position, transform.rotation);
        if (Colpito.gameObject.transform.position.y < 0)
        {
            MossaInstanziata.transform.position = new Vector3(Colpito.transform.position.x, -0.94f, Colpito.transform.position.z);
        }
        Destroy(MossaInstanziata, 1.5f);
    }

    IEnumerator WaitAnimationMossaDiagonale(Unit Attacante, Unit Colpito, Quaternion Inclinazione, Mossa mossa)
    {
        if (mossa.nomeMossa== "Scarica Di Coltelli")
        {
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < x; i++)
            {
                yield return new WaitForSeconds(0.8f);
                GameObject MossaInstanziataColtelli = Instantiate(MossaAnimation, Attacante.transform.position, Inclinazione);
                if (Attacante.transform.position.y > 0)
                    MossaInstanziataColtelli.GetComponent<SpriteRenderer>().flipX = true;
                MossaInstanziataColtelli.transform.DOMove(Colpito.transform.position, 1f);
                Destroy(MossaInstanziataColtelli, 1.5f);
            }
        }
        else
        {
            yield return new WaitForSeconds(3f);
            GameObject MossaInstanziata = Instantiate(MossaAnimation, Attacante.transform.position, Inclinazione);
            if (Attacante.transform.position.y > 0 && mossa.nomeMossa != "Bomba a prua" && mossa.nomeMossa != "Il sinistro magico del numero 7")
                MossaInstanziata.GetComponent<SpriteRenderer>().flipX = true;
            if (mossa.nomeMossa == "Lanciafiamme")
            {
                yield return new WaitForSeconds(0.4f);
                MossaInstanziata.transform.DOMove(Colpito.transform.position, 1.3f);
                Destroy(MossaInstanziata, 1.3f);
            }
            else if(mossa.nomeMossa == "Bomba a prua" || mossa.nomeMossa == "Il sinistro magico del numero 7")
            {
                if (Attacante.transform.position.y > 0)
                {
                    Destroy(MossaInstanziata);
                    GameObject MossaInstanziataUp = Instantiate(MossaAnimationUp, Attacante.transform.position, new Quaternion(0, 1, 0, 0));
                    yield return new WaitForSeconds(0.7f);
                    MossaInstanziataUp.transform.DOMove(Colpito.transform.position, 0.7f);
                    Destroy(MossaInstanziataUp, 1.3f);
                }
                else
                {
                    yield return new WaitForSeconds(0.7f);
                    MossaInstanziata.transform.DOMove(Colpito.transform.position, 0.7f);
                    Destroy(MossaInstanziata, 1.3f);
                }
            }
            else
            {
                MossaInstanziata.transform.DOMove(Colpito.transform.position, 1.3f);
                Destroy(MossaInstanziata, 1.3f);
            }
        }

        /*int audioRandom = UnityEngine.Random.RandomRange(0, 2);
        if (mossa.tipo == "SPECIALE" || mossa.tipo == "FISICO")
        {
            yield return new WaitForSeconds(1f);
            CameraAudio.PlayOneShot(Colpito.audioAttacchiSubiti[audioRandom]);
        }*/
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
        attacanteHUD.SetHP(Attacante);
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
        //Debug.Log(ColpitoGiusto);
        return (ColpitoGiusto);
    }

    public GameObject FindColpitoHUD(Unit colpitoUnit)
    {
        if (GameObject.Find("Battle System").GetComponent<BattleSystem>().playerPrefab.GetComponent<Unit>().unitID == colpitoUnit.GetComponent<Unit>().unitID)
            ColpitoGiusto = GameObject.Find("Battle System").GetComponent<BattleSystem>().playerHUD.gameObject;
        else if (GameObject.Find("Battle System").GetComponent<BattleSystem>().enemyPrefab.GetComponent<Unit>().unitID == colpitoUnit.GetComponent<Unit>().unitID)
            ColpitoGiusto = GameObject.Find("Battle System").GetComponent<BattleSystem>().enemyHUD.gameObject;
        else if (GameObject.Find("Battle System").GetComponent<BattleSystem>().enemy2Prefab.GetComponent<Unit>().unitID == colpitoUnit.GetComponent<Unit>().unitID)
            ColpitoGiusto = GameObject.Find("Battle System").GetComponent<BattleSystem>().enemy2HUD.gameObject;
        else if (GameObject.Find("Battle System").GetComponent<BattleSystem>().friendPrefab.GetComponent<Unit>().unitID == colpitoUnit.GetComponent<Unit>().unitID)
            ColpitoGiusto = GameObject.Find("Battle System").GetComponent<BattleSystem>().friendHUD.gameObject;
        return (ColpitoGiusto);
    }

    public void Esegui(Mossa mossa, Unit attaccanteUnit, BattleHUD attacanteHUD, Unit colpitoUnit, BattleHUD colpitoHUD) //Quando viene eseguita questa funzione, la mossa viene realmente lanciata. 
    {
        //Debug.Log(attaccanteUnit.unitName + " prova ad usare " + mossa.nomeMossa);

        if (mossa.tipologiaDiMosaa == "ATTACCO NORMALE")
        {
            //Debug.Log("QUAAA");
            StartCoroutine(mossa.GetComponent<AttaccoNormale>().Attacco(mossa, attaccanteUnit, attacanteHUD, colpitoUnit, colpitoHUD));
            if(mossa.nomeMossa == "Lanciafiamme" && AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, new Quaternion(0, 0, 0.258819103f, 0.965925813f), mossa));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
            }
            else if (mossa.nomeMossa == "Pioggia Di Meteoriti" && AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, new Quaternion(0, 0, 0.216439605f, 0.976296067f), mossa));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
            }
            else if (mossa.nomeMossa == "Uragano" && AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, Quaternion.identity, mossa));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
            }
            else if (mossa.nomeMossa == "Bomba a prua" && AttaccoNormale.Successo == true)
            {
                CameraAudio.PlayOneShot(attaccanteUnit.AudioMossaSpeciale);
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, Quaternion.identity, mossa));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
            }
            else if (mossa.nomeMossa == "Il sinistro magico del numero 7" && AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, Quaternion.identity, mossa));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
            }
            else if (AttaccoNormale.Successo == true)
            {
                StartCoroutine(WaitAnimationMossa(colpitoUnit));
                StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
                StartCoroutine(WaitMossaAttaccoFuoriPosto());

                if (mossa.nomeMossa == "Leggendaria Spada del Vento")
                {
                    CameraAudio.PlayOneShot(attaccanteUnit.AudioMossaSpeciale);
                }
                else if (mossa.nomeMossa == "Pugno Radioattivo")
                {
                    CameraAudio.PlayOneShot(attaccanteUnit.AudioMossaSpeciale);

                }
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

                int z = 0;
                foreach (Unit personaggio in battleSystem.amici)
                {
                    if (personaggio.unitID == attaccanteUnit.unitID)
                    {
                        if (z == 0)
                        {
                            giocatoreAmicoONemico = battleSystem.amici[1];
                        }
                        else
                        {
                            giocatoreAmicoONemico = battleSystem.amici[0];
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
                            giocatoreAmicoONemico = battleSystem.nemici[1];
                        }
                        else
                        {
                            giocatoreAmicoONemico = battleSystem.nemici[0];
                        }
                    }
                    z++;
                }

                if (attaccanteUnit.currentHP <= 0 && giocatoreAmicoONemico.currentHP <= 0)
                {
                    battleSystem.state = BattleState.FINISHED;
                    //qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP = 0);
                    battleSystem.EndBattle();

                    if (attaccanteUnit.unitID == 0 || giocatoreAmicoONemico.unitID == 1)
                    {
                        FinePartita(battleSystem.playerPrefab, battleSystem.friendPrefab);
                    }
                    else
                    {
                        FinePartita(battleSystem.enemyPrefab, battleSystem.enemy2Prefab);
                    }
                }
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
            //paralizza qualcu0
            BacioDellaPrincipessa(mossa, colpitoUnit, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Ordine Della Futura Regina")
        {
            //Il suo compagno di squadra riceve un boost di 3 punti alla difesa e alla difesa speciale.
            OrdineDellaFuturaRegina(mossa, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Sguardo Del Drago")
        {
            //-1 attacco avversari
            SguardoDelDrago(mossa, colpitoUnit, attaccanteUnit);
            //battleSystem.ProssimoCheAttacca();
        }
        else if (mossa.nomeMossa == "Assist al bacio")
        {
            AssistAlBacio(mossa, attaccanteUnit);
        }
        else if (mossa.nomeMossa == "Cartellino rosso")
        {
            CartellinoRosso(mossa, colpitoUnit, attaccanteUnit);
        }
        else
        {
            Debug.Log("Mossa ancora non programmata: " + mossa.nomeMossa);
        }

        if (AttaccoNormale.Successo == true && battleSystem.state != BattleState.FINISHED)
        {
            StartCoroutine(WaitAudioAttaccoSubito(mossa, colpitoUnit));
        }
    }

    IEnumerator WaitAudioAttaccoSubito(Mossa mossa,Unit colpitoUnit)
    {
        int audioRandom = UnityEngine.Random.RandomRange(0, 2);
        if (mossa.tipologiaDiMosaa == "ATTACCO NORMALE")
        {
            float a = Random.Range(0, 100);
            //Debug.Log("numero" + a);

            yield return new WaitForSeconds(4f);
            if (a > 30f)
            {
                CameraAudio.PlayOneShot(colpitoUnit.audioAttacchiSubiti[audioRandom]);
            }
        }
    }



    public void DifensoreDellaGiustizia(Mossa mossa, Unit attaccanteUnit)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            string DifensoreGiustizia = attaccanteUnit.unitName + " usa Difensore della Giustizia.";
            string AumentaDifesa = attaccanteUnit.unitName + " aumenta di 2 punti la sua difesa speciale.";
            StartCoroutine(WaitAnimationMossa(attaccanteUnit));
            StartCoroutine(BoosterLampeggiante(


                (attaccanteUnit).gameObject)); /////DSHALKDJHLSABDJHKVDSKJHSDAVKDJHSAVKDJSAHVD
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
                x = Random.Range(1, 5);
                Debug.Log("Scarica di coltelli viene usata " + x + " volte");
                for (int i = 0; i < x; i++)
                {
                    StartCoroutine(ColpitoLampeggiante(FindColpito(colpitoUnit)));
                    StartCoroutine(WaitMossaAttaccoFuoriPosto());
                    StartCoroutine(Coltelli(mossa, attaccanteUnit, colpitoUnit, colpitoHUD));
                    StartCoroutine(WaitAnimationMossaDiagonale(attaccanteUnit, colpitoUnit, new Quaternion(0, 0, 0.216439605f, 0.976296067f), mossa));

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

                    /*if (colpitoUnit.currentHP <= 0 && giocatoreNONAttaccato.currentHP <= 0)
                    {
                        battleSystem.state = BattleState.FINISHED;
                        //qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP = 0);
                        Debug.Log("MORTO CON COLTELLI");
                        battleSystem.EndBattle();
                        if (attaccanteUnit.unitID == 0 || attaccanteUnit.unitID == 1)
                        {
                            mossa.gameObject.GetComponent<AttaccoNormale>().FinePartita(battleSystem.playerPrefab, battleSystem.friendPrefab);
                            Debug.Log("MORTO CON COLTELLI 1");
                        }
                        else
                        {
                            mossa.gameObject.GetComponent<AttaccoNormale>().FinePartita(battleSystem.enemyPrefab, battleSystem.enemy2Prefab);
                            Debug.Log("MORTO CON COLTELLI 2");
                        }
                    }*/

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
            Debug.Log(mossa.elemento + " non ha efficacia contro personaggi di tipo " + colpitoUnit.elemento);
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
            StartCoroutine(WaitVitaUp(attaccanteUnit, attacanteHUD));
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
        }
    }

    IEnumerator WaitVitaUp(Unit attaccanteUnit, BattleHUD attacanteHUD)
    {
        yield return new WaitForSeconds(5f);
        int vita = attaccanteUnit.maxHP;
        int cura = vita / 3;
        attaccanteUnit.Heal(cura);
        attacanteHUD.SetHP(attaccanteUnit);
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
            StartCoroutine(WaitActivateParalizzato(colpitoUnit));
            Debug.Log(colpitoUnit.unitName + " viene paralizzato.");
            //combactButtons.SetActive(false);
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
        }
    }

    public void CartellinoRosso(Mossa mossa, Unit colpitoUnit, Unit attaccanteUnit)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            string BacioPrincipessa = attaccanteUnit.unitName + " usa Cartellino rosso.";
            string VieneParalalizzato = colpitoUnit.unitName + " viene espulso (paralizzato).";
            mossa.puoiUsarla = false;
            StartCoroutine(WaitAnimationMossa(colpitoUnit));
            StartCoroutine(Paralizzato(FindColpito(colpitoUnit)));
            StartCoroutine(ShowTextDouble(BacioPrincipessa, VieneParalalizzato));
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            colpitoUnit.paralizzato = true;
            StartCoroutine(WaitActivateParalizzato(colpitoUnit));
            Debug.Log(colpitoUnit.unitName + " viene espulso.");
            //combactButtons.SetActive(false);
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
        }
    }

    IEnumerator WaitActivateParalizzato(Unit colpitoUnit)
    {
        yield return new WaitForSeconds(3);
        FindColpitoHUD(colpitoUnit).transform.GetChild(4).gameObject.SetActive(true);
    }


    public void OrdineDellaFuturaRegina(Mossa mossa, Unit attaccanteUnit)
    {
        Debug.Log("OrdineDellaFuturaRegina");

        if (AttaccoRiesce(mossa.precisione))
        {

            int z = 0;
            CameraAudio.PlayOneShot(attaccanteUnit.AudioMossaSpeciale);

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

            if (amicoDaBoostare.currentHP > 0)
            {
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
                string AttaccoFallito = /*amicoDaBoostare.unitName + " è esausto quindi " +*/ attaccanteUnit.unitName + " fallisce!";
                StartCoroutine(ShowText(AttaccoFallito));
                Debug.Log("ATTACCO FALLITO ORDINE");
            }
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
            Debug.Log("ATTACCO FALLITO ORDINE");
        }
    }

    public void AssistAlBacio(Mossa mossa, Unit attaccanteUnit)
    {
        Debug.Log("Assist al bacio");

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

            if (amicoDaBoostare.currentHP > 0)
            {
                amicoDaBoostare.attacco += 5;
                string Assist = attaccanteUnit.unitName + " usa Assist al bacio.";
                string AmicoBoostato = "L'attacco di " + amicoDaBoostare.unitName + " aumenta di 5 punti!";
                StartCoroutine(ShowTextDouble(Assist, AmicoBoostato));
                StartCoroutine(WaitAnimationMossa(amicoDaBoostare));
                StartCoroutine(BoosterLampeggiante(FindColpito(amicoDaBoostare)));
                StartCoroutine(WaitMossaAttaccoFuoriPosto());
                Debug.Log(attaccanteUnit.unitName + " aumenta l'attacco di " + amicoDaBoostare.unitName + " 5 punti!");
            }
            else
            {
                string AttaccoFallito = /*amicoDaBoostare.unitName + " è esausto quindi " +*/ attaccanteUnit.unitName + " fallisce!";
                StartCoroutine(ShowText(AttaccoFallito));
            }
        }
        else
        {
            string AttaccoFallito = attaccanteUnit.unitName + " prova ad attaccare ma fallisce!";
            StartCoroutine(ShowText(AttaccoFallito));
        }
    }


    public void SguardoDelDrago(Mossa mossa, Unit colpitoUnit, Unit attaccanteUnit)
    {
        if (AttaccoRiesce(mossa.precisione))
        {
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            string SguardoDrago = attaccanteUnit.unitName + " usa Sguardo Del Drago.";
            string RiduciAttacco = attaccanteUnit.unitName + " riduce di 1 l'attacco degli avversari.";
            //StartCoroutine(WaitAnimationMossa(colpitoUnit));
            //StartCoroutine(MalusLampeggiante(GameObject.Find(colpitoUnit.unitName)));
            StartCoroutine(ShowTextDouble(SguardoDrago, RiduciAttacco));
            StartCoroutine(WaitMossaAttaccoFuoriPosto());
            int z = 0;
            foreach (Unit personaggio in battleSystem.amici)
            {
                if (personaggio.unitID == colpitoUnit.unitID)
                {
                    if (battleSystem.amici[0].currentHP > 0 && battleSystem.amici[1].currentHP > 0)
                    {
                        battleSystem.amici[0].attacco -= 1;
                        battleSystem.amici[1].attacco -= 1;
                        StartCoroutine(WaitAnimationMossa(battleSystem.amici[0]));
                        StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.amici[0])));
                        StartCoroutine(WaitAnimationMossa(battleSystem.amici[1]));
                        StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.amici[1])));
                    }
                    else if (battleSystem.amici[0].currentHP > 0 && battleSystem.amici[1].currentHP <= 0)
                    {
                        battleSystem.amici[0].attacco -= 1;
                        StartCoroutine(WaitAnimationMossa(battleSystem.amici[0]));
                        StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.amici[0])));
                    }
                    else if (battleSystem.amici[0].currentHP <= 0 && battleSystem.amici[1].currentHP > 0)
                    {
                        battleSystem.amici[1].attacco -= 1;
                        StartCoroutine(WaitAnimationMossa(battleSystem.amici[1]));
                        StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.amici[1])));
                    }
                }
                z++;
            }

            z = 0;

            foreach (Unit personaggio in battleSystem.nemici)
            {
                if (personaggio.unitID == colpitoUnit.unitID)
                {
                    if (battleSystem.nemici[0].currentHP > 0 && battleSystem.nemici[1].currentHP > 0)
                    {
                        battleSystem.nemici[0].attacco -= 1;
                        battleSystem.nemici[1].attacco -= 1;
                        StartCoroutine(WaitAnimationMossa(battleSystem.nemici[0]));
                        StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.nemici[0])));
                        StartCoroutine(WaitAnimationMossa(battleSystem.nemici[1]));
                        StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.nemici[1])));
                    }
                    else if (battleSystem.nemici[0].currentHP > 0 && battleSystem.nemici[1].currentHP <= 0)
                    {
                        battleSystem.nemici[0].attacco -= 1;
                        StartCoroutine(WaitAnimationMossa(battleSystem.nemici[0]));
                        StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.nemici[0])));
                    }
                    else if (battleSystem.nemici[0].currentHP <= 0 && battleSystem.nemici[1].currentHP > 0)
                    {
                        battleSystem.nemici[1].attacco -= 1;
                        StartCoroutine(WaitAnimationMossa(battleSystem.nemici[1]));
                        StartCoroutine(MalusLampeggiante(FindColpito(battleSystem.nemici[1])));
                    }
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


    public IEnumerator Coltelli(Mossa mossa, Unit giocatoreCheAttacca, Unit qualeNemicoAttacchi, BattleHUD qualeNemicoHUD)
    {
        Debug.Log(giocatoreCheAttacca.unitName + " usa " + mossa.nomeMossa + " contro " + qualeNemicoAttacchi.unitName);
        //StartCoroutine(WaitAnimationMossaDiagonaleColtelli(giocatoreCheAttacca, qualeNemicoAttacchi, new Quaternion(0, 0, 0.216439605f, 0.976296067f)));
        string UsaSuccesso = giocatoreCheAttacca.unitName + " usa " + mossa.nomeMossa + " contro " + qualeNemicoAttacchi.unitName + "!";
        StartCoroutine(ShowText(UsaSuccesso));
        yield return new WaitForSecondsRealtime(5);
        int dannoEffettivo = battleSystem.calcolaDannoEffettivo(mossa.danni, giocatoreCheAttacca, qualeNemicoAttacchi, mossa);
        string perdeXP = qualeNemicoAttacchi.unitName + " perde " + dannoEffettivo*x + "HP.";
        StartCoroutine(ShowText(perdeXP));
        yield return new WaitForSecondsRealtime(2);
        bool isDead = qualeNemicoAttacchi.TakeDamage(dannoEffettivo);
        qualeNemicoHUD.SetHP(qualeNemicoAttacchi);
        Debug.Log(dannoEffettivo);

        if (qualeNemicoAttacchi.currentHP <= 0 && giocatoreNONAttaccato.currentHP <= 0)
        {
            battleSystem.state = BattleState.FINISHED;
            //qualeNemicoHUD.SetHP(qualeNemicoAttacchi.currentHP = 0);
            battleSystem.EndBattle();
            if (giocatoreCheAttacca.unitID == 0 || giocatoreCheAttacca.unitID == 1)
            {
                FinePartita(battleSystem.playerPrefab, battleSystem.friendPrefab);
            }
            else
            {
                FinePartita(battleSystem.enemyPrefab, battleSystem.enemy2Prefab);
            }
        }
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

    public void FinePartita(GameObject Vincitore1, GameObject Vincitore2)
    {
        //GameObject.FindGameObjectWithTag("BattleSystem").SetActive(false);
        partitaFinita.SetActive(true);
        partitaFinita.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = Vincitore1.name + " e " + Vincitore2.name + " vincono la battaglia!";
        partitaFinita.transform.GetChild(2).GetComponent<Image>().sprite = Vincitore1.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
        partitaFinita.transform.GetChild(3).GetComponent<Image>().sprite = Vincitore2.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(WaitAudioEsultanza(Vincitore1, Vincitore2));

        PlayerPrefs.SetInt("MONETE", PlayerPrefs.GetInt("MONETE", 0) + 50);
    }

    public IEnumerator WaitAudioEsultanza(GameObject Vincitore1, GameObject Vincitore2)
    {
        yield return new WaitForSeconds(1);
        CameraAudio.PlayOneShot(Vincitore1.GetComponent<Unit>().AudioEsultanza);
        yield return new WaitForSeconds(Vincitore1.GetComponent<Unit>().AudioEsultanza.length);
        CameraAudio.PlayOneShot(Vincitore2.GetComponent<Unit>().AudioEsultanza);
    }
}
