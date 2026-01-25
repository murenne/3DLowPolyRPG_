using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Attack/Base Attack Data")]
public class AttackDataBase_SO : ScriptableObject
{
    [Header("Base attack data")]
    public float attackRange;
    public float kickBackForce;
    public float coolDownTime;
    public float minAttackPoint;
    public float maxAttackPoint;
    public float criticalMultiplier;
    public float criticalChance;

    /// <summary>
    /// set up weapon data
    /// </summary>
    /// <param name="weapon"></param>
    public void ApplyWeaponData(AttackDataBase_SO weapon)
    {
        attackRange = weapon.attackRange;
        coolDownTime = weapon.coolDownTime;

        minAttackPoint = weapon.minAttackPoint;
        maxAttackPoint = weapon.maxAttackPoint;

        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }

}
