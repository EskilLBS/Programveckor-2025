using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    [SerializeField] int damage;

    public override void Attack()
    {
        Debug.Log(CombatManager.Instance.currentTarget.name + ": " + (CombatManager.Instance.currentTarget.health - currentAttack.damage));
        CombatManager.Instance.currentTarget.TakeDamage(currentAttack.damage);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
