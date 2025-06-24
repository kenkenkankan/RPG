using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] private Animator animator;

    [SerializeField] private Slider healthSlider; // Tambahkan referensi ke slider

    private int animDie;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10);
        }
    }
}
