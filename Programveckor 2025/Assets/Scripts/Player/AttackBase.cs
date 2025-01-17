using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Scriptable Objects/Attack")]
public class AttackBase : ScriptableObject
{
    public float damage;
    public float armorIncrease;
    public string attackName = "New Attack";

    public float healAmount;

    public bool selfDamage = false;

}
