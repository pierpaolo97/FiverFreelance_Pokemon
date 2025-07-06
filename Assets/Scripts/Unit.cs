using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

	public string unitName;
	public int unitLevel;
	public int damage;
	public int maxHP;
	public int currentHP;
    public int attacco;
    public int difesa;
    public int attacco_speciale;
    public int difesa_speciale;
    public int velocita;
    public string elemento;
    public bool paralizzato = false;
    public bool avvelenato = false;
	public bool subisceDanno = true;

	public float boost; //setto boost, che può essere sia positivo che negativo. Se una mossa ha un boost momentaneo, ad esempio 2 turni, faccio un check su nTurni;
	public int nTurniBoost;
	public string whichBoost;

	public int turniAvvelenamento;

    public List<Mossa> mosse;
    public Sprite spriteUnit;
	public Sprite spriteUnitDown;
	public Sprite arthurSprite;
    public int unitID;
    public bool maschio;

    public List<AudioClip> audioAttacchiSubiti;
    public List<AudioClip> audioSelezioni;
	public AudioClip AudioMossaSpeciale;
	public AudioClip AudioEsultanza;

    public void Update()
    {
		//if (this.transform.position.y < 0 && unitName == "Mary")
		//{
		//	SpriteRenderer sr = transform.Find("ImmaginePG")?.GetComponent<SpriteRenderer>();
		//	if (sr != null)
		//	{
		//		sr.sprite = spriteUnitDown;
		//	}
		//}
	}

    public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if (currentHP <= 0)
			return true;
		else
			return false;
	}

	public void Heal(int amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
			currentHP = maxHP;
	}

}
