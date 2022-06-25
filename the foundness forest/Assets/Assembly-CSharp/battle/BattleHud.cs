using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    public Text nameText;
    public Text LevelText;
    public Slider hpslider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        LevelText.text = "lvl " + unit.unitLevel;
        hpslider.maxValue = unit.maxHP;
        hpslider.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        hpslider.value = hp;
    }

}

