using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerControllerScript : MonoBehaviour
{
    Animator animator;
    CharacterController cc;
    PlayerInput playerInput;
    CharacterAiming characterAiming;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;
    Vector3 playerVelocity;
    bool groundedPlayer;

    public float jumpHeight;
    public float gravityValue;
    public float playerSpeed;
    public float turnSpeed;
    public float cameraRotationSpeed;
    public float animationSmoothTime;

    public Transform cameraTransform;
    public Cinemachine.CinemachineVirtualCamera vCam;
    public Transform CameraLookAt;

    private InputAction moveAction;
    private InputAction jumpAction;

    float xRotation;
    float yRotation;
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
        playerInput = GetComponent<PlayerInput>();
        characterAiming = GetComponent<CharacterAiming>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        //float moveSpeed = 0;

        groundedPlayer = cc.isGrounded;

        if (groundedPlayer && playerVelocity.y <= 0)
        {
            playerVelocity.y = 0f;
            animator.SetBool(jumpHash, false);
        }

        //Vector3 inputDir = new Vector3(characterAiming.move.x, 0, characterAiming.move.y);
        //float targetRotation = 0;

        //if (characterAiming.move != Vector2.zero)
        //{
        //    moveSpeed = playerSpeed;
        //    targetRotation = Quaternion.LookRotation(inputDir).eulerAngles.y + cameraTransform.rotation.eulerAngles.y;
        //    Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, playerSpeed * Time.deltaTime);
        //}

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);

        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        cc.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat(inputXAnimation, characterAiming.move.x);
        animator.SetFloat(inputYAnimation, characterAiming.move.y);


        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetBool(jumpHash, true);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        //CameraRotation();
    }

    void CameraRotation()
    {
        xRotation += characterAiming.look.y * cameraRotationSpeed * Time.deltaTime;
        yRotation += characterAiming.look.x * cameraRotationSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -40, 50);
        //yRotation = Mathf.Clamp(yRotation, -60, 80);

        Quaternion rotation = Quaternion.Euler(-xRotation, yRotation, 0);
        CameraLookAt.rotation = rotation;

        float yCam = cameraTransform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCam, 0), turnSpeed * Time.deltaTime);
    }
}
