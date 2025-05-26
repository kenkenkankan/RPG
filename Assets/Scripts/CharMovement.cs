using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

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


    [Header("Input")]
    private float moveInput;
    private float turnInput;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        SetupAnimator();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        InputManagement();
        Movement();

    }

    private void Movement()
    {
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
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = transform.TransformDirection(move);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, sprintSpeed, sprintTransitSpeed * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, walkSpeed, sprintTransitSpeed * Time.deltaTime);
        }

        move.y = VerticalForceCalculation();

        move *= speed;

        controller.Move(move * Time.deltaTime);

        //Animations
        animator.SetFloat("MoveInput", speed * MathF.Max(MathF.Abs(moveInput), MathF.Abs(turnInput)));
    }

    private void SetupAnimator()
    {
        animMoveInput = Animator.StringToHash("MoveInput");
        animJump = Animator.StringToHash("Jump");
        animGrounded = Animator.StringToHash("Grounded");
        animAttack = Animator.StringToHash("Attack");
        animDie = Animator.StringToHash("Die");
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


    private IEnumerator StartJump()
    {
        yield return new WaitForSeconds(.5f);

        verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);
    }

    private bool GroundCheck()
    {
        if (Physics.Raycast(new Vector3(controller.center.x, controller.center.y - controller.height, controller.center.z), 
            -controller.transform.up, out RaycastHit hit, 2f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
