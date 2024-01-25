using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnInterval = 2f;
    public float spawnRange = 2f;
    
    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > spawnInterval)
        {
            SpawnObstacle();

            elapsedTime = 0f;
        }
    }

    void SpawnObstacle()
    {
        float randomY = Random.Range(-spawnRange, spawnRange);
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        Instantiate(obstaclePrefabs[randomIndex], new Vector2(transform.position.x, randomY), Quaternion.identity);
    }
}
