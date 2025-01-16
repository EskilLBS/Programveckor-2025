using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Scriptable Objects/Attack")]
public class AttackBase : ScriptableObject
{
    public float damage;
    public string attackName = "New Attack";

    public bool selfDamage = false;
}
