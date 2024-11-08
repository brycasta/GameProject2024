using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab; // Reference to the asteroid prefab
    [SerializeField] private float initialDelay = 30f; // Delay before spawning starts
    [SerializeField] private float spawnDuration = 60f; // Duration for spawning (1 minute)
    [SerializeField] private float spawnInterval = 1f; // Interval between each spawn at a position

    private float spawnTimer; // Timer to track spawning duration

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
        // Initialize the spawn timer
        spawnTimer = spawnDuration;

        // Start the coroutine for continuous spawning with an initial delay
        StartCoroutine(SpawnAsteroidsContinuously());
    }

    private IEnumerator SpawnAsteroidsContinuously()
    {
        // Wait for the initial delay before starting to spawn
        yield return new WaitForSeconds(initialDelay);

        // Start spawning asteroids for the specified duration
        while (spawnTimer > 0f)
        {
            spawnTimer -= spawnInterval; // Reduce the spawn timer by the interval

            // Spawn an asteroid at each position in the array
            foreach (Vector2 position in spawnPositions)
            {
                SpawnAsteroid(position);
            }

            yield return new WaitForSeconds(spawnInterval); // Wait before the next spawn cycle
        }
    }

    private void SpawnAsteroid(Vector2 position)
    {
        // Instantiate asteroid at the specified spawn position
        GameObject asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity);

        // Attach the collision handler to the asteroid
        asteroid.AddComponent<AsteroidCollisionHandler>();
    }
}
