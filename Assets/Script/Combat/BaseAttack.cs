using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public LayerMask targetLayerMask;

	public float damage = 10f;
	public float cooldown = 1f;
	public float radius = 0.5f;
	public float range = 2f;

    public virtual void AttackActivate(string particleName)
	{
        var attackPosition = transform.position + transform.forward * range;
		var hitColliders = Physics.OverlapSphere(attackPosition, radius, targetLayerMask);
		foreach (var hitCollider in hitColliders)
		{
			var damageable = hitCollider.GetComponent<IDamageable>();
			damageable?.TakeDamage(hitCollider.transform, damage, particleName);
        }
    }

    public virtual void ShowBeginParticle(string particlePrefabName, Vector3 attackPosition)
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
}
