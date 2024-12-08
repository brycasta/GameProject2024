using UnityEngine;

public class PlayerRespawner : MonoBehaviour //Created by Bryan Castaneda
{
    public GameObject player; // Reference to the player GameObject

    private void Start()
    {
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        if (Checkpoint.lastCheckpointPosition != Vector3.zero)
        {
            // Respawn the player at the last saved checkpoint position
            player.transform.position = Checkpoint.lastCheckpointPosition;

            Debug.Log("Player respawned at: " + Checkpoint.lastCheckpointPosition);
        }
        else
        {
            // Default respawn at the player's original position
            Vector3 startPosition = player.transform.position; // Original position in the scene
            player.transform.position = startPosition;

            Debug.LogWarning("No checkpoint found! Respawning at start position: " + startPosition);
        }
    }
}
