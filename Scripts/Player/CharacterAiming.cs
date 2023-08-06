using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class CharacterAiming : MonoBehaviour
{
    public float aimDuration = 0.3f;
    public float pauseTime = 0.1f;
    public GameObject gunSound;

    public Transform cameraLookAt;
    public FixedTouchField touchField;
    public float lookSenstivity;
    public float turnSpeed;
    public float minY;
    public float maxY;

    float rotationX;
    float rotationY;

    [HideInInspector] public bool Sound = false;
    [HideInInspector] public Vector2 look;
    [HideInInspector] public Vector2 move;

    Animator animator;
    PlayerInput playerInput;
    RaycastWeapon weapon;
    Camera cameraTransform;


    private InputAction aimAction;
    private InputAction fireAction;

    int isAimingPara = Animator.StringToHash("isAiming");

    void Start()
    {
        cameraTransform = Camera.main;

        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<RaycastWeapon>();
        playerInput = GetComponent<PlayerInput>();
        gunSound.SetActive(false);

        fireAction = playerInput.actions["Fire"];
        aimAction = playerInput.actions["Aim"];

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (aimAction.inProgress)
        {
            animator.SetBool(isAimingPara, true);
        }
        else
        {
            animator.SetBool(isAimingPara, false);
        }
    }

    void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }


    private void LateUpdate()
    {
        CameraRotation();

        if (fireAction.inProgress)
        {
            weapon.StartFiring();
            ReadySound();
            Sound = true;
        }
        else
        {
            weapon.StopFiring();
            Sound = false;
            gunSound.SetActive(false);
        }
        if (weapon.isFiring)
        {
            weapon.UpdateFiring(Time.deltaTime);
        }
        weapon.UpdateBullets(Time.deltaTime);
    }

    void CameraRotation()
    {
        rotationX += touchField.TouchDist.x * lookSenstivity * Time.deltaTime;
        rotationY += touchField.TouchDist.y * lookSenstivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        cameraLookAt.eulerAngles = new Vector3(-rotationY, rotationX, 0f);

        float yCam = cameraTransform.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCam, 0), turnSpeed * Time.deltaTime);
    }

    void ReadySound()
    {
        StartCoroutine(StartSound());
        gunSound.SetActive(true);
    }

    IEnumerator StartSound()
    {
        yield return new WaitForSeconds(pauseTime);
        if (Sound == true)
        {
            ReadySound();
        }
    }
}
