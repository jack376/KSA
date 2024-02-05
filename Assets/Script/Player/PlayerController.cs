using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode upButton    = KeyCode.W;
    public KeyCode leftButton  = KeyCode.A;
    public KeyCode downButton  = KeyCode.S;
    public KeyCode rightButton = KeyCode.D;

    public KeyCode attackButton    = KeyCode.I;
    public KeyCode judgementButton = KeyCode.J;
    public KeyCode debuffButton    = KeyCode.K;
    public KeyCode shieldButton    = KeyCode.L;

    public float moveSpeed = 10f;
    public float rotateSpeed = 90f;

    public float knockdownTime = 1.5f;
    public float getupTime = 2f;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private BaseAttack playerAttack;
    private DebuffSkill playerDebuff;
    private JudgementSkill playerJudgement;
    private ShieldSkill playerShield;

    private LivingEntity livingEntity;

    private float flowTime;

    private float previousMoveSpeed;
    private float previousRotateSpeed;

    private bool isAct = true;
    private bool isKnockdown = false;
    private bool isGetup = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator  = GetComponent<Animator>();

        playerAttack    = GetComponent<BaseAttack>();
        playerDebuff    = GetComponent<DebuffSkill>();
        playerJudgement = GetComponent<JudgementSkill>();
        playerShield    = GetComponent<ShieldSkill>();

        livingEntity = GetComponent<LivingEntity>();

        flowTime = 0f;

        previousMoveSpeed = moveSpeed;
        previousRotateSpeed = rotateSpeed;

        isAct = true;
        isKnockdown = false;
        isGetup = false;

        livingEntity.OnDeath += OnDeath;
    }

    private void OnDestroy()
    {
        livingEntity.OnDeath -= OnDeath;
    }

    private void FixedUpdate()
    {
        flowTime += Time.fixedDeltaTime;

        float horizontal = Input.GetKey(rightButton) ? 1f : Input.GetKey(leftButton) ? -1f : 0f;
        float vertical   = Input.GetKey(upButton)    ? 1f : Input.GetKey(downButton) ? -1f : 0f;
                
        var inputVector  = new Vector3(horizontal, 0f, vertical).normalized;
        var moveVelocity = inputVector * moveSpeed;

        playerRigidbody.velocity = new Vector3(moveVelocity.x, playerRigidbody.velocity.y, moveVelocity.z);

        if (inputVector != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(inputVector);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
        }

        playerAnimator.SetFloat("MoveSpeed", inputVector.magnitude);

        if (isKnockdown && !livingEntity.Dead)
        {
            if (flowTime >= knockdownTime)
            {
                isKnockdown = false;
                isGetup = true;

                playerAnimator.SetBool("IsKnockdown", isKnockdown);
                rotateSpeed = previousRotateSpeed * 0.05f;
            }

            return;
        }

        if (isGetup && flowTime >= getupTime)
        {
            rotateSpeed = previousRotateSpeed;
            moveSpeed = previousMoveSpeed;

            isGetup = false;
        }
    }

    private void Update()
    {
        flowTime += Time.deltaTime;
        if (flowTime >= playerAttack.cooldown && isAct)
        {
            if (Input.GetKey(attackButton))
            {
                playerAnimator.SetTrigger("Attack");
                playerAttack.ShowBeginParticle("Stone slash", transform.position + Vector3.up * 0.75f);
                OnActStart();
            }
            if (Input.GetKey(debuffButton))
            {
                playerAnimator.SetTrigger("Debuff");
                playerDebuff.ShowBeginParticle("Electro slash fix", transform.position + Vector3.up * 0.75f);
                OnActStart();
            }
            if (Input.GetKey(judgementButton))
            {
                playerAnimator.SetTrigger("Judgement");
                playerJudgement.ShowBeginParticle("AoE slash orange", transform.position + Vector3.up * 0.75f);
                isAct = false;
            }
            if (Input.GetKey(shieldButton))
            {
                playerAnimator.SetTrigger("Shield");
                playerShield.ShowBeginParticle("Magic shield blue", transform.position + Vector3.up * 0.75f);
                OnActStart();
            }
        }
    }

    private void OnDeath()
    {
        moveSpeed = 0f;
        rotateSpeed = 0f;

        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.isKinematic = true;
        playerRigidbody.useGravity = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        isKnockdown = true;

        playerAnimator.SetTrigger("Knockdown");
        playerAnimator.SetBool("IsKnockdown", isKnockdown);
    }

    private void OnActStart()
    {
        previousMoveSpeed = moveSpeed;
        previousRotateSpeed = rotateSpeed;

        moveSpeed = 0f;
        rotateSpeed = 0f;

        isAct = false;
    }

    public void OnActEnd()
    {
        moveSpeed = previousMoveSpeed;
        rotateSpeed = previousRotateSpeed;

        isAct = true;
    }
}
