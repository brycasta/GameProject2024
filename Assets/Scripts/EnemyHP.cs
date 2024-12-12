using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject healthBarPrefab;   // Assign the slider prefab in the Inspector
    public int maxHealth = 100;
    public float healthBarVisibleDuration = 2f; // Time in seconds to show the health bar after taking damage

    private int currentHealth;
    private GameObject healthBarInstance;
    private Slider healthBarSlider;
    private Canvas mainCanvas; // Reference to the screen-space overlay canvas
    private float healthBarTimer; // Timer to track health bar visibility

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Find the screen-space canvas
        mainCanvas = FindObjectOfType<Canvas>();

        if (mainCanvas == null)
        {
            Debug.LogError("No Screen Space - Overlay Canvas found in the scene!");
            return;
        }

        // Instantiate health bar under the screen-space canvas
        healthBarInstance = Instantiate(healthBarPrefab, mainCanvas.transform);
        healthBarSlider = healthBarInstance.GetComponent<Slider>();

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
            healthBarInstance.SetActive(false); // Hide health bar initially
        }
        else
        {
            Debug.LogError("Health bar slider component is missing from the prefab!");
        }
    }

    void Update()
    {
        // Update health bar position to follow the enemy in screen space
        if (healthBarInstance != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            healthBarInstance.transform.position = screenPosition;

            // Handle health bar visibility based on timer
            if (healthBarTimer > 0)
            {
                healthBarTimer -= Time.deltaTime;
                if (healthBarTimer <= 0)
                {
                    healthBarInstance.SetActive(false); // Hide health bar after timer ends
                }
            }
        }
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        // Show the health bar and reset the timer
        healthBarInstance.SetActive(true);
        healthBarTimer = healthBarVisibleDuration;

        // Destroy enemy if health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damageAmount);
            }
            Destroy(other.gameObject);
        }
    }

    private void Die()
    {
        Destroy(gameObject);             // Destroy the enemy object
        Destroy(healthBarInstance);      // Destroy the health bar UI
    }

    private void OnDestroy()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }
    }
}