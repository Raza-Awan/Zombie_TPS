using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;
    CharacterController cc;

    Vector2 input;
    Vector3 rootMotion;
    Vector3 playerVelocity;
    bool groundedPlayer;

    public float jumpHeight = 2f;
    public float gravityValue = 3f;
    public float stepDown = 0.1f;
    public float airControl = 2.5f;
    public float jumpDamp = 0.5f;
    public float playerSpeed = 1.2f;

    int inputXAnimation;
    int inputYAnimation;
    int jumpHash;

    private void Awake()
    {
        inputXAnimation = Animator.StringToHash("InputX");
        inputYAnimation = Animator.StringToHash("InputY");
        jumpHash = Animator.StringToHash("Jump");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat(inputXAnimation, input.x);
        animator.SetFloat(inputYAnimation, input.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void FixedUpdate()
    {
        if (groundedPlayer)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }
    }

    private void UpdateOnGround()
    {
        Vector3 stepDownAmount = Vector3.down * stepDown;
        Vector3 stepForwardAmount = rootMotion * playerSpeed;

        cc.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;
        if (!cc.isGrounded)
        {
            SetInAir(0);
        }
    }

    private void UpdateInAir()
    {
        playerVelocity.y -= gravityValue * Time.fixedDeltaTime;
        Vector3 displacement = playerVelocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        cc.Move(displacement);
        groundedPlayer = !cc.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool(jumpHash, false);
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100);
    }

    void Jump()
    {
        if (!groundedPlayer)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravityValue * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        groundedPlayer = true;
        playerVelocity = animator.velocity * jumpDamp * playerSpeed;
        playerVelocity.y = jumpVelocity;
        animator.SetBool(jumpHash, true);
    }
}
