using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyUnit : UnitBase
{
    [SerializeField] float damage;

    [HideInInspector] public bool spared;

    [SerializeField] string playerName;
    static string pName;

    Rigidbody2D rb;
    [SerializeField] float sparedLifeRetreatSpeed;

    // Start is called before the first frame update
    void Start()
    {
        pName = playerName;
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Attack()
    {
        if (!spared)
        {
            AssignNewAttack(attacks[Random.Range(0, attacks.Count)]);

            UnitBase currentTarget;

            currentTarget = CombatManager.Instance.playersInCombat[Random.Range(0, CombatManager.Instance.playersInCombat.Count)];

            currentTarget.TakeDamage(currentAttack.damage);
            Debug.Log(currentTarget.gameObject.name + ": " + currentTarget.health);
        }
        
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CombatManager.Instance.SetCurrentTarget(this);
        }
    }

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
