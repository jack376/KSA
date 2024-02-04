using UnityEngine;

public interface IDamageable
{
    void TakeDamage(Transform transform, float damage, string particlePrefabName);
    void TakeDotDamage(Transform transform, float damage, float duration, float tickRate, string particlePrefabName);
}