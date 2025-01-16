using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public float attackMult = 1f;
    public float attackIncreasePerLevel = 1.1f; // The multiplicative increase per level, (this number)^playerLevel

    [SerializeField] bool keepHealthAfterCombat = false;

    public override void Attack()
    {
        
        if(currentAttack.selfDamage == true)
        {
            if(Random.Range(0, 4) == 0)
            {
                TakeDamage(currentAttack.damage * 0.5f);
                return;
            }
        }

        Debug.Log(CombatManager.Instance.currentTarget.name + ": " + (CombatManager.Instance.currentTarget.health - currentAttack.damage));
        CombatManager.Instance.currentTarget.TakeDamage(currentAttack.damage * attackMult);        
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        attackMult = Mathf.Pow(attackIncreasePerLevel, Experience.Instance.playerLevel);
    }


    // Initialize some values when combat starts
    public void OnStartCombat()
    {
        if (!keepHealthAfterCombat)
        {
            health = maxHealth;

            healthBar.value = health / maxHealth;
        }  
    }

    public void IncreaseMaxHealth()
    {
        maxHealth++;
    }

    // Remove the unit from the "playersInCombat" list and disable the object
    public override void Die()
    {
        CombatManager.Instance.playersInCombat.Remove(this);
        gameObject.SetActive(false);
    }
}
