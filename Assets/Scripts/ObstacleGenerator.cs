using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public int minObstacleCount = 10;
    public int maxObstacleCount = 30;
    public float startingObstaclePosition = 100;
    public float minObstacleDistance = 10;
    public float maxObstacleDistance = 15;
    public Transform Z_pos;
    private Transform environment_transform;
    public int Zdistance;
    private void Awake()
    {
        environment_transform = transform.parent;
    }

    private void Start()
    {
        var obstacleCount = Random.Range(minObstacleCount, maxObstacleCount);

        float xPosition = startingObstaclePosition;
        
        for (int i = 0; i < obstacleCount; i++) {
            SpawnObstacle(xPosition);
            xPosition +=  Random.Range(minObstacleDistance, maxObstacleDistance);
        }
    }

    void SpawnObstacle(float xPosition)
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        Instantiate(obstaclePrefabs[randomIndex], new Vector3(xPosition, 0, Zdistance), Quaternion.identity, environment_transform);
    }
}
