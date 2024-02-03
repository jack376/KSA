using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode upButton    = KeyCode.W;
    public KeyCode leftButton  = KeyCode.A;
    public KeyCode downButton  = KeyCode.S;
    public KeyCode rightButton = KeyCode.D;

    public KeyCode attackButton = KeyCode.Space;

    public KeyCode hitButton = KeyCode.C; // Test Code
    public KeyCode knockdownButton = KeyCode.V; // Test Code

    public float moveSpeed = 10f;
    public float rotateSpeed = 90f;

    public float knockdownTime = 1.5f;
    public float getupTime = 2f;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private float flowTime;

    private float prevMoveSpeed;
    private float prevRotateSpeed;

    private bool isKnockdown = false;
    private bool isGetup = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        prevMoveSpeed = moveSpeed;
        prevRotateSpeed = rotateSpeed;
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
                rotateSpeed = prevRotateSpeed * 0.05f;
            }

            return;
        }

        if (isGetup && flowTime >= getupTime)
        {
            rotateSpeed = prevRotateSpeed;
            moveSpeed = prevMoveSpeed;

            isGetup = false;
        }
    }

    private void Update()
    {
        // Test Code

        if (Input.GetKeyDown(attackButton))
        {
            playerAnimator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(hitButton))
        {
            playerAnimator.SetTrigger("Hit");
        }

        if (Input.GetKeyDown(knockdownButton))
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
