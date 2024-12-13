using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    

    // Add a reference for the AudioSource
    
    [SerializeField] private AudioClip rayGunSound; // Drag your ray gun sound clip here
    private AudioSource audioSource;

    // Invulnerability (iFrame) Variables
    [Header("Invulnerability")]
    [SerializeField] private float iFrameTime;
    [SerializeField] private int flashNumber;
    private SpriteRenderer spriteRenderer;

    private bool isGameOver = false; // New flag for game over state


    [Header("Canvas References")]
    public Canvas gameCanvas; // Reference to the canvas to delete



    //Fanial W. (variables)

    // Gun Variables
    [Header("Pickup")]
    public bool hasGun = false;
    public GameObject gunPrefab;
    public Transform gunHoldPoint;
    public SpriteRenderer gunSpriteRenderer;
    public bool isGunActive = false;

    [Header("Duration")]
    public float gunDuration = 60f;
    private float gunTimer = 0f;

    [Header("Gun UI")]
    public Slider gunTimerSlider;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI gunPickupCountText;
    public int gunPickups = 0;

    [Header("Shooting Control")]
    public float fireRate = 3f;
    private float lastFired = 0f;

    [Header("Gun Offset")]
    public float gunOffset = 0.2f; // Adjust as needed for the distance of the gun from the player

    // Speed adjustment while shooting
    public float shootingSpeedMultiplier = 0.5f; // 50% of normal speed while shooting

    // New variable for the firing spot Animator
    [Header("Firing Spot")]
    public GameObject firingSpot; // Reference to the firing spot GameObject
    private Animator firingSpotAnimator;

    void Awake()
    {
        // Initialize components
        shield = transform.Find("Shield").gameObject;
        DeactivateShield();
        playerRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<Animator>();


        //Fanial (Awake)
        if (gunTimerSlider != null)
        {
            gunTimerSlider.gameObject.SetActive(false);
        }

        if (ammoText != null)
        {
            ammoText.text = "";
        }

        if (gunPickupCountText != null)
        {
            gunPickupCountText.text = "Unlimited Pickups: " + gunPickups;
        }

        // Ensure gun is hidden by default until activated
        if (gunSpriteRenderer != null)
        {
            gunSpriteRenderer.enabled = false;
        }

        // Get the Animator component on the firing spot GameObject
        if (firingSpot != null)
        {
            firingSpotAnimator = firingSpot.GetComponent<Animator>();
            if (firingSpotAnimator == null)
            {
                Debug.LogError("No Animator component found on firing spot!");
            }

            // Ensure the firing spot's Sprite Renderer is hidden at the start
            SpriteRenderer firingSpotRenderer = firingSpot.GetComponent<SpriteRenderer>();
            if (firingSpotRenderer != null)
            {
                firingSpotRenderer.enabled = false;
            }
        }


        // Fanial W. (Initialize components)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on Player GameObject!");
        }
    }

    void Update() //Bryan
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

        // Adjust speed based on shooting status
        float currentSpeed = isGunActive && Input.GetMouseButton(0) ? speed * shootingSpeedMultiplier : speed;

        if (direction > 0f) // Player movement going and facing right
        {
            playerRB.velocity = new Vector2(direction * currentSpeed, playerRB.velocity.y);
            transform.localScale = new Vector2(0.4314091f, 0.4314091f);
            GetComponent<SpriteRenderer>().flipX = false; // Facing right
            if (gunSpriteRenderer != null)
            {
                gunSpriteRenderer.flipX = false;
            }
            
        }
        else if (direction < 0f) // Player movement going and facing left
        {
            playerRB.velocity = new Vector2(direction * currentSpeed, playerRB.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true; // Facing left
            if (gunSpriteRenderer != null)
            {
                gunSpriteRenderer.flipX = true;
            }
            
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




        // Fanial W. (Gun Activation)
        if (gunPickups > 0 && Input.GetKeyDown(KeyCode.G) && !isGunActive)
        {
            ActivateGun();
        }

        if (isGunActive)
        {
            gunTimer -= Time.deltaTime;
            gunTimerSlider.value = gunTimer;

            if (gunTimer <= 0f)
            {
                DeactivateGun();
            }

            if (Input.GetMouseButton(0))
            {
                Shoot();
            }

            if (gunSpriteRenderer != null)
            {
                float offsetDirection = spriteRenderer.flipX ? -1 : 1;
                gunHoldPoint.position = transform.position + new Vector3(offsetDirection * gunOffset, 0, 0);
                gunSpriteRenderer.transform.position = gunHoldPoint.position;
                gunSpriteRenderer.transform.rotation = gunHoldPoint.rotation;

                // Adjust firing spot position to be further away from the player
                float firingSpotOffset = 0.5f; // Increase this value to move the firing spot further away
                firingSpot.transform.position = new Vector3(gunHoldPoint.position.x + (offsetDirection * firingSpotOffset), firingSpot.transform.position.y, firingSpot.transform.position.z);
                firingSpot.transform.rotation = gunHoldPoint.rotation;
            }
        }

        UpdateAmmoUI();
    }

    public void TriggerGameOver() //Bryan
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // Activate the game over UI
        }

        // Destroy the specified canvas
        if (gameCanvas != null)
        {
            Destroy(gameCanvas.gameObject);
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

        if (collision.CompareTag("GunPickup"))
        {
            PickupGun(collision.gameObject);
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



    //Fanial W. 
    private void PickupGun(GameObject gun)
    {
        gunPickups++;
        UpdateGunPickupCountUI();
        Destroy(gun);
    }



    //Fanial W.

    private void ActivateGun()
    {
        isGunActive = true;
        gunPickups--;
        gunTimer = gunDuration;

        float offsetDirection = spriteRenderer.flipX ? -1 : 1;
        gunHoldPoint.position = transform.position + new Vector3(offsetDirection * gunOffset, 0, 0);
        gunHoldPoint.rotation = transform.rotation;

        if (gunSpriteRenderer != null)
        {
            gunSpriteRenderer.enabled = true;
            gunSpriteRenderer.transform.position = gunHoldPoint.position;
            gunSpriteRenderer.transform.rotation = gunHoldPoint.rotation;
        }

        if (gunTimerSlider != null)
        {
            gunTimerSlider.gameObject.SetActive(true);
            gunTimerSlider.maxValue = gunDuration;
            gunTimerSlider.value = gunDuration;
        }

        UpdateGunPickupCountUI();
    }


    //Fanial W.
    private void DeactivateGun()
    {
        isGunActive = false;

        if (gunSpriteRenderer != null)
        {
            gunSpriteRenderer.enabled = false;
        }

        if (gunTimerSlider != null)
        {
            gunTimerSlider.gameObject.SetActive(false);
        }
    }


    //Fanial W.
    private void Shoot()
    {

        
        // Play the firing animation on the firing spot, regardless of firing cooldown
        if (firingSpotAnimator != null)
        {
            firingSpotAnimator.SetTrigger("Flash");
        }

        // Only shoot a bullet if enough time has passed since the last shot
        if (Time.time >= lastFired + (1f / fireRate))
        {
            if (gunPrefab != null)
            {
                // Determine the direction and spawn position for the bullet
                float offsetDirection = spriteRenderer.flipX ? -1 : 1;
                Vector3 bulletPosition = gunHoldPoint.position + new Vector3(offsetDirection * gunOffset, 0, 0);
                Quaternion bulletRotation = Quaternion.Euler(0, 0, spriteRenderer.flipX ? 180 : 0);

                // Create and shoot the bullet
                GameObject bullet = Instantiate(gunPrefab, bulletPosition, bulletRotation);
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                bulletRB.velocity = new Vector2(offsetDirection * 10f, 0);

                lastFired = Time.time;

                // Play the ray gun sound
                if (audioSource != null && rayGunSound != null)
                {
                    audioSource.PlayOneShot(rayGunSound);
                }
            }
        }
    }

    


    //Fanial W.

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            if (isGunActive)
            {
                // Display "Unlimited Ammo" with remaining seconds
                ammoText.text = $"Unlimited Ammo ({gunTimer:F1}s)";
            }
            else
            {
                ammoText.text = "";
            }
        }
    }


    //Fanial W.

    private void UpdateGunPickupCountUI()
    {
        if (gunPickupCountText != null)
        {
            gunPickupCountText.text = "Unlimited Pickups: " + gunPickups;
        }
    }
}
