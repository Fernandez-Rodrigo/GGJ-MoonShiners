using UnityEngine.Events;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    public UnityEvent onDamage;
    public UnityEvent OnDeath;
    protected new Collider2D collider;
    private bool hasCollider;
    private Action schedule;


    private void Start()
    {
        health = maxHealth;
        hasCollider = TryGetComponent<Collider2D>(out collider);
    }

    private void LateUpdate()
    {
        if (schedule != null)
        {
            schedule();
            schedule = null;
        }
    }

    public void TakeDamage(float value)
    {
        health = health > 0 ? health - value : 0;

        if (health <= 0)
        {
            Debug.Log("DEAD");

            OnDeath.Invoke(); 
        }
        else
        {
            onDamage.Invoke();
        }

        Debug.Log(health);

    }

    public void SetStateCollider(bool state)
    {
        if (hasCollider) 
            collider.enabled = state;
    }
}
