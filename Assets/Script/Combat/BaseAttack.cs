using System;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public LayerMask targetLayerMask;

	public float attackDamage = 10f;
	public float attackRate   = 1f;
	public float attackRadius = 0.5f;
	public float attackRange  = 2f;

    private float flowTime;
	private bool isAttack;

    private void Start()
    {
        flowTime = 0f;
		isAttack = false;
    }

    private void Update()
	{
		flowTime += Time.deltaTime;
		if (flowTime >= attackRate)
		{
			if (isAttack)
			{
				Attack();
                flowTime = 0f;
                isAttack = false;

                Debug.Log("Use Attack");
			}
		}
	}

    public void Attack()
	{
        var attackPosition = transform.position + transform.forward * attackRange;
		var hitColliders = Physics.OverlapSphere(attackPosition, attackRadius, targetLayerMask);
		foreach (var hitCollider in hitColliders)
		{
			var damageable = hitCollider.GetComponent<IDamageable>();
			damageable?.TakeDamage(attackDamage);

			Debug.Log("Hit!");
		}


    }

	private void OnDrawGizmosSelected()
	{
		// Test Code
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRadius);
	}
}
