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

    public void SetHUD(Unit unit)
	{
		nameText.text = unit.unitName;
		//levelText.text = "Lvl " + unit.unitLevel;
		hpSlider.maxValue = unit.maxHP;
		hpSlider.value = unit.currentHP;
		HPText.text = hpSlider.value.ToString() + "/" + hpSlider.maxValue.ToString();
	}

	public void SetHP(int hp)
	{
		hpSlider.value = hp;
		HPText.text = hp.ToString() + "/" + hpSlider.maxValue.ToString();
	}

}
