using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 6f;
    private float direction = 0f;
    private Rigidbody2D playerRB;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;
    private bool wasGrounded;

    private Animator playerAnimation;

    // Game Over screen
    public GameObject gameOverUI;

    private int jumpCount = 0; // To track how many jumps have been performed //Bryan Castaneda
    public int maxJumps = 2;   // The maximum number of jumps allowed (double jump) //Bryan Castaneda
    private bool canJump = true;

    // Shield variable
    GameObject shield;

    // SFX variables
    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip hurtSound;

    // Invulnerability (iFrame) Variables
    [Header("Invulnerability")]
    [SerializeField] private float iFrameTime;
    [SerializeField] private int flashNumber;
    private SpriteRenderer spriteRenderer;

    private bool isGameOver = false; // New flag for game over state

    void Awake()
    {
        // Initialize components
        shield = transform.Find("Shield").gameObject;
        DeactivateShield();
        playerRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGameOver) return; // Stop processing if the game is over

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Checks if player is grounded

        // Reset jump count if the player lands on the ground
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
            canJump = true; // Reset jump ability
        }

        direction = Input.GetAxis("Horizontal");

        if (direction > 0f) // Player movement going and facing right
        {
            playerRB.velocity = new Vector2(direction * speed, playerRB.velocity.y);
            transform.localScale = new Vector2(0.4314091f, 0.4314091f);
            GetComponent<SpriteRenderer>().flipX = false; // Facing right
        }
        else if (direction < 0f) // Player movement going and facing left
        {
            playerRB.velocity = new Vector2(direction * speed, playerRB.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true; // Facing left
        }
        else
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && canJump && (isGrounded || jumpCount < maxJumps))
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpSpeed);
            jumpCount++;  // Increment jump count
            canJump = false; // Disable further jumps until button is released

            // Play jump sound
            if (SoundManager.instance != null && jumpSound != null)
            {
                SoundManager.instance.PlaySound(jumpSound);
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            canJump = true;
        }

        wasGrounded = isGrounded;

        // Animation updates
        playerAnimation.SetFloat("Speed", Mathf.Abs(playerRB.velocity.x));
        playerAnimation.SetBool("OnGround", isGrounded);
    }

    public void TriggerGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // Activate the game over UI
        }

        // Set game over state
        isGameOver = true;

        // Stop player movement
        playerRB.velocity = Vector2.zero;

        // Play hurt sound
        if (SoundManager.instance != null && hurtSound != null)
        {
            SoundManager.instance.PlaySound(hurtSound);
        }

    }

    // Shield methods created by Lee
    public void ActivateShield()
    {
        shield.SetActive(true);
    }

   public void DeactivateShield()
    {
        shield.SetActive(false);
    }

  public bool HasShield()
    {
        return shield.activeSelf;
    }

    // Handling collisions //Created by Lee
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageSource damageSource = collision.GetComponent<DamageSource>();

        if (damageSource != null)
        {
            if (HasShield())
            {
                DeactivateShield();
                StartCoroutine(Invulnerability());
                // Play shield break sound

                SoundManager.instance.PlaySound(breakSound);
            }
            else
            {
                SoundManager.instance.PlaySound(hurtSound);
                TriggerGameOver(); // Call game over function if no shield
            }

            Destroy(damageSource.gameObject); // Destroy the enemy or damage source
        }

        ShieldPowerUp shieldPowerUp = collision.GetComponent<ShieldPowerUp>();
        if (shieldPowerUp != null)
        {
            ActivateShield();
            Destroy(shieldPowerUp.gameObject); // Destroy the shield power-up after activation
        }
    }

    // Invulnerability routine
    public IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for (int i = 0; i < flashNumber; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameTime / (flashNumber * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFrameTime / (flashNumber * 2));
        }
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }
}
