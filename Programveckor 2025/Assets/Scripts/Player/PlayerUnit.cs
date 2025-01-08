using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    [SerializeField] int damage;

    public override void Attack()
    {
        CombatManager.Instance.currentTarget.TakeDamage(damage);
        Debug.Log(CombatManager.Instance.currentTarget.health);
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
