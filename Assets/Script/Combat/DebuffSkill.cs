using UnityEngine;

public class DebuffSkill : BaseAttack
{
    public float dotDamage = 5f;
    public float dotDuration = 5f;
    public float dotTickInterval = 1f;

    private float flowTime;

    public void DebuffActivate(string particleName)
    {
        var attackPosition = transform.position + transform.forward * range;
        var hitColliders = Physics.OverlapSphere(attackPosition, radius, targetLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponent<IDamageable>();
            damageable?.TakeDotDamage(hitCollider.transform, dotDamage, dotDuration, dotTickInterval, particleName);
        }
    }

    public override void AttackActivate(string particleName) { }
}