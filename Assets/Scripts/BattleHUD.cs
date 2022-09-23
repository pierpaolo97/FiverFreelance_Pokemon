using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
	public Text nameText;
	public TextMeshProUGUI HPText;
	//public Text levelText;
	public Slider hpSlider;
	GameObject ColpitoGiusto;
    public TextMeshProUGUI elementoText;
    public RawImage coloreElemento;

    public void SetHUD(Unit unit)
	{
		nameText.text = unit.unitName;
		//levelText.text = "Lvl " + unit.unitLevel;
		hpSlider.maxValue = unit.maxHP;
		hpSlider.value = unit.currentHP;
		HPText.text = hpSlider.value.ToString() + "/" + hpSlider.maxValue.ToString();
        elementoText.text = unit.elemento;

        switch (unit.elemento)
        {
            case "VENTO": coloreElemento.color = new Color32(116, 146, 226, 255); break;
            case "NORMALE": coloreElemento.color = new Color32(255, 255, 255, 255); break;
            case "TERRA": coloreElemento.color = new Color32(101, 67, 33, 255); break;
            case "SPAZIO": coloreElemento.color = new Color32(37, 40, 80, 255); break;
            case "FUOCO": coloreElemento.color = new Color32(255, 0, 0, 255); break;
            case "NESSUNO": coloreElemento.color = new Color32(255, 255, 102, 255); break;
        }


        
    }

	public void SetHP(Unit Colpito)
	{
		hpSlider.value = Colpito.currentHP;
		if (Colpito.currentHP <= 0)
        {
			HPText.text = 0 + "/" + hpSlider.maxValue.ToString();
			FindColpito(Colpito).gameObject.GetComponent<Animator>().Play("EsaustoPg");
		}
		else
			HPText.text = Colpito.currentHP.ToString() + "/" + hpSlider.maxValue.ToString();
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
}
