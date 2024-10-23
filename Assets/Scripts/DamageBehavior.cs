using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehavior : MonoBehaviour
{
    public GameOverScript gameOverScript;  // Reference to the GameOverScript

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageSource damageSource = collision.GetComponent<DamageSource>();

        if (damageSource != null)
        {
            // Trigger the Game Over screen before destroying the player
            gameOverScript.gameOver();

            // Delay the destruction of the player to ensure the game over screen is triggered first
            Destroy(gameObject);  // Destroy the enemy
            Destroy(damageSource.gameObject, 0.1f);  // Delay destroying the player slightly
        }
    }
}
