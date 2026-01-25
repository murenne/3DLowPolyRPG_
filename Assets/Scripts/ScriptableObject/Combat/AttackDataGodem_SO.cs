using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Godem Attack Data", menuName = "Attack/Godem Attack Data")]
public class AttackDataGodem_SO : AttackDataBase_SO
{
    [Header("Godem Rock")]
    public int rockDamage;
    public int rockSpeed;
    public int rockKickBackForce;
    public GameObject rockPrefab;
    public GameObject brokenRocksPrefab;
}
