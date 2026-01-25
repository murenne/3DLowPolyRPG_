using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class Attack : MonoBehaviour
{
    public UnitStatus OwnerUnitStatus { get; set; }

    void Awake()
    {
        if (OwnerUnitStatus == null)
        {
            if (transform.root.GetComponent<UnitStatus>())
            {
                OwnerUnitStatus = transform.root.GetComponent<UnitStatus>();
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == OwnerUnitStatus.gameObject)
        {
            return;
        }

        if (other.TryGetComponent<UnitStatus>(out UnitStatus targetStatus))
        {
            targetStatus.GetUnitDamage(OwnerUnitStatus, targetStatus);

            if (OwnerUnitStatus.IsCritical)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                targetStatus.HurtState = HurtState.DizzyHurt;

                //player Knock back         
                if (other.TryGetComponent<CharacterController>(out CharacterController characterController))
                {
                    transform.LookAt(other.transform);

                    ApplyPlayerKnockback(other.gameObject, direction, OwnerUnitStatus.RuntimeAttackData.kickBackForce);
                }
                else
                {
                    other.gameObject.TryGetComponent<NavMeshAgent>(out NavMeshAgent navMeshAgent);
                    navMeshAgent.isStopped = true;
                    navMeshAgent.velocity = direction * OwnerUnitStatus.RuntimeAttackData.kickBackForce;
                }
            }
        }
    }

    protected void ApplyPlayerKnockback(GameObject target, Vector3 direction, float force)
    {
        Transform targetTransform = target.transform;
        CharacterController characterController = target.GetComponent<CharacterController>();
        Animator animator = target.GetComponent<Animator>();
        UnitStatus unitStatus = target.GetComponent<UnitStatus>();

        targetTransform.DOKill();

        if (characterController)
        {
            characterController.enabled = false;
        }
        if (animator)
        {
            animator.enabled = false;
        }

        float originalY = targetTransform.position.y;
        Vector3 targetPos = targetTransform.position + direction.normalized * force;
        targetPos.y = originalY;

        // DOTween move
        targetTransform.DOMove(targetPos, 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (characterController) characterController.enabled = true;
                if (animator) animator.enabled = true;
            });
    }
}
