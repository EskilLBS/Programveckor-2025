using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitBase : MonoBehaviour
{
    public List<AttackBase> attacks;
    public AttackBase currentAttack;

    [SerializeField] Slider healthBar;

    public int maxHealth;
    [HideInInspector] public int health;

    public UnitBase()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;

        healthBar.value = health / (float)maxHealth;

        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Attack()
    {

    }

    public virtual void Die()
    {

    }

    public virtual void AssignNewAttack(AttackBase newAttack)
    {
        currentAttack = newAttack;
    }
}
