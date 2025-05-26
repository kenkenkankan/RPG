using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // ---- Player Health ---- //
    public float currentHealth;
    public float maxHealth;

    public bool isDead { get; private set; } = false;


    [SerializeField] Animator animator;

    private int animDie;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        SetupAnimator();

      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // <- abaikan damage jika sudah mati

        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            Debug.Log("Player is dead.");
            isDead = true;
            animator.SetTrigger(animDie);
        }
        else
        {
            Debug.Log("Player is hurt.");
        }
    }




    private void SetupAnimator()
    {
        animDie = Animator.StringToHash("Die");
    }
}

