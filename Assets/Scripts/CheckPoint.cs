using UnityEngine;

public class Checkpoint : MonoBehaviour //Bryan Castaneda
{
    // Static variable to hold the last checkpoint position
    public static Vector3 lastCheckpointPosition = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Save the player's position in the static variable
            lastCheckpointPosition = transform.position;

            Debug.Log("Checkpoint updated to: " + lastCheckpointPosition);
        }
    }
}
