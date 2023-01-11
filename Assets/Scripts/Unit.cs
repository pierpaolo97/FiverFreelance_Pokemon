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
    public List<Mossa> mosse;
    public Sprite spriteUnit;
    public int unitID;
    public bool maschio;

    public List<AudioClip> audioAttacchiSubiti;
    public List<AudioClip> audioSelezioni;
	public AudioClip AudioMossaSpeciale;
	public AudioClip AudioEsultanza;

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
