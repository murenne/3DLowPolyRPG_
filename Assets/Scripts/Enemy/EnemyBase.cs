using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(UnitStatus))]
public class EnemyBase : MonoBehaviour, IEndGameObserver
{
    [Header("Component")]
    private NavMeshAgent _navAgent;
    private Animator _animator;
    private Collider _collider;
    private EnemyState _enemyState;
    protected UnitStatus _unitStatus;

    [Header("Guard")]
    [SerializeField] private bool _isGuard;
    private Vector3 _defaultGuardPosePoint;
    private Quaternion _defaultGuardRotation;

    [Header("Patrol")]
    [SerializeField] private float _patrolRange;
    private Vector3 _patrolPosition;

    [Header("Chase")]
    [SerializeField] private float _sightRadius;
    [SerializeField] private float _stopAndLookTime;
    private float _stopAndLookTimer;
    private float _speed;

    [Header("Attack")]
    protected GameObject _attackTarget;
    private float _attackCoolDownTimer;

    [Header("Animation")]
    bool _isMove;
    bool _isChase;
    bool _isFollow;
    bool _isDead;
    bool _isPlayerDead;
    bool _isAttack;

    protected virtual void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _speed = _navAgent.speed;
        _animator = GetComponent<Animator>();
        _defaultGuardPosePoint = transform.position;
        _defaultGuardRotation = transform.rotation;
        _stopAndLookTimer = _stopAndLookTime;
        _collider = GetComponent<Collider>();
        _unitStatus = GetComponent<UnitStatus>();
    }

    void Start()
    {
        if (_isGuard)
        {
            _enemyState = EnemyState.GUARD;
        }
        else
        {
            _enemyState = EnemyState.PATROL;
            GetNewPatrolPosition();
        }

        // as a end game observer join in observer list
        GameManager.Instance.AddObserver(this);
    }

    void OnDisable()
    {
        if (!GameManager.IsInitialized)
        {
            return;
        }

        // enemy died and remove from observer list
        GameManager.Instance.RemoveObserver(this);

        if (GetComponent<LootDropper>() && _isDead)
        {
            GetComponent<LootDropper>().SpawnLoot();
        }
    }

    void Update()
    {
        if (_unitStatus.CurrentHealth == 0)
        {
            _isDead = true;
        }
        if (!_isPlayerDead)
        {
            SwitchStatus();
            SwitchAnimation();
            _attackCoolDownTimer -= Time.deltaTime;
        }
    }

    void SwitchAnimation()
    {
        _animator.SetBool("Walk", _isMove);
        _animator.SetBool("Chase", _isChase);
        _animator.SetBool("Follow", _isFollow);
        _animator.SetBool("Critical", _unitStatus.IsCritical);
        _animator.SetBool("Death", _isDead);

        if (_isAttack)
        {
            _animator.SetTrigger("Attack");
            _isAttack = false;
        }
    }

    void SwitchStatus()
    {
        if (_isDead)
        {
            _enemyState = EnemyState.DEAD;
        }
        else if (CanFindPlayer())
        {
            _enemyState = EnemyState.CHASE;
        }

        switch (_enemyState)
        {
            case EnemyState.GUARD:
                {
                    // return to the guard position
                    _isChase = false;
                    if (transform.position != _defaultGuardPosePoint)
                    {
                        _isMove = true;
                        _navAgent.isStopped = false;
                        _navAgent.destination = _defaultGuardPosePoint;

                        // turn to the default rotation
                        if (Vector3.Distance(transform.position, _defaultGuardPosePoint) <= _navAgent.stoppingDistance)// squrMagnitude()也可以用
                        {
                            _isMove = false;
                            transform.rotation = Quaternion.Lerp(transform.rotation, _defaultGuardRotation, 0.05f);
                        }
                    }
                    break;
                }
            case EnemyState.PATROL:
                {
                    _isChase = false;
                    _navAgent.speed = _speed * 0.5f;

                    if (Vector3.Distance(_patrolPosition, transform.position) <= _navAgent.stoppingDistance)
                    {
                        // after arriving at the location, patrol for a while.
                        _isMove = false;
                        if (_stopAndLookTimer > 0)
                        {
                            _stopAndLookTimer -= Time.deltaTime;
                        }
                        else
                        {
                            GetNewPatrolPosition();
                        }
                    }
                    else
                    {
                        // go to the new psotition
                        _isMove = true;
                        _navAgent.destination = _patrolPosition;
                    }
                    break;
                }
            case EnemyState.CHASE:
                {
                    _isMove = false;
                    _isChase = true;
                    _navAgent.speed = _speed;

                    // cannot find player
                    if (!CanFindPlayer())
                    {
                        // wait in place for a while then return to default status
                        _isFollow = false;
                        if (_stopAndLookTimer > 0)
                        {
                            _navAgent.destination = transform.position;
                            _stopAndLookTimer -= Time.deltaTime;
                        }
                        else if (_isGuard)
                        {
                            _enemyState = EnemyState.GUARD;
                        }
                        else
                        {
                            _enemyState = EnemyState.PATROL;
                        }
                    }
                    // found player then go to the player's position
                    else
                    {
                        _isFollow = true;
                        if (!TargetInAttackRange())
                        {
                            _navAgent.isStopped = false;
                            _navAgent.destination = _attackTarget.transform.position;
                        }
                    }

                    // in attack range then attack player
                    if (TargetInAttackRange())
                    {
                        _isFollow = false;
                        _navAgent.isStopped = true;
                        if (_attackCoolDownTimer < 0)
                        {
                            ExecuteAttack();
                        }
                    }
                    break;
                }
            case EnemyState.DEAD:
                {
                    _collider.enabled = false;
                    _navAgent.radius = 0;
                    Destroy(gameObject, 2f);
                    break;
                }
        }
    }

    /// <summary>
    /// execute attack 
    /// </summary>
    void ExecuteAttack()
    {
        _isAttack = true;
        transform.LookAt(_attackTarget.transform);
        _attackCoolDownTimer = _unitStatus.RuntimeAttackData.coolDownTime;
        _unitStatus.IsCritical = Random.value < _unitStatus.RuntimeAttackData.criticalChance;
    }

    /// <summary>
    /// check if a player can be found.
    /// </summary>
    /// <returns></returns>
    bool CanFindPlayer()
    {
        var _collideriders = Physics.OverlapSphere(transform.position, _sightRadius);
        foreach (var target in _collideriders)
        {
            if (target.CompareTag("Player"))
            {
                _attackTarget = target.gameObject;
                return true;
            }
        }
        _attackTarget = null;
        return false;
    }

    /// <summary>
    /// target in attack range
    /// </summary>
    /// <returns></returns>
    bool TargetInAttackRange()
    {
        if (_attackTarget != null)
        {
            return Vector3.Distance(_attackTarget.transform.position, transform.position) <= _unitStatus.RuntimeAttackData.attackRange;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// get new patrol position
    /// </summary>
    void GetNewPatrolPosition()
    {
        _stopAndLookTimer = _stopAndLookTime;
        float randomX = Random.Range(-_patrolRange, _patrolRange);
        float randomZ = Random.Range(-_patrolRange, _patrolRange);
        Vector3 randowPoint = new Vector3(_defaultGuardPosePoint.x + randomX, transform.position.y, _defaultGuardPosePoint.z + randomZ);
        NavMeshHit hit;
        _patrolPosition = NavMesh.SamplePosition(randowPoint, out hit, _patrolRange, 1) ? hit.position : transform.position;
    }

    /// <summary>
    /// win notify
    /// </summary>
    public void EndGameNotify()
    {
        _animator.SetBool("Win", true);
        _isPlayerDead = true;
        _isChase = false;
        _isMove = false;
        _attackTarget = null;
    }

    /// <summary>
    /// visualization
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRadius);
    }
}


