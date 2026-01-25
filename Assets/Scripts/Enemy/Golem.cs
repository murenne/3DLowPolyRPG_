using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyBase
{
    [Header("Golem")]
    [SerializeField] private Transform _handPosition;
    private AttackDataGodem_SO _data;

    protected override void Awake()
    {
        base.Awake();
        _data = (AttackDataGodem_SO)_unitStatus.RuntimeAttackData;
    }

    /// <summary>
    /// animation event: throw rock
    /// </summary>
    public void AnimThrowRock()
    {
        var rock = Instantiate(_data.rockPrefab, _handPosition.position, Quaternion.identity);
        rock.GetComponent<GolemRock>().Initialize(this.gameObject, _attackTarget, _data);
    }
}
