using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public GameObject player; // Reference to the player
    public float activationDistance = 5.0f; // Distance within which the enemy starts moving

    private bool playerInRange = false; // Tracks whether the player is in range

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // If the player is within activation distance, start moving
        if (distanceToPlayer <= activationDistance)
        {
            playerInRange = true;
        }
    }

    private void FixedUpdate()
    {
        // Only move the enemy if the player is within range
        if (playerInRange)
        {
            Vector2 position = transform.position;

            position.x -= moveSpeed * Time.fixedDeltaTime;

            transform.position = position;

            // Destroy the enemy if it moves too far off screen 
            if (position.x < -12)
            {
                Destroy(gameObject);
            }
        }
    }
}
