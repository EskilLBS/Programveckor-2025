using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public float attackMult = 1f;
    public float attackIncreasePerLevel = 1.1f; // The multiplicative increase per level, (this number)^playerLevel

    public override void Attack()
    {
        attackMult = Mathf.Pow(attackIncreasePerLevel, Experience.Instance.playerLevel);

        Debug.Log(CombatManager.Instance.currentTarget.name + ": " + (CombatManager.Instance.currentTarget.health - currentAttack.damage));
        CombatManager.Instance.currentTarget.TakeDamage(currentAttack.damage * attackMult);        
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public override void Die()
    {
        CombatManager.Instance.playersInCombat.Remove(this);
        Debug.Log(CombatManager.Instance.playersInCombat.Count);
        gameObject.SetActive(false);
    }
}
