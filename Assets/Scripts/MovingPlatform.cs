using UnityEngine;

public class MovingPlatform : MonoBehaviour //Created by Bryan Castaneda

    //This script is so when the player is on a moving platform is rigidbody stays
    //stuck to the platform and not fall off of it when it moves
{
    private Vector3 lastPosition;
    private GameObject playerOnPlatform;


    void Start()
    {
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Calculate the platform's velocity
        Vector3 platformVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;

        // Apply the platform's velocity to the player if they are on the platform
        if (playerOnPlatform != null)
        {
            Rigidbody2D playerRb = playerOnPlatform.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity += new Vector2(platformVelocity.x, 0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = null;
        }
    }
}
