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

    public void FullHeal()
    {
        health = maxHealth;
        SmoothHealthBar();
    }

    // Initialize some values when combat starts
    public void OnStartCombat()
    {
        if(health <= 0)
        {
            health = 1;
        }

        if (!keepHealthAfterCombat)
        {
            health = maxHealth;

            
        }
        healthBar.gameObject.SetActive(true);

        healthBar.value = health / maxHealth;
    }

    public void OnCombatEnd()
    {
        healthBar.gameObject.SetActive(false);
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
