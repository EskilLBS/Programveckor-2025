using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyUnit : UnitBase
{
    // Has the enemy been spared or not?
    [HideInInspector] public bool hasBeenSpared;

    Rigidbody2D rb;
    [SerializeField] float sparedLifeRetreatSpeed;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Attack logic
    public override void Attack()
    {
        // Only attack if it hasn't been spared, to avoid bugs where enemies attack after being spared
        if (!hasBeenSpared)
        {
            // Assign a random attack to the player
            AssignNewAttack(attacks[Random.Range(0, attacks.Count)]);

            // Set "currentTarget" to a random player unit
            UnitBase currentTarget;
            currentTarget = CombatManager.Instance.playersInCombat[Random.Range(0, CombatManager.Instance.playersInCombat.Count)];

            // Make the current target take damage equal to the damage of the current attack
            currentTarget.TakeDamage(currentAttack.damage);
        }
        
    }

    // Check if the unit is clicked
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CombatManager.Instance.SetCurrentTarget(this);
        }
    }

    // On death remove enemy from lists, give xp to the player and add to the "badness" count
    public override void Die()
    {
        CombatManager.Instance.RemoveEnemyFromList(this);
        CombatManager.Instance.SetCurrentTarget(null);

        GoodOrBadDecision.Instance.BadDecision(1);

        Experience.Instance.GainExperience(1, 11);

        Destroy(gameObject);
    }

    // Caleld when you spare the life of an enemty
    public void OnSpared()
    {
        Destroy(gameObject);
        Destroy(CombatManager.Instance.currentTargetMarker);

        GoodOrBadDecision.Instance.GoodDecision(1);
    }
}
