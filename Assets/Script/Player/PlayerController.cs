using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode upButton    = KeyCode.W;
    public KeyCode leftButton  = KeyCode.A;
    public KeyCode downButton  = KeyCode.S;
    public KeyCode rightButton = KeyCode.D;

    public KeyCode attackButton = KeyCode.Space;

    public float moveSpeed = 10f;
    public float rotateSpeed = 90f;

    public float knockdownTime = 1.5f;
    public float getupTime = 2f;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    private BaseAttack playerAttack;
    private LivingEntity playerLivingEntity;

    private float flowTime;

    private float previousMoveSpeed;
    private float previousRotateSpeed;

    private bool isKnockdown = false;
    private bool isGetup = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAttack = GetComponent<BaseAttack>();
        playerLivingEntity = GetComponent<LivingEntity>();

        flowTime = 0f;

        previousMoveSpeed = moveSpeed;
        previousRotateSpeed = rotateSpeed;
    }

    private void FixedUpdate()
    {
        flowTime += Time.fixedDeltaTime;

        float horizontal = Input.GetKey(rightButton) ? 1f : Input.GetKey(leftButton) ? -1f : 0f;
        float vertical   = Input.GetKey(upButton)    ? 1f : Input.GetKey(downButton) ? -1f : 0f;
                
        var inputVector  = new Vector3(horizontal, 0, vertical).normalized;
        var moveVelocity = inputVector * moveSpeed;

        playerRigidbody.velocity = new Vector3(moveVelocity.x, playerRigidbody.velocity.y, moveVelocity.z);

        if (inputVector != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(inputVector);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
        }

        playerAnimator.SetFloat("MoveSpeed", inputVector.magnitude);

        if (isKnockdown)
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
        // Test Code
        flowTime += Time.deltaTime;
        if (Input.GetKey(attackButton) && flowTime >= playerAttack.attackRate)
        {
            playerAnimator.SetTrigger("Attack");
            playerAttack.ShowAttackParticle("Stone slash", transform.position + Vector3.up * 0.75f);
            flowTime = 0f;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            isKnockdown = true;

            playerAnimator.SetTrigger("Knockdown");
            playerAnimator.SetBool("IsKnockdown", isKnockdown);

            moveSpeed = 0f;
            rotateSpeed = 0f;
            flowTime = 0f;
        }
    }
}
