using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemRock : Attack
{
    // target
    private GameObject _owner;
    private GameObject _target;

    // Data
    private AttackDataGodem_SO _data;
    private int _rockDamage;
    private float _rockSpeed;
    private GameObject _brokenRocks;

    // calculate
    private Rigidbody _rb;
    private Vector3 _moveDirection;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(GameObject owner, GameObject target, AttackDataBase_SO data)
    {
        _data = (AttackDataGodem_SO)data;
        _owner = owner;
        _target = target;
        _rockDamage = _data.rockDamage;
        _rockSpeed = _data.rockSpeed;
        _brokenRocks = _data.brokenRocksPrefab;

        if (_owner != null && _owner.TryGetComponent<UnitStatus>(out UnitStatus ownerStatus))
        {
            OwnerUnitStatus = ownerStatus;
        }

        FlyToTarget();
    }

    /// <summary>
    /// rock move to target
    /// </summary>
    public void FlyToTarget()
    {
        if (_target == null)
        {
            _target = FindObjectOfType<PlayerController>().gameObject;
        }

        _moveDirection = (_target.transform.position - transform.position + Vector3.up).normalized;
        _rb.AddForce(_moveDirection * _rockSpeed, ForceMode.Impulse);
    }

    /// <summary>
    /// on trigger enter
    /// </summary>
    protected override void OnTriggerEnter(Collider other)
    {
        if (_owner != null && other.gameObject == _owner)
        {
            return;
        }

        if (other.TryGetComponent<UnitStatus>(out UnitStatus targetStatus))
        {
            targetStatus.GetRockDamage(_rockDamage, targetStatus);
            targetStatus.HurtState = HurtState.DizzyHurt;

            if (other.TryGetComponent<CharacterController>(out CharacterController cc))
            {
                ApplyPlayerKnockback(other.gameObject, _moveDirection, _data.rockKickBackForce);
            }
            else if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                agent.isStopped = true;
                agent.velocity = _moveDirection * _data.rockKickBackForce;
            }

            if (_brokenRocks != null)
            {
                Instantiate(_brokenRocks, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
