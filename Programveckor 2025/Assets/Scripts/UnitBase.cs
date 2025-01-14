using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitBase : MonoBehaviour
{
    public string unitName;

    public List<AttackBase> attacks;
    public AttackBase currentAttack;

    [SerializeField] Slider healthBar;

    public float maxHealth;
    [HideInInspector] public float health;

    public UnitBase()
    {
        health = maxHealth;
    }
    
    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.value = health / maxHealth;

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
