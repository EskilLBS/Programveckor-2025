using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI textElement;

    AttackBase attack;
    float damageMult;

    public void SetText(AttackBase newAttack, float mult)
    {
        if(CombatManager.Instance.currentCombatState != CombatManager.CombatState.PlayerTurn)
        {
            return;
        }

        textElement = GameObject.Find("AttackInfoText").GetComponent<TextMeshProUGUI>();

        attack = newAttack;
        damageMult = mult;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(attack.healAmount > 0)
        {
            textElement.text = attack.healAmount + " healing to self";
            return;
        }

        if(attack.armorIncrease > 0)
        {
            textElement.text = attack.armorIncrease + " armor";
            return;
        }

        if (attack.selfDamage)
        {
            textElement.text = Mathf.Round(attack.damage * damageMult * 10) / 10 + " damage\n25% chance to inflict damage on self instead";
        }
        else
        {
            textElement.text = Mathf.Round(attack.damage * damageMult * 10) / 10 + " damage";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textElement.text = "";
    }
}
