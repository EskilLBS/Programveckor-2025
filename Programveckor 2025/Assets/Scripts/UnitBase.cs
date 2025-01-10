using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitBase : MonoBehaviour
{
    public List<AttackBase> attacks;
    public AttackBase currentAttack;

    [SerializeField] Slider healthBar;

    public float maxHealth;
    [HideInInspector] public float health;

    public UnitBase()
    {
        health = maxHealth;
    }

    [Tooltip("Called when the unit takes damage, by default it subtracts the amount that gets passed in from the player's current health")]
    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.value = health / maxHealth;

        if(health <= 0)
        {
            Die();
        }
    }

    [Tooltip("Called when a unit attacks, doesn't do anything by default")]
    public virtual void Attack()
    {

    }

    [Tooltip("Called when the unit's health drops to or below zero by default, doesn't do anything by default")]
    public virtual void Die()
    {

    }

    public virtual void AssignNewAttack(AttackBase newAttack)
    {
        currentAttack = newAttack;
    }
}
