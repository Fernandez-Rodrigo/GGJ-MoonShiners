using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class MeleeSystem : MonoBehaviour
{
    private bool hasAnimator;
    public Animator animator;
    public bool canAttack;
    public bool attack;
    private int hashMeleeAttack;
    private int hashStateTime;
    public int damage = 1;

    public LayerMask targetLayers;

    public bool throwingHit;
    protected bool inAttack = false;
    private GameObject owner;

    private Coroutine AttackWaitCoroutine;
    public float waitTimeInput = 0.03f;


    private void Start()
    {
        hasAnimator = TryGetComponent(out animator);
        canAttack = true;
        if (hasAnimator)
        {
            hashMeleeAttack = Animator.StringToHash("MeleeAttack");
            hashStateTime = Animator.StringToHash("StateTime");
        }
    }

    private IEnumerator AttackWait()
    {
        attack = true;
        yield return new WaitForSeconds(waitTimeInput);
        attack = false;
    }


    public void SetCanAttack(bool state)
    {
        canAttack = state;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (AttackWaitCoroutine != null)
                StopCoroutine(AttackWaitCoroutine);

            AttackWaitCoroutine = StartCoroutine(AttackWait());
        }

        if (attack && canAttack)
        {
            if (hasAnimator)
            {
                animator.SetFloat(hashStateTime, Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
                animator.ResetTrigger(hashMeleeAttack);
                animator.SetTrigger(hashMeleeAttack);
            }
            attack = false;
        }

    }


    public void StartAttack()
    {
        inAttack = true;
    }

    public void EndAttack()
    {
        inAttack = false;
    }


    private void FixedUpdate()
    {
        if (inAttack)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.forward, 1.5f, ~0, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider col = colliders[i];

                if (col != null)
                    CheckDamage(col);
            }
        }
    }

    private bool CheckDamage(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health == null)
        {
            return false;
        }

        if (health.gameObject == owner)
            return true; //ignore self harm, but do not end the attack (we don't "bounce" off ourselves)

        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
        {
            //hit an object that is not in our layer, this end the attack. we "bounce" off it
            return false;
        }

        health.TakeDamage(damage);

        return true;
    }


}


