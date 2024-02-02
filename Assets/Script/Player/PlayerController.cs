using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 90f;

    public KeyCode upButton    = KeyCode.W;
    public KeyCode downButton  = KeyCode.S;
    public KeyCode leftButton  = KeyCode.A;
    public KeyCode rightButton = KeyCode.D;

    public KeyCode attackButton = KeyCode.Space;
    public KeyCode hitButton = KeyCode.C;
    public KeyCode KnockdownButton = KeyCode.V;

    private float prevMoveSpeed;
    private float prevRotationSpeed;

    private float knockdownTime = 1.5f;
    private float getupTime = 2f;
    private float flowTime = 0f;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public bool IsKnockdown { get; set; } = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        prevMoveSpeed = moveSpeed;
        prevRotationSpeed = rotationSpeed;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetKey(rightButton) ? 1f : Input.GetKey(leftButton) ? -1f : 0f;
        float vertical   = Input.GetKey(upButton)    ? 1f : Input.GetKey(downButton) ? -1f : 0f;

        var inputVector  = new Vector3(horizontal, 0, vertical).normalized;
        var moveVelocity = inputVector * moveSpeed;

        playerRigidbody.velocity = new Vector3(moveVelocity.x, playerRigidbody.velocity.y, moveVelocity.z);

        if (inputVector != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(inputVector);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }

        playerAnimator.SetFloat("MoveSpeed", inputVector.magnitude);
    }

    private void Update()
    {
        flowTime += Time.deltaTime;
        if (IsKnockdown)
        {
            if (flowTime >= knockdownTime)
            {
                IsKnockdown = false;

                playerAnimator.SetBool("IsKnockdown", IsKnockdown);

                rotationSpeed = prevRotationSpeed * 0.05f;
            }

            return;
        }

        if (flowTime >= getupTime)
        {
            rotationSpeed = prevRotationSpeed;
            moveSpeed = prevMoveSpeed;
            flowTime = 0f;
        }

        if (Input.GetKeyDown(attackButton))
        {
            playerAnimator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(hitButton))
        {
            playerAnimator.SetTrigger("Hit");
        }

        if (Input.GetKeyDown(KnockdownButton))
        {
            IsKnockdown = true;

            playerAnimator.SetTrigger("Knockdown");
            playerAnimator.SetBool("IsKnockdown", IsKnockdown);

            moveSpeed = 0f;
            rotationSpeed = 0f;
            flowTime = 0f;
        }
    }
}
