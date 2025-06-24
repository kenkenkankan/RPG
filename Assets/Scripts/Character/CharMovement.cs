using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    private PlayerState playerState;

    [Header("References")]
    private CharacterController controller;
    [SerializeField] private Transform camera;
    [SerializeField] private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float sprintTransitSpeed = 5f;
    [SerializeField] private float turningSpeed = 2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Attack Attributes")]
    private Animator anim;
    [SerializeField] private Collider weaponCollider;

    [Header("Player Attributes")]
    public float currentHealth;
    public float maxHealth;

    private float slowSpeed = 1f;
    private float currentSpeed;

    private float verticalVelocity;
    private float speed;

    [Header("Animation")]
    private int animMoveInput;
    private int animJump;
    private int animGrounded;
    private int animAttack;
    private int animDie;
    private int animHorizontal;
    private int animVertical;

    private float animH;
    private float animV;

    [Header("Input")]
    private float moveInput;
    private float turnInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        SetupAnimator();
        
    }


    private void Update()
    {
        

        InputManagement();
        Movement();
    }

    private void Movement()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.isDead)
        {
            return;
        }

        GroundMovement();
        Turn();
  
        Attack();
    }

    private void Turn()
    {
        if (Mathf.Abs(turnInput) > 0 || Mathf.Abs(moveInput) > 0)
        {
            Vector3 currentLookDirection = camera.forward;
            currentLookDirection.y = 0;

            currentLookDirection.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(currentLookDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turningSpeed);
        }

    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = 0;

            animator.SetBool(animGrounded, true);

            if (Input.GetButton("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);

                animator.SetTrigger(animJump);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;

            animator.SetBool(animGrounded, false);
        }
        return verticalVelocity;
    }

    private void GroundMovement()
    {
        float rawH = Input.GetAxis("Horizontal");
        float rawV = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(rawH, 0, rawV).normalized;

        // Arah relatif terhadap kamera
        Vector3 camForward = camera.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = camera.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camForward * input.z + camRight * input.x;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float movementMultiplier = isSprinting ? 1.5f : 1f;

        Vector3 localMove = transform.InverseTransformDirection(moveDir) * movementMultiplier;

        speed = Mathf.Lerp(speed, isSprinting ? sprintSpeed : walkSpeed, sprintTransitSpeed * Time.deltaTime);

        animH = Mathf.Lerp(animH, localMove.x, Time.deltaTime * 10f);
        animV = Mathf.Lerp(animV, localMove.z, Time.deltaTime * 10f);

        moveDir.y = VerticalForceCalculation();
        controller.Move(moveDir * speed * Time.deltaTime);

        animator.SetFloat("Horizontal", animH);
        animator.SetFloat("Vertical", animV);
    }



    private void SetupAnimator()
    {
        animJump = Animator.StringToHash("Jump");
        animGrounded = Animator.StringToHash("Grounded");
        animAttack = Animator.StringToHash("Attack");
        animDie = Animator.StringToHash("Die");

        // Tambahan
        animHorizontal = Animator.StringToHash("Horizontal");
        animVertical = Animator.StringToHash("Vertical");
    }

    void UpdateAnimator(Vector3 localMove)
    {
        animH = Mathf.Lerp(animH, localMove.x, Time.deltaTime * 10f);
        animV = Mathf.Lerp(animV, localMove.z, Time.deltaTime * 10f);

        animator.SetFloat("Horizontal", animH);
        animator.SetFloat("Vertical", animV);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger(animAttack);
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attack"))
        {
            currentSpeed = slowSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    private void InputManagement()
    {

        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }
    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }

    public void TakeDamage(int damage)
    {
   

        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            animator.SetTrigger("Die");
        }
    }
}
