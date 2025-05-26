using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] private Animator animator;

    private int animDie;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            animator.SetTrigger("Die");   
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentHealth -= 10;
        }
    }

    private void SetupAnimator()
    {
        animDie = Animator.StringToHash("Die");
    }

    private void Die()
    {
        Debug.Log($"{animalName} has died.");
        Destroy(gameObject);
    }
}
