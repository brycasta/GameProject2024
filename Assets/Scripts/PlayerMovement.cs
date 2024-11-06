using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    //Lee - for the Game Over screen
    public GameObject gameOverUI;

    private int jumpCount = 0; // To track how many jumps have been performed
    public int maxJumps = 2;   // The maximum number of jumps allowed (double jump)
    private bool canJump = true;
    //Lee - Shield variable
    GameObject shield;
    //Lee - SFX variable
    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    //Lee - Adding damage SFX
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip hurtSound;
    //Lee - IFrame Variables
    [Header("Invulnerability")]
    [SerializeField] private float iFrameTime;
    [SerializeField] private int flashNumber;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        //Lee - Getting the shield and deactivating it on awake
        shield = transform.Find("Shield").gameObject;
        DeactivateShield();
        playerRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Checks players feet is touching the ground 

        // Reset jump count if the player lands on the ground
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
            canJump = true; // Reset jump ability
        }

        direction = Input.GetAxis("Horizontal");

        if (direction > 0f) //Player movement going and facing right
        {
            playerRB.velocity = new Vector2(direction * speed, playerRB.velocity.y);
            transform.localScale = new Vector2(0.4314091f, 0.4314091f);
            GetComponent<SpriteRenderer>().flipX = false; // Facing right
        }
        else if (direction < 0f) //Player movement going and facing left
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
            //Lee - For playing the jump sound
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("AudioPlaying");
                SoundManager.instance.PlaySound(jumpSound);
            }
        }

        // Allow jump again after button release (prevents multiple jumps from holding the button)
        if (Input.GetButtonUp("Jump"))
        {
            canJump = true;
        }

        // Update the grounded state for the next frame
        wasGrounded = isGrounded;

        //Animation Scripts - Bryan
        playerAnimation.SetFloat("Speed", Mathf.Abs(playerRB.velocity.x));
        playerAnimation.SetBool("OnGround", isGrounded);
    }


    //Lee - All three methods work with the shield
    void ActivateShield()
    {
        shield.SetActive(true);
    }
    
    void DeactivateShield()
    {
        shield.SetActive(false);
    }

    bool HasShield()
    {
        return shield.activeSelf;
    }
    // Lee - this block is for the colliders and shield
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //DamageBehavior damageBehavior = collision.GetComponent<DamageBehavior>();
        DamageSource damageSource = collision.GetComponent<DamageSource>();
        if(damageSource != null)
        {
            if(HasShield())
            {
                DeactivateShield();
                StartCoroutine(Invulnerability());
                SoundManager.instance.PlaySound(breakSound);
            }
            else
            {
                Destroy(gameObject);
                SoundManager.instance.PlaySound(hurtSound);
                gameOverUI.SetActive(true);
            }
            Destroy(damageSource.gameObject);
        }
        ShieldPowerUp shieldPowerUp = collision.GetComponent<ShieldPowerUp>();
        if (shieldPowerUp != null)
        {
            ActivateShield();
            Destroy(shieldPowerUp.gameObject);
        }
        
    }
    // Lee - This code makes the player flash and turn invincible
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(7,8, true);
        for(int i = 0; i< flashNumber; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameTime / (flashNumber * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFrameTime / (flashNumber * 2));
        }
        Physics2D.IgnoreLayerCollision(7, 8, false);

    }
}
