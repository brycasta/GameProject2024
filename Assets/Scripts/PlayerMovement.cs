using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int jumpCount = 0; // To track how many jumps have been performed
    public int maxJumps = 2;   // The maximum number of jumps allowed (double jump)
    private bool canJump = true;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
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

        if (direction > 0f)
        {
            playerRB.velocity = new Vector2(direction * speed, playerRB.velocity.y);
        }
        else if (direction < 0f)
        {
            playerRB.velocity = new Vector2(direction * speed, playerRB.velocity.y);
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
        }

        // Allow jump again after button release (prevents multiple jumps from holding the button)
        if (Input.GetButtonUp("Jump"))
        {
            canJump = true;
        }

        // Update the grounded state for the next frame
        wasGrounded = isGrounded;

    }
}
