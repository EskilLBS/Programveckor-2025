using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public override void Attack()
    {
        UnitBase currentTarget;

        currentTarget = CombatManager.Instance.playerCharacters[Random.Range(0, CombatManager.Instance.playerCharacters.Count)];

        currentTarget.TakeDamage(damage);
        Debug.Log(currentTarget.gameObject.name + ": " + currentTarget.health);
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CombatManager.Instance.SetCurrentTarget(this);
        }
    }
}
