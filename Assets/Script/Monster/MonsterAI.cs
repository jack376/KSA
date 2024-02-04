using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
	public enum State { Idle, Patrol, Chase, Attack, Death, }

	public State currentState;

	public NavMeshAgent pathFinder;
	public GameObject playerRef;

	public float patrolDistance = 10f;
	public float chaseDistance = 5f;
	public float attackDistance = 2f;

	public float attackCooldown = 1f;
	public float skillCooldown = 5f;

    public float idleStateRandomPercent = 0.5f;
    public float runSpeedRate = 2.5f;

    public float idleDuration = 2f;
    public float lerpAcceleration = 4f;

    private Animator monsterAnimator;
    private LivingEntity livingEntity;

    private Vector3 nextPatrolPosition;

    private float flowTime;
    private float moveSpeed;
	private float runSpeed;
    private float previousMoveSpeed;

	private bool isDie = false;

    private void Start()
	{
		pathFinder = GetComponent<NavMeshAgent>();
		monsterAnimator = GetComponent<Animator>();
        livingEntity = GetComponent<LivingEntity>();

        flowTime = 0f;

		currentState = State.Idle;

        moveSpeed = pathFinder.speed;
        previousMoveSpeed = pathFinder.speed;

        runSpeed = pathFinder.speed * runSpeedRate;

        livingEntity.OnDeath += OnDeath;
    }

    private void OnDestroy()
    {
        livingEntity.OnDeath -= OnDeath;
    }
    
    private void FixedUpdate()
	{
        flowTime += Time.fixedDeltaTime;

        var distance = Vector3.Distance(playerRef.transform.position, transform.position);
        switch (currentState)
		{
			case State.Idle  : Idle(distance);   break;
			case State.Patrol: Patrol(distance); break;
			case State.Chase : Chase(distance);  break;
			case State.Attack: Attack(distance); break;
			case State.Death : Death(); break;
		}

        if (isDie)
        {
            return;
        }

        pathFinder.speed = Mathf.Lerp(pathFinder.speed, moveSpeed, lerpAcceleration * Time.fixedDeltaTime);
        monsterAnimator.SetFloat("MoveSpeed", pathFinder.speed);
    }

    private void Idle(float distance)
	{
        moveSpeed = 0f;
        pathFinder.isStopped = true;

        if (distance <= chaseDistance)
		{
			currentState = State.Chase;
		}

        if (flowTime >= idleDuration)
        {
            currentState = State.Patrol;
        }
    }

    private void Patrol(float distance)
    {
        moveSpeed = previousMoveSpeed;
        pathFinder.isStopped = false;

        if (distance <= chaseDistance)
        {
            currentState = State.Chase;
        }
        else
        {
            if (!pathFinder.pathPending && pathFinder.remainingDistance <= pathFinder.stoppingDistance)
            {
                if (Random.value < idleStateRandomPercent) // 50%
                {
                    currentState = State.Idle;
					flowTime = 0f;
                }
				else
				{
					NewPatrolPosition();
                }	
            }
        }
    }

    private void Chase(float distance)
	{
		moveSpeed = runSpeed;
        pathFinder.isStopped = false;
        pathFinder.destination = playerRef.transform.position;

        if (distance <= attackDistance)
		{
			currentState = State.Attack;
		}
		else if (distance > chaseDistance)
		{
			currentState = State.Patrol;
		}
    }

	private void Attack(float distance)
	{
		moveSpeed = 0f;
        pathFinder.isStopped = true;

        if (distance > attackDistance)
        {
            currentState = State.Chase;
        }

        var lookRotation = Quaternion.LookRotation((playerRef.transform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);

        if (flowTime >= attackCooldown)
		{
            monsterAnimator.SetTrigger("Attack");
            currentState = State.Chase;
			flowTime = 0f;
        }
    }

	private void Death()
	{
        moveSpeed = 0f;
        pathFinder.isStopped = true;

        if (!isDie)
        {
            pathFinder.velocity = Vector3.zero;
            pathFinder.ResetPath();

            monsterAnimator.SetTrigger("Die");
            isDie = true;
        }
    }

    public void OnDeath()
    {
        currentState = State.Death;
    }
	
    private void NewPatrolPosition()
    {
        var randomDirection = Random.insideUnitSphere * patrolDistance;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out var hit, patrolDistance, NavMesh.AllAreas))
        {
            nextPatrolPosition = hit.position;
        }
        else
        {
            nextPatrolPosition = transform.position;
        }

        pathFinder.SetDestination(nextPatrolPosition);
    }

    private void OnDrawGizmosSelected()
    {
        // Test Code
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}