using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public event Action OnHit;
    public event Action OnDeath;

    public float Health { get; protected set; } = 100f;
    public bool Dead { get; protected set; }

    protected virtual void OnEnable()
    {
        Dead = false;
    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        OnHit?.Invoke();

        if (Health <= 0 && !Dead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();
        Dead = true;
    }
}