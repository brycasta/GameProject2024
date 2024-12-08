using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //This script is attached to the Spaceship collider box to prevent the user moving the player out of the bounds
    //It compares the palyers collider tag and stops its rigidbody to stop the movement - Bryan Castaneda
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with the barrier
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody2D component of the player
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // Stop the player's velocity to prevent them from moving further
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero;
            }
        }
    }
}
