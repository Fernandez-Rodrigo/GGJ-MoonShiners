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

    public class AttackPoint
    {
        public float radius;
        public Vector3 offset;
        public Transform attackRoot;

        //editor only as it's only used in editor to display the path of the attack that is used by the raycast
        [NonSerialized] public List<Vector3> previousPositions = new List<Vector3>();
    }

    public LayerMask targetLayers;
    public AttackPoint[] attackPoints = new AttackPoint[0];
    protected Vector3[] m_PreviousPos = null;


    public bool throwingHit;
    protected bool inAttack = false;
    protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
    protected static Collider[] s_ColliderCache = new Collider[32];
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
        yield return new WaitForSeconds (waitTimeInput);
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
            animator.SetFloat(hashStateTime, Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            animator.ResetTrigger(hashMeleeAttack);

            if (hasAnimator)
            {
                animator.SetTrigger(hashMeleeAttack);
            }

            attack = false;
        }
    }


    public void OnStartAttack(bool throwingAttack)
    {
        throwingHit = throwingAttack;
        inAttack = true;
        m_PreviousPos = new Vector3[attackPoints.Length];

        for (int i = 0; i < attackPoints.Length; ++i)
        {
            Vector3 worldPos = attackPoints[i].attackRoot.position +
                               attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
            m_PreviousPos[i] = worldPos;

        //#if UNITY_EDITOR
        //    attackPoints[i].previousPositions.Clear();
        //    attackPoints[i].previousPositions.Add(m_PreviousPos[i]);
        //#endif
        }
    }

        public void EndAttack()
        {
            inAttack = false;

            //#if UNITY_EDITOR
            //    for (int i = 0; i < attackPoints.Length; ++i)
            //    {
            //        attackPoints[i].previousPositions.Clear();
            //    }
            //#endif
        }


        private void FixedUpdate()
        {
            if (inAttack)
            {
                for (int i = 0; i < attackPoints.Length; ++i)
                {
                    AttackPoint pts = attackPoints[i];

                    Vector3 worldPos = pts.attackRoot.position + pts.attackRoot.TransformVector(pts.offset);
                    Vector3 attackVector = worldPos - m_PreviousPos[i];

                    if (attackVector.magnitude < 0.001f)
                    {
                        // A zero vector for the sphere cast don't yield any result, even if a collider overlap the "sphere" created by radius. 
                        // so we set a very tiny microscopic forward cast to be sure it will catch anything overlaping that "stationary" sphere cast
                        attackVector = Vector3.forward * 0.0001f;
                    }

                    Ray r = new Ray(worldPos, attackVector.normalized);

                    int contacts = Physics.SphereCastNonAlloc(r, pts.radius, s_RaycastHitCache, attackVector.magnitude,
                        ~0,
                        QueryTriggerInteraction.Ignore);

                    for (int k = 0; k < contacts; ++k)
                    {
                        Collider col = s_RaycastHitCache[k].collider;

                        if (col != null)
                            CheckDamage(col, pts);
                    }

                    m_PreviousPos[i] = worldPos;

                    #if UNITY_EDITOR
                        pts.previousPositions.Add(m_PreviousPos[i]);
                    #endif
                }
            }
        }

        private bool CheckDamage(Collider other, AttackPoint pts)
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


