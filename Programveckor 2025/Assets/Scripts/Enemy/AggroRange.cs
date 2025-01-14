using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour
{
    [SerializeField] Collider2D aggroCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if the player is inside the collider, if so then start combat
        if(collision.tag == "Player")
        {
            CombatManager.Instance.StartCombat(GetComponentInParent<EnemyGroup>().enemiesInGroup);

            aggroCollider.enabled = false; 
        }
    }
}
