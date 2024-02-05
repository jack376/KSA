using UnityEngine;

public class JudgementSkill : BaseAttack
{
    public float spinDuration = 5f;
    public float damageInterval = 0.49f;
    public float finishDamage = 30f;

    public ParticleSystem spinParticle;
    public string finishParticleName;

    private PlayerController playerController;
    private Animator playerAnimator;

    private string particleName;
    private float flowTime;
    private bool isSpinning = false;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        flowTime += Time.deltaTime;

        if (isSpinning)
        {
            if (flowTime >= spinDuration)
            {
                isSpinning = false;
                flowTime = 0f;
                playerAnimator.SetBool("IsJudgement", false);

                JudgementFinishTakeDamage();

                spinParticle.gameObject.SetActive(false);
                playerController.OnActEnd();
            }
            else
            {
                if (flowTime % damageInterval <= Time.deltaTime)
                {
                    JudgementTakeDamage();
                }
            }
        }        
    }

    public void JudgementActivate(string particleName)
    {
        this.particleName = particleName;

        isSpinning = true;
        flowTime = 0f;
        playerAnimator.SetBool("IsJudgement", true);
    }

    public override void ShowBeginParticle(string particlePrefabName, Vector3 attackPosition)
    {
        spinParticle.gameObject.SetActive(true);
    }

    private void JudgementTakeDamage()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, range, targetLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(hitCollider.transform, damage, particleName);
            }
        }
    }

    private void JudgementFinishTakeDamage()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, range, targetLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(hitCollider.transform, finishDamage, finishParticleName);
            }
        }
    }

    public override void AttackActivate(string particleName) { }
}
