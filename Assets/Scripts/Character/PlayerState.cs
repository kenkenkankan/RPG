using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // ---- Player Health ---- //
    public float currentHealth;
    public float maxHealth;

    [SerializeField] private bool enableRegen = true;
    [SerializeField] private int regenAmount = 2;
    [SerializeField] private float regenInterval = 2f; // dalam detik

    [SerializeField] private Slider healthSlider;

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
        if (currentHealth <= 0.0f)
        {
            Debug.Log("Player is dead.");
            isDead = true;

            Animator bearAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
            if (bearAnimator != null)
            {
                bearAnimator.SetBool("isDead", true);
            }
        }

    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            isDead = true;
            Debug.Log("Player is dead.");
            animator.SetTrigger(animDie);

            // Mulai reset scene setelah delay
            StartCoroutine(ReloadSceneAfterDelay(3f));
        }
        else
        {
            Debug.Log("Player is hurt.");
        }
    }

    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("MainMenu");
    }

    private void SetupAnimator()
    {
        animDie = Animator.StringToHash("Die");
    }

    private Coroutine regenCoroutine;

    private void OnEnable()
    {
        if (enableRegen)
        {
            regenCoroutine = StartCoroutine(RegenHealthOverTime());
        }
    }

    private void OnDisable()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
    }

    private IEnumerator RegenHealthOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval);

            if (currentHealth < maxHealth)
            {
                currentHealth += regenAmount;
                currentHealth = Mathf.Min(currentHealth, maxHealth);

                if (healthSlider != null)
                    healthSlider.value = currentHealth;
            }
        }
    }

}

