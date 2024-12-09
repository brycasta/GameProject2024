using UnityEngine;

public class AsteroidCollisionHandler : MonoBehaviour //Created by Bryan Castaneda
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
                // Check if the player has a shield
                if (player.HasShield())
                {
                    // Deactivate the shield and start invulnerability frames
                    player.DeactivateShield();
                    player.StartCoroutine(player.Invulnerability());

                }
                else
                {
                    // Trigger game over if no shield
                    player.TriggerGameOver();
                }
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
