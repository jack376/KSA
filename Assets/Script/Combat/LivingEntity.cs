using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public event Action OnDeath;

    public HpSlider hpSlider;

    public int maxHealth = 100;

    public float Health { get; protected set; }
    public bool  Dead   { get; protected set; }

    protected virtual void OnEnable()
    {
        Dead = false;
        Health = maxHealth;
        hpSlider?.UpdateHP(Health / maxHealth);
    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        hpSlider?.UpdateHP(Health / maxHealth);

        if (Health <= 0 && !Dead)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        OnDeath?.Invoke();
        Dead = true;
    }
}