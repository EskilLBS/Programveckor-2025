using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitBase : MonoBehaviour
{
    public string unitName;

    public List<AttackBase> attacks;
    protected AttackBase currentAttack;

    [SerializeField] protected Slider healthBar;

    [SerializeField] protected float maxHealth;
    [HideInInspector] public float health;

    public UnitBase()
    {
        health = maxHealth;
    }

    // Called when the unit takes damage, can be overwritten if the behaviour should be different than simply losing health
    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.value = health / maxHealth;

        if(health <= 0)
        {
            Die();
        }
    }

    // Called when the unit attacks, doesn't do anything by default
    public virtual void Attack()
    {

    }

    // Called when the unit dies, doesn't do anything by default
    public virtual void Die()
    {

    }

    // Assigns a new attack to the unit
    public virtual void AssignNewAttack(AttackBase newAttack)
    {
        currentAttack = newAttack;
    }
}
