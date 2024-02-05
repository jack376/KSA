using System;
using System.Collections;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public event Action OnDeath;
    public HpSlider hpSlider;

    public int maxHealth = 100;
    public float defense = 0f;

    private Coroutine dotDamageCoroutine;
    private float flowTime;

    public float Health { get; protected set; }
    public bool Dead { get; protected set; }

    protected virtual void OnEnable()
    {
        Dead = false;
        Health = maxHealth;
        hpSlider?.UpdateHP(Health / maxHealth);
    }

    protected virtual void Update()
    {
        flowTime += Time.deltaTime;
    }

    public virtual void TakeDamage(Transform transform, float damage, string particleName)
    {
        Health -= Mathf.Max(damage - defense, 0);
        hpSlider?.UpdateHP(Health / maxHealth);

        var damageTextPool = ObjectPoolManager.Instance.GetPool("DamageText");

        var damageTextInstance = damageTextPool.Get();
        damageTextInstance.transform.position = transform.position;
        damageTextInstance.GetComponent<DamageText>().SetText(damage.ToString());

        var damageText = damageTextInstance.GetComponent<DamageText>();
        damageText.OnDamageText += ReleaseDamageText;

        void ReleaseDamageText()
        {
            damageText.OnDamageText -= ReleaseDamageText;
            damageTextPool.Release(damageTextInstance);
        }

        ShowHitParticle(particleName, transform.position);

        if (Health <= 0 && !Dead)
        {
            Death();
        }
    }

    public void TakeDotDamage(Transform transform, float damage, float duration, float tickRate, string particleName)
    {
        if (dotDamageCoroutine != null)
        {
            StopCoroutine(dotDamageCoroutine);
        }

        dotDamageCoroutine = StartCoroutine(DotDamage(transform, damage, duration, tickRate, particleName));
    }

    private IEnumerator DotDamage(Transform transform, float damage, float duration, float tickRate, string particleName)
    {
        float tickTime = 0f;

        while (tickTime < duration)
        {
            if (flowTime > tickRate)
            {
                flowTime = 0f;
                tickTime += tickRate;

                TakeDamage(transform, damage, particleName);

                if (Health <= 0 && !Dead)
                {
                    Death();
                }
            }

            yield return null;
        }
    }

    public virtual void Death()
    {
        OnDeath?.Invoke();
        Dead = true;

        if (dotDamageCoroutine != null)
        {
            StopCoroutine(dotDamageCoroutine);
            dotDamageCoroutine = null;
        }
    }

    public virtual void ShowHitParticle(string particlePrefabName, Vector3 hitPosition)
    {
        var hitFxPool = ObjectPoolManager.Instance.GetPool(particlePrefabName);

        var hitFxInstance = hitFxPool.Get();
        hitFxInstance.transform.position = hitPosition + Vector3.up * 0.75f;
        hitFxInstance.transform.rotation = Quaternion.identity;

        var hitParticle = hitFxInstance.GetComponent<ParticleReleaseHandler>();
        hitParticle.OnParticleRelease += ReleaseHitParticle;

        void ReleaseHitParticle()
        {
            hitParticle.OnParticleRelease -= ReleaseHitParticle;
            hitFxPool.Release(hitFxInstance);
        }
    }
}