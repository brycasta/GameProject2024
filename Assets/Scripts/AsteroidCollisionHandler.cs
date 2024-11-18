using UnityEngine;

public class AsteroidCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the asteroid collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerMovement component from the player GameObject
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                // Call the method to handle player destruction and game over
                player.TriggerGameOver();
            }

            // Destroy the asteroid
            Destroy(gameObject);
        }
        // Destroy the asteroid if it hits "MainGround 3"
        else if (collision.gameObject.name == "MainGround 3")
        {
            Destroy(gameObject);
        }
    }
}
