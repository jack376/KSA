using UnityEngine;

public class ShieldSkill : BaseAttack
{
    public float shieldDuration = 3f;
    public float shieldPoint = 10f;

    public ParticleSystem shieldParticle;

    private PlayerController playerController;
    private LivingEntity playerLivingEntity;
    private Animator playerAnimator;

    private bool isShieldActive = false;
    private float shieldTimer = 0f;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
        playerLivingEntity = GetComponent<LivingEntity>();
    }

    private void Update()
    {
        if (isShieldActive)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer >= shieldDuration)
            {
                isShieldActive = false;

                playerAnimator.SetBool("IsShield", isShieldActive);
                shieldParticle.gameObject.SetActive(isShieldActive);
                playerLivingEntity.defense = 0f;

                playerController.OnActEnd();
            }
        }
    }

    public override void ShowBeginParticle(string particlePrefabName, Vector3 attackPosition)
    {
        shieldParticle.gameObject.SetActive(true);
    }

    public void ShieldActivate()
    {
        if (!isShieldActive)
        {
            isShieldActive = true;

            playerAnimator.SetBool("IsShield", isShieldActive);
            playerLivingEntity.defense = shieldPoint;

            shieldTimer = 0f;
        }
    }

    public override void AttackActivate(string particleName) { }
}
