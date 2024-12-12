using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public int damageAmount = 10; // Damage dealt by this bullet
    public GameObject explosionPrefab; // Reference to explosion effect prefab

    private Rigidbody2D rb;
    private bool isFacingRight; // Track bullet direction

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Ensures the bullet doesn't fall
        Destroy(gameObject, lifeTime); // Destroy bullet after lifeTime expires
    }

    void Start()
    {
        rb.velocity = transform.right * speed; // Set the bullet velocity
        isFacingRight = transform.localScale.x > 0; // Determine if facing right based on local scale
    }

    // Trigger explosion and destroy bullet upon collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Instantiate explosion effect immediately at the bullet's position
        if (explosionPrefab != null)
        {
            // Instantiate the explosion
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Rotate the explosion prefab 180 degrees if the bullet is facing left
            if (!isFacingRight)
            {
                explosion.transform.localRotation = Quaternion.Euler(0, 0, 180); // Rotate 180 degrees around Z-axis
            }
        }

        // Deal damage to enemies if they are hit
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount); // Apply damage to the enemy
            }
        }

        // Immediately destroy the bullet to avoid multiple collisions or lingering
        Destroy(gameObject);
    }
}