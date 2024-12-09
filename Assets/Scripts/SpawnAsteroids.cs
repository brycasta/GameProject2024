using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab; // Reference to the asteroid prefab
    [SerializeField] private float initialDelay = 1f; // Delay before spawning starts
    [SerializeField] private float spawnCycleInterval = 0.5f; // Interval between each spawn cycle
    [SerializeField] private float staggerDelay = 0.05f; // Delay between spawning asteroids in the same cycle
    [SerializeField] private bool randomizeOrder = true; // Option to randomize the order of spawning

    // Array of positions where asteroids will spawn
    private Vector2[] spawnPositions =
    {
        new Vector2(40.83f, 5.00f),
        new Vector2(44.20f, 4.66f),
        new Vector2(48.95f, 4.00f),
        new Vector2(51.21f, 5.20f),
        new Vector2(54.3f, 5.20f),
        new Vector2(57.3f, 5.00f),
        new Vector2(61.8f, 4.86f),
    };

    private void Start()
    {
        // Start the coroutine for spawning asteroids
        StartCoroutine(SpawnAsteroidsWithRandomPatterns());
    }

    private IEnumerator SpawnAsteroidsWithRandomPatterns()
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(initialDelay);

        while (true) // Infinite loop for continuous spawning
        {
            // Create a list of positions to shuffle or modify
            List<Vector2> currentCyclePositions = new List<Vector2>(spawnPositions);

            if (randomizeOrder)
            {
                // Shuffle the positions for this cycle
                ShufflePositions(currentCyclePositions);
            }

            // Iterate through the current cycle positions
            foreach (Vector2 position in currentCyclePositions)
            {
                // Random chance to skip spawning this asteroid (e.g., 5% chance to skip)
                if (Random.value < 0.05f) continue;

                // Spawn the asteroid
                SpawnAsteroid(position);

                // Wait before spawning the next asteroid
                yield return new WaitForSeconds(staggerDelay);
            }

            // Wait for the next cycle
            yield return new WaitForSeconds(spawnCycleInterval);
        }
    }

    private void SpawnAsteroid(Vector2 position)
    {
        // Instantiate the asteroid prefab at the specified position
        GameObject asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity);

        // Attach the collision handler to the asteroid if not already part of the prefab
        if (asteroid.GetComponent<AsteroidCollisionHandler>() == null)
        {
            asteroid.AddComponent<AsteroidCollisionHandler>();
        }
    }

    private void ShufflePositions(List<Vector2> positions)
    {
        // Fisher-Yates shuffle algorithm
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector2 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }
    }
}
