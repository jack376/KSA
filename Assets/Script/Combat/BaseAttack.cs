using System;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public LayerMask targetLayerMask;

	public float attackDamage = 10f;
	public float attackRate   = 1f;
	public float attackRadius = 0.5f;
	public float attackRange  = 2f;

    public void OnAttack(string particlePrefabName)
	{
        var attackPosition = transform.position + transform.forward * attackRange;
		var hitColliders = Physics.OverlapSphere(attackPosition, attackRadius, targetLayerMask);
		foreach (var hitCollider in hitColliders)
		{
			var damageable = hitCollider.GetComponent<IDamageable>();
			damageable?.TakeDamage(attackDamage);

            ShowDamageText(hitCollider.transform.position, attackDamage);
            ShowHitParticle(particlePrefabName, hitCollider.transform.position);
        }
    }

    private void ShowDamageText(Vector3 attackPosition, float attackDamage)
    {
        var damageTextPool = ObjectPoolManager.Instance.GetPool("DamageText");

        var damageTextInstance = damageTextPool.Get();
        damageTextInstance.transform.position = attackPosition;
        damageTextInstance.GetComponent<DamageText>().SetText(attackDamage.ToString());

        var damageText = damageTextInstance.GetComponent<DamageText>();
        damageText.OnDamageText += ReleaseDamageText;

        void ReleaseDamageText()
        {
            damageText.OnDamageText -= ReleaseDamageText;
            damageTextPool.Release(damageTextInstance);
        }
    }

    public void ShowAttackParticle(string particlePrefabName, Vector3 attackPosition)
    {
        var attackFxPool = ObjectPoolManager.Instance.GetPool(particlePrefabName);

        var attackFxInstance = attackFxPool.Get();
        attackFxInstance.transform.position = attackPosition + Vector3.up * 0.5f;
        attackFxInstance.transform.rotation = transform.rotation;

        var attackParticle = attackFxInstance.GetComponent<ParticleReleaseHandler>();
        attackParticle.OnParticleRelease += ReleaseAttackParticle;

        void ReleaseAttackParticle()
        {
            attackParticle.OnParticleRelease -= ReleaseAttackParticle;
            attackFxPool.Release(attackFxInstance);
        }
    }

    private void ShowHitParticle(string particlePrefabName, Vector3 hitPosition)
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

    private void OnDrawGizmosSelected()
    {
        // Test Code
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRadius);
    }
}
