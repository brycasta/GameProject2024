using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Lee - Whole Script
public class DamageBehavior : MonoBehaviour
{
    // Reference to the GameOverScript that triggers the Game Over screen (uncomment if needed)
    // public GameOverScript gameOverScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageSource damageSource = collision.GetComponent<DamageSource>();

        // Check if the colliding object is a DamageSource (e.g., player or specific objects)
        if (damageSource != null)
        {
            // Trigger the Game Over screen before destroying the player
            // Uncomment the line below if you have a GameOverScript to manage the game over UI
            // gameOverScript.gameOver();

            // Destroy the asteroid or enemy
            Destroy(gameObject);

            // Delay the destruction of the player to ensure game over actions complete first
            Destroy(damageSource.gameObject, 0.1f);  // Adjust delay as necessary
        }
    }
}
